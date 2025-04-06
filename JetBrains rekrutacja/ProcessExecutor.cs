using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{
    /// <summary>
    /// Interface that defines a method for receiving notifications about process output.
    /// </summary>
    interface IStreamObserver
    {
        /// <summary>
        /// Notifies the observer about new output from a process.
        /// </summary>
        /// <param name="output">The output from the process (either standard output or error output).</param>
        /// <param name="st">The type of stream (standard output or error output).</param>
        public void Notify(string output, StreamType st);
    }

    /// <summary>
    /// Interface that defines methods for subscribing and unsubscribing observers.
    /// </summary>
    interface ISubject
    {
        /// <summary>
        /// Subscribes an observer to receive notifications about process output.
        /// </summary>
        /// <param name="ob">The observer to subscribe.</param>
        public void Subscribe(IStreamObserver ob);

        /// <summary>
        /// Unsubscribes an observer from receiving notifications about process output.
        /// </summary>
        /// <param name="ob">The observer to unsubscribe.</param>
        public void Unsubscribe(IStreamObserver ob);
    }

    /// <summary>
    /// A class that handles executing processes and notifying observers about their output.
    /// Implements the <see cref="ISubject"/> interface.
    /// </summary>
    class ProcessExecutor : ISubject
    {
        private static readonly Lazy<ProcessExecutor> _instance = new Lazy<ProcessExecutor>(() => new ProcessExecutor());

        private static object _lock = new object();
        private CancellationTokenSource? source = null;
        private Process? p = null;

        /// <summary>
        /// Indicates whether a process is currently running.
        /// </summary>
        public bool IsRunning => p != null && source != null;

        private List<IStreamObserver> _observers;

        /// <summary>
        /// Private constructor for initializing the observer list.
        /// </summary>
        private ProcessExecutor()
        {
            _observers = new List<IStreamObserver>();
        }

        /// <summary>
        /// Gets the singleton instance of <see cref="ProcessExecutor"/>.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="ProcessExecutor"/>.</returns>
        public static ProcessExecutor GetInstance()
        {
            return _instance.Value;
        }

        /// <summary>
        /// Starts the specified process asynchronously and reads its output.
        /// </summary>
        /// <param name="info">The <see cref="ProcessStartInfo"/> that contains the process information.</param>
        /// <returns>A task representing the asynchronous execution of the process.</returns>
        public async Task Run(ProcessStartInfo info)
        {
            p = Process.Start(info);
            if (p != null)
            {
                source = new CancellationTokenSource();
                Task outputTask = Task.Run(async () => await ReadStreamAsync(StreamType.STDOUT, p.StandardOutput, source));
                Task errorTask = Task.Run(async () => await ReadStreamAsync(StreamType.STDERR, p.StandardError, source));
                await Task.WhenAll(outputTask, errorTask);
                await p.WaitForExitAsync();
            }
        }

        /// <summary>
        /// Subscribes an observer to receive notifications about process output.
        /// </summary>
        /// <param name="ob">The observer to subscribe.</param>
        public void Subscribe(IStreamObserver ob)
        {
            _observers.Add(ob);
        }

        /// <summary>
        /// Unsubscribes an observer from receiving notifications about process output.
        /// </summary>
        /// <param name="ob">The observer to unsubscribe.</param>
        public void Unsubscribe(IStreamObserver ob)
        {
            _observers.Remove(ob);
        }

        /// <summary>
        /// Notifies all subscribed observers about new output from a process.
        /// </summary>
        /// <param name="output">The output from the process.</param>
        /// <param name="st">The type of stream (standard output or error output).</param>
        public void NotifySubscribers(string output, StreamType st)
        {
            foreach (var observer in _observers)
            {
                observer.Notify(output, st);
            }
        }

        /// <summary>
        /// Reads the process output stream asynchronously and notifies observers.
        /// </summary>
        /// <param name="st">The type of stream (standard output or error output).</param>
        /// <param name="reader">The stream reader for reading the process output.</param>
        /// <param name="ct">The cancellation token source used to cancel reading.</param>
        /// <returns>A task representing the asynchronous operation of reading the stream.</returns>
        private async Task ReadStreamAsync(StreamType st, StreamReader reader, CancellationTokenSource ct)
        {
            char[] buffer = new char[1024];
            int read;
            while ((read = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string output = new string(buffer, 0, read);
                if (st == StreamType.STDERR)
                {
                    ct.Cancel();
                }
                if (st == StreamType.STDOUT && ct.IsCancellationRequested) break;
                NotifySubscribers(output, st);
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// Cancels the running process and clears resources.
        /// </summary>
        public void Cancel()
        {
            if (source != null && p != null)
            {
                p.Kill();
                source.Cancel();
            }

            source = null;
            p = null;
        }
    }
}

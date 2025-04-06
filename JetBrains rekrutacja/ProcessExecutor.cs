using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{

    interface IStreamObserver
    {
        public void Notify(string output, StreamType st);
    }

    interface ISubject
    {
        public void Subscribe(IStreamObserver ob);
        public void Unsubscribe(IStreamObserver ob);

    }
    class ProcessExecutor: ISubject
    {
        private static readonly Lazy<ProcessExecutor> _instance = new Lazy<ProcessExecutor>(() => new ProcessExecutor());

        private static object _lock = new object();
        CancellationTokenSource? source = null;
        Process? p = null;

        private List<IStreamObserver> _observers;
        private ProcessExecutor()
        {
            _observers = new List<IStreamObserver>();
        }

        public static ProcessExecutor GetInstance()
        {
            return _instance.Value;
        }
        public async Task Run(ProcessStartInfo info)
        {
            p = Process.Start(info);
            if (p != null)
            {
                source = new CancellationTokenSource();
                Task outputTask = Task.Run(async () => await ReadStreamAsync(StreamType.STDOUT, p.StandardOutput,  source));
                Task errorTask = Task.Run(async () => await ReadStreamAsync(StreamType.STDERR, p.StandardError,  source));
                await Task.WhenAll(outputTask, errorTask);
                await p.WaitForExitAsync();
            }
        }

        public void Subscribe(IStreamObserver ob)
        {
            _observers.Add(ob);
        }

        public void Unsubscribe(IStreamObserver ob)
        {
            _observers.Remove(ob);
        }

        public void NotifySubscribers(string output, StreamType st)
        {
            foreach(var observer in _observers)
            {
                observer.Notify(output, st);
            }
        }

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

        public void Cancel()
        {
            _observers.Clear();
            if(source != null)
            {
                p.Kill();
                source.Cancel();
            }
        }
    }
}

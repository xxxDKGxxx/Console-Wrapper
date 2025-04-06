using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{
    /// <summary>
    /// Interface representing a command that can be executed asynchronously.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of the command.</returns>
        Task ExecuteAsync();
    }

    /// <summary>
    /// Command class for executing CMD commands.
    /// </summary>
    public class CmdCommand : ICommand
    {
        private readonly string _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdCommand"/> class.
        /// </summary>
        /// <param name="command">The command to be executed in CMD.</param>
        public CmdCommand(string command)
        {
            _command = command;
        }

        /// <summary>
        /// Executes the CMD command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of the command.</returns>
        public async Task ExecuteAsync()
        {
            await ExecuteProcess("cmd.exe", $"/c {_command}");
        }

        /// <summary>
        /// Executes a process with the specified process name and arguments.
        /// </summary>
        /// <param name="processName">The name of the process to be started (e.g., "cmd.exe").</param>
        /// <param name="arguments">The arguments to pass to the process (e.g., the command to run).</param>
        /// <returns>A task representing the asynchronous execution of the process.</returns>
        private async Task ExecuteProcess(string processName, string arguments)
        {
            var info = new ProcessStartInfo(processName, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            await ProcessExecutor.GetInstance().Run(info);
        }
    }

    /// <summary>
    /// Command class for executing PowerShell commands.
    /// </summary>
    public class PowerShellCommand : ICommand
    {
        private readonly string _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerShellCommand"/> class.
        /// </summary>
        /// <param name="command">The command to be executed in PowerShell.</param>
        public PowerShellCommand(string command)
        {
            _command = command;
        }

        /// <summary>
        /// Executes the PowerShell command asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous execution of the command.</returns>
        public async Task ExecuteAsync()
        {
            await ExecuteProcess("PowerShell.exe", $"-Command {_command}");
        }

        /// <summary>
        /// Executes a process with the specified process name and arguments.
        /// </summary>
        /// <param name="processName">The name of the process to be started (e.g., "PowerShell.exe").</param>
        /// <param name="arguments">The arguments to pass to the process (e.g., the command to run).</param>
        /// <returns>A task representing the asynchronous execution of the process.</returns>
        private async Task ExecuteProcess(string processName, string arguments)
        {
            var info = new ProcessStartInfo(processName, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            await ProcessExecutor.GetInstance().Run(info);
        }
    }
}

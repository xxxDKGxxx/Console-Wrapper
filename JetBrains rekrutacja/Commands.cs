using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }

    public class CmdCommand : ICommand
    {
        private readonly string _command;
        public CmdCommand(string command)
        {
            _command = command;
        }

        public async Task ExecuteAsync()
        {
            await ExecuteProcess("cmd.exe", $"/c {_command}");
        }

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

    public class PowerShellCommand : ICommand
    {
        private readonly string _command;
        public PowerShellCommand(string command)
        {
            _command = command;
        }

        public async Task ExecuteAsync()
        {
            await ExecuteProcess("PowerShell.exe", $"-Command {_command}");
        }

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

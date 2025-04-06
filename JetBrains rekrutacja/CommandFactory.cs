using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{
    /// <summary>
    /// Factory class that creates instances of ICommand based on the specified console type.
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// Creates a command instance based on the specified console type.
        /// </summary>
        /// <param name="command">The command string to be executed.</param>
        /// <param name="consoleType">The type of console (CMD or PowerShell) where the command should be executed.</param>
        /// <returns>An instance of ICommand that can execute the command in the specified console.</returns>
        /// <exception cref="NotImplementedException">Thrown when an unsupported console type is specified.</exception>
        public static ICommand CreateCommand(string command, ConsoleType consoleType)
        {
            switch (consoleType)
            {
                case ConsoleType.CMD:
                    return new CmdCommand(command);
                case ConsoleType.POWERSHELL:
                    return new PowerShellCommand(command);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

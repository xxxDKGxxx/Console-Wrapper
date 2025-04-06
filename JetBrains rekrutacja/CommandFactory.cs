using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetBrains_rekrutacja
{
    public class CommandFactory
    {
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

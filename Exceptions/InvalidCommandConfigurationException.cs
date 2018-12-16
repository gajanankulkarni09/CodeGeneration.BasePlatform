using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class InvalidCommandConfigurationException : ApplicationException
    {
        public string CommandName { get; set; }

        public string ChildCommandName { get; set; }

        public InvalidCommandConfigurationException(string commandName)
        {
            CommandName = commandName;
        }

        public InvalidCommandConfigurationException(string commandName, string message) : base(message)
        {
            CommandName = commandName;
        }
    }
}

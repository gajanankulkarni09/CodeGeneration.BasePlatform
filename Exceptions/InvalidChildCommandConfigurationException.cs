using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class InvalidChildCommandConfigurationException : ApplicationException
    {
        public string CommandName { get; set; }

        public string StepName { get; set; }

        public string[] ErrorMessages { get; set; }

        public InvalidChildCommandConfigurationException(string commandName,string stepName):base()
        {
            CommandName = commandName;
            StepName = stepName;
        }

        public InvalidChildCommandConfigurationException(string commandName, string childCommandName, string[] errorMessages) : this(commandName,childCommandName)
        {
            ErrorMessages = errorMessages;
        }
    }
}

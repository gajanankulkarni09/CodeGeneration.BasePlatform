using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class DuplicateCommandRegisteredException : ApplicationException
    {
        public string CommandName { get; set; }

        public DuplicateCommandRegisteredException(string commandName):base()
        {
            CommandName = commandName;
        }

    }
}

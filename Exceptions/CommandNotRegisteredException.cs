using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class CommandNotRegisteredException : ApplicationException
    {
        public string CommandName { get; private set; }
        public CommandNotRegisteredException() : base() { }

        public CommandNotRegisteredException(string commandName) : base($"{commandName} not registered")
        {
            CommandName = commandName;
        }

    }
}

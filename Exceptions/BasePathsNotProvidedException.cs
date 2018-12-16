using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class BasePathsNotProvidedException : AggregateException
    {
        public string CommandName { get; set; }

        public BasePathsNotProvidedException(string commandName) : base()
        {
            CommandName = commandName;
        }
    }
}

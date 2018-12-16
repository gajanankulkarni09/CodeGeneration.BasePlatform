using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public  class CommandInputParametersMissingException:ApplicationException
    {
        public string CommandName { get; set; }

        public string[] MissingInputParameters { get; set; }

        public CommandInputParametersMissingException(string commandName,string[] parameterNames):base()
        {
            CommandName = commandName;
            MissingInputParameters = parameterNames;
        }
    }
}

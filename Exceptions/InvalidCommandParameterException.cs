using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class InvalidCommandParameterException : ApplicationException
    {
        public string[] InvalidInputFileNames { get; set; }

        public string[] InputInputParameterNames { get; set; }

        public InvalidCommandParameterException(string[] fileNames, string[] inputParameterNames) : base()
        {
            InvalidInputFileNames = fileNames;
            InputInputParameterNames = inputParameterNames;
        }

        public InvalidCommandParameterException(string[] fileNames, string[] inputParameterNames, string message) : base(message)
        {
            InvalidInputFileNames = fileNames;
            InputInputParameterNames = inputParameterNames;
        }
    }
}

namespace CodeGeneration.BasePlatform.Exceptions
{
    class InvalidChildInputFileConfiguration : InvalidChildCommandConfigurationException
    {
        public int Index { get; set; }
        public InvalidChildInputFileConfiguration(string commandName, string stepName,int index) : base(commandName, stepName)
        {
            Index = index;
        }
    }
}

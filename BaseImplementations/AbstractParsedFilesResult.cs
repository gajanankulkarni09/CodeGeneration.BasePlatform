namespace CodeGeneration.BasePlatform.BaseImplementations
{
    public abstract class AbstractParsedFilesResult
    {
        public string[] InputFiles { get; private set; }

        public AbstractParsedFilesResult(string[] inputFiles)
        {
            InputFiles = inputFiles;
        }
    }
}

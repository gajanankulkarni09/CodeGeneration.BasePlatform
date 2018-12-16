namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IFilesProcessor<T>
    {
        T ProcessFiles(string[] files);
    }
}

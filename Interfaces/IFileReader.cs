namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IFileReader
    {
        string ReadToEnd(string path);

        string[] ReadLines(string path);
    }
}

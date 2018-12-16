using CodeGeneration.BasePlatform.Models;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IFileWriter
    {
        void WriteToFile(string fullPath, string content, FileGenerationInfo fileGenerationInfo);
    }
}

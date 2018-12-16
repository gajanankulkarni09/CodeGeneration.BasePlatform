using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;
using Io = System.IO;

namespace CodeGeneration.BasePlatform.Utilities
{
    internal class FileWriter : IFileWriter
    {
        public void WriteToFile(string fullPath, string content, FileGenerationInfo fileGenerationInfo)
        {
            Io.Directory.CreateDirectory(Io.Path.GetDirectoryName(fullPath));
            Io.File.WriteAllText(fullPath, content);
        }
    }
}

using CodeGeneration.BasePlatform.Models;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IFileInputProvider
    {
        string[] GetInputFilePaths(string rootPath, string[] parentInputFileNames, Dictionary<string, CommandResult> commandResultFile);
    }
}

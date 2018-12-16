using CodeGeneration.BasePlatform.Models;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface ICommand
    {
        CommandResult Execute(string[] fileNames,  Dictionary<string, string> inputParams);
    }
}

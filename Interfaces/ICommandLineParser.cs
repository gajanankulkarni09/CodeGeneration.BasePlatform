using CodeGeneration.BasePlatform.Models;

namespace CodeGeneration.BasePlatform.Interfaces
{
    internal interface ICommandParser
    {
        ParsedCommand Parse(string command);
    }
}

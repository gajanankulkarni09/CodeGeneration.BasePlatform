using CodeParser.Contracts.Interfaces;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IFileParser<ResultType>
    {
        ResultType Parse(string fileContent);
    }
}

using CodeGeneration.BasePlatform.Interfaces;
using CodeParser.Contracts.Interfaces;
using CodeParser.Contracts.Models;

namespace CodeGeneration.BasePlatform.Utilities
{
    internal class RoslynCsClassParser : IFileParser<CodeStructure>
    {
        ICodeParser _classParser;

        internal RoslynCsClassParser(ICodeParser classParser)
        {
            _classParser = classParser;
        }

        public CodeStructure Parse(string classText)
        {
            return _classParser.Parse(classText);
        }
    }
}

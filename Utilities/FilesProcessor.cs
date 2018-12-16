using CodeGeneration.BasePlatform.Interfaces;
using CodeParser.Contracts.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class FilesProcessor : IFilesProcessor<ParsedFilesResult>
    {
        IFileParser<CodeStructure> _csClassFileParser;
        IFileParser<string[]> _textFileParser;
        IFileReader _fileReader;

        public FilesProcessor()
        {
            _csClassFileParser = FileParserProvider.GetCodeFileParser("cs");
            _textFileParser = FileParserProvider.GetFlatFileParser<string[]>("txt");
            _fileReader = new FileReader();
        }
        public ParsedFilesResult ProcessFiles(string[] files)
        {
            var codeStructures = new Dictionary<string, CodeStructure>();
            var textFiles = new Dictionary<string, string[]>();
            var jsonFile = new Dictionary<string, string>();
            
            var result = new ParsedFilesResult(codeStructures,textFiles,jsonFile,files);

            foreach(var file in files.Distinct())
            {
                var extention = Path.GetExtension(file);
                var fileContent = _fileReader.ReadToEnd(file);

                switch(extention.ToLower())
                {
                    case ".cs":
                        codeStructures.Add(file, _csClassFileParser.Parse(fileContent));
                        break;
                    case ".txt":
                        textFiles.Add(file, _textFileParser.Parse(fileContent));
                        break;
                    case ".json":
                        jsonFile.Add(file, fileContent);
                        break;
                }
            }

            return result;
        }


    }
}

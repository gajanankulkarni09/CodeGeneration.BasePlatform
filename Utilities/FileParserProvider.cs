using CodeGeneration.BasePlatform.Interfaces;
using CodeParser.Contracts.Models;
using System;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Utilities
{
    public static class FileParserProvider
    {
        static IFileParser<CodeStructure> _roslynCsClassParser;

        static FileParserProvider()
        {
            _roslynCsClassParser= new RoslynCsClassParser(new RoslynCodeParser.RoslynClassParser());
        }

        public static IFileParser<CodeStructure> GetCodeFileParser(string fileExtention)
        {
            switch (fileExtention.ToLower())
            {
                case "cs":
                    return _roslynCsClassParser;
            }
            throw new NotSupportedException($"{fileExtention} not supported");
        }

        public static IFileParser<T> GetFlatFileParser<T>(string fileExtention)
        {
            switch(fileExtention.ToLower())
            {
                case "json":
                    return new JsonParser<T>();
                case "txt":
                    return (IFileParser<T>) new TextFileParser();
            }
            throw new NotSupportedException($"{fileExtention} not supported");
        }
    }
}

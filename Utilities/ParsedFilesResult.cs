using CodeGeneration.BasePlatform.BaseImplementations;
using CodeParser.Contracts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class ParsedFilesResult : AbstractParsedFilesResult
    {
        readonly Dictionary<string, CodeStructure> _codeFiles = new Dictionary<string, CodeStructure>();
        readonly Dictionary<string, string[]> _textFiles = new Dictionary<string, string[]>();
        readonly Dictionary<string, string> _jsonFiles = new Dictionary<string, string>();

        public ParsedFilesResult(
            Dictionary<string, CodeStructure> codeFiles,
            Dictionary<string, string[]> textFiles,
            Dictionary<string, string> jsonFiles,
            string[] inputFiles) : base(inputFiles)
        {
            _codeFiles = codeFiles;
            _textFiles = textFiles;
            _jsonFiles = jsonFiles;
        }

        public ClassDetails GetClassDetails(string fileName)
        {
            return _codeFiles[fileName].ClassDetails;
        }

        public ClassDetails GetClassDetails(string fileName, string className)
        {
            return _codeFiles[fileName].GetClassDetails(className);
        }

        public EnumDetails[] GetAllEnumDetails(string fileName)
        {
            return _codeFiles[fileName].GetEnums();
        }

        public bool IsEnum(string fileName, string enumName)
        {
            return _codeFiles[fileName].IsEnum(enumName);
        }

        public string[] GetTextFileLines(string fileName)
        {
            return _textFiles[fileName];
        }

        public T GetObject<T>(string fileName)
        {
            return JsonConvert.DeserializeObject<T>(_jsonFiles[fileName]);
        }

        public ClassDetails[] GetAllClassDetails()
        {
            return _codeFiles
                        .Values
                        .Where(codeStructure => codeStructure.ClassDetails != null)
                        .Select(codeStructure => codeStructure.ClassDetails)
                        .ToArray();
        }
    }
}

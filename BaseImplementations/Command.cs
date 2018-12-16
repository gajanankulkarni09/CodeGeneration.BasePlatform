using CodeGeneration.BasePlatform.BaseImplementations;
using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;
using CodeGeneration.BasePlatform.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.BasePlatform
{
    public abstract class Command<
                                    ModelType,
                                    ParsedFilesResultType,
                                    InputParameterType
                                > : BaseCommand, ICommand
                                                    where ModelType : class
                                                    where ParsedFilesResultType : AbstractParsedFilesResult
                                                    where InputParameterType : class
    {
        readonly ITextTransformer _textTransformer;
        readonly string _commandName;
        readonly IFilesProcessor<ParsedFilesResultType> _filesProcessor;
        readonly IFileReader _fileReader;
        readonly IFileWriter _fileWriter;
        string _outputBasePath;

        protected struct Result
        {
            public string FileName { get; set; }

            public string FileType { get; set; }

            public string RelativeFilePath { get; set; }

            public FileGenerationInfo FileGenerationInfo { get; set; }

            public string TemplateName;

            public ModelType Model;

            public List<string> Messages { get; set; }

            public DependentOutputFileGenreation[] DependentOutputFileGenreations { get; set; }

        }

        protected struct DependentOutputFileGenreation
        {
            public string[] InputFiles { get; set; }

            public string RelativeOutputPath { get; set; }

            public bool ProcessDependentClassIfExist { get; set; }
        }

        protected struct ValidationResult
        {
            ///Todo: validation for missing input Files and missing input Parameters
            ///
            public bool IsValid { get; set; }

            public string[] InvalidInputFileNames { get; set; }

            public string[] InvalidInputParameterNames { get; set; }
        }

        //Todo: try to remove textTransformer and fileParser from below constructor
        public Command(string commandName,
                                ITextTransformer textTransformer,
                                IFilesProcessor<ParsedFilesResultType> filesProcessor)
                                        : this(commandName,
                                                textTransformer,
                                                filesProcessor,
                                                new FileReader(),
                                                new FileWriter())
        {

        }

        public Command(string commandName,
                                ITextTransformer textTransformer,
                                IFilesProcessor<ParsedFilesResultType> filesProcessor,
                                IFileReader fileReader,
                                IFileWriter fileWriter)
        {
            _commandName = commandName;
            _textTransformer = textTransformer;
            _filesProcessor = filesProcessor;
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            ParameterBuilder = new ParameterBuilder();
        }

        public Command(string commandName,
                                ITextTransformer textTransformer,
                                IFilesProcessor<ParsedFilesResultType> filesProcessor,
                                IFileReader fileReader,
                                IFileWriter fileWriter,
                                ExpectedInputFilesInfo expectedInputFilesInfo) : base(expectedInputFilesInfo)
        {
            _commandName = commandName;
            _textTransformer = textTransformer;
            _filesProcessor = filesProcessor;
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            ParameterBuilder = new ParameterBuilder();
        }

        public IParameterBuilder ParameterBuilder { get; set; }

        public CommandResult Execute(string[] inputFiles, Dictionary<string, string> inputParams)
        {
            if (inputFiles == null)
                throw new System.ArgumentNullException(nameof(inputFiles));

            if (inputParams == null)
                throw new System.ArgumentNullException(nameof(inputParams));

            var validationResult = ValidateCommand(inputFiles, inputParams);

            ///Todo: validation for missing input Files and missing input Parameters

            if (!inputParams.ContainsKey("BaseOutputPath"))
            {
                validationResult.IsValid = false;
                validationResult.InvalidInputParameterNames.Union(new string[] { "BaseOutputPath" });
            }

            if (!validationResult.IsValid)
                throw new InvalidCommandParameterException(validationResult.InvalidInputFileNames, validationResult.InvalidInputParameterNames);

            _outputBasePath = inputParams["BaseOutputPath"];

            ///Todo: Handle File not found case 
            return ExecuteCommand(inputFiles, inputParams);
        }

        protected abstract ValidationResult ValidateCommand(string[] inputFileNames, Dictionary<string, string> inputParams);

        protected abstract Result GetModel(ParsedFilesResultType parsedFilesResultType, InputParameterType parameters);

        protected ParsedFilesResultType ParseFiles(string inputFile)
        {
            if (System.IO.File.Exists(inputFile))
                return _filesProcessor.ProcessFiles(new string[] { inputFile });
            return null;
        }

        private CommandResult ExecuteCommand(string[] inputFiles, Dictionary<string, string> inputParams)
        {
            ValidateInputFileCount(inputFiles);
            var defaultFileNames = GetDefaultFiles(inputFiles);
            inputFiles = inputFiles.Concat(defaultFileNames).ToArray();

            var commandResult = new CommandResult(_commandName, inputFiles);
            var invalidFilePaths = inputFiles.Where(fileName => !System.IO.File.Exists(fileName));
            if (invalidFilePaths.Any())
            {
                commandResult.IsSuccessful = false;
                commandResult.Messages.AddRange(invalidFilePaths.Select(fileName => $"could not find {fileName} file"));
            }
            else
            {
                var parsedResults = _filesProcessor.ProcessFiles(inputFiles);
                var builtParameter = ParameterBuilder.BuildParameterFrom<InputParameterType>(inputParams);
                var result = GetModel(parsedResults, builtParameter);
                foreach (var outputFileGeneration in result.DependentOutputFileGenreations)
                {
                    var outputPath = $"{_outputBasePath}/{outputFileGeneration.RelativeOutputPath}";
                    if (outputFileGeneration.ProcessDependentClassIfExist || !System.IO.File.Exists(outputPath))
                    {
                        var dependentFileGenerationResult = Execute(outputFileGeneration.InputFiles, inputParams);
                        commandResult.MergeDependentFileGenerationResults(dependentFileGenerationResult);
                    }
                }
                var generatedCode = _textTransformer.GetTransformedCode(result.TemplateName, result.Model);
                WriteFiles(result, generatedCode, _outputBasePath);
                UpdateSuccessfulResult(commandResult, result);
            }
            return commandResult;
        }

        private void UpdateSuccessfulResult(CommandResult commandResult, Result result)
        {
            var outputFilePath = $"{_outputBasePath}/{result.FileName}";
            commandResult.IsSuccessful = true;
            commandResult.OutputFiles.Add(new FileOutput()
            {
                FullPath = outputFilePath,
                Type = result.FileType
            });
            commandResult.FilesGenerated.Add(outputFilePath);
            commandResult.Messages.AddRange(result.Messages);
        }

        private void WriteFiles(Result result, string generatedCode, string outputBasePath)
        {
            var outputPath = string.IsNullOrWhiteSpace(result.RelativeFilePath) ? $"{outputBasePath}/{result.FileName}" : $"{outputBasePath}/{result.RelativeFilePath}/{result.FileName}";
            _fileWriter.WriteToFile(outputPath, generatedCode, result.FileGenerationInfo);
        }

    }
}

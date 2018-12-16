using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeGeneration.BasePlatform.BaseImplementations;
using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;
using Newtonsoft.Json;

namespace CodeGeneration.BasePlatform
{
    internal class JsonCompositeCommandBuilder
    {
        readonly ICommandFactory _commandFactory;

        internal JsonCompositeCommandBuilder(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory ?? throw new System.ArgumentNullException(nameof(commandFactory));
        }

        private class Helper : IInputParamProvider, IFileInputProvider
        {
            JsonChildCommandConfiguration _childCommandConfiguration;
            ExpectedInputFilesInfo _parentInputFilesInfo;
            Dictionary<string, object> _customObjects = new Dictionary<string, object>();

            string _parentCommandName;

            internal Helper(JsonCompositeCommandConfiguration commandConfiguration, JsonChildCommandConfiguration childCommandConfiguration)
            {
                _childCommandConfiguration = childCommandConfiguration;
                _parentInputFilesInfo = commandConfiguration.ExpectedInputFilesInfo;
                _parentCommandName = commandConfiguration.CommandName;
                if (commandConfiguration.CustomObjects != null)
                    commandConfiguration.CustomObjects.ToList().ForEach(o => _customObjects.Add(o.Name, o.Value));
            }

            string[] IFileInputProvider.GetInputFilePaths(string rootPath, string[] parentInputFileNames, Dictionary<string, CommandResult> commandResults)
            {
                var inputFileNames = new List<string>();
                var index = 0;


                foreach (var inputFileConfiguration in _childCommandConfiguration.InputFiles)
                {
                    switch (inputFileConfiguration.InpueFileType)
                    {
                        case InputFileType.Parent:
                            AddInputFileFromParent(rootPath, parentInputFileNames, inputFileNames, index, inputFileConfiguration);
                            break;
                        case InputFileType.Sibling:
                            AddInputFileFromSibling(commandResults, inputFileNames, index, inputFileConfiguration);
                            break;
                    }
                    index++;
                }

                return inputFileNames.ToArray();
            }

            

            private void AddInputFileFromSibling(Dictionary<string, CommandResult> commandResults, List<string> inputFileNames, int index, InputFile inputFileConfiguration)
            {
                var requiredCommandResult = commandResults[inputFileConfiguration.InputFileFromSibling.StepName];
                var outputFile = requiredCommandResult.OutputFiles.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.Type) && f.Type.Equals(inputFileConfiguration.InputFileFromSibling.FileType));

                if (!string.IsNullOrWhiteSpace(outputFile?.FullPath))
                {
                    inputFileNames.Add(outputFile.FullPath);
                }
                else
                {
                    throw new InvalidChildInputFileConfiguration(_parentCommandName, _childCommandConfiguration.StepName, index);
                }
            }

            private void AddInputFileFromParent(string rootPath, string[] parentInputFileNames, List<string> inputFileNames, int index, InputFile inputFileConfiguration)
            {
                if (parentInputFileNames.Length > inputFileConfiguration.InputFileFromParent.Index)
                {
                    var projectDirectoryPath = $"{rootPath}/{ _parentInputFilesInfo.RelativePaths[inputFileConfiguration.InputFileFromParent.Index]}";
                    var fileName = parentInputFileNames[inputFileConfiguration.InputFileFromParent.Index];
                    var firstProbablePath = $"{projectDirectoryPath}/{fileName }";

                    if (!System.IO.File.Exists(firstProbablePath) || _parentInputFilesInfo.AutoSearchSubDirectories)
                    {
                        var filePath = Directory.GetFiles(projectDirectoryPath, fileName, SearchOption.AllDirectories).FirstOrDefault();
                        inputFileNames.Add(filePath);
                    }
                    else
                    {
                        inputFileNames.Add(firstProbablePath);
                    }
                }
                else
                {
                    throw new InvalidChildInputFileConfiguration(_parentCommandName, _childCommandConfiguration.StepName, index);
                }
            }

            Dictionary<string, string> IInputParamProvider.GetInputParams(string stepName, Dictionary<string, string> inputParams)
            {
                if (inputParams == null)
                {
                    throw new System.ArgumentNullException(nameof(inputParams));
                }

                var childInputParams = new Dictionary<string, string>();

                if (_childCommandConfiguration.InputParameters != null)
                {
                    foreach (var parameter in _childCommandConfiguration.InputParameters)
                    {
                        childInputParams.Add(parameter.Name, parameter.Value);
                    }
                }

                if (_childCommandConfiguration.InputParametersFromParent != null)
                {
                    foreach (var parameterFromParent in _childCommandConfiguration.InputParametersFromParent)
                    {
                        if (inputParams.ContainsKey(parameterFromParent.ParentParameterName))
                            childInputParams.Add(parameterFromParent.ChildParameterName, inputParams[parameterFromParent.ParentParameterName]);
                    }
                }

                if (_childCommandConfiguration.CustomObjectInputParameter != null)
                    childInputParams.Add(_childCommandConfiguration.CustomObjectInputParameter.Name, JsonConvert.SerializeObject(_customObjects[_childCommandConfiguration.CustomObjectInputParameter.CustomObjectName]));

                return childInputParams;
            }

        }

        internal CompositeCommand Build(JsonCompositeCommandConfiguration commandConfiguration)
        {
            if (commandConfiguration == null)
            {
                throw new System.ArgumentNullException(nameof(commandConfiguration));
            }

            commandConfiguration.Validate();

            var command = new CompositeCommand(commandConfiguration.CommandName,
                                                GetExpectedParameters(commandConfiguration),commandConfiguration.ExpectedInputFilesInfo);

            foreach (var childCommand in commandConfiguration.ChildCommands)
            {
                var helper = new Helper(commandConfiguration, childCommand);
                command.AddCommand(childCommand.StepName,
                    new ChildCommandConfiguration()
                    {
                        StepName = childCommand.StepName,
                        StepNumber = childCommand.StepNumber,
                        Command = _commandFactory.GetCommand(childCommand.CommandName),
                        FileInputProvider = helper,
                        InputParamProvider = helper,
                        OutputPath = childCommand.OutputPath,
                    });
            }
            return command;
        }

        private static ExpectedParameter[] GetExpectedParameters(JsonCompositeCommandConfiguration commandConfiguration)
        {
            return commandConfiguration
                            .InputParameters
                            ?.Select(parameter => parameter.ToExpectedParameter())
                            ?.ToArray();
        }

    }
}

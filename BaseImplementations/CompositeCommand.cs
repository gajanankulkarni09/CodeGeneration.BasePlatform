using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.BasePlatform.BaseImplementations
{
    public class CompositeCommand : BaseCommand, ICommand
    {
        readonly List<ChildCommandConfiguration> _childCommandConfigurations;
        readonly string _commandName;
        readonly ExpectedParameter[] _expectedParameters;
        public CompositeCommand(string commandName, ExpectedParameter[] parameters)
        {
            _commandName = commandName;
            _childCommandConfigurations = new List<ChildCommandConfiguration>();
            _expectedParameters = parameters;
        }

        public CompositeCommand(string commandName, ExpectedParameter[] parameters, ExpectedInputFilesInfo expectedInputFilesInfo) : base(expectedInputFilesInfo)
        {
            _commandName = commandName;
            _childCommandConfigurations = new List<ChildCommandConfiguration>();
            _expectedParameters = parameters;
        }

        public CompositeCommand(string commandName) : this(commandName, null)
        {

        }

        public void AddCommand(string stepName, ChildCommandConfiguration childCommandConfiguration)
        {
            if (string.IsNullOrWhiteSpace(stepName))
                throw new ArgumentException("child commandname not provided", nameof(stepName));

            if (childCommandConfiguration == null || !childCommandConfiguration.IsValid())
                throw new InvalidChildCommandConfigurationException(_commandName, stepName);

            if (!childCommandConfiguration.StepNumber.HasValue)
                childCommandConfiguration.StepNumber = _childCommandConfigurations.Count + 1;

            _childCommandConfigurations.Add(childCommandConfiguration);
        }

        public virtual CommandResult Execute(string[] inputFileNames, Dictionary<string, string> inputParams)
        {
            if (inputFileNames == null || inputFileNames.Length == 0)
                throw new ArgumentNullException(nameof(inputFileNames));

            ValidateInputParameters(inputParams);
            AddDefaultParameterValue(inputParams);

            ValidateInputFileCount(inputFileNames);
            var defaultFileNames = GetDefaultFiles(inputFileNames);
            inputFileNames = inputFileNames.Concat(defaultFileNames).ToArray();

            var childCommandresults = ExecuteChildCommands(inputFileNames, inputParams);
            var result = new CommandResult(_commandName, inputFileNames)
            {
                IsSuccessful = !childCommandresults.Any(childResult => !childResult.IsSuccessful),
                FilesGenerated = childCommandresults.SelectMany(childResult => childResult.FilesGenerated).ToList(),
                Messages = childCommandresults.SelectMany(childResult => childResult.Messages).ToList(),
                OutputFiles = childCommandresults.SelectMany(childResult => childResult.OutputFiles).ToList()
            };
            return result;
        }

        private CommandResult[] ExecuteChildCommands(string[] inputFileNames, Dictionary<string, string> inputParams)
        {
            Dictionary<string, CommandResult> commandResults = new Dictionary<string, CommandResult>();

            foreach (var childCommandConfiguration in _childCommandConfigurations.OrderBy(config => config.StepNumber))
            {
                Dictionary<string, string> childInputParams = null;

                var command = childCommandConfiguration.Command;

                if (command == null)
                    throw new CommandNotRegisteredException(childCommandConfiguration.StepName);

                if (childCommandConfiguration.InputParamProvider != null)
                    childInputParams = childCommandConfiguration.InputParamProvider.GetInputParams(childCommandConfiguration.StepName, inputParams);

                if (!string.IsNullOrWhiteSpace(childCommandConfiguration.OutputPath))
                {
                    childInputParams.Add("BaseOutputPath", $"{EnvironmentVariables.BasePath }/{ childCommandConfiguration.OutputPath}");
                }

                var childInputFilePaths = childCommandConfiguration.FileInputProvider.GetInputFilePaths(EnvironmentVariables.BasePath, inputFileNames, commandResults);
                var commandResult = command.Execute(childInputFilePaths, childInputParams);
                commandResults.Add(childCommandConfiguration.StepName, commandResult);
            }
            return commandResults.Values.ToArray();
        }

        private void AddDefaultParameterValue(Dictionary<string, string> inputParams)
        {
            var missingParameters = GetMissingInputParameters(inputParams);
            if (missingParameters != null)
            {
                foreach (var missingParameter in missingParameters)
                {
                    if (!string.IsNullOrWhiteSpace(missingParameter.DefaultValue))
                        inputParams.Add(missingParameter.Name, missingParameter.DefaultValue);
                }
            }
        }

        private bool ValidateInputParameters(Dictionary<string, string> inputParams)
        {
            var missingParameters = GetMissingInputParameters(inputParams);

            if (missingParameters?.Length > 0)
            {
                var missingMandatoryParameterNames = missingParameters.Where(param => param.IsMandatory).Select(parameter => parameter.Name).ToArray();

                if (missingMandatoryParameterNames?.Count() > 0)
                    throw new CommandInputParametersMissingException(_commandName, missingMandatoryParameterNames);
            }

            return true;
        }

        private ExpectedParameter[] GetMissingInputParameters(Dictionary<string, string> inputParams)
        {
            return _expectedParameters
                        ?.Where(parameter => !inputParams.ContainsKey(parameter.Name))
                        ?.ToArray();

        }

    }
}

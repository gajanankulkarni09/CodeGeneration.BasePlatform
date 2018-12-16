using CodeGeneration.BasePlatform.BaseImplementations;
using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;

namespace CodeGeneration.BasePlatform
{
    public class CommandProcessor
    {
        ICommandParser _commandParser;
        IApplicationFactoriesProvider _applicationFactoriesProvider;
        EnvironmentVariables _environmentVariables;

        public CommandProcessor(IApplicationFactoriesProvider applicationFactoriesProvider, EnvironmentVariables environmentVariables)
        {
            _commandParser = new SimpleCommandParser();
            _applicationFactoriesProvider = applicationFactoriesProvider;
            _environmentVariables = environmentVariables;
            EnvironmentVariables.Set(_environmentVariables);
        }

        internal CommandProcessor(IApplicationFactoriesProvider applicationFactoriesProvider, ICommandParser commandParser)
        {
            _applicationFactoriesProvider = applicationFactoriesProvider;
            _commandParser = commandParser;
        }

        public string[] ProcessCommand(string commandLine)
        {
            var parsedCommand = _commandParser.Parse(commandLine);
            var commandFactory = _applicationFactoriesProvider.GetCommandFactory(_environmentVariables.AppName);
            var requiredCommand = commandFactory.GetCommand(parsedCommand.Name);
            BaseCommand.SetGlobalVariables(_environmentVariables);
            var commandResult = requiredCommand.Execute(parsedCommand.Args, parsedCommand.NamedParameters);
            return commandResult.Messages.ToArray();
        }

        public void ChangeAppName(string appName, string basePath)
        {
            _environmentVariables.AppName = appName;
            _environmentVariables.BasePath = basePath;
        }
    }
}

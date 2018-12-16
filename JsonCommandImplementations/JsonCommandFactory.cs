using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.JsonCommandImplementations
{
    internal class JsonCommandFactory : ICommandFactory
    {
        ICommandFactory _baseCommandsFactory;
        Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
        IFileReader _fileReader;
        JsonCompositeCommandBuilder _jsonCompositeCommandBuilder;

        internal JsonCommandFactory(ICommandFactory baseCommandFactory,
                                    IFileReader fileReader)
        {
            _baseCommandsFactory = baseCommandFactory;
            _fileReader = fileReader;
            _jsonCompositeCommandBuilder = new JsonCompositeCommandBuilder(this);
        }   

        internal void RegisterCommands(string configurationDirPath,string indexFileName)
        {

            var index = JsonConvert.DeserializeObject<JsonCommandIndex>(_fileReader.ReadToEnd($"{configurationDirPath}/{indexFileName}"));
            foreach(var commandInfo in index.Commands)
            {
                var commandConfigurationJson = _fileReader.ReadToEnd($"{configurationDirPath}/{commandInfo.FileName}");
                var commandConfiguration = JsonConvert.DeserializeObject<JsonCompositeCommandConfiguration>(commandConfigurationJson);
                var command = _jsonCompositeCommandBuilder.Build(commandConfiguration);
                if (_commands.ContainsKey(commandConfiguration.CommandName))
                {
                    throw new DuplicateCommandRegisteredException(commandConfiguration.CommandName);
                }
                else
                {
                    _commands.Add(commandConfiguration.CommandName, command);
                }
            }
        }

        /// <summary>
        /// Returns registered command.If command is not registered throws CommandNotRegisteredException
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        ICommand ICommandFactory.GetCommand(string commandName)
        {
            if (string.IsNullOrEmpty(commandName?.Trim()))
                throw new ArgumentException($"{nameof(commandName)} is require parameter");

            if (_commands.ContainsKey(commandName))
                return _commands[commandName];

            return _baseCommandsFactory.GetCommand(commandName);
        }
    }
}

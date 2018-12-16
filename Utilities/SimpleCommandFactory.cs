using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class SimpleCommandFactory : ICommandFactory
    {
        Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public SimpleCommandFactory()
        {

        }

        public SimpleCommandFactory(Dictionary<string, ICommand> commands)
        {
            _commands = commands;
        }

        public void AddCommand(string commandName, ICommand command)
        {
            if (string.IsNullOrEmpty(commandName?.Trim()))
                throw new ArgumentException($"{nameof(commandName)} is required");

            if (command == null)
                throw new ArgumentException($"{nameof(command)} is required");

            if (_commands.ContainsKey(commandName))
                throw new InvalidOperationException($"{commandName} is already registered");

            _commands.Add(commandName, command);
        }

        public ICommand GetCommand(string commandName)
        {
            if (string.IsNullOrEmpty(commandName?.Trim()))
                throw new ArgumentException($"{nameof(commandName)} is required");

            if (_commands.ContainsKey(commandName))
                return _commands[commandName];

            throw new CommandNotRegisteredException($"{commandName} is not registered");
        }
    }
}

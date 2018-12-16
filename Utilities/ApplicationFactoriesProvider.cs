using CodeGeneration.BasePlatform.Interfaces;
using System;
using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class ApplicationFactoriesProvider : IApplicationFactoriesProvider
    {
        Dictionary<string, ICommandFactory> _commandFactories = new Dictionary<string, ICommandFactory>();

        public void AddCommandFactory(string appName,ICommandFactory commandFactory)
        {
            _commandFactories.Add(appName, commandFactory);
        }

        public ICommandFactory GetCommandFactory(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName?.Trim()))
                throw new ArgumentException($"{nameof(applicationName)} cannot  be null or empty", $"{nameof(applicationName)}");

            if (_commandFactories.ContainsKey(applicationName))
                return _commandFactories[applicationName];
            else
                throw new InvalidOperationException($"{applicationName} not registered");
        }
    }
}

using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Models;
using System;
using System.Linq;

namespace CodeGeneration.BasePlatform
{
    internal class SimpleCommandParser : ICommandParser
    {
        public ParsedCommand Parse(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim()))
                throw new ArgumentException($"{nameof(command)} is required", nameof(command));

            var commandSegments = command.Split(new char[] { ' ' });

            if (commandSegments.Length < 2)
                throw new ArgumentException("Invalid command");

            var parsedCommand = new ParsedCommand()
            {
                Name = commandSegments[0],
                Args = commandSegments
                            .Skip(1)
                            .TakeWhile(segment => !segment.Trim().StartsWith("--"))
                            .Where(segment => !string.IsNullOrWhiteSpace(segment))
                            .ToArray()
            };

            ExtractNamedParameters(commandSegments, parsedCommand);

            return parsedCommand;
        }

        private static void ExtractNamedParameters(string[] commandSegments, ParsedCommand parsedCommand)
        {
            var i = 0;

            while (i < commandSegments.Length && !commandSegments[i].Trim().StartsWith("--"))
            {
                i++;
            }

            while (i + 1 < commandSegments.Length)
            {
                var paramName = commandSegments[i].Trim().Substring(2);
                var paramValue = commandSegments[++i].Trim();
                parsedCommand.NamedParameters.Add(paramName, paramValue);
                i++;
            }
        }
    }
}

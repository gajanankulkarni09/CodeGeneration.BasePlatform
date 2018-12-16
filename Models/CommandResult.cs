using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Models
{
    public class CommandResult
    {
        public CommandResult(string commandName,string[] inputFiles)
        {
            CommandName = commandName;
            InputFiles = inputFiles;
        }

        public string CommandName { get; set; }

        public string[] InputFiles { get; set; }

        public bool IsSuccessful { get; set; }

        public List<FileOutput> OutputFiles { get; set; } = new List<FileOutput>();

        public List<string> FilesGenerated { get; set; } = new List<string>();

        public List<string> Messages { get; set; } = new List<string>();

        public void MergeDependentFileGenerationResults(CommandResult commandResult)
        {
            FilesGenerated.AddRange(commandResult.FilesGenerated);
            Messages.AddRange(commandResult.Messages);
        }
    }

    public class FileOutput
    {
        public string Type { get; set; }

        public string FullPath { get; set; }

    }

}

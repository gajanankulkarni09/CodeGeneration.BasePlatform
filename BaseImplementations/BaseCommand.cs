using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Models;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.BasePlatform.BaseImplementations
{
    public class BaseCommand
    {
        private readonly ExpectedInputFilesInfo _expectedInputFilesInfo;

        protected static EnvironmentVariables EnvironmentVariables { get; private set; }

        internal static void SetGlobalVariables(EnvironmentVariables environmentVariables)
        {
            EnvironmentVariables = environmentVariables;
        }

        public BaseCommand(ExpectedInputFilesInfo expectedInputFilesInfo)
        {
            _expectedInputFilesInfo = expectedInputFilesInfo;
        }

        public BaseCommand()
        {

        }

        protected bool ValidateInputFileCount(string[] inputFileNames)
        {
            if (_expectedInputFilesInfo == null)
                return true;

            var minimumInputFilesCount = _expectedInputFilesInfo.Count;

            var optionFilesCount = _expectedInputFilesInfo?.FileNameRules?.Count(r => !r.Mandatory) ?? 0;
            minimumInputFilesCount = minimumInputFilesCount - optionFilesCount;

            if (minimumInputFilesCount > inputFileNames.Length)
            {
                throw new InputFilesNotProvidedException(minimumInputFilesCount);
            }
            return true;
        }

        protected string[] GetDefaultFiles(string[] inputFileNames)
        {
            if (_expectedInputFilesInfo?.FileNameRules != null)
            {
                var fileNames = new List<string>();
                for (var i = 0; i < _expectedInputFilesInfo.Count; i++)
                {
                    var rule = _expectedInputFilesInfo.FileNameRules.FirstOrDefault(r => r.Index == i);
                    if (rule != null && !rule.Mandatory)
                    {
                        if (rule.CopyFromIndex.HasValue)
                            fileNames.Add(inputFileNames[rule.CopyFromIndex.Value]);
                        else if (!string.IsNullOrWhiteSpace(rule.DefaultName))
                            fileNames.Add(rule.DefaultName);
                    }
                }
                return fileNames.ToArray();
            }

            return new string[] { };
        }
    }
}

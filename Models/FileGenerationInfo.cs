using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Models
{
    public class FileGenerationInfo
    {
        public FileGenerationType FileGenerationType { get; set; }

        public Dictionary<string, string> FileGenerationParameters { get; set; }

        public bool UpdateFileIfExists { get; set; }
    }
    public enum FileGenerationType
    {
        CreateNew,
        UpdateExisting
    }
}

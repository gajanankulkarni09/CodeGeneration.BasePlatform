namespace CodeGeneration.BasePlatform.Models
{
    public class ExpectedInputFilesInfo
    {
        public int Count { get; set; }

        public FileNameRule[] FileNameRules { get; set; }
    }

    public class FileNameRule
    {
        public bool Mandatory { get; set; }

        public int Index { get; set; }

        public int? CopyFromIndex { get; set; }

        public string DefaultName { get; set; }
    }
}

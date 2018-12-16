using CodeGeneration.BasePlatform.Models;

namespace CodeGeneration.BasePlatform
{
    public class JsonCompositeCommandConfiguration
    {
        public string CommandName { get; set; }

        public ExpectedInputFilesInfo ExpectedInputFilesInfo { get; set; }

        public ExpectedInputParameter[] InputParameters { get; set; }

        public JsonChildCommandConfiguration[] ChildCommands { get; set; }


        public bool Validate()
        {
            //todo:throw InvalidChildCommandConfigurationException when invalid

            return true;
        }

        public CustomObject[] CustomObjects { get; set; }
    }

    public class CustomObject
    {
        public string Name { get; set; }

        public dynamic Value { get; set; }
    }

    public class JsonChildCommandConfiguration
    {
        public string StepName { get; set; }

        public string CommandName { get; set; }

        public int? StepNumber { get; set; }

        public InputParametersFromParent[] InputParametersFromParent { get; set; }

        public InputParameter[] InputParameters { get; set; }

        public InputFile[] InputFiles { get; set; }

        public string OutputPath { get; set; }

        public CustomObjectInputParameter CustomObjectInputParameter { get; set; }
    }

    public class InputParametersFromParent
    {
        public string ParentParameterName { get; set; }

        public string ChildParameterName { get; set; }
    }

    public class CustomObjectInputParameter
    {
        public string Name { get; set; }

        public string CustomObjectName { get; set; }
    }

    public class ExpectedInputFilesInfo
    {
        public int Count { get; set; }

        public FileNameRule[] FileNameRules { get; set; }

        public bool AutoSearchSubDirectories { get; set; }

        public string[] RelativePaths { get; set; }

        public bool Validate()
        {
            if (RelativePaths?.Length == Count)
                return true;

            return false;
        }
    }

    public class FileNameRule
    {
        public bool Mandatory { get; set; }

        public int Index { get; set; }

        public int? CopyFromIndex { get; set; }

        public string DefaultName { get; set; }
    }


    public class ExpectedInputParameter
    {
        public string Name { get; set; }

        public bool IsMandatory { get; set; }

        public string DefaultValue { get; set; }

        public ExpectedParameter ToExpectedParameter()
        {
            return new ExpectedParameter()
            {
                Name = Name,
                IsMandatory = IsMandatory,
                DefaultValue = DefaultValue
            };
        }
    }

    public class InputParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public enum InputFileType
    {
        Parent,
        Sibling
    }

    public class InputFile
    {
        public InputFileType InpueFileType { get; set; }

        public InputFileFromParent InputFileFromParent { get; set; }

        public InputFileFromSibling InputFileFromSibling { get; set; }
    }

    public class InputFileFromParent
    {
        public int Index { get; set; }
    }

    public class InputFileFromSibling
    {
        public string StepName { get; set; }

        public string FileType { get; set; }
    }
}

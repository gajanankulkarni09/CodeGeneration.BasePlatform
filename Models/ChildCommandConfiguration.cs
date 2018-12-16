using CodeGeneration.BasePlatform.Interfaces;

namespace CodeGeneration.BasePlatform.Models
{
    public class ChildCommandConfiguration
    {
        public string StepName { get; set; }

        public ICommand Command { get; set; }

        public int? StepNumber { get; set; }

        public IInputParamProvider InputParamProvider { get; set; }

        public IFileInputProvider FileInputProvider { get; set; }

        public string OutputPath { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(StepName) ||
                Command == null ||
                FileInputProvider == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}

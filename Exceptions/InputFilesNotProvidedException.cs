using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class InputFilesNotProvidedException:ApplicationException
    {
        public int FilesCount { get; set; }

        public InputFilesNotProvidedException(int fileCount):base($"minimum {fileCount} number of files required in command input")
        {
            FilesCount = fileCount;
        }
    }
}

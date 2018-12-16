using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.JsonCommandImplementations
{
    public class JsonApplicationIndex
    {
        public List<ApplicationInfo > Applications { get; set; }
    }

    public class ApplicationInfo
    {
        public string Name { get; set; }

        public string DirectoryName { get; set; }

        public string CommandsIndexFileName { get; set; }
    }
}

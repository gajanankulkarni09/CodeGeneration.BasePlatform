using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Models
{
    public class ParsedCommand
    {
        public string Name { get; set; }

        public string[] Args { get; set; }

        public Dictionary<string, string> NamedParameters { get; set; } = new Dictionary<string, string>();
    }
}

using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.JsonCommandImplementations
{
    public class JsonCommandIndex
    {
        public List<CommandInfo> Commands { get; set; }
    }

    public class CommandInfo
    {
        public string CommandName { get; set; }

        public string FileName { get; set; }
    }

}

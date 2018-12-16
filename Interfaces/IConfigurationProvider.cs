using Newtonsoft.Json.Linq;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IConfigurationProvider
    {
        JObject GetConfigration(string commandName);
    }
}

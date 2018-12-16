using CodeGeneration.BasePlatform.Interfaces;
using Newtonsoft.Json;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class JsonParser<T> : IFileParser<T>
    {
        public T Parse(string fileContent)
        {
            return JsonConvert.DeserializeObject<T>(fileContent);
        }
    }

}

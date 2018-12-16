using Newtonsoft.Json.Linq;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public  interface ITextTransformer
    {
        string GetTransformedCode<ModelType>(string templateName, ModelType model);
    }
}

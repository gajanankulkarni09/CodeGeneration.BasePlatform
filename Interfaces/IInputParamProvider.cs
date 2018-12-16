using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IInputParamProvider
    {
        Dictionary<string, string> GetInputParams(string stepName,Dictionary<string, string> inputParams);
    }
}

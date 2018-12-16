using System.Collections.Generic;

namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IParameterBuilder
    {
        T BuildParameterFrom<T>(Dictionary<string, string> inputParams);
    }
}

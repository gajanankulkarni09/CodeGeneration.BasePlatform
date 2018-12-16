using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGeneration.BasePlatform.Models
{
    public class InputParamConverter
    {
        public T ConvertToObject<T>(Dictionary<string, string> inputParams)
        {
            T resultParameter = Activator.CreateInstance<T>();
            Type parameterType = typeof(T);
            var publicProperties = parameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var publicProperty in publicProperties)
            {
                if (publicProperty.CanWrite)
                {
                    var propertyType = publicProperty.PropertyType;
                    var propertyName = publicProperty.Name;
                    if (inputParams.ContainsKey(propertyName))
                    {
                        if (propertyType.IsClass && !propertyType.Name.Equals("string", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var value = JsonConvert.DeserializeObject(inputParams[propertyName], propertyType);
                            publicProperty.SetValue(resultParameter, value);
                        }
                        else
                        {
                            var value = Convert.ChangeType(inputParams[propertyName], propertyType);
                            publicProperty.SetValue(resultParameter, value);
                        }
                    }
                }
            }
            return resultParameter;
        }
    }
}

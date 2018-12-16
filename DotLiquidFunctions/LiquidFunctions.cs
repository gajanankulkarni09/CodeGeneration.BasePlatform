using DotLiquid;
using System;
using System.Linq;
using System.Reflection;

namespace CodeGeneration.BasePlatform.DotLiquidFunctions
{
    internal static class LiquidFunctions
    {
        internal static void RegisterViewModel(Assembly assembly,string modelsNamespace)
        {
            assembly
                 .GetTypes()
                 .Where(t => t.Namespace == modelsNamespace)
                 .ToList()
                 .ForEach(RegisterSafeTypeWithAllProperties);
        }


        private static void RegisterSafeTypeWithAllProperties(Type type)
        {
            Template.RegisterSafeType(type,
                type
                    .GetProperties()
                    .Select(p => p.Name)
                    .ToArray());
        }

        internal static string RenderModel( this Template template, object root)
        {
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            return template.Render(
                Hash.FromAnonymousObject(root));
        }
    }
}

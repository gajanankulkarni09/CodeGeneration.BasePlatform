using CodeGeneration.BasePlatform.DotLiquidFunctions;
using CodeGeneration.BasePlatform.Exceptions;
using CodeGeneration.BasePlatform.Interfaces;
using DotLiquid;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class LiquidTextTransformer : ITextTransformer
    {
        readonly Dictionary<string, Template> _templates;

        public LiquidTextTransformer()
        {
            _templates = new Dictionary<string, Template>();
        }

        public void RegisterTypesInNamespace(Assembly assembly, string modelsNamespace)
        {
            LiquidFunctions.RegisterViewModel(assembly, modelsNamespace);
        }

        public void RegisterTemplate(string templateName, string template)
        {
            if (_templates.ContainsKey(templateName))
                throw new DuplicateTemplateRegisteredException(templateName);

            Template dotLiquidTemplate = null;
            try
            {
                dotLiquidTemplate = Template.Parse(template);
            }
            catch (Exception ex)
            {
                throw new TemplateCompilationException(templateName, ex.Message);
            }

            _templates.Add(templateName, dotLiquidTemplate);
        }

        public string GetTransformedCode<ModelType>(string templateName, ModelType model)
        {
            if (!_templates.ContainsKey(templateName))
                throw new TemplateNotRegisteredException(templateName);

            try
            {
                var template = _templates[templateName];
                var transformedCode = template.RenderModel(model);
                return transformedCode;
            }
            catch(Exception ex)
            {
                throw new TemplateTransformationException(templateName, ex.Message);
            }
        }
    }
}

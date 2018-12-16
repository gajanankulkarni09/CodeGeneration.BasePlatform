using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class TemplateCompilationException : ApplicationException
    {
        public string TemplateName { get; set; }

        public TemplateCompilationException(string templateName,string errorMessage) : base($"{templateName} template did not compiled successfully.Error message - {errorMessage}")
        {
            TemplateName = templateName;
        }

    }
}

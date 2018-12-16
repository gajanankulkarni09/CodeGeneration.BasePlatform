using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class TemplateNotRegisteredException : ApplicationException
    {
        public string TemplateName { get; set; }

        public TemplateNotRegisteredException(string templateName):base()
        {
            TemplateName = templateName;
        }
    }
}

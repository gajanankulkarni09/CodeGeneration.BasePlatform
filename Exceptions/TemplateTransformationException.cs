using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class TemplateTransformationException : AggregateException
    {
        public string TemplateName { get; set; }

        public TemplateTransformationException(string templateName,string message): base(message)
        {
            TemplateName = templateName;
        }
    }
}

using System;

namespace CodeGeneration.BasePlatform.Exceptions
{
    public class DuplicateTemplateRegisteredException:ApplicationException
    {
        public string TemplateName { get; set; }

        public DuplicateTemplateRegisteredException(string templateName):base()
        {
            TemplateName = templateName;
        }
    }
}

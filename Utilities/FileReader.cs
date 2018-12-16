using CodeGeneration.BasePlatform.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeGeneration.BasePlatform.Utilities
{
    public class FileReader : IFileReader
    {
        public string[] ReadLines(string path)
        {
            if (File.Exists(path))
            {
                List<string> lines = new List<string>();
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    while (!streamReader.EndOfStream)
                    {
                        lines.Add(streamReader.ReadLine());
                    }
                }
                return lines.ToArray();
            }
            else
            {
                throw new FileNotFoundException("File not found", path);
            }
        }

        public string ReadToEnd(string path)
        {
            string text;
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            return text;
        }
    }
}

using CodeGeneration.BasePlatform.Interfaces;
using CodeGeneration.BasePlatform.Utilities;
using Newtonsoft.Json;

namespace CodeGeneration.BasePlatform.JsonCommandImplementations
{
    public class JsonApplicationFactoriesProvider : ApplicationFactoriesProvider
    {
        readonly IFileReader _fileReader;
        readonly ICommandFactory _baseCommandFactory;

        public JsonApplicationFactoriesProvider(IFileReader fileReader, ICommandFactory baseCommandFactory)
        {
            _fileReader = fileReader;
            _baseCommandFactory = baseCommandFactory;
        }

        public JsonApplicationFactoriesProvider(ICommandFactory baseCommandFactory)
        {
            _fileReader = new FileReader();
            _baseCommandFactory = baseCommandFactory;
        }

        public void Register(string configurationDirectoryPath, string indexFileName)
        {
            var index = JsonConvert.DeserializeObject<JsonApplicationIndex>(_fileReader.ReadToEnd($"{configurationDirectoryPath}/{indexFileName}"));
            foreach (var application in index.Applications)
            {
                var commandsFactory = new JsonCommandFactory(_baseCommandFactory, _fileReader);
                commandsFactory.RegisterCommands($"{configurationDirectoryPath}/{application.DirectoryName}", application.CommandsIndexFileName);
                AddCommandFactory(application.Name, commandsFactory);
            }
        }
    }
}

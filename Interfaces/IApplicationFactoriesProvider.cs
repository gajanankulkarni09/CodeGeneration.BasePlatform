namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface IApplicationFactoriesProvider
    {
        ICommandFactory GetCommandFactory(string applicationName);
    }
}

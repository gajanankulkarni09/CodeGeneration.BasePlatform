namespace CodeGeneration.BasePlatform.Interfaces
{
    public interface ICommandFactory
    {
        ICommand GetCommand(string commandName);
    }
}

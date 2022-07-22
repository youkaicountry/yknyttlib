namespace YKnyttLib.Parser
{
    public interface ICommand
    {
        string Execute<T>(T environment);
    }
}

using Model;
namespace View.Commands.Implements;

public class RevertCommand : IConsoleCommand
{
    private readonly Patcher _receiver;

    public RevertCommand(Patcher receiver)
    {
        _receiver = receiver;
    }

    public Task Execute()
    {
        Console.WriteLine("------------------------- Revert -----------------------");
        Console.WriteLine("[RevertCommand]: Reverting changes...");
        _receiver.Revert();
        return Task.CompletedTask;
    }
}
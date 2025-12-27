using Model;

namespace View.Commands.Implements;

public class UpdateCommand : IConsoleCommand
{
    private readonly UpdateManager _receiver;
    
    
    public UpdateCommand(UpdateManager receiver)
    {
        _receiver = receiver;
    }
    
    public async Task Execute()
    {
        Console.WriteLine("---------------------- Updating ------------------------");
        await _receiver.ApplyUpdateAsync();
    }
}
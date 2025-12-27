using Model;

namespace View.Commands.Implements;

public class ResizeCommand : IConsoleCommand
{
    private readonly Patcher _receiver;
    
    private readonly int _width;
    private readonly int _height;

    public ResizeCommand(Patcher receiver, int width, int height)
    {
        _receiver = receiver;
        _width = width;
        _height = height;
    }

    public Task Execute()
    {
        Console.WriteLine("--------------------- Apply Patch ----------------------");
        Console.WriteLine("[ResizeCommand]: Delegating to Patcher...");
        _receiver.ApplyPatch(_width, _height);
        return Task.CompletedTask;
    }
}
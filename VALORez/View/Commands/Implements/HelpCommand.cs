using System.Reflection;

namespace View.Commands.Implements;

public class HelpCommand : IConsoleCommand
{
    public Task Execute()
    {
        string? exeName = Assembly.GetExecutingAssembly().GetName().Name;

        Console.WriteLine(">> Usage:");
        Console.WriteLine($"\t{exeName} --patch [width]x[height]");
        Console.WriteLine($"\t{exeName} --revert-patch");
        Console.WriteLine();
        Console.WriteLine($"\t{exeName} --version");
        Console.WriteLine($"\t{exeName} --update");
        Console.WriteLine($"\t{exeName} --help");
        return Task.CompletedTask;
    }
}
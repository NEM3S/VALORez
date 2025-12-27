using System.Diagnostics;
using System.Reflection;
using Model;
namespace View.Commands.Implements;

public class VersionCommand : IConsoleCommand
{
    public Task Execute()
    {
        Console.WriteLine(">> VALORez v"
                          + Assembly
                              .GetEntryAssembly()?
                              .GetName()
                              .Version?
                              .ToString(3)
                          );
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}
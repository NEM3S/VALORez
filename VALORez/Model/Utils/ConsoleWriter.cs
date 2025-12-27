using Spectre.Console;
namespace Model.Utils;

public abstract class ConsoleWriter
{
    public static void PrintSuccess(string message)
    {
        AnsiConsole.MarkupLine($" [bold green]SUCCESS[/] {DateTime.Now:T} - {message}.");
    }
    
    public static void PrintFailure(string message)
    {
        AnsiConsole.MarkupLine($" [bold red]FAIL[/]    {DateTime.Now:T} - {message}!");
    }
    
    public static void PrintInfo(string message)
    {
        AnsiConsole.MarkupLine($" [bold gold1]INFO[/]    {DateTime.Now:T} - {message}.");
    }
    
    public static void PrintError(string message)
    {
        AnsiConsole.MarkupLine($" [bold purple]ERROR[/]   {DateTime.Now:T} - {message}");
    }
}
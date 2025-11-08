namespace Model.Utils;

public abstract class ConsoleWriter
{
    public static void PrintSuccess(string message)
    {
        Console.WriteLine($" [\x1B[32m\u2713\x1B[97m][{DateTime.Now:T}] - {message}.");
    }
    
    public static void PrintFailure(string message)
    {
        Console.WriteLine($" [\x1B[31mx\x1B[97m][{DateTime.Now:T}] - {message}!");
    }
    
    public static void PrintInfo(string message)
    {
        Console.WriteLine($" [\x1B[33mO\x1B[97m][{DateTime.Now:T}] - {message}.");
    }
    
    public static void PrintError(string message)
    {
        Console.WriteLine($" [\x1B[35mERROR\x1B[97m][{DateTime.Now:T}] - {message}");
    }
}
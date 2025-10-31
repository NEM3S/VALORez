using System;

namespace Model;

public class ConsoleWriter
{
    public static void PrintSuccess(string message)
    {
        Console.WriteLine($" [\x1B[32m\u2714\x1B[97m][{DateTime.Now:T}] - {message}.");
    }
    
    public static void PrintFailure(string message)
    {
        Console.WriteLine($" [\x1B[31m\u2718\x1B[97m][{DateTime.Now:T}] - {message}!");
    }
    
    public static void PrintInfo(string message)
    {
        Console.WriteLine($" [\x1B[33m\u25cf\x1B[97m][{DateTime.Now:T}] - {message}.");
    }
    
    public static void PrintError(string message)
    {
        Console.WriteLine($" [\x1B[35mERROR\x1B[97m][{DateTime.Now:T}] - {message}");
    }
}
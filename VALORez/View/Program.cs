using Model;
using Model.Utils;

namespace View;

public abstract class Program
{
    public static void Main(string[] args)
    {
        
        Console.WriteLine("[START]");
        
        string[] sizes = ["1280","720"];
        Patcher patcher = new Patcher();

        try
        {
            if (args.Any())
            {
                Console.WriteLine("[OPTION claimed]");
                if (args[0].Contains("x"))
                {
                    sizes = args[0].Split('x');
                    patcher = new Patcher(int.Parse(sizes[0]), int.Parse(sizes[1]));
                    patcher.Initialize();
                }
                else if (args[0] == "--reset")
                {
                    patcher.Reset();
                }
            }
            else
            {
                patcher.Initialize();
            }
        }
        catch (Exception e)
        {
            ConsoleWriter.PrintError(e.ToString());
        }
        
        Console.WriteLine("[END]");
        Console.WriteLine("  -- Press any key to terminate --");
        Console.ReadLine();
    }
}
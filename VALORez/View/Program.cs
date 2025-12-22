using System.Diagnostics;
using System.Reflection;
using System.Text;
using Model;
using Model.Utils;

namespace View;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        Console.WriteLine($"------------------------ v{Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3)} ------------------------");
 
        SplashArt.Draw();
        
        Console.WriteLine("------------------------ Update ------------------------");

        CleanupUpdate();
        var updater = new UpdateManager();
        await updater.CheckAndApplyUpdateAsync();

        Console.WriteLine("------------------------ Apply Patch ------------------------");
        Patcher patcher = new Patcher();

        try
        {
            if (args.Any())
            {
                Console.WriteLine(" [OPTION claimed]");
                if (args[0].Contains("x"))
                {
                    string[] sizes = args[0].Split('x');
                    patcher = new Patcher(int.Parse(sizes[0]), int.Parse(sizes[1]));
                    patcher.Initialize();
                }
                else if (args[0] == "--revert")
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
        
        Console.Write("\n  -- Press enter to terminate --");
        Console.ReadLine();
        Console.WriteLine("-------------------------------------------------------");
    }
    
    private static void CleanupUpdate()
    {
        var currentExe = Process.GetCurrentProcess().MainModule?.FileName;
        var oldExe = currentExe + ".old";

        if (File.Exists(oldExe))
        {
            try
            {
                // On essaie de supprimer l'ancienne version maintenant qu'elle ne tourne plus
                File.Delete(oldExe); 
            }
            catch 
            {
                // Ignorer si échec (c'est du nettoyage silencieux)
            }
        }
    }
}
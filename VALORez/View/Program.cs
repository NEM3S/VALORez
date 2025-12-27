using System.Diagnostics;
using System.Reflection;
using System.Text;
using Model;
using Model.Utils;
using View.Commands;
using View.Commands.Implements;

namespace View;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        SetupConsole();
        
        Patcher patcherReceiver = new Patcher();
        UpdateManager updateManagerReceiver = new UpdateManager();

        StartupInvoker invoker = new StartupInvoker();
        invoker.SetCommand(new HelpCommand());

        // Select the right command
        if (args.Any())
        {
            if (args[0] == "--revert-patch")
            {
                await CheckUpdateProcess();
                invoker.SetCommand(new RevertCommand(patcherReceiver));
            }
            
            else if (args[0] == "--patch" && args[1].ToLower().Contains("x"))
            {
                var parts = args[1].ToLower().Split('x');
                int w = int.Parse(parts[0]);
                int h = int.Parse(parts[1]);
                
                await CheckUpdateProcess();
                invoker.SetCommand(new ResizeCommand(patcherReceiver, w, h));
            }
            
            if (args[0] == "--update")
            {
                invoker.SetCommand(new UpdateCommand(updateManagerReceiver));
            }
            
            else if (args[0] == "--version")
            {
                invoker.SetCommand(new VersionCommand());
            }
            
        }

        // Execute command
        await invoker.Run();

        WaitForExit();
    }
    
    private static void SetupConsole()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("--------------------------------------------------------");
        SplashArt.Draw();
    }
    
    private static void CleanupUpdate()
    {
        var currentExe = Process.GetCurrentProcess().MainModule?.FileName;
        var oldExe = currentExe + ".old";

        if (File.Exists(oldExe))
        {
            try
            {
                File.Delete(oldExe); 
            }
            catch
            {
                // ignored
            }
        }
    }
    
    private static async Task CheckUpdateProcess()
    {
        Console.WriteLine("------------------------ Update ------------------------");
        CleanupUpdate();
        var updater = new UpdateManager();
        await updater.CheckUpdateAsync();
        Console.WriteLine();
    }
    
    private static void WaitForExit()
    {
        Console.Write("\n   -- Press enter to terminate --    ");
        Console.ReadLine();
        Console.WriteLine("--------------------------------------------------------");
    }
}
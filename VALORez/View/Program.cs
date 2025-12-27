using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.Utils;
using View.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace View;

public abstract class Program
{
    public static async Task<int> Main(string[] args)
    {
        SetupConsole();
        
        // Dependency Injections
        var services = new ServiceCollection();
        services.AddSingleton<Patcher>();       // Patcher
        services.AddSingleton<UpdateManager>(); // UpdateManager
        
        var registrar = new Utils.TypeRegistrar(services);
        var app = new CommandApp(registrar);

        // Routes configuration
        app.Configure(config =>
        {
            config.SetApplicationName("VALORez");
            
            config.Settings.HelpProviderStyles = new HelpProviderStyle
            {
                Usage = new UsageStyle
                {
                    Header = "bold blue",
                    Options = "gray",
                    OptionalArgument = "darkblue",
                    RequiredArgument = "blue3",
                    Command = "bold skyblue1",
                    CurrentCommand = "bold skyblue1",
                },
                Description = new DescriptionStyle
                {
                    Header = "bold white",
                },
                Arguments = new ArgumentStyle
                {
                    Header = "bold blue3",
                },
                Options = new OptionStyle
                {
                    Header = "bold blue",
                },
                Commands = new CommandStyle
                {
                    Header = "bold blue",
                    ChildCommand = "bold skyblue1",
                }
            };
            
            config.SetApplicationVersion(Assembly.GetEntryAssembly()?.GetName().Name + " v" + Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3) ?? "1.0.0");
            
            
            config.AddCommand<PatchCommand>("patch")
                .WithDescription("Apply the true stretched resolution patch to VALORANT, with your desired custom resolution");
            
            config.AddCommand<RevertPatchCommand>("revert-patch")
                .WithDescription("Revert the previous applied patch");
            
            config.AddCommand<UpdateCommand>("update")
                .WithDescription("Update the application from GitHub server");

        });

        return await app.RunAsync(args);
    }
    
    private static void SetupConsole()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Console.OutputEncoding = Encoding.UTF8;
        SplashArt.Draw();
        CleanupUpdate();
    }
    
    private static void CleanupUpdate()
    {
        var currentExe = Process.GetCurrentProcess().MainModule?.FileName;
        var oldExe = currentExe + ".old";

        if (File.Exists(oldExe))
        {
            File.Delete(oldExe);
        }
    }
}
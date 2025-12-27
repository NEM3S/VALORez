using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Json;
using System.Reflection;
using Spectre.Console;
using Model.DTO;
using Model.Utils;

namespace Model;

public class UpdateManager
{
    private const string RepoOwner = "NEM3S";
    private const string RepoName  = "VALORez";
    
    private readonly HttpClient _httpClient = new() { 
        DefaultRequestHeaders = { { "User-Agent", "VALORez-Updater" } } 
    };

    /// <summary>
    /// Checks for updates and applies them if available.
    /// </summary>
    public async Task CheckAndApplyUpdateAsync()
    {
        GitHubRelease? release = null;
        Version? localVersion = Assembly.GetEntryAssembly()?.GetName().Version;
        Version? serverVersion = null;

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .StartAsync("Contacting GitHub...", async ctx =>
            {
                try
                {
                    // Fetch release info
                    var url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
                    release = await _httpClient.GetFromJsonAsync<GitHubRelease>(url);
                    
                    if (release?.TagName != null)
                    {
                        // Clean tag: "v1.0.0" -> "1.0.0"
                        var cleanTag = release.TagName.TrimStart('v');
                        serverVersion = new Version(cleanTag);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleWriter.PrintFailure("Update failed");
                    ConsoleWriter.PrintError($"{ex.Message}");
                }
            });

        // If check failed or no release found
        if (release == null || serverVersion == null) return;

        // Display version comparison
        AnsiConsole.Write(new Rule("[yellow]Version Check[/]").LeftJustified());
        AnsiConsole.MarkupLine($"Local Version : [blue]{localVersion?.ToString(3)}[/]");
        AnsiConsole.MarkupLine($"Latest Version: [green]{serverVersion}[/]");

        // Logic
        if (serverVersion > localVersion)
        {
#if DEBUG
            AnsiConsole.MarkupLine("[bold yellow]>> (DEV MODE) Update available but skipped in debug.[/]");
#else
            AnsiConsole.MarkupLine("[bold green]>> New update found![/]");
            await PerformUpdate(release); 
#endif
        }
        else
        {
            AnsiConsole.MarkupLine("[grey]>> You are using the latest version.[/]");
        }
        
        AnsiConsole.WriteLine(); // Spacing
    }

    private async Task PerformUpdate(GitHubRelease release)
    {
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .StartAsync("[yellow]Starting update process...[/]", async ctx =>
            {
                AnsiConsole.Write(new Rule("[tan]Perform Update[/]").LeftJustified());
                // Find ZIP asset
                var asset = release.Assets?.FirstOrDefault(a => a.Name.EndsWith(".zip"));
                if (asset == null)
                {
                    AnsiConsole.MarkupLine("[red]Error: No ZIP asset found in release.[/]");
                    return;
                }

                // Paths setup
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                var currentExe = Process.GetCurrentProcess().MainModule?.FileName ?? "VALORez.exe";
                var zipPath = Path.Combine(appPath, "update_temp.zip");
                var extractPath = Path.Combine(appPath, "update_temp_dir");

                // Download archive
                try
                {
                    ctx.Status("Downloading release...");
                    var zipBytes = await _httpClient.GetByteArrayAsync(asset.BrowserDownloadUrl);
                    await File.WriteAllBytesAsync(zipPath, zipBytes);
                    AnsiConsole.MarkupLine("[grey]LOG: Download finished.[/]");
                }
                catch (Exception ex)
                {
                    ConsoleWriter.PrintFailure("Download failed");
                    ConsoleWriter.PrintError($"{ex.Message}");
                }

                // Extract archive
                ctx.Status("Extracting files...");
                if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                
                var newExePath = Directory.GetFiles(extractPath, "VALORez.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (newExePath == null) throw new FileNotFoundException("VALORez.exe not found in extracted update.");
                AnsiConsole.MarkupLine("[grey]LOG: Extract finished.[/]");
                
                // Install + Swap
                ctx.Status("Installing update...");
                var oldExeName = currentExe + ".old";
                
                if (File.Exists(oldExeName)) File.Delete(oldExeName); 

                File.Move(currentExe, oldExeName);  // Rename current -> .old
                File.Move(newExePath, currentExe);  // Move new -> current
                
                AnsiConsole.MarkupLine("[grey]LOG: Install finished.[/]");

                // Cleanup
                ctx.Status("Cleaning up...");
                if (File.Exists(zipPath)) File.Delete(zipPath);
                if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                AnsiConsole.MarkupLine("[grey]LOG: Cleanup finished.[/]");
            });
        
        AnsiConsole.MarkupLine("[bold green]Update completed successfully![/]");
    }
}
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Json;
using System.Reflection;
using Model.DTO;
using Model.Utils;

namespace Model;

public class UpdateManager
{
    private const string RepoOwner = "NEM3S";
    private const string RepoName  = "VALORez";
    
    // Header User-Agent
    private readonly HttpClient _httpClient = new() { 
        DefaultRequestHeaders = { { "User-Agent", "VALORez-Updater" } } 
    };

    /// <summary>
    /// Update the application from GitHub server.
    /// </summary>
    public async Task CheckUpdateAsync()
    {
        Console.WriteLine("Checking for update...");

        try
        {
            // Get the latest release from GitHub
            var url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(url);

            if (release == null) return;
            
            // Local version
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            
            // Clean tag to get server version
            // Delete 'v': e.g. "v.1.2.3" -> "1.2.3"
            var serverVersionRaw = release.TagName?.TrimStart('v');
            if (serverVersionRaw != null)
            {
                var serverVersion = new Version(serverVersionRaw);

                Console.WriteLine(serverVersion > localVersion
                    ? $">> New version available (v{serverVersion}), use --update to perform update."
                    : $">> {Assembly.GetEntryAssembly()?.GetName().Name} is up to date.");
            }
        }
        catch (Exception)
        {
            ConsoleWriter.PrintFailure("Update checking failed");
            throw;
        }
    }
    
    /// <summary>
    /// Update the application from GitHub server.
    /// </summary>
    public async Task ApplyUpdateAsync()
    {
        try
        {
            // Get the latest release from GitHub
            var url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(url);

            if (release == null) return;
            
            // Local version
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            
            // Clean tag to get server version
            // Delete 'v': e.g. "v.1.2.3" -> "1.2.3"
            var serverVersionRaw = release.TagName?.TrimStart('v');
            if (serverVersionRaw != null)
            {
                var serverVersion = new Version(serverVersionRaw);
                if (serverVersion > localVersion)
                {
                    // Skipped update during development mode
#if DEBUG
                    Console.WriteLine(">> [DEV MODE] Debug mode: Update skipped.");
#else
                    Console.WriteLine(">> Downloading...");
                    Console.WriteLine($">> Current: v{localVersion}  ->  New : v{serverVersion}");
                    await PerformUpdate(release);
#endif
                }
                else
                {
                    Console.WriteLine($">> {Assembly.GetEntryAssembly()?.GetName().Name} is already up to date.");
                }
            }
        }
        catch (Exception)
        {
            ConsoleWriter.PrintFailure("Update failed");
            throw;
        }
    }

    private async Task PerformUpdate(GitHubRelease release)
    {   
        // Find .zip archive in release assets
        var asset = release.Assets?.FirstOrDefault(a => a.Name.EndsWith(".zip"));
        if (asset == null)
        {
            Console.WriteLine(">> No ZIP file was found.");
            return;
        }

        // Paths
        var appPath = AppDomain.CurrentDomain.BaseDirectory;
        var currentExe = Process.GetCurrentProcess().MainModule?.FileName ?? "VALORez.exe";
        var zipPath = Path.Combine(appPath, "update_temp.zip");
        var extractPath = Path.Combine(appPath, "update_temp_dir");

        // Download .zip archive
        var zipBytes = await _httpClient.GetByteArrayAsync(asset.BrowserDownloadUrl);
        await File.WriteAllBytesAsync(zipPath, zipBytes);

        // Extract archive
        if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        // Find executable in the extracted folder
        var newExePath = Directory.GetFiles(extractPath, "VALORez.exe", SearchOption.AllDirectories).FirstOrDefault();
        if (newExePath == null) throw new FileNotFoundException("VALORez.exe not found in extracted update.");

        // Rename & Swap
        // Windows is able to rename executables during execution.
        var oldExeName = currentExe + ".old";
        
        if (File.Exists(oldExeName)) File.Delete(oldExeName); // Cleaning

        File.Move(currentExe, oldExeName);  // 1. Renaming current to .old
        File.Move(newExePath, currentExe);  // 2. Replace with the new one

        Console.WriteLine(">> Update completed!");
        Console.WriteLine(">> You can now restart the CLI.");
        
        if (File.Exists(zipPath)) File.Delete(zipPath);
        if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
        
        Thread.Sleep(3000);
        
        // Restart
        Process.Start(currentExe);
        Environment.Exit(0);
    }
}
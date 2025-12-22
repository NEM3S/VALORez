using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Json;
using System.Reflection;
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

    public async Task CheckAndApplyUpdateAsync()
    {
        Console.WriteLine("Check for update...");

        try
        {
            // 1. Récupérer la dernière release depuis GitHub
            var url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(url);

            if (release == null) return;

            // 2. Nettoyer le tag (enlever le 'v') pour comparer
            // Votre YAML produit des tags comme "v1.0.0", mais AssemblyVersion est "1.0.0.0"
            var serverVersionRaw = release.tag_name.TrimStart('v'); 
            var serverVersion = new Version(serverVersionRaw);
            
            // Version locale (celle de l'image que vous avez fournie)
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;

            Console.WriteLine($">> Current Version : {localVersion?.ToString(3)}  |  New version : {serverVersion}");

            // Don't perform update while development.
            if (localVersion == new Version("0.0.0.0"))
            {
                Console.WriteLine(">> Skipped ( Development mode )");
            }
            else if (serverVersion > localVersion)
            {
                Console.WriteLine(">> New update found!  Downloading...");
                await PerformUpdate(release);
            }
            else
            {
                Console.WriteLine($">> {Assembly.GetEntryAssembly()?.GetName().Name} is already up to date.");
            }
        }
        catch (Exception ex)
        {
            ConsoleWriter.PrintFailure($"Update failed");
            throw;
        }
    }

    private async Task PerformUpdate(GitHubRelease release)
    {
        // Find .zip archive in release assets
        var asset = release.assets.FirstOrDefault(a => a.name.EndsWith(".zip"));
        if (asset == null)
        {
            Console.WriteLine(">> No ZIP file was found.");
            return;
        }

        // Paths
        var appPath = AppDomain.CurrentDomain.BaseDirectory;
        var currentExe = Process.GetCurrentProcess().MainModule?.FileName;
        var zipPath = Path.Combine(appPath, "update_temp.zip");
        var extractPath = Path.Combine(appPath, "update_temp_dir");

        try
        {
            // Download .zip archive
            var zipBytes = await _httpClient.GetByteArrayAsync(asset.browser_download_url);
            await File.WriteAllBytesAsync(zipPath, zipBytes);

            // Extract archive
            if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            // C. Trouver le nouvel exécutable (VALORez.exe) dans le dossier extrait
            // Votre YAML zippe le contenu, il faut trouver l'exe peu importe la structure
            var newExePath = Directory.GetFiles(extractPath, "VALORez.exe", SearchOption.AllDirectories).FirstOrDefault();

            if (newExePath == null) throw new FileNotFoundException("VALORez.exe not found in extracted update.");

            // D. LA MAGIE "RENAME & SWAP"
            // Windows permet de renommer un EXE en cours d'exécution, mais pas de le supprimer/écraser.
            var oldExeName = currentExe + ".old";
            
            if (File.Exists(oldExeName)) File.Delete(oldExeName); // Nettoyage préventif

            File.Move(currentExe, oldExeName);       // 1. On renomme l'actuel en .old
            File.Move(newExePath, currentExe);       // 2. On met le nouveau à la place

            Console.WriteLine(">> Update completed!");
            Console.WriteLine(">> Program will restart in few seconds.");
            
            if (File.Exists(zipPath)) File.Delete(zipPath);
            if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
            
            Thread.Sleep(3000);
            
            // E. Redémarrage automatique (Optionnel)
            Process.Start(currentExe);
            Environment.Exit(0);
        }
        catch
        {
            // ignored
        }
    }

    // Classes pour parser le JSON GitHub
    private class GitHubRelease
    {
        public string tag_name { get; set; }
        public List<GitHubAsset> assets { get; set; }
    }

    private class GitHubAsset
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }
    }
}
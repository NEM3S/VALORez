namespace Model.Utils;

using System.Text.RegularExpressions;


public static class ValorantRegionParser
{
    private static string? _redShooterGameLog;
    private static string RedShooterGameLog => _redShooterGameLog ??= ReadShooterGameLog();

    private static string ReadShooterGameLog()
    {
        // Build complete path %LOCALAPPDATA%
        string logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "VALORANT",
            "Saved",
            "Logs",
            "ShooterGame.log"
        );

        if (!File.Exists(logFilePath))
        {
            ConsoleWriter.PrintFailure("Log file reading failed");
            throw new FileNotFoundException("ShooterGame.log file not found", logFilePath);
        }

        // Read file content
        using var fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        return reader.ReadToEnd();
    }
    
    /// <summary>
    /// Parse region and shard from ShooterGame.log file.
    /// </summary>
    /// <returns>
    /// Tuple: (string, string)<br/>
    /// Item1 = region<br/>
    /// Item2 = shard
    /// </returns>
    public static (string region, string shard) ParseRegionAndShard()
    {
        var regex = new Regex(@"https:\/\/glz-(.+?)-1\.(.+?)\.a\.pvp\.net", RegexOptions.Compiled);

        var match = regex.Match(RedShooterGameLog);

        (string region, string shard) result = new();
        
        if (match is { Success: true, Groups.Count: >= 3 })
        {
            result.region = match.Groups[1].Value;
            result.shard = match.Groups[2].Value;
        }
        else
        {
            result.region = "none";
            result.shard = "none";
        }

        return result;
    }
}

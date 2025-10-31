using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.Exceptions;

namespace Model;

internal class PuuidPeeker
{
    private readonly string _pathRiotPrivateSettings = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData),
        @"Riot Games\Riot Client\Data\RiotGamesPrivateSettings.yaml");
    
    private readonly string _pathConfigFilesDir = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData),
        @"VALORANT\Saved\Config");

    private StreamReader _fileStream;
    
    /// <summary>
    /// Try to get puuid of the current logged user.
    /// </summary>
    /// <exception cref="FileNotFoundException">If RiotGamesPrivateSettings.yaml not found.</exception>
    /// <exception cref="SignedInException">If user is not signed in or forgot "stay sign in".</exception>
    /// <returns>
    /// success -> e.g.: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    /// </returns>
    private string FindPuuid()
    {
        string puuid = "unknown";
        if (!Path.Exists(_pathRiotPrivateSettings))
        {
            ConsoleWriter.PrintFailure("Riot Games local folder does not exist");
            throw new PrivateYamlNotFoundException();
        }
        using (_fileStream = new StreamReader(_pathRiotPrivateSettings))
        {
            while (_fileStream?.ReadLine() is { } line)
            {
                // Some magic numbers
                if (line.Contains("value: ") && line.Length >= 60 && line[60] == '"')
                {
                    puuid = line.Substring(24, 36);
                    ConsoleWriter.PrintSuccess($"PUUID has been found successfully in RiotGames private settings ({puuid})");
                }
            }
        }

        if (puuid == "unknown")
        {
            ConsoleWriter.PrintFailure("PUUID not found, please make sure you are logged in or you checked \"Stay signed in\"");
            throw new SignedInException();
        }
            
        return puuid;
    }

    /// <summary>
    /// Try to get puuid of the current logged user.
    /// </summary>
    /// <exception cref="FirstTimeLoggedInException">If this is a new account.</exception>
    /// <returns>The folder name of the user</returns>
    public string FindPuuidFolder()
    {
        string puuid = FindPuuid();
        List<string> folders = Directory.EnumerateDirectories(_pathConfigFilesDir).ToList();
        string? folder = folders.FirstOrDefault(f => f.Contains(puuid));

        if (string.IsNullOrWhiteSpace(folder))
        {
            ConsoleWriter.PrintFailure("Saved config folder not found, this may be the time playing on this account");
            throw new FirstTimeLoggedInException();
        }
        
        string result = Path.GetFileName(folder);
        ConsoleWriter.PrintSuccess($"Saved config folder found successfully ({Path.GetFileName(result)})");
        return result;
    }
}
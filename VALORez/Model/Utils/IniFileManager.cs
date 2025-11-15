using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Model.Utils;

internal class IniFileManager(string puuid)
{
    private const string CONFIG_END_PATH = @"WindowsClient\GameUserSettings.ini";
    
    private string Puuid => string.IsNullOrEmpty(puuid) ? "Unknown" : puuid;
    
    public string PathConfig => Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), $@"VALORANT\Saved\Config\{Puuid}\{CONFIG_END_PATH}");
    
    private readonly string? _exe = Assembly.GetExecutingAssembly().GetName().Name;

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(string? section, string? key, string? value, string filePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(string? section, string key, string @default, StringBuilder retVal, int size, string filePath);

    private string Read(string key, string? section = null)
    {
        var retVal = new StringBuilder(255);
        GetPrivateProfileString(section ?? _exe, key, "", retVal, 255, PathConfig);
        return retVal.ToString();
    }

    public void Write(string? key, string? value, string? section = null)
    {
        WritePrivateProfileString(section ?? _exe, key, value, PathConfig);
    }

    public void DeleteKey(string? key, string? section = null)
    {
        Write(key, null, section ?? _exe);
    }

    public void DeleteSection(string? section = null)
    {
        Write(null, null, section ?? _exe);
    }

    public bool KeyExists(string key, string? section = null)
    {
        return section != null && Read(key, section).Length > 0;
    }

    public void EnableReadOnly()
    {
        if (Path.Exists(PathConfig))
        {
            File.SetAttributes(PathConfig, FileAttributes.ReadOnly);
            ConsoleWriter.PrintInfo($"{Path.GetFileName(PathConfig)} is now read-only");
        }
        else
        {
            ConsoleWriter.PrintFailure("Config file does not exist");
            throw new FileNotFoundException();
        }
            
    }
    
    public void DisableReadOnly()
    {
        if (Path.Exists(PathConfig))
        {
            File.SetAttributes(PathConfig, File.GetAttributes(PathConfig) & ~FileAttributes.ReadOnly);
            ConsoleWriter.PrintInfo($"{Path.GetFileName(PathConfig)}'s read-only attribute has been deleted");
        }
        else
        {
            ConsoleWriter.PrintFailure("Config file does not exist");
            throw new FileNotFoundException();
        }
    }

    public void DeleteFile()
    {
        if (Path.Exists(PathConfig))
        {
            File.Delete(PathConfig);
            ConsoleWriter.PrintInfo($"{Path.GetFileName(PathConfig)} has been deleted");
        }
        else
        {
            ConsoleWriter.PrintFailure("Config file does not exist");
            throw new FileNotFoundException();
        }
    }
}
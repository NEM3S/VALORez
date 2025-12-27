using Model.Exceptions;
using Model.Utils;

namespace Model;

public class Patcher
{
    private IniFileManager? _iniFileManager;
    private ConfigSaverService? _consoleSaver;
    private readonly string _mainSection = "/Script/ShooterGame.ShooterGameUserSettings";
    
    public void ApplyPatch(int width, int height)
    {
        var keys = new Dictionary<string, string>
        {
            { "bshouldletterbox", "False" },
            { "blastconfirmedshouldletterbox", "False" },
            { "resolutionsizex", width.ToString() },
            { "resolutionsizey", height.ToString() },
            { "lastuserconfirmedresolutionsizex", width.ToString() },
            { "lastuserconfirmedresolutionsizey", height.ToString() },
            { "windowposx", "0" },
            { "windowposy", "0" },
            { "lastconfirmedfullscreenmode", "2" },
            { "preferredfullscreenmode", "1" },
            { "fullscreenmode", "2" }
        };

        try
        {
            PuuidPeeker puuidPeeker = new PuuidPeeker();
            _iniFileManager = new IniFileManager(puuidPeeker.FindPuuidFolder());
            _consoleSaver = new ConfigSaverService(_iniFileManager.PathConfig);
            _consoleSaver.Save();

            _iniFileManager.DisableReadOnly();
            foreach (var key in keys)
            {
                if (_iniFileManager.KeyExists(key.Key, _mainSection))
                {
                    _iniFileManager.Write(key.Key, key.Value, _mainSection);
                    ConsoleWriter.PrintSuccess($"\"{key.Key}\" key has been set on {key.Value} successfully");
                }
                else
                {
                    _iniFileManager.Write(key.Key, key.Value, _mainSection);
                    ConsoleWriter.PrintInfo($"\"{key.Key}\" does not exist, created and set on {key.Value}");
                }
            }

            _iniFileManager.EnableReadOnly();

            ConsoleWriter.PrintSuccess($"Patch completed for resolution {width}x{height}");
        }
        catch (ValorezException)
        {
            // ignored
        }
        catch (Exception e)
        {
            ConsoleWriter.PrintError(e.ToString());
        }
    }

    public void Revert()
    {
        try
        {
            PuuidPeeker puuidPeeker = new PuuidPeeker();
            _iniFileManager = new IniFileManager(puuidPeeker.FindPuuidFolder());
            _consoleSaver = new ConfigSaverService(_iniFileManager.PathConfig);

            if (_consoleSaver.BackupExists())
            {
                _iniFileManager.DisableReadOnly();
                _iniFileManager.DeleteFile();
            }
            _consoleSaver.Revert();
        }
        catch (ValorezException)
        {
            // Nothing to do...
        }
        catch (Exception e)
        {
            ConsoleWriter.PrintError(e.ToString());
        }
    }
}
using Model.Exceptions;
using Model.Utils;

namespace Model;

public class Patcher
{
    private IniFileManager? _iniFileManager;

    private readonly string _mainSection = "/Script/ShooterGame.ShooterGameUserSettings";
    private readonly Dictionary<string, string> _keys;

    public Patcher(int width = 1280, int height = 720)
    {
        _keys = new()
        { 
            {"bshouldletterbox", "False"},
            {"blastconfirmedshouldletterbox", "False"},
            {"resolutionsizex", width.ToString()},
            {"resolutionsizey", height.ToString()},
            {"lastuserconfirmedresolutionsizex", width.ToString()},
            {"lastuserconfirmedresolutionsizey", height.ToString()},
            {"windowposx", "0"},
            {"windowposy", "0"},
            {"lastconfirmedfullscreenmode", "2"},
            {"preferredfullscreenmode", "1"},
            {"fullscreenmode", "2"}
        };
    }

    public void Initialize()
    {
        try
        {
            PuuidPeeker puuidPeeker = new PuuidPeeker();
            _iniFileManager = new IniFileManager(puuidPeeker.FindPuuidFolder());
            
            _iniFileManager.DisableReadOnly();
            foreach (var key in _keys)
            {
                if (_iniFileManager.KeyExists(key.Key, _mainSection))
                {
                    _iniFileManager.Write(key.Key, key.Value, _mainSection);
                    ConsoleWriter.PrintSuccess($"\"{key.Key}\" key has been set on {key.Value} successfully");
                }
                else
                {
                    _iniFileManager.Write(key.Key, key.Value, _mainSection);
                    ConsoleWriter.PrintInfo($"\"{key.Key}\" does not exist, so the key has been created and set on {key.Value} successfully");
                }
            }
            _iniFileManager.EnableReadOnly();
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

    public void Reset()
    {
        try
        {
            PuuidPeeker puuidPeeker = new PuuidPeeker();
            _iniFileManager = new IniFileManager(puuidPeeker.FindPuuidFolder());
            
            _iniFileManager.DisableReadOnly();
            _iniFileManager.DeleteFile();
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
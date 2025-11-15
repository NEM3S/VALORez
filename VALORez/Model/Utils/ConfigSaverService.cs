namespace Model.Utils;

internal class ConfigSaverService(string path)
{
    private bool Check()
    {
        if (!File.Exists(path))
        {
            ConsoleWriter.PrintFailure("Check config failed");
            throw new FileNotFoundException("Cannot save because the file does not exist");
        }
            
        return !File.Exists(path + ".bak");
    }

    public bool BackupExists()
    {
        return File.Exists(path + ".bak");
    }

    public void Save()
    {
        if (Check())
        {
            File.Copy(path, path+".bak");
            ConsoleWriter.PrintSuccess("Backup config does not exist, so the current config was copied successfully");
        }
        else
        {
            ConsoleWriter.PrintInfo("An existing backup file was found. Use --revert option if you want to save again");
        }
    }

    public void Revert(bool force = false)
    {
        if (BackupExists())
        {
            ConsoleWriter.PrintSuccess("Revert done successfully");
            File.Move(path + ".bak", path, force);
        }
        else
        {
            ConsoleWriter.PrintFailure("Revert failed, no backup was found");
        }
    }
}
# VALORez

> **Status:** Working and fully functional as of **November 2025**.

**VALORez** is a lightweight console tool built with **.NET 8 (C#)** that allows you to **patch VALORANT** to enable **stretched resolutions**.

This one uses a **safe method** that doesn’t require uninstalling or modifying your display drivers.

## Requirements

- Windows 10 or later
- .NET 8 Runtime ([download here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))  
- VALORANT installed

## Installation

1. Download the [*latest release*](https://github.com/NEM3S/VALORez/releases/latest).
2. Extract the `.exe` file.

## Usage
---
### Setup
Run the program from the command line with your desired stretched resolution, for example:  
```bash
VALORez.exe 1024x1080
```
If no resolution is specified, the default one is 1280x720.

Once the patch is applied:
- Make sure to set the same resolution on your monitor before launching VALORANT.
- ⚠️ Important: Your GPU’s Scaling Mode must be set to Full-screen for the stretched resolution to take effect.
---
### Uninstalling
To revert the patch, simply run the program with the `--reset` option:
```bash
VALORez.exe --reset
```
This will delete your VALORANT configuration file, which will automatically be recreated the next time you launch the game.
After that, you’ll just need to reapply your usual graphics settings in VALORANT.

> I’m currently working on a better solution to avoid losing user settings during reset.

## Disclaimer

This project is not affiliated with **Riot Games**.

Use at your own risk. Patching game files may violate the Terms of Service — ensure you understand the risks before proceeding.
> **Note:** As of today, no bans have been reported from users of VALORez.

## Credits

Passionately created by Mathis ❤️
Thanks to all my friends for their constant support.

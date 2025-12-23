<div align="center">
  <h1 align="center">VALORez</h1>
  
  [![C#](https://img.shields.io/badge/language-C%23-green.svg)](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))
  [![dotnet](https://img.shields.io/badge/Core-8.0-purple.svg?logo=dotnet)]([https://en.wikipedia.org/wiki/C_Sharp_(programming_language)](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.415-windows-x64-installer))
  [![Publish](https://img.shields.io/github/actions/workflow/status/NEM3S/VALORez/ci.yml)](https://github.com/NEM3S/VALORez/actions/workflows/ci.yml)
  [![Download](https://img.shields.io/github/downloads/NEM3S/VALORez/total)](https://github.com/NEM3S/VALORez/releases)
  [![Latest](https://img.shields.io/github/v/release/NEM3S/VALORez?label=latest)](https://github.com/NEM3S/VALORez/releases/latest)

![LOGO](valorez.gif)
</div>


> **Status:** Fully functional and operational from **December 2025**.

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
### Setup
Run the program from the command line with your desired stretched resolution, for example:  
```bash
VALORez.exe 1280x1024
```
If no resolution is specified, the default one is 1280x720.

Once the patch is applied, make sure to set the same resolution on your monitor before launching VALORANT.

> [!IMPORTANT]
> Your GPU’s Scaling Mode must be set to Full-screen for the stretched resolution to take effect.

---
### Uninstalling
To revert the patch, simply run the program with the `--revert` option:
```bash
VALORez.exe --revert
```
This restores your original configuration file, which was saved when the patch was first applied.
This ensures your initial settings are brought back safely.

## Disclaimer

This project is not affiliated with **Riot Games**.

Use at your own risk. Patching game files may violate the Terms of Service — ensure you understand the risks before proceeding.
> [!NOTE]
> As of today, no bans have been reported from users using stretched resolution.

## Credits

Passionately created by _**Mathis**_ ❤️
Thanks to my gf and all my friends for their constant support.

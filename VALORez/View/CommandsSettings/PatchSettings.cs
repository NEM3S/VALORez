using System.ComponentModel;
using Spectre.Console.Cli;

namespace View.CommandsSettings;

public class PatchSettings : CommandSettings
{
    [CommandArgument(0, "<resolution>")]
    [Description("Target resolution. (e.g: 1280x1024)")]
    public string Resolution { get; init; } = string.Empty;
}
using Model;
using Spectre.Console;
using Spectre.Console.Cli;
using View.CommandsSettings;

namespace View.Commands;

public class PatchCommand : Command<PatchSettings>
{
    private readonly Patcher _patcher;

    public PatchCommand(Patcher patcher)
    {
        _patcher = patcher;
    }

    protected override int Execute(CommandContext context, PatchSettings settings, CancellationToken cancellationToken)
    {
        if (!settings.Resolution.ToLower().Contains("x"))
        {
            AnsiConsole.MarkupLine("[red]Error[/] : Invalid resolution format. Expected format \"WIDTHxHEIGHT\".");
            return 1;
        }

        var parts = settings.Resolution.ToLower().Split('x');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int w) || !int.TryParse(parts[1], out int h))
        {
            AnsiConsole.MarkupLine("[red]Error[/] : Dimensions must be integers. Expected format \"WIDTHxHEIGHT\".");
            return 1;
        }

        _patcher.ApplyPatch(w, h);
        return 0;
    }
}
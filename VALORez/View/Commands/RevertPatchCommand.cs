using Model;
using Spectre.Console;
using Spectre.Console.Cli;
using View.CommandsSettings;

namespace View.Commands;

public class RevertPatchCommand : Command
{
    private readonly Patcher _patcher;

    public RevertPatchCommand(Patcher patcher)
    {
        _patcher = patcher;
    }
    
    protected override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        _patcher.Revert();
        return 0;
    }
}
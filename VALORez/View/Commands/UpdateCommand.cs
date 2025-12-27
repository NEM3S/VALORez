using Model;
using Spectre.Console.Cli;

namespace View.Commands;

public class UpdateCommand : AsyncCommand
{
    private readonly UpdateManager _updateManager;

    public UpdateCommand(UpdateManager manager)
    {
        _updateManager = manager;
    }
    
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        // Delegate the visual process to the manager
        await _updateManager.CheckAndApplyUpdateAsync();
        return 0;
    }
}
using Model.Utils;
using View.Commands;

namespace View;

/// <summary>
/// Invoker for the command
/// </summary>
public class StartupInvoker
{
    private IConsoleCommand? _command;

    /// <summary>
    /// Set the command to run
    /// </summary>
    /// <param name="command">Command to run</param>
    public void SetCommand(IConsoleCommand command)
    {
        _command = command;
    }

    /// <summary>
    /// Run the command
    /// </summary>
    public async Task Run()
    {
        if (_command != null)
        {
            await _command.Execute();
        }
        else
        {
            ConsoleWriter.PrintFailure("No command set!");
        }
    }
}
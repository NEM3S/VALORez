namespace View.Commands;

/// <summary>
/// Interface for the command
/// </summary>
public interface IConsoleCommand
{
    /// <summary>
    /// Logic to execute
    /// </summary>
    Task Execute();
}
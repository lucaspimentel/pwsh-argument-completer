namespace PowerShellArgumentCompleter.Completions;

/// <summary>
/// Represents an alias for a CommandParameter. This allows both the full parameter name
/// and its short alias to appear in completion results.
/// </summary>
public sealed class AliasCompletion(string alias, CommandParameter parameter) : ICompletion
{
    public string CompletionText { get; } = alias;
    public string? Tooltip { get; } = parameter.Tooltip;

    public ICompletion? FindNode(ReadOnlySpan<char> wordToComplete)
    {
        // Delegate to the underlying parameter
        return parameter.FindNode(wordToComplete);
    }

    public override string ToString()
    {
        return CompletionText;
    }
}

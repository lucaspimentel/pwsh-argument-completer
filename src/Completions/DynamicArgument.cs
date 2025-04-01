using System.Diagnostics;

namespace PowerShellArgumentCompleter.Completions;

[DebuggerDisplay("{CompletionText,nq}")]
public sealed class DynamicArgument(string name, string? tooltip = null, string? displayText = null)
    : ICompletion
{
    public string CompletionText { get; } = name;
    public string? DisplayText { get; } = displayText;
    public string? Tooltip { get; } = tooltip;

    public ICompletion? FindNode(ReadOnlySpan<char> wordToComplete)
    {
        return Helpers.ThisIfEquals(this, wordToComplete);
    }

    public override string ToString()
    {
        return CompletionText;
    }
}

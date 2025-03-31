using System.Diagnostics;

namespace PowerShellArgumentCompleter.Completions;

[DebuggerDisplay("{Name,nq}")]
public sealed class StaticArgument(string name) : ICompletion
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }

    public ICompletion? FindNode(ReadOnlySpan<char> wordToComplete)
    {
        return Helpers.ThisIfEquals(this, wordToComplete);
    }

    public override string ToString()
    {
        return Name;
    }
}

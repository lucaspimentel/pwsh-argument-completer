namespace PowerShellArgumentCompleter.Completions;

public sealed class CommandParameter(string name, string? tooltip = null) : ICompletionWithChildren
{
    public string Name { get; } = name;
    public string? Tooltip { get; } = tooltip;
    public StaticArgument[] StaticArguments { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public ICompletion? FindNode(ReadOnlySpan<char> wordToComplete)
    {
        var completion = Helpers.ThisIfEquals(this, wordToComplete) ??
                         Helpers.FindEquals(StaticArguments, wordToComplete);

        if (completion is null && DynamicArguments?.Invoke() is { } arguments)
        {
            completion = Helpers.FindEquals(arguments, wordToComplete);
        }

        return completion;
    }

    public List<ICompletion> GetCompletions(ReadOnlySpan<char> wordToComplete)
    {
        var results = new List<ICompletion>();

        Helpers.AddWhereStartsWith(StaticArguments, results, wordToComplete);

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            Helpers.AddWhereStartsWith(arguments, results, wordToComplete);
        }

        return results;
    }

    public override string ToString()
    {
        return Name;
    }
}

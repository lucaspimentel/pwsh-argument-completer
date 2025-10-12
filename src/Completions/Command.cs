namespace PowerShellArgumentCompleter.Completions;

public delegate IEnumerable<DynamicArgument> DynamicArgumentsFactory();

public sealed class Command(string completionText, string? tooltip = null)
    : ICompletionWithChildren
{
    public string CompletionText { get; } = completionText;
    public string? Tooltip { get; } = tooltip;
    public Command[] SubCommands { get; init; } = [];
    public CommandParameter[] Parameters { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public ICompletion? FindNode(ReadOnlySpan<char> wordToComplete)
    {
        var completion = Helpers.ThisIfEquals(this, wordToComplete) ??
                         Helpers.FindEquals(SubCommands, wordToComplete) ??
                         Helpers.FindEquals(Parameters, wordToComplete);

        if (completion is null && DynamicArguments?.Invoke() is { } arguments)
        {
            completion = Helpers.FindEquals(arguments, wordToComplete);
        }

        return completion;
    }

    public List<ICompletion> GetCompletions(ReadOnlySpan<char> wordToComplete)
    {
        var results = new List<ICompletion>();

        Helpers.AddWhereStartsWith(SubCommands, results, wordToComplete);
        Helpers.AddWhereStartsWith(Parameters, results, wordToComplete);

        // Add alias completions for parameters that have aliases
        foreach (var param in Parameters)
        {
            if (param.Alias is not null && Helpers.StartsWith(param.Alias, wordToComplete))
            {
                results.Add(new AliasCompletion(param.Alias, param));
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            Helpers.AddWhereStartsWith(arguments, results, wordToComplete);
        }

        return results;
    }

    public override string ToString()
    {
        return CompletionText;
    }
}

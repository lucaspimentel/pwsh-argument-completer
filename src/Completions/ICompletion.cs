namespace PowerShellArgumentCompleter.Completions;

public interface ICompletion
{
    string Name { get; }
    string? Tooltip { get; }
    ICompletion? FindNode(ReadOnlySpan<char> wordToComplete);
}

public interface ICompletionWithChildren : ICompletion
{
    List<ICompletion> GetCompletions(ReadOnlySpan<char> wordToComplete);
}

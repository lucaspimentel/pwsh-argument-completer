namespace PowerShellArgumentCompleter;

public interface ISegment
{
    string Name { get; }
    string? Tooltip { get; }
}

public interface ISegmentWithPredictions : ISegment
{
    List<ISegment> GetPredictions(ReadOnlySpan<char> wordToComplete);
}

public sealed class Command(string name) : ISegmentWithPredictions
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
    public Command[] SubCommands { get; init; } = [];
    public CommandParameter[] Parameters { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public ISegment Find(ReadOnlySpan<char> segment)
    {
        Logger.Debug($"Searching for \"{segment}\"");
        segment = segment.Trim();

        if (segment.Length == 0 || segment.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
            Logger.Debug($"Returning root command for \"{Name}\".");
            return this;
        }

        foreach (var command in SubCommands)
        {
            if (command.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug($"Found \"{command.Name}\"");
                return command;
            }
        }

        foreach (var parameter in Parameters)
        {
            if (parameter.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug($"Found \"{parameter.Name}\"");
                return parameter;
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Debug($"Found \"{argument.Name}\"");
                    return argument;
                }
            }
        }

        Logger.Debug($"\"{segment}\" not found. Returning root command.");
        return this;
    }

    public List<ISegment> GetPredictions(ReadOnlySpan<char> wordToComplete)
    {
        var results = new List<ISegment>();
        wordToComplete = wordToComplete.Trim();

        foreach (var command in SubCommands)
        {
            if (wordToComplete.Length == 0 || wordToComplete.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(command);
            }
        }

        foreach (var parameter in Parameters)
        {
            if (wordToComplete.Length == 0 || wordToComplete.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(parameter);
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            foreach (var argument in arguments)
            {
                if (wordToComplete.Length == 0 || wordToComplete.Equals(argument.Name, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(argument);
                }
            }
        }

        return results;
    }
}

public sealed class CommandParameter(string name) : ISegmentWithPredictions
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
    public StaticArgument[] StaticArguments { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public List<ISegment> GetPredictions(ReadOnlySpan<char> wordToComplete)
    {
        var results = new List<ISegment>();

        foreach (var argument in StaticArguments)
        {
            if (wordToComplete.Equals(argument.Name, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(argument);
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            foreach (var argument in arguments)
            {
                if (wordToComplete.Equals(argument.Name, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(argument);
                }
            }
        }

        return results;
    }
}

public sealed class StaticArgument(string name) : ISegment
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
}

public sealed class DynamicArgument(string name) : ISegment
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
}

public delegate DynamicArgument[] DynamicArgumentsFactory();

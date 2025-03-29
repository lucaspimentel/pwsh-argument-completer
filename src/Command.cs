using System.Diagnostics;

namespace PowerShellArgumentCompleter;

public interface ISegment
{
    string Name { get; }
    string? Tooltip { get; }
}

public interface ISegmentWithPredictions : ISegment
{
    List<ISegment> GetCompletion(ReadOnlySpan<char> wordToComplete);
}

public sealed class Command(string name) : ISegmentWithPredictions
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
    public Command[] SubCommands { get; init; } = [];
    public CommandParameter[] Parameters { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public ISegment? Find(ReadOnlySpan<char> segment)
    {
        Logger.Debug($"Searching for \"{segment}\"");
        segment = segment.Trim();

        if (segment.Length == 0 || Name.AsSpan().Equals(segment, StringComparison.OrdinalIgnoreCase))
        {
            Logger.Debug($"Returning root command for \"{Name}\".");
            return this;
        }

        foreach (var command in SubCommands)
        {
            if (command.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug($"Found subcommand \"{command.Name}\"");
                return command;
            }
        }

        foreach (var parameter in Parameters)
        {
            if (parameter.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug($"Found parameter \"{parameter.Name}\"");
                return parameter;
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument.Name.AsSpan().StartsWith(segment, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Debug($"Found dynamic argument \"{argument.Name}\"");
                    return argument;
                }
            }
        }

        Logger.Debug($"\"{segment}\" not found.");
        return null;
    }

    public List<ISegment> GetCompletion(ReadOnlySpan<char> wordToComplete)
    {
        var results = new List<ISegment>();
        wordToComplete = wordToComplete.Trim();

        if (wordToComplete.Length != 0 && Name.AsSpan().StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
        {
            results.Add(this);
        }

        foreach (var command in SubCommands)
        {
            if (wordToComplete.Length == 0 || command.Name.AsSpan().StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(command);
            }
        }

        foreach (var parameter in Parameters)
        {
            if (wordToComplete.Length == 0 || parameter.Name.AsSpan().StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(parameter);
            }
        }

        if (DynamicArguments?.Invoke() is { } arguments)
        {
            foreach (var argument in arguments)
            {
                if (wordToComplete.Length == 0 || argument.Name.AsSpan().StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(argument);
                }
            }
        }

        return results;
    }

    public override string ToString()
    {
        return Name;
    }
}

public sealed class CommandParameter(string name) : ISegmentWithPredictions
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }
    public StaticArgument[] StaticArguments { get; init; } = [];
    public DynamicArgumentsFactory? DynamicArguments { get; init; }

    public List<ISegment> GetCompletion(ReadOnlySpan<char> wordToComplete)
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

    public override string ToString()
    {
        return Name;
    }
}

[DebuggerDisplay("{Name,nq}")]
public sealed class StaticArgument(string name) : ISegment
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }

    public override string ToString()
    {
        return Name;
    }
}

[DebuggerDisplay("{Name,nq}")]
public sealed class DynamicArgument(string name) : ISegment
{
    public string Name { get; } = name;
    public string? Tooltip { get; init; }

    public override string ToString()
    {
        return Name;
    }
}

public delegate DynamicArgument[] DynamicArgumentsFactory();

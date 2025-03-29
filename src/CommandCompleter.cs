using PowerShellArgumentCompleter.Commands;

namespace PowerShellArgumentCompleter;

public static class CommandCompleter
{
    public static List<ISegment> GetCompletions(ReadOnlySpan<char> commandLine)
    {
        if (commandLine.StartsWith("scoop", StringComparison.OrdinalIgnoreCase))
        {
            var scoopCommand = ScoopCommand.Create();
            var argumentEnumerator = commandLine.Split(' ');
            ISegment? segment = null;
            ReadOnlySpan<char> lastArgument = default;

            // Skip the first argument (the command name)
            argumentEnumerator.MoveNext();

            while (argumentEnumerator.MoveNext())
            {
                lastArgument = commandLine[argumentEnumerator.Current];
                Logger.Debug($"Enumerating. Current: {lastArgument}");

                segment = scoopCommand.Find(lastArgument);

                if (segment is null)
                {
                    Logger.Debug($"No segment found for \"{lastArgument}\"");
                    break;
                }
            }

            segment ??= scoopCommand;

            var completions = segment switch
            {
                ISegmentWithPredictions swp => swp.GetCompletion(lastArgument),
                _ => [segment]
            };

            return completions;
        }

        return [];
    }
}

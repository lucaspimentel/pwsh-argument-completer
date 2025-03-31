using PowerShellArgumentCompleter.Completions;
using PowerShellArgumentCompleter.KnownCompletions;

namespace PowerShellArgumentCompleter;

public static class CommandCompleter
{
    public static List<ICompletion> GetCompletions(ReadOnlySpan<char> commandLine)
    {
        ICompletion? currentCompletion;

        if (Helpers.StartsWith(commandLine, "scoop"))
        {
            currentCompletion = ScoopCommand.Create();
        }
        else
        {
            return [];
        }

        ReadOnlySpan<char> currentArgument = default;

        // Split the command line and skip the first argument
        var argumentEnumerator = commandLine.Split(' ');
        argumentEnumerator.MoveNext();

        while (argumentEnumerator.MoveNext())
        {
            currentArgument = commandLine[argumentEnumerator.Current].Trim();
            Logger.Debug($"Searching for \"{currentArgument}\"");

            if (currentCompletion.FindNode(currentArgument) is { } node)
            {
                Logger.Debug($"Found node \"{node}\"");
                currentCompletion = node;
                currentArgument = default;
            }
            else
            {
                Logger.Debug($"Did not find \"{currentArgument}\"");
                break;
            }
        }

        return currentCompletion switch
        {
            ICompletionWithChildren cwc => cwc.GetCompletions(currentArgument),
            not null => [currentCompletion],
            null => []
        };
    }
}

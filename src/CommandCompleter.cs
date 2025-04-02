using PowerShellArgumentCompleter.Completions;
using PowerShellArgumentCompleter.KnownCompletions;
using PowerShellArgumentCompleter.KnownCompletions.Azure;

namespace PowerShellArgumentCompleter;

public static class CommandCompleter
{
    public static IEnumerable<ICompletion> GetCompletions(ReadOnlySpan<char> commandLine)
    {
        ICompletion? currentCompletion;

        if (Helpers.StartsWith(commandLine, "scoop"))
        {
            currentCompletion = ScoopCommand.Create();
        }
        else if (Helpers.StartsWith(commandLine, "winget"))
        {
            currentCompletion = WingetCommand.Create();
        }
        else if (Helpers.StartsWith(commandLine, "code"))
        {
            currentCompletion = VsCodeCommand.Create();
        }
        else if (Helpers.StartsWith(commandLine, "az"))
        {
            currentCompletion = AzCommand.Create();
        }
        else if (Helpers.StartsWith(commandLine, "azd"))
        {
            currentCompletion = AzdCommand.Create();
        }
        else if (Helpers.StartsWith(commandLine, "func"))
        {
            currentCompletion = FuncCommand.Create();
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

            if (currentCompletion.FindNode(currentArgument) is { } node)
            {
                currentCompletion = node;
                currentArgument = default;
            }
            else
            {
                break;
            }
        }

        return currentCompletion switch
        {
            ICompletionWithChildren cwc => cwc.GetCompletions(currentArgument).OrderBy(c => c.CompletionText),
            not null => [currentCompletion],
            null => []
        };
    }
}

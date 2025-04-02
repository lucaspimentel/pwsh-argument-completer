using PowerShellArgumentCompleter.Completions;
using PowerShellArgumentCompleter.KnownCompletions;
using PowerShellArgumentCompleter.KnownCompletions.Azure;

namespace PowerShellArgumentCompleter;

public static class CommandCompleter
{
    public static IEnumerable<ICompletion> GetCompletions(ReadOnlySpan<char> commandLine)
    {
        var index = commandLine.IndexOf(' ');

        if (index == -1)
        {
            index = commandLine.Length;
        }

        Span<char> mainCommand = stackalloc char[index];
        commandLine[..index].ToLowerInvariant(mainCommand);

        ICompletion? currentCompletion = mainCommand switch
        {
            "scoop" => ScoopCommand.Create(),
            "winget" => WingetCommand.Create(),
            "code" => VsCodeCommand.Create(),
            "azd" => AzdCommand.Create(),
            "az" => AzCommand.Create(),
            "func" => FuncCommand.Create(),
            _ => null
        };

        if (currentCompletion is null)
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

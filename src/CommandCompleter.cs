using PowerShellArgumentCompleter.Completions;
using PowerShellArgumentCompleter.KnownCompletions;
using PowerShellArgumentCompleter.KnownCompletions.Azure;

namespace PowerShellArgumentCompleter;

public static class CommandCompleter
{
    public static IEnumerable<ICompletion> GetCompletions(ReadOnlySpan<char> commandLine)
    {
        var length = GetCommandLength(commandLine);

        if (length < 0 || length > commandLine.Length)
        {
            Logger.Write($"commandLine: {commandLine}, length: {length} is invalid");
            return [];
        }

        Span<char> mainCommand = stackalloc char[length];
        commandLine[..length].ToLowerInvariant(mainCommand);

        ICompletion? currentCompletion = mainCommand switch
        {
            "scoop" => ScoopCommand.Create(),
            "code" => VsCodeCommand.Create(),
            "winget" => WingetCommand.Create(),
            "chezmoi" => ChezmoiCommand.Create(),
            "git" => GitCommand.Create(),
            "gh" => GhCommand.Create(),
            "tre" => TreCommand.Create(),
            "lsd" => LsdCommand.Create(),
            "dust" => DustCommand.Create(),
            "azd" => AzdCommand.Create(),
            "az" => AzCommand.Create(),
            "func" => FuncCommand.Create(),
            _ => null
        };

        if (currentCompletion is null)
        {
            Logger.Write($"{mainCommand} is not a known command");
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

    private static int GetCommandLength(ReadOnlySpan<char> commandLine)
    {
        var exeIndex = commandLine.IndexOf(".exe");

        if (exeIndex > 0)
        {
            return exeIndex;
        }

        var spaceIndex = commandLine.IndexOf(' ');

        if (spaceIndex > 0)
        {
            return spaceIndex;
        }

        return commandLine.Length;
    }
}

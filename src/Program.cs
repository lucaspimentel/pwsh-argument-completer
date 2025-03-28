using PowerShellArgumentCompleter.Commands;

namespace PowerShellArgumentCompleter;

// https://learn.microsoft.com/en-us/powershell/scripting/learn/shell/tab-completion
// https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/register-argumentcompleter
// https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/tabexpansion2
// https://techcommunity.microsoft.com/blog/itopstalkblog/autocomplete-in-powershell/2604524
// https://github.com/abgox/PSCompletions/blob/main/completions/scoop/language/en-US.json
/*
$scriptblock = {
    param($wordToComplete, $commandAst, $cursorPosition)

    dotnet complete --position $cursorPosition $commandAst.ToString() | ForEach-Object {
        [System.Management.Automation.CompletionResult]::new(
            $_,               # completionText
            $_,               # listItemText
            'ParameterValue', # resultType
            $_                # toolTip
        )
    }
}

Register-ArgumentCompleter -Native -CommandName dotnet -ScriptBlock $scriptblock
*/

internal static class Program
{
    public static void Main(string[] args)
    {
        var allArgs = string.Join(" ", args);
        Logger.Debug($"Received args: {allArgs}");

        // var wordToComplete = args[0];
        var commandAst = args[1];
        var cursorPosition = int.Parse(args[2]);

        if (commandAst.StartsWith("scoop", StringComparison.OrdinalIgnoreCase))
        {
            var command = ScoopCommand.Create();
            var argumentEnumerator = commandAst.AsSpan().Split(' ');
            ISegment? segment = null;
            ReadOnlySpan<char> lastArgument = default;

            // Skip the first argument (the command name)
            argumentEnumerator.MoveNext();

            while (argumentEnumerator.MoveNext())
            {
                lastArgument = commandAst.AsSpan(argumentEnumerator.Current);

                Logger.Debug($"Enumerating. Current: {argumentEnumerator.Current}");

                segment = command.Find(lastArgument);
            }

            if (segment is ISegmentWithPredictions swp)
            {
                var predictions = swp.GetPredictions(lastArgument);

                foreach (var prediction in predictions)
                {
                    // completionText|listItemText|toolTip
                    Console.WriteLine($"{prediction.Name}|{prediction.Name}|{prediction.Tooltip}");
                }
            }
            else if (segment is not null)
            {
                // completionText|listItemText|toolTip
                Console.WriteLine($"{segment.Name}|{segment.Name}|{segment.Tooltip}");
            }
        }
    }
}

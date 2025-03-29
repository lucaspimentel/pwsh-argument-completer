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
        try
        {
            var allArgs = string.Join(", ", args.Select(a => $"\"{a}\""));
            Logger.Debug($"Received args: {allArgs}");

            // var wordToComplete = args[0];
            var cursorPosition = int.Parse(args[2]);
            var commandAst = args[1].AsSpan(0, Math.Max(0, cursorPosition - 1));

            var completions = CommandCompleter.GetCompletions(commandAst);

            foreach (var prediction in completions)
            {
                // completionText|listItemText|toolTip
                Output($"{prediction.Name}|{prediction.Name}|{prediction.Tooltip ?? prediction.Name}");
            }
        }
        catch (Exception ex)
        {
            Logger.Debug($"Error: {ex}");
        }
    }

    private static void Output(string text)
    {
        Logger.Debug(text);
        Console.WriteLine(text);
    }
}

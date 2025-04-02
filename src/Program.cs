namespace PowerShellArgumentCompleter;

// https://learn.microsoft.com/en-us/powershell/scripting/learn/shell/tab-completion
// https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/register-argumentcompleter
// https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/tabexpansion2
// https://techcommunity.microsoft.com/blog/itopstalkblog/autocomplete-in-powershell/2604524
// https://github.com/abgox/PSCompletions/blob/main/completions/scoop/language/en-US.json

/*
Register-ArgumentCompleter -Native -CommandName $command -ScriptBlock {
    param($wordToComplete, $commandAst, $cursorPosition)

    pwsh-argument-completer $wordToComplete, $commandAst, $cursorPosition | ForEach-Object {
        [System.Management.Automation.CompletionResult]::new(
            $_,               # completionText
            $_,               # listItemText
            'ParameterValue', # resultType
            $_                # toolTip
        )
    }
}
*/

internal static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var allArgs = string.Join(" ", args.Select(a => $"\"{a}\""));
            Logger.Write($"Received args: {allArgs}");

            if (args.Length != 3)
            {
                return;
            }

            // var wordToComplete = args[0];
            var cursorPosition = int.Parse(args[2]);
            var commandAst = cursorPosition >= 0 && cursorPosition < args[1].Length ?
                args[1].AsSpan(0, cursorPosition) :
                args[1].AsSpan();

            var completions = CommandCompleter.GetCompletions(commandAst);

            foreach (var completion in completions)
            {
                // completionText|toolTip
                Output($"{completion.CompletionText}|{completion.Tooltip ?? completion.CompletionText}");
            }
        }
        catch (Exception ex)
        {
            Logger.Write($"Error: {ex}");
        }
    }

    private static void Output(string text)
    {
        Console.WriteLine(text);
    }
}

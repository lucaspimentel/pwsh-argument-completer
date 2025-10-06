using System.Diagnostics;
using System.Runtime.CompilerServices;
using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter;

public static class Helpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        return a.Equals(b, StringComparison.OrdinalIgnoreCase);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWith(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        return a.StartsWith(b, StringComparison.OrdinalIgnoreCase);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ICompletion? ThisIfEquals(ICompletion completion, ReadOnlySpan<char> search)
    {
        return Equals(completion.CompletionText, search) ? completion : null;
    }

    public static ICompletion? FindEquals(IEnumerable<ICompletion> completions, ReadOnlySpan<char> search)
    {
        foreach (var completion in completions)
        {
            if (Equals(completion.CompletionText, search))
            {
                return completion;
            }
        }

        return null;
    }

    public static void AddWhereStartsWith(IEnumerable<ICompletion> completions, List<ICompletion> result, ReadOnlySpan<char> search)
    {
        foreach (var completion in completions)
        {
            if (StartsWith(completion.CompletionText, search))
            {
                result.Add(completion);
            }
        }
    }

    public static IEnumerable<string> ExecuteCommand(string command)
    {
        using var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "pwsh.exe",
            Arguments = $"-NoProfile -NonInteractive -Command \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        process.Start();

        while (process.StandardOutput.ReadLine() is { } line)
        {
            yield return line;
        }

        process.WaitForExit();
    }
}

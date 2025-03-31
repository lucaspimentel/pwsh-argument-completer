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
        return Equals(completion.Name, search) ? completion : null;
    }

    public static ICompletion? FindEquals(IEnumerable<ICompletion> completions, ReadOnlySpan<char> search)
    {
        foreach (var completion in completions)
        {
            if (Equals(completion.Name, search))
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
            if (StartsWith(completion.Name, search))
            {
                result.Add(completion);
            }
        }
    }
}

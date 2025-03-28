using System.Text;

namespace PowerShellArgumentCompleter;

public static class Logger
{
    private static readonly Lock FileLock = new();

    public static void Debug(string message)
    {
        lock (FileLock)
        {
            using var stream = File.OpenWrite(@"D:\source\lucaspimentel\pwsh-argument-completer\src\debug-log.txt");
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.ffff} {message}");
            writer.Flush();
            stream.Flush();
        }
    }
}

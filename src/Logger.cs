using System.Text;

namespace PowerShellArgumentCompleter;

public static class Logger
{
    private static readonly Lock FileLock = new();
    private static readonly FileStream FileStream = File.Open(@"D:\source\lucaspimentel\pwsh-argument-completer\src\debug-log.txt", FileMode.Create, FileAccess.Write, FileShare.Read);
    private static readonly StreamWriter StreamWriter = new(FileStream, Encoding.UTF8);

    public static void Debug(string message)
    {
        var log = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.ffff} {message}";

        lock (FileLock)
        {
            StreamWriter.WriteLine(log);
            StreamWriter.Flush();
        }
    }
}

namespace PowerShellArgumentCompleter.KnownCompletions;

using Completions;

internal static class LsdCommand
{
    public static Command Create() =>
        new("lsd", "An ls command with a lot of pretty colors and some other stuff")
        {
            Parameters =
            [
                new("-a", "Do not ignore entries starting with ."),
                new("--all", "Do not ignore entries starting with ."),
                new("-A", "Do not list implied . and .."),
                new("--almost-all", "Do not list implied . and .."),
                new("--color", "When to use terminal colours")
                {
                    StaticArguments =
                    [
                        new("always", "Always use colors"),
                        new("auto", "Automatically detect if colors should be used"),
                        new("never", "Never use colors")
                    ]
                },
                new("--icon", "When to print the icons")
                {
                    StaticArguments =
                    [
                        new("always", "Always print icons"),
                        new("auto", "Automatically detect if icons should be printed"),
                        new("never", "Never print icons")
                    ]
                },
                new("--icon-theme", "Whether to use fancy or unicode icons")
                {
                    StaticArguments =
                    [
                        new("fancy", "Use fancy icons"),
                        new("unicode", "Use unicode icons")
                    ]
                },
                new("-F", "Append indicator (one of */=>@|) at the end of the file names"),
                new("--classify", "Append indicator (one of */=>@|) at the end of the file names"),
                new("-l", "Display extended file metadata as a table"),
                new("--long", "Display extended file metadata as a table"),
                new("--ignore-config", "Ignore the configuration file"),
                new("--config-file", "Provide a custom lsd configuration file"),
                new("-1", "Display one entry per line"),
                new("--oneline", "Display one entry per line"),
                new("-R", "Recurse into directories"),
                new("--recursive", "Recurse into directories"),
                new("-h", "For ls compatibility purposes ONLY, currently set by default"),
                new("--human-readable", "For ls compatibility purposes ONLY, currently set by default"),
                new("--tree", "Recurse into directories and present the result as a tree"),
                new("--depth", "Stop recursing into directories after reaching specified depth"),
                new("-d", "Display directories themselves, and not their contents"),
                new("--directory-only", "Display directories themselves, and not their contents"),
                new("--permission", "How to display permissions")
                {
                    StaticArguments =
                    [
                        new("rwx", "Display as rwx"),
                        new("octal", "Display as octal"),
                        new("attributes", "Display as attributes (Windows)"),
                        new("disable", "Don't display permissions")
                    ]
                },
                new("--size", "How to display size")
                {
                    StaticArguments =
                    [
                        new("default", "Default size display"),
                        new("short", "Short size display"),
                        new("bytes", "Display in bytes")
                    ]
                },
                new("--total-size", "Display the total size of directories"),
                new("--date", "How to display date")
                {
                    StaticArguments =
                    [
                        new("date", "Display as date"),
                        new("locale", "Display using locale format"),
                        new("relative", "Display as relative time")
                    ]
                },
                new("-t", "Sort by time modified"),
                new("--timesort", "Sort by time modified"),
                new("-S", "Sort by size"),
                new("--sizesort", "Sort by size"),
                new("-X", "Sort by file extension"),
                new("--extensionsort", "Sort by file extension"),
                new("-G", "Sort by git status"),
                new("--gitsort", "Sort by git status"),
                new("-v", "Natural sort of (version) numbers within text"),
                new("--versionsort", "Natural sort of (version) numbers within text"),
                new("--sort", "Sort by TYPE instead of name")
                {
                    StaticArguments =
                    [
                        new("size", "Sort by size"),
                        new("time", "Sort by time"),
                        new("version", "Sort by version"),
                        new("extension", "Sort by extension"),
                        new("git", "Sort by git status"),
                        new("none", "No sorting")
                    ]
                },
                new("-U", "Do not sort. List entries in directory order"),
                new("--no-sort", "Do not sort. List entries in directory order"),
                new("-r", "Reverse the order of the sort"),
                new("--reverse", "Reverse the order of the sort"),
                new("--group-dirs", "Sort the directories then the files")
                {
                    StaticArguments =
                    [
                        new("none", "Don't group directories"),
                        new("first", "Group directories first"),
                        new("last", "Group directories last")
                    ]
                },
                new("--group-directories-first", "Groups the directories at the top before the files"),
                new("--blocks", "Specify the blocks that will be displayed and in what order"),
                new("--classic", "Enable classic mode (display output similar to ls)"),
                new("--no-symlink", "Do not display symlink target"),
                new("-I", "Do not display files/directories matching the glob pattern"),
                new("--ignore-glob", "Do not display files/directories matching the glob pattern"),
                new("-i", "Display the index number of each file"),
                new("--inode", "Display the index number of each file"),
                new("-g", "Show git status on file and directory (only with --long)"),
                new("--git", "Show git status on file and directory (only with --long)"),
                new("-L", "Show information for the file the link references"),
                new("--dereference", "Show information for the file the link references"),
                new("-Z", "Print security context (label) of each file"),
                new("--context", "Print security context (label) of each file"),
                new("--hyperlink", "Attach hyperlink to filenames")
                {
                    StaticArguments =
                    [
                        new("always", "Always attach hyperlinks"),
                        new("auto", "Automatically attach hyperlinks"),
                        new("never", "Never attach hyperlinks")
                    ]
                },
                new("--header", "Display block headers"),
                new("--truncate-owner-after", "Truncate user and group names after NUM characters"),
                new("--truncate-owner-marker", "Truncation marker for truncated user or group name"),
                new("--system-protected", "Includes files with the windows system protection flag set"),
                new("-N", "Print entry names without quoting"),
                new("--literal", "Print entry names without quoting"),
                new("--help", "Print help information"),
                new("-V", "Print version"),
                new("--version", "Print version")
            ]
        };
}

namespace PowerShellArgumentCompleter.KnownCompletions;

using Completions;

internal static class DustCommand
{
    public static Command Create() =>
        new("dust", "Like du but more intuitive")
        {
            Parameters =
            [
                new("-d", "Depth to show"),
                new("--depth", "Depth to show"),
                new("-T", "Number of threads to use"),
                new("--threads", "Number of threads to use"),
                new("--config", "Specify a config file to use"),
                new("-n", "Number of lines of output to show"),
                new("--number-of-lines", "Number of lines of output to show"),
                new("-p", "Subdirectories will not have their path shortened"),
                new("--full-paths", "Subdirectories will not have their path shortened"),
                new("-X", "Exclude any file or directory with this path"),
                new("--ignore-directory", "Exclude any file or directory with this path"),
                new("-I", "Exclude any file or directory with a regex matching that listed in this file"),
                new("--ignore-all-in-file", "Exclude any file or directory with a regex matching that listed in this file"),
                new("-L", "dereference sym links - Treat sym links as directories and go into them"),
                new("--dereference-links", "dereference sym links - Treat sym links as directories and go into them"),
                new("-x", "Only count the files and directories on the same filesystem as the supplied directory"),
                new("--limit-filesystem", "Only count the files and directories on the same filesystem as the supplied directory"),
                new("-s", "Use file length instead of blocks"),
                new("--apparent-size", "Use file length instead of blocks"),
                new("-r", "Print tree upside down (biggest highest)"),
                new("--reverse", "Print tree upside down (biggest highest)"),
                new("-c", "No colors will be printed (Useful for commands like: watch)"),
                new("--no-colors", "No colors will be printed (Useful for commands like: watch)"),
                new("-C", "Force colors print"),
                new("--force-colors", "Force colors print"),
                new("-b", "No percent bars or percentages will be displayed"),
                new("--no-percent-bars", "No percent bars or percentages will be displayed"),
                new("-B", "percent bars moved to right side of screen"),
                new("--bars-on-right", "percent bars moved to right side of screen"),
                new("-z", "Minimum size file to include in output"),
                new("--min-size", "Minimum size file to include in output"),
                new("-R", "For screen readers. Removes bars. Adds new column: depth level"),
                new("--screen-reader", "For screen readers. Removes bars. Adds new column: depth level"),
                new("--skip-total", "No total row will be displayed"),
                new("-f", "Directory 'size' is number of child files instead of disk size"),
                new("--filecount", "Directory 'size' is number of child files instead of disk size"),
                new("-i", "Do not display hidden files"),
                new("--ignore-hidden", "Do not display hidden files"),
                new("-v", "Exclude filepaths matching this regex"),
                new("--invert-filter", "Exclude filepaths matching this regex"),
                new("-e", "Only include filepaths matching this regex"),
                new("--filter", "Only include filepaths matching this regex"),
                new("-t", "show only these file types"),
                new("--file-types", "show only these file types"),
                new("-w", "Specify width of output overriding the auto detection of terminal width"),
                new("--terminal-width", "Specify width of output overriding the auto detection of terminal width"),
                new("-P", "Disable the progress indication"),
                new("--no-progress", "Disable the progress indication"),
                new("--print-errors", "Print path with errors"),
                new("-D", "Only directories will be displayed"),
                new("--only-dir", "Only directories will be displayed"),
                new("-F", "Only files will be displayed. (Finds your largest files)"),
                new("--only-file", "Only files will be displayed. (Finds your largest files)"),
                new("-o", "Changes output display size")
                {
                    StaticArguments =
                    [
                        new("si", "SI prefix (powers of 1000)"),
                        new("b", "byte (B)"),
                        new("k", "kibibyte (KiB)"),
                        new("m", "mebibyte (MiB)"),
                        new("g", "gibibyte (GiB)"),
                        new("t", "tebibyte (TiB)"),
                        new("kb", "kilobyte (kB)"),
                        new("mb", "megabyte (MB)"),
                        new("gb", "gigabyte (GB)"),
                        new("tb", "terabyte (TB)")
                    ]
                },
                new("--output-format", "Changes output display size")
                {
                    StaticArguments =
                    [
                        new("si", "SI prefix (powers of 1000)"),
                        new("b", "byte (B)"),
                        new("k", "kibibyte (KiB)"),
                        new("m", "mebibyte (MiB)"),
                        new("g", "gibibyte (GiB)"),
                        new("t", "tebibyte (TiB)"),
                        new("kb", "kilobyte (kB)"),
                        new("mb", "megabyte (MB)"),
                        new("gb", "gigabyte (GB)"),
                        new("tb", "terabyte (TB)")
                    ]
                },
                new("-S", "Specify memory to use as stack size"),
                new("--stack-size", "Specify memory to use as stack size"),
                new("-j", "Output the directory tree as json to the current directory"),
                new("--output-json", "Output the directory tree as json to the current directory"),
                new("-M", "Matches files modified more/less than n days ago"),
                new("--mtime", "Matches files modified more/less than n days ago"),
                new("-A", "just like -mtime, but based on file access time"),
                new("--atime", "just like -mtime, but based on file access time"),
                new("-y", "just like -mtime, but based on file change time"),
                new("--ctime", "just like -mtime, but based on file change time"),
                new("--files0-from", "run dust on NUL-terminated file names specified in file"),
                new("--collapse", "Keep these directories collapsed"),
                new("-m", "Directory 'size' is max filetime of child files instead of disk size")
                {
                    StaticArguments =
                    [
                        new("a", "last accessed time"),
                        new("c", "last changed time"),
                        new("m", "last modified time")
                    ]
                },
                new("--filetime", "Directory 'size' is max filetime of child files instead of disk size")
                {
                    StaticArguments =
                    [
                        new("a", "last accessed time"),
                        new("c", "last changed time"),
                        new("m", "last modified time")
                    ]
                },
                new("-h", "Print help"),
                new("--help", "Print help"),
                new("-V", "Print version"),
                new("--version", "Print version")
            ]
        };
}

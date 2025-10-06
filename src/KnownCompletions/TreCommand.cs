// Usage: tre [OPTIONS] [PATH]
//
// Tree command, improved.
//
// OPTIONS:
//     -a, --all                   Print all files and directories, including hidden ones
//     -c, --color <WHEN>          When to color the output [default: automatic] [possible values: automatic, always, never]
//     -d, --directories           Only list directories in output
//     -e, --editor [<COMMAND>]    Create aliases for each displayed result
//     -E, --exclude <PATTERN>     Exclude paths matching a regex pattern. Repeatable
//     -h, --help                  Print help information
//     -j, --json                  Output JSON instead of tree diagram
//     -l, --limit <LIMIT>         Limit depth of the tree in output
//     -p, --portable              Generate portable (absolute) paths for editor aliases
//     -V, --version               Print version information

using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter.KnownCompletions;

public static class TreCommand
{
    public static Command Create()
    {
        return new Command("tre")
        {
            Parameters =
            [
                new("-a", "Print all files and directories, including hidden ones"),
                new("--all", "Print all files and directories, including hidden ones"),
                new("-c", "When to color the output")
                {
                    StaticArguments =
                    [
                        new("automatic", "Color when printing to a terminal"),
                        new("always", "Always use colors"),
                        new("never", "Never use colors"),
                    ]
                },
                new("--color", "When to color the output")
                {
                    StaticArguments =
                    [
                        new("automatic", "Color when printing to a terminal"),
                        new("always", "Always use colors"),
                        new("never", "Never use colors"),
                    ]
                },
                new("-d", "Only list directories in output"),
                new("--directories", "Only list directories in output"),
                new("-e", "Create aliases for each displayed result"),
                new("--editor", "Create aliases for each displayed result"),
                new("-E", "Exclude paths matching a regex pattern"),
                new("--exclude", "Exclude paths matching a regex pattern"),
                new("-h", "Print help information"),
                new("--help", "Print help information"),
                new("-j", "Output JSON instead of tree diagram"),
                new("--json", "Output JSON instead of tree diagram"),
                new("-l", "Limit depth of the tree in output"),
                new("--limit", "Limit depth of the tree in output"),
                new("-p", "Generate portable (absolute) paths for editor aliases"),
                new("--portable", "Generate portable (absolute) paths for editor aliases"),
                new("-V", "Print version information"),
                new("--version", "Print version information"),
            ]
        };
    }
}

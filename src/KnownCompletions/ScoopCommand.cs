// Usage: scoop <command> [<args>]
//
// Available commands are listed below.
//
// Type 'scoop help <command>' to get more help for a specific command.
//
// Command    Summary
// -------    -------
// alias      Manage scoop aliases
// bucket     Manage Scoop buckets
// cache      Show or clear the download cache
// cat        Show content of specified manifest.
// checkup    Check for potential problems
// cleanup    Cleanup apps by removing old versions
// config     Get or set configuration values
// create     Create a custom app manifest
// depends    List dependencies for an app, in the order they'll be installed
// download   Download apps in the cache folder and verify hashes
// export     Exports installed apps, buckets (and optionally configs) in JSON format
// help       Show help for a command
// hold       Hold an app to disable updates
// home       Opens the app homepage
// import     Imports apps, buckets and configs from a Scoopfile in JSON format
// info       Display information about an app
// install    Install apps
// list       List installed apps
// prefix     Returns the path to the specified app
// reset      Reset an app to resolve conflicts
// search     Search available apps
// shim       Manipulate Scoop shims
// status     Show status and check for new app versions
// unhold     Unhold an app to enable updates
// uninstall  Uninstall an app
// update     Update apps, or Scoop itself
// virustotal Look for app's hash or url on virustotal.com
// which      Locate a shim/executable (similar to 'which' on Linux)

using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter.KnownCompletions;

public static class ScoopCommand
{
    public static Command Create()
    {
        return new Command("scoop")
        {
            SubCommands =
            [
                new("alias", "Manage scoop aliases"),
                new("bucket", "Manage Scoop buckets"),
                new("cache", "Show or clear the download cache"),
                new("cat", "Show content of specified manifest."),
                new("checkup", "Check for potential problems"),
                new("cleanup", "Cleanup apps by removing old versions")
                {
                    Parameters =
                    [
                        new("*", "Cleanup all apps"),
                        new("--all", "Cleanup all apps (-a)") { Alias = "-a" },
                        new("--global", "Cleanup a globally installed app (-g)") { Alias = "-g" },
                        new("--cache", "Remove outdated download cache (-k)") { Alias = "-k" }
                    ],
                    DynamicArguments = GetInstalledPackages
                },
                new("config", "Get or set configuration values"),
                new("create", "Create a custom app manifest"),
                new("depends", "List dependencies for an app, in the order they'll be installed"),
                new("download", "Download apps in the cache folder and verify hashes"),
                new("export", "Exports installed apps, buckets (and optionally configs) in JSON format"),
                new("help", "Show help for a command"),
                new("hold", "Hold an app to disable updates")
                {
                    Parameters =
                    [
                        new("--global", "Hold globally installed apps (-g)") { Alias = "-g" }
                    ],
                    DynamicArguments = GetInstalledPackages
                },
                new("home", "Opens the app homepage"),
                new("import", "Imports apps, buckets and configs from a Scoopfile in JSON format"),
                new("info", "Display information about an app"),
                new("install", "Install apps"),
                new("list", "List installed apps"),
                new("prefix", "Returns the path to the specified app"),
                new("reset", "Reset an app to resolve conflicts")
                {
                    Parameters =
                    [
                        new("*", "Reset all apps"),
                        new("--all", "Reset all apps (-a)") { Alias = "-a" }
                    ],
                    DynamicArguments = GetInstalledPackages
                },
                new("search", "Search available apps"),
                new("shim", "Manipulate Scoop shims"),
                new("status", "Show status and check for new app versions")
                {
                    Parameters =
                    [
                        new("--local", "Checks the status for only the locally installed apps, and disables remote fetching/checking for Scoop and buckets (-l)") { Alias = "-l" }
                    ]
                },
                new("unhold", "Unhold an app to enable updates")
                {
                    Parameters =
                    [
                        new("--global", "Unhold globally installed apps (-g)") { Alias = "-g" }
                    ],
                    DynamicArguments = GetInstalledPackages
                },
                new("uninstall", "Uninstall an app")
                {
                    Parameters =
                    [
                        new("--global", "Uninstall a globally installed app (-g)") { Alias = "-g" },
                        new("--purge", "Remove all persistent data (-p)") { Alias = "-p" }
                    ],
                    DynamicArguments = GetInstalledPackages
                },
                new("update", "Update apps, or Scoop itself")
                {
                    Parameters = [new("*", "Update all apps")],
                    DynamicArguments = GetInstalledPackages
                },
                new("virustotal", "Look for app's hash or url on virustotal.com"),
                new("which", "Locate a shim/executable (similar to 'which' on Linux)")
            ],
        };
    }

    private static IEnumerable<DynamicArgument> GetInstalledPackages()
    {
        var foundHeader = false;
        foreach (var line in Helpers.ExecuteCommand("scoop", "list"))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Skip the "Installed apps:" line
            if (line.StartsWith("Installed apps:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Skip header line (contains ANSI color codes like [32;1m)
            if (line.Contains("[32;1m"))
            {
                foundHeader = true;
                continue;
            }

            // Skip separator line (contains "----")
            if (line.Contains("----"))
            {
                continue;
            }

            // Only start yielding results after we've seen the header
            if (foundHeader)
            {
                var trimmed = line.TrimStart();
                if (trimmed.Length > 0)
                {
                    var index = trimmed.IndexOf(' ');
                    yield return new DynamicArgument(index == -1 ? trimmed : trimmed[..index]);
                }
            }
        }
    }
}

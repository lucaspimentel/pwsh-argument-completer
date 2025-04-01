using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter.KnownCompletions;

public static class WingetCommand
{
    public static Command Create()
    {
        return new Command("winget")
        {
            SubCommands =
            [
                new("install", "Installs the given package"),
                new("show", "Shows information about a package"),
                new("source", "Manage sources of packages"),
                new("search", "Find and show basic info of packages"),
                new("list", "Display installed packages"),
                new("upgrade", "Shows and performs available upgrades"),
                new("uninstall", "Uninstalls the given package"),
                new("hash", "Helper to hash installer files"),
                new("validate", "Validates a manifest file"),
                new("settings", "Open settings or set administrator settings"),
                new("features", "Shows the status of experimental features"),
                new("export", "Exports a list of the installed packages"),
                new("import", "Installs all the packages in a file"),
                new("pin", "Manage package pins"),
                new("configure", "Configures the system into a desired state"),
                new("download", "Downloads the installer from a given package"),
                new("repair", "Repairs the selected package"),
            ],
            Parameters =
            [
                new("-v", "Display the version of the tool"),
                new("--version", "Display the version of the tool"),
                new("--info", "Display general info of the tool"),
                new("-?", "Shows help about the selected command"),
                new("--help", "Shows help about the selected command"),
                new("--wait", "Prompts the user to press any key before exiting"),
                new("--logs", "Open the default logs location"),
                new("--open-logs", "Open the default logs location"),
                new("--verbose", "Enables verbose logging for winget"),
                new("--verbose-logs", "Enables verbose logging for winget"),
                new("--nowarn", "Suppresses warning outputs"),
                new("--ignore-warnings", "Suppresses warning outputs"),
                new("--disable-interactivity", "Disable interactive prompts"),
                new("--proxy", "Set a proxy to use for this execution"),
                new("--no-proxy", "Disable the use of proxy for this execution"),
            ]
        };
    }
}

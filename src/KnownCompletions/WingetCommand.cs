// Windows Package Manager (Preview) v1.11.180-preview
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// The winget command line utility enables installing applications and other packages from the command line.
//
// usage: winget  [<command>] [<options>]
//
// The following commands are available:
//   install    Installs the given package
//   show       Shows information about a package
//   source     Manage sources of packages
//   search     Find and show basic info of packages
//   list       Display installed packages
//   upgrade    Shows and performs available upgrades
//   uninstall  Uninstalls the given package
//   hash       Helper to hash installer files
//   validate   Validates a manifest file
//   settings   Open settings or set administrator settings
//   features   Shows the status of experimental features
//   export     Exports a list of the installed packages
//   import     Installs all the packages in a file
//   pin        Manage package pins
//   configure  Configures the system into a desired state
//   download   Downloads the installer from a given package
//   repair     Repairs the selected package
//
// For more details on a specific command, pass it the help argument. [-?]
//
// The following options are available:
//   -v,--version                Display the version of the tool
//   --info                      Display general info of the tool
//   -?,--help                   Shows help about the selected command
//   --wait                      Prompts the user to press any key before exiting
//   --logs,--open-logs          Open the default logs location
//   --verbose,--verbose-logs    Enables verbose logging for winget
//   --nowarn,--ignore-warnings  Suppresses warning outputs
//   --disable-interactivity     Disable interactive prompts
//   --proxy                     Set a proxy to use for this execution
//   --no-proxy                  Disable the use of proxy for this execution
//
// More help can be found at: https://aka.ms/winget-command-help

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

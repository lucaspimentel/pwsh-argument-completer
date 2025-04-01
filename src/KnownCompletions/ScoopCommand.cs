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
                new("install", "Install apps"),
                new("update", "Update apps, or Scoop itself")
                {
                    Parameters = [new("*", "Update all apps")],
                    DynamicArguments = GetInstalledPackages
                },
                new("status", "Show status and check for new app versions")
                {
                    Parameters =
                    [
                        new("-l", "Checks the status for only the locally installed apps, and disables remote fetching/checking for Scoop and buckets"),
                        new("--local", "Checks the status for only the locally installed apps, and disables remote fetching/checking for Scoop and buckets")
                    ]
                },
                new("checkup", "Check for potential problems")
            ]
        };
    }

    private static IEnumerable<DynamicArgument> GetInstalledPackages()
    {
        return Helpers.ExecuteCommand("scoop list")
                      .Select(app => new DynamicArgument(app));
    }
}

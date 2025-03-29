namespace PowerShellArgumentCompleter.Commands;

public static class ScoopCommand
{
    public static Command Create()
    {
        return new Command("scoop")
        {
            SubCommands =
            [
                new("install")
                {
                    Tooltip = "Install apps"
                },
                new("update")
                {
                    Tooltip = "Update apps, or Scoop itself",
                    Parameters = [new("*")],
                    DynamicArguments = GetInstalledPackages
                },
                new("status")
                {
                    Tooltip = "Show status and check for new app versions",
                    Parameters =
                    [
                        new("-l")
                        {
                            Tooltip = "Checks the status for only the locally installed apps, and disables remote fetching/checking for Scoop and buckets"
                        },
                        new("--local")
                        {
                            Tooltip = "Checks the status for only the locally installed apps, and disables remote fetching/checking for Scoop and buckets"
                        }
                    ]
                },
                new("checkup")
                {
                    Tooltip = "Check for potential problems"
                }
            ]
        };
    }

    private static DynamicArgument[] GetInstalledPackages()
    {
        return
        [
            new("TODO1"),
            new("TODO2"),
        ];
    }
}

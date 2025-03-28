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
                    Tooltip = "Installs a package"
                },
                new("update")
                {
                    Tooltip = "Update scoop, an installed package, or all installed packages",
                    Parameters = [new("*")],
                    DynamicArguments = GetInstalledPackages
                },
                new("status"),
                new("checkup"),
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

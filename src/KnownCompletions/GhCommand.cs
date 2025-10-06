// Usage: gh <command> <subcommand> [flags]
//
// Core commands:
//    auth          Authenticate gh and git with GitHub
//    browse        Open repositories, issues, pull requests, and more in the browser
//    codespace     Connect to and manage codespaces
//    gist          Manage gists
//    issue         Manage issues
//    org           Manage organizations
//    pr            Manage pull requests
//    project       Work with GitHub Projects
//    release       Manage releases
//    repo          Manage repositories
//
// GitHub Actions commands:
//    cache         Manage GitHub Actions caches
//    run           View details about workflow runs
//    workflow      View details about GitHub Actions workflows
//
// Additional commands:
//    alias         Create command shortcuts
//    api           Make an authenticated GitHub API request
//    completion    Generate shell completion scripts
//    config        Manage configuration for gh
//    extension     Manage gh extensions
//    gpg-key       Manage GPG keys
//    label         Manage labels
//    search        Search for repositories, issues, and pull requests
//    secret        Manage GitHub secrets
//    ssh-key       Manage SSH keys
//    status        Print information about relevant issues, pull requests, and notifications
//    variable      Manage GitHub Actions variables

using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter.KnownCompletions;

public static class GhCommand
{
    public static Command Create()
    {
        return new Command("gh")
        {
            SubCommands =
            [
                new("auth", "Authenticate gh and git with GitHub")
                {
                    SubCommands =
                    [
                        new("login", "Log in to a GitHub account")
                        {
                            Parameters =
                            [
                                new("-h", "Hostname to authenticate with"),
                                new("--hostname", "Hostname to authenticate with"),
                                new("-p", "Authentication protocol"),
                                new("--git-protocol", "Authentication protocol"),
                                new("-s", "Additional authentication scopes"),
                                new("--scopes", "Additional authentication scopes"),
                                new("-w", "Open browser to authenticate"),
                                new("--web", "Open browser to authenticate"),
                            ]
                        },
                        new("logout", "Log out of a GitHub account")
                        {
                            Parameters =
                            [
                                new("-h", "Hostname to log out of"),
                                new("--hostname", "Hostname to log out of"),
                            ]
                        },
                        new("refresh", "Refresh stored authentication credentials")
                        {
                            Parameters =
                            [
                                new("-h", "Hostname to refresh"),
                                new("--hostname", "Hostname to refresh"),
                                new("-s", "Additional authentication scopes"),
                                new("--scopes", "Additional authentication scopes"),
                            ]
                        },
                        new("setup-git", "Setup git with GitHub CLI"),
                        new("status", "Display active account and authentication state"),
                        new("switch", "Switch active GitHub account"),
                        new("token", "Print the authentication token gh uses")
                        {
                            Parameters =
                            [
                                new("-h", "Hostname for the token"),
                                new("--hostname", "Hostname for the token"),
                            ]
                        },
                    ]
                },
                new("browse", "Open repositories, issues, pull requests, and more in the browser")
                {
                    Parameters =
                    [
                        new("-b", "Select branch"),
                        new("--branch", "Select branch"),
                        new("-c", "Open commit"),
                        new("--commit", "Open commit"),
                        new("-n", "Don't open browser"),
                        new("--no-browser", "Don't open browser"),
                        new("-R", "Select repository"),
                        new("--repo", "Select repository"),
                    ]
                },
                new("codespace", "Connect to and manage codespaces")
                {
                    SubCommands =
                    [
                        new("code", "Open a codespace in VS Code"),
                        new("create", "Create a codespace"),
                        new("delete", "Delete a codespace"),
                        new("list", "List codespaces"),
                        new("ssh", "SSH into a codespace"),
                        new("stop", "Stop a codespace"),
                    ]
                },
                new("gist", "Manage gists")
                {
                    SubCommands =
                    [
                        new("create", "Create a new gist")
                        {
                            Parameters =
                            [
                                new("-d", "Description"),
                                new("--desc", "Description"),
                                new("-f", "Filename"),
                                new("--filename", "Filename"),
                                new("-p", "Make gist public"),
                                new("--public", "Make gist public"),
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                        new("clone", "Clone a gist"),
                        new("delete", "Delete a gist"),
                        new("edit", "Edit a gist"),
                        new("list", "List gists")
                        {
                            Parameters =
                            [
                                new("--public", "Show only public gists"),
                                new("--secret", "Show only secret gists"),
                            ]
                        },
                        new("view", "View a gist")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                    ]
                },
                new("issue", "Manage issues")
                {
                    SubCommands =
                    [
                        new("create", "Create a new issue")
                        {
                            Parameters =
                            [
                                new("-a", "Assign people by login"),
                                new("--assignee", "Assign people by login"),
                                new("-b", "Body text"),
                                new("--body", "Body text"),
                                new("-l", "Add labels"),
                                new("--label", "Add labels"),
                                new("-t", "Title"),
                                new("--title", "Title"),
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                        new("close", "Close an issue"),
                        new("comment", "Add a comment to an issue"),
                        new("delete", "Delete an issue"),
                        new("edit", "Edit an issue"),
                        new("list", "List issues")
                        {
                            Parameters =
                            [
                                new("-a", "Filter by assignee"),
                                new("--assignee", "Filter by assignee"),
                                new("-A", "Filter by author"),
                                new("--author", "Filter by author"),
                                new("-l", "Filter by label"),
                                new("--label", "Filter by label"),
                                new("-s", "Filter by state"),
                                new("--state", "Filter by state"),
                                new("-L", "Maximum number to fetch"),
                                new("--limit", "Maximum number to fetch"),
                            ]
                        },
                        new("reopen", "Reopen an issue"),
                        new("status", "Show status of relevant issues"),
                        new("view", "View an issue")
                        {
                            Parameters =
                            [
                                new("-c", "View comments"),
                                new("--comments", "View comments"),
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                    ],
                    Parameters =
                    [
                        new("-R", "Select repository"),
                        new("--repo", "Select repository"),
                    ]
                },
                new("org", "Manage organizations")
                {
                    SubCommands =
                    [
                        new("list", "List organizations for the authenticated user"),
                    ]
                },
                new("pr", "Manage pull requests")
                {
                    SubCommands =
                    [
                        new("create", "Create a pull request")
                        {
                            Parameters =
                            [
                                new("-a", "Assign people by login"),
                                new("--assignee", "Assign people by login"),
                                new("-B", "Base branch"),
                                new("--base", "Base branch"),
                                new("-b", "Body text"),
                                new("--body", "Body text"),
                                new("-d", "Mark as draft"),
                                new("--draft", "Mark as draft"),
                                new("-f", "Use commit message for title and body"),
                                new("--fill", "Use commit message for title and body"),
                                new("-H", "Head branch"),
                                new("--head", "Head branch"),
                                new("-l", "Add labels"),
                                new("--label", "Add labels"),
                                new("-t", "Title"),
                                new("--title", "Title"),
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                        new("checkout", "Check out a pull request in git"),
                        new("checks", "Show CI status for a single pull request")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                        new("close", "Close a pull request"),
                        new("comment", "Add a comment to a pull request"),
                        new("diff", "View changes in a pull request")
                        {
                            Parameters =
                            [
                                new("--color", "Use color in diff output"),
                                new("--patch", "Display in patch format"),
                            ]
                        },
                        new("edit", "Edit a pull request"),
                        new("list", "List pull requests")
                        {
                            Parameters =
                            [
                                new("-a", "Filter by assignee"),
                                new("--assignee", "Filter by assignee"),
                                new("-A", "Filter by author"),
                                new("--author", "Filter by author"),
                                new("-B", "Filter by base branch"),
                                new("--base", "Filter by base branch"),
                                new("-l", "Filter by label"),
                                new("--label", "Filter by label"),
                                new("-s", "Filter by state"),
                                new("--state", "Filter by state"),
                                new("-L", "Maximum number to fetch"),
                                new("--limit", "Maximum number to fetch"),
                            ]
                        },
                        new("merge", "Merge a pull request")
                        {
                            Parameters =
                            [
                                new("-m", "Merge commit"),
                                new("--merge", "Merge commit"),
                                new("-r", "Rebase and merge"),
                                new("--rebase", "Rebase and merge"),
                                new("-s", "Squash and merge"),
                                new("--squash", "Squash and merge"),
                                new("-d", "Delete branch after merge"),
                                new("--delete-branch", "Delete branch after merge"),
                            ]
                        },
                        new("ready", "Mark a pull request as ready for review"),
                        new("reopen", "Reopen a pull request"),
                        new("review", "Add a review to a pull request")
                        {
                            Parameters =
                            [
                                new("-a", "Approve pull request"),
                                new("--approve", "Approve pull request"),
                                new("-c", "Comment on pull request"),
                                new("--comment", "Comment on pull request"),
                                new("-r", "Request changes"),
                                new("--request-changes", "Request changes"),
                            ]
                        },
                        new("status", "Show status of relevant pull requests"),
                        new("view", "View a pull request")
                        {
                            Parameters =
                            [
                                new("-c", "View comments"),
                                new("--comments", "View comments"),
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                    ],
                    Parameters =
                    [
                        new("-R", "Select repository"),
                        new("--repo", "Select repository"),
                    ]
                },
                new("project", "Work with GitHub Projects"),
                new("release", "Manage releases")
                {
                    SubCommands =
                    [
                        new("create", "Create a new release")
                        {
                            Parameters =
                            [
                                new("-d", "Mark as draft"),
                                new("--draft", "Mark as draft"),
                                new("-n", "Release notes"),
                                new("--notes", "Release notes"),
                                new("-p", "Mark as prerelease"),
                                new("--prerelease", "Mark as prerelease"),
                                new("-t", "Release title"),
                                new("--title", "Release title"),
                            ]
                        },
                        new("delete", "Delete a release"),
                        new("download", "Download release assets"),
                        new("list", "List releases")
                        {
                            Parameters =
                            [
                                new("-L", "Maximum number to fetch"),
                                new("--limit", "Maximum number to fetch"),
                            ]
                        },
                        new("upload", "Upload release assets"),
                        new("view", "View a release")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                    ]
                },
                new("repo", "Manage repositories")
                {
                    SubCommands =
                    [
                        new("create", "Create a new repository")
                        {
                            Parameters =
                            [
                                new("--public", "Make repository public"),
                                new("--private", "Make repository private"),
                                new("--internal", "Make repository internal"),
                                new("-d", "Description"),
                                new("--description", "Description"),
                                new("-h", "Homepage URL"),
                                new("--homepage", "Homepage URL"),
                                new("--clone", "Clone repository after creation"),
                            ]
                        },
                        new("clone", "Clone a repository locally"),
                        new("delete", "Delete a repository"),
                        new("fork", "Create a fork of a repository")
                        {
                            Parameters =
                            [
                                new("--clone", "Clone fork after creation"),
                                new("--remote", "Add remote for fork"),
                            ]
                        },
                        new("list", "List repositories owned by user or organization")
                        {
                            Parameters =
                            [
                                new("-L", "Maximum number to fetch"),
                                new("--limit", "Maximum number to fetch"),
                                new("--source", "Show only non-forks"),
                                new("--fork", "Show only forks"),
                            ]
                        },
                        new("rename", "Rename a repository"),
                        new("sync", "Sync a repository"),
                        new("view", "View a repository")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                    ]
                },
                new("run", "View details about workflow runs")
                {
                    SubCommands =
                    [
                        new("list", "List workflow runs")
                        {
                            Parameters =
                            [
                                new("-L", "Maximum number to fetch"),
                                new("--limit", "Maximum number to fetch"),
                                new("-w", "Filter by workflow"),
                                new("--workflow", "Filter by workflow"),
                            ]
                        },
                        new("view", "View a workflow run")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                                new("--log", "View full log"),
                            ]
                        },
                        new("watch", "Watch a workflow run"),
                        new("rerun", "Rerun a workflow run"),
                        new("cancel", "Cancel a workflow run"),
                    ]
                },
                new("workflow", "View details about GitHub Actions workflows")
                {
                    SubCommands =
                    [
                        new("list", "List workflows"),
                        new("view", "View a workflow")
                        {
                            Parameters =
                            [
                                new("-w", "Open in browser"),
                                new("--web", "Open in browser"),
                            ]
                        },
                        new("run", "Run a workflow"),
                        new("enable", "Enable a workflow"),
                        new("disable", "Disable a workflow"),
                    ]
                },
                new("alias", "Create command shortcuts")
                {
                    SubCommands =
                    [
                        new("set", "Create a shortcut for a gh command"),
                        new("delete", "Delete an alias"),
                        new("list", "List aliases"),
                    ]
                },
                new("api", "Make an authenticated GitHub API request")
                {
                    Parameters =
                    [
                        new("-H", "Add HTTP header"),
                        new("--header", "Add HTTP header"),
                        new("-X", "HTTP method"),
                        new("--method", "HTTP method"),
                        new("-F", "Add form field"),
                        new("--field", "Add form field"),
                    ]
                },
                new("completion", "Generate shell completion scripts")
                {
                    Parameters =
                    [
                        new("-s", "Shell type (bash, zsh, fish, powershell)"),
                        new("--shell", "Shell type (bash, zsh, fish, powershell)"),
                    ]
                },
                new("config", "Manage configuration for gh")
                {
                    SubCommands =
                    [
                        new("get", "Get configuration value"),
                        new("set", "Set configuration value"),
                        new("list", "List configuration values"),
                    ]
                },
                new("extension", "Manage gh extensions")
                {
                    SubCommands =
                    [
                        new("install", "Install an extension"),
                        new("list", "List installed extensions"),
                        new("remove", "Remove an extension"),
                        new("upgrade", "Upgrade extensions"),
                    ]
                },
                new("label", "Manage labels")
                {
                    SubCommands =
                    [
                        new("create", "Create a label"),
                        new("delete", "Delete a label"),
                        new("edit", "Edit a label"),
                        new("list", "List labels"),
                    ]
                },
                new("search", "Search for repositories, issues, and pull requests")
                {
                    SubCommands =
                    [
                        new("repos", "Search for repositories"),
                        new("issues", "Search for issues"),
                        new("prs", "Search for pull requests"),
                    ]
                },
                new("secret", "Manage GitHub secrets")
                {
                    SubCommands =
                    [
                        new("set", "Set a secret"),
                        new("delete", "Delete a secret"),
                        new("list", "List secrets"),
                    ]
                },
                new("status", "Print information about relevant issues, pull requests, and notifications"),
            ],
            Parameters =
            [
                new("--help", "Show help for command"),
                new("--version", "Show gh version"),
            ]
        };
    }
}

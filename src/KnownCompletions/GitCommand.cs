// Usage: git [--version] [--help] [-C <path>] [-c <name>=<value>]
//            [--exec-path[=<path>]] [--html-path] [--man-path] [--info-path]
//            [-p | --paginate | -P | --no-pager] [--no-replace-objects] [--bare]
//            [--git-dir=<path>] [--work-tree=<path>] [--namespace=<name>]
//            [--config-env=<name>=<envvar>] <command> [<args>]
//
// Common git commands:
//    add        Add file contents to the index
//    bisect     Use binary search to find the commit that introduced a bug
//    branch     List, create, or delete branches
//    checkout   Switch branches or restore working tree files
//    cherry-pick Apply the changes introduced by some existing commits
//    clean      Remove untracked files from the working tree
//    clone      Clone a repository into a new directory
//    commit     Record changes to the repository
//    diff       Show changes between commits, commit and working tree, etc
//    fetch      Download objects and refs from another repository
//    init       Create an empty Git repository or reinitialize an existing one
//    log        Show commit logs
//    merge      Join two or more development histories together
//    mv         Move or rename a file, a directory, or a symlink
//    pull       Fetch from and integrate with another repository or a local branch
//    push       Update remote refs along with associated objects
//    rebase     Reapply commits on top of another base tip
//    reset      Reset current HEAD to the specified state
//    restore    Restore working tree files
//    revert     Revert some existing commits
//    rm         Remove files from the working tree and from the index
//    show       Show various types of objects
//    stash      Stash the changes in a dirty working directory away
//    status     Show the working tree status
//    switch     Switch branches
//    tag        Create, list, delete or verify a tag object signed with GPG
//    worktree   Manage multiple working trees

using PowerShellArgumentCompleter.Completions;

namespace PowerShellArgumentCompleter.KnownCompletions;

public static class GitCommand
{
    public static Command Create()
    {
        return new Command("git")
        {
            SubCommands =
            [
                new("add", "Add file contents to the index")
                {
                    Parameters =
                    [
                        new("-A", "Add all changes"),
                        new("--all", "Add all changes"),
                        new("-f", "Force add ignored files"),
                        new("--force", "Force add ignored files"),
                        new("-p", "Interactively choose hunks"),
                        new("--patch", "Interactively choose hunks"),
                        new("-u", "Update tracked files"),
                        new("--update", "Update tracked files"),
                        new(".", "Add all changes in current directory"),
                    ]
                },
                new("bisect", "Use binary search to find the commit that introduced a bug")
                {
                    SubCommands =
                    [
                        new("start", "Start bisecting"),
                        new("bad", "Mark current revision as bad"),
                        new("good", "Mark current revision as good"),
                        new("reset", "Finish bisecting and return to original HEAD"),
                        new("skip", "Skip current revision"),
                    ]
                },
                new("branch", "List, create, or delete branches")
                {
                    Parameters =
                    [
                        new("-a", "List all branches"),
                        new("--all", "List all branches"),
                        new("-d", "Delete branch"),
                        new("--delete", "Delete branch"),
                        new("-D", "Force delete branch"),
                        new("-m", "Move/rename branch"),
                        new("--move", "Move/rename branch"),
                        new("-M", "Force move/rename branch"),
                        new("-r", "List remote branches"),
                        new("--remote", "List remote branches"),
                        new("-v", "Verbose output"),
                        new("--verbose", "Verbose output"),
                        new("-vv", "Very verbose output"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("checkout", "Switch branches or restore working tree files")
                {
                    Parameters =
                    [
                        new("-b", "Create and checkout new branch"),
                        new("-B", "Create/reset and checkout branch"),
                        new("-f", "Force checkout"),
                        new("--force", "Force checkout"),
                        new("-", "Switch to previous branch"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("cherry-pick", "Apply the changes introduced by some existing commits")
                {
                    Parameters =
                    [
                        new("--continue", "Continue cherry-pick"),
                        new("--abort", "Abort cherry-pick"),
                        new("--skip", "Skip current commit"),
                    ]
                },
                new("clean", "Remove untracked files from the working tree")
                {
                    Parameters =
                    [
                        new("-d", "Remove untracked directories"),
                        new("-f", "Force clean"),
                        new("--force", "Force clean"),
                        new("-n", "Dry run"),
                        new("--dry-run", "Dry run"),
                        new("-x", "Remove ignored files too"),
                    ]
                },
                new("clone", "Clone a repository into a new directory")
                {
                    Parameters =
                    [
                        new("--bare", "Create a bare repository"),
                        new("--depth", "Create a shallow clone with history truncated"),
                        new("--branch", "Checkout specific branch"),
                        new("-b", "Checkout specific branch"),
                    ]
                },
                new("commit", "Record changes to the repository")
                {
                    Parameters =
                    [
                        new("-a", "Commit all changes"),
                        new("--all", "Commit all changes"),
                        new("--amend", "Amend previous commit"),
                        new("-m", "Commit message"),
                        new("--message", "Commit message"),
                        new("--no-verify", "Skip pre-commit and commit-msg hooks"),
                        new("-v", "Show diff in commit message editor"),
                        new("--verbose", "Show diff in commit message editor"),
                    ]
                },
                new("diff", "Show changes between commits, commit and working tree, etc")
                {
                    Parameters =
                    [
                        new("--cached", "Show staged changes"),
                        new("--staged", "Show staged changes"),
                        new("--stat", "Show diffstat"),
                        new("--name-only", "Show only names of changed files"),
                        new("--name-status", "Show names and status of changed files"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("fetch", "Download objects and refs from another repository")
                {
                    Parameters =
                    [
                        new("--all", "Fetch all remotes"),
                        new("-p", "Prune deleted remote branches"),
                        new("--prune", "Prune deleted remote branches"),
                        new("--tags", "Fetch all tags"),
                    ],
                    DynamicArguments = GetRemotes
                },
                new("init", "Create an empty Git repository or reinitialize an existing one")
                {
                    Parameters =
                    [
                        new("--bare", "Create a bare repository"),
                        new("--initial-branch", "Set initial branch name"),
                        new("-b", "Set initial branch name"),
                    ]
                },
                new("log", "Show commit logs")
                {
                    Parameters =
                    [
                        new("--oneline", "Show one line per commit"),
                        new("--graph", "Show commit graph"),
                        new("--all", "Show all branches"),
                        new("--stat", "Show file change stats"),
                        new("-p", "Show patch"),
                        new("--patch", "Show patch"),
                        new("-n", "Limit number of commits"),
                        new("--follow", "Follow file history across renames"),
                    ]
                },
                new("merge", "Join two or more development histories together")
                {
                    Parameters =
                    [
                        new("--abort", "Abort merge"),
                        new("--continue", "Continue merge"),
                        new("--no-ff", "Create merge commit even when fast-forward is possible"),
                        new("--ff-only", "Only allow fast-forward merge"),
                        new("--squash", "Squash commits"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("mv", "Move or rename a file, a directory, or a symlink")
                {
                    Parameters =
                    [
                        new("-f", "Force move"),
                        new("--force", "Force move"),
                    ]
                },
                new("pull", "Fetch from and integrate with another repository or a local branch")
                {
                    Parameters =
                    [
                        new("--rebase", "Rebase instead of merge"),
                        new("--no-rebase", "Merge instead of rebase"),
                        new("--ff-only", "Only allow fast-forward"),
                        new("--all", "Fetch all remotes"),
                    ],
                    DynamicArguments = GetRemotes
                },
                new("push", "Update remote refs along with associated objects")
                {
                    Parameters =
                    [
                        new("-u", "Set upstream tracking"),
                        new("--set-upstream", "Set upstream tracking"),
                        new("-f", "Force push"),
                        new("--force", "Force push"),
                        new("--force-with-lease", "Force push with lease"),
                        new("--all", "Push all branches"),
                        new("--tags", "Push all tags"),
                        new("--delete", "Delete remote branch"),
                    ],
                    DynamicArguments = GetRemotes
                },
                new("rebase", "Reapply commits on top of another base tip")
                {
                    Parameters =
                    [
                        new("--continue", "Continue rebase"),
                        new("--abort", "Abort rebase"),
                        new("--skip", "Skip current commit"),
                        new("-i", "Interactive rebase"),
                        new("--interactive", "Interactive rebase"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("reset", "Reset current HEAD to the specified state")
                {
                    Parameters =
                    [
                        new("--hard", "Reset index and working tree"),
                        new("--soft", "Keep changes staged"),
                        new("--mixed", "Keep changes unstaged (default)"),
                        new("HEAD", "Reset to HEAD"),
                        new("HEAD~1", "Reset to previous commit"),
                    ]
                },
                new("restore", "Restore working tree files")
                {
                    Parameters =
                    [
                        new("--staged", "Restore staged files"),
                        new("--source", "Restore from specific commit"),
                        new(".", "Restore all files"),
                    ]
                },
                new("revert", "Revert some existing commits")
                {
                    Parameters =
                    [
                        new("--continue", "Continue revert"),
                        new("--abort", "Abort revert"),
                        new("--skip", "Skip current commit"),
                        new("--no-commit", "Don't create commit"),
                    ]
                },
                new("rm", "Remove files from the working tree and from the index")
                {
                    Parameters =
                    [
                        new("-f", "Force remove"),
                        new("--force", "Force remove"),
                        new("-r", "Recursively remove"),
                        new("--cached", "Remove only from index"),
                    ]
                },
                new("show", "Show various types of objects")
                {
                    Parameters =
                    [
                        new("--stat", "Show diffstat"),
                        new("--name-only", "Show only names"),
                        new("--name-status", "Show names and status"),
                    ]
                },
                new("stash", "Stash the changes in a dirty working directory away")
                {
                    SubCommands =
                    [
                        new("push", "Save local modifications to a new stash"),
                        new("pop", "Apply stash and remove from stash list"),
                        new("apply", "Apply stash without removing from stash list"),
                        new("list", "List stashes"),
                        new("show", "Show stash contents"),
                        new("drop", "Remove a single stash"),
                        new("clear", "Remove all stashes"),
                        new("branch", "Create a new branch from stash"),
                    ],
                    Parameters =
                    [
                        new("-u", "Include untracked files"),
                        new("--include-untracked", "Include untracked files"),
                        new("-a", "Include ignored files"),
                        new("--all", "Include ignored files"),
                    ]
                },
                new("status", "Show the working tree status")
                {
                    Parameters =
                    [
                        new("-s", "Short format"),
                        new("--short", "Short format"),
                        new("-b", "Show branch info"),
                        new("--branch", "Show branch info"),
                        new("--porcelain", "Machine-readable format"),
                    ]
                },
                new("switch", "Switch branches")
                {
                    Parameters =
                    [
                        new("-c", "Create and switch to new branch"),
                        new("--create", "Create and switch to new branch"),
                        new("-C", "Create/reset and switch to branch"),
                        new("-d", "Switch to a commit for inspection"),
                        new("--detach", "Switch to a commit for inspection"),
                        new("-", "Switch to previous branch"),
                    ],
                    DynamicArguments = GetBranches
                },
                new("tag", "Create, list, delete or verify a tag object signed with GPG")
                {
                    Parameters =
                    [
                        new("-a", "Create annotated tag"),
                        new("--annotate", "Create annotated tag"),
                        new("-d", "Delete tag"),
                        new("--delete", "Delete tag"),
                        new("-l", "List tags"),
                        new("--list", "List tags"),
                        new("-m", "Tag message"),
                        new("--message", "Tag message"),
                    ],
                    DynamicArguments = GetTags
                },
                new("worktree", "Manage multiple working trees")
                {
                    SubCommands =
                    [
                        new("add", "Create a new working tree"),
                        new("list", "List working trees"),
                        new("remove", "Remove a working tree"),
                        new("prune", "Prune working tree information"),
                    ]
                },
            ],
            Parameters =
            [
                new("--version", "Show git version"),
                new("--help", "Show help"),
                new("-C", "Run as if git was started in path"),
                new("-c", "Set config variable"),
            ]
        };
    }

    private static IEnumerable<DynamicArgument> GetBranches()
    {
        foreach (var line in Helpers.ExecuteCommand("git branch --format='%(refname:short)'"))
        {
            var branch = line.Trim();
            if (!string.IsNullOrWhiteSpace(branch))
            {
                yield return new DynamicArgument(branch);
            }
        }
    }

    private static IEnumerable<DynamicArgument> GetRemotes()
    {
        foreach (var line in Helpers.ExecuteCommand("git remote"))
        {
            var remote = line.Trim();
            if (!string.IsNullOrWhiteSpace(remote))
            {
                yield return new DynamicArgument(remote);
            }
        }
    }

    private static IEnumerable<DynamicArgument> GetTags()
    {
        foreach (var line in Helpers.ExecuteCommand("git tag"))
        {
            var tag = line.Trim();
            if (!string.IsNullOrWhiteSpace(tag))
            {
                yield return new DynamicArgument(tag);
            }
        }
    }
}

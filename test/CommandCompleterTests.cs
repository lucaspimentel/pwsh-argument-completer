using FluentAssertions;

namespace PowerShellArgumentCompleter.Tests;

public class CommandCompleterTests
{
    [Fact]
    public void Scoop()
    {
        CommandCompleter.GetCompletions("scoop")
                        .Should().Contain(x => x.CompletionText == "install")
                        .And.Contain(x => x.CompletionText == "update")
                        .And.Contain(x => x.CompletionText == "status")
                        .And.Contain(x => x.CompletionText == "checkup");
    }

    [Fact]
    public void Scoop_Install()
    {
        CommandCompleter.GetCompletions("scoop in")
                        .Should().Contain(x => x.CompletionText == "install")
                        .And.Contain(x => x.CompletionText == "info");
    }

    [Fact]
    public void Scoop_Update()
    {
        CommandCompleter.GetCompletions("scoop up")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("update");
    }

    [Fact]
    public void Scoop_Update_All()
    {
        CommandCompleter.GetCompletions("scoop update")
                        .Should().HaveCountGreaterThanOrEqualTo(2)
                        .And.ContainSingle(x => x.CompletionText == "*");
    }

    [Fact]
    public void Scoop_Update_z()
    {
        CommandCompleter.GetCompletions("scoop update z")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("zoxide");
    }

    [Fact]
    public void Scoop_Update_b()
    {
        CommandCompleter.GetCompletions("scoop update b")
                        .Should().HaveCount(3)
                        .And.ContainSingle(x => x.CompletionText == "bat")
                        .And.ContainSingle(x => x.CompletionText == "bottom")
                        .And.ContainSingle(x => x.CompletionText == "broot");
    }

    [Fact]
    public void Scoop_Status()
    {
        CommandCompleter.GetCompletions("scoop st")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("status");
    }

    [Fact]
    public void Scoop_Checkup()
    {
        CommandCompleter.GetCompletions("scoop ch")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("checkup");
    }

    [Fact]
    public void Git()
    {
        CommandCompleter.GetCompletions("git")
                        .Should().Contain(x => x.CompletionText == "add")
                        .And.Contain(x => x.CompletionText == "commit")
                        .And.Contain(x => x.CompletionText == "push")
                        .And.Contain(x => x.CompletionText == "pull")
                        .And.Contain(x => x.CompletionText == "status")
                        .And.Contain(x => x.CompletionText == "branch")
                        .And.Contain(x => x.CompletionText == "checkout");
    }

    [Fact]
    public void Git_Add()
    {
        CommandCompleter.GetCompletions("git ad")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("add");
    }

    [Fact]
    public void Git_Add_Parameters()
    {
        CommandCompleter.GetCompletions("git add")
                        .Should().Contain(x => x.CompletionText == "-A")
                        .And.Contain(x => x.CompletionText == "--all")
                        .And.Contain(x => x.CompletionText == "-p")
                        .And.Contain(x => x.CompletionText == ".");
    }

    [Fact]
    public void Git_Commit()
    {
        CommandCompleter.GetCompletions("git com")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("commit");
    }

    [Fact]
    public void Git_Commit_Parameters()
    {
        CommandCompleter.GetCompletions("git commit")
                        .Should().Contain(x => x.CompletionText == "-a")
                        .And.Contain(x => x.CompletionText == "--amend")
                        .And.Contain(x => x.CompletionText == "-m")
                        .And.Contain(x => x.CompletionText == "--message");
    }

    [Fact]
    public void Git_Stash()
    {
        CommandCompleter.GetCompletions("git st")
                        .Should().HaveCount(2)
                        .And.Contain(x => x.CompletionText == "stash")
                        .And.Contain(x => x.CompletionText == "status");
    }

    [Fact]
    public void Git_Stash_Subcommands()
    {
        CommandCompleter.GetCompletions("git stash")
                        .Should().Contain(x => x.CompletionText == "push")
                        .And.Contain(x => x.CompletionText == "pop")
                        .And.Contain(x => x.CompletionText == "apply")
                        .And.Contain(x => x.CompletionText == "list")
                        .And.Contain(x => x.CompletionText == "clear");
    }

    [Fact]
    public void Gh()
    {
        CommandCompleter.GetCompletions("gh")
                        .Should().Contain(x => x.CompletionText == "auth")
                        .And.Contain(x => x.CompletionText == "repo")
                        .And.Contain(x => x.CompletionText == "pr")
                        .And.Contain(x => x.CompletionText == "issue")
                        .And.Contain(x => x.CompletionText == "release");
    }

    [Fact]
    public void Gh_Repo()
    {
        CommandCompleter.GetCompletions("gh repo")
                        .Should().Contain(x => x.CompletionText == "clone")
                        .And.Contain(x => x.CompletionText == "create")
                        .And.Contain(x => x.CompletionText == "list");
    }

    [Fact]
    public void Gh_Repo_Clone()
    {
        CommandCompleter.GetCompletions("gh repo cl")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("clone");
    }

    [Fact]
    public void Gh_Auth()
    {
        CommandCompleter.GetCompletions("gh auth")
                        .Should().Contain(x => x.CompletionText == "login")
                        .And.Contain(x => x.CompletionText == "logout")
                        .And.Contain(x => x.CompletionText == "status");
    }

    [Fact]
    public void Gh_Auth_Login()
    {
        CommandCompleter.GetCompletions("gh auth login")
                        .Should().Contain(x => x.CompletionText == "-w")
                        .And.Contain(x => x.CompletionText == "--web")
                        .And.Contain(x => x.CompletionText == "-h");
    }

    [Fact]
    public void Gh_Pr()
    {
        CommandCompleter.GetCompletions("gh pr")
                        .Should().Contain(x => x.CompletionText == "create")
                        .And.Contain(x => x.CompletionText == "list")
                        .And.Contain(x => x.CompletionText == "view")
                        .And.Contain(x => x.CompletionText == "checkout")
                        .And.Contain(x => x.CompletionText == "checks")
                        .And.Contain(x => x.CompletionText == "diff");
    }

    [Fact]
    public void Gh_Pr_View()
    {
        CommandCompleter.GetCompletions("gh pr v")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("view");
    }

    [Fact]
    public void Gh_Pr_View_Parameters()
    {
        CommandCompleter.GetCompletions("gh pr view")
                        .Should().Contain(x => x.CompletionText == "-w")
                        .And.Contain(x => x.CompletionText == "--web")
                        .And.Contain(x => x.CompletionText == "-c")
                        .And.Contain(x => x.CompletionText == "--comments");
    }

    [Fact]
    public void Gh_Pr_Checks()
    {
        CommandCompleter.GetCompletions("gh pr checks")
                        .Should().Contain(x => x.CompletionText == "-w")
                        .And.Contain(x => x.CompletionText == "--web");
    }

    [Fact]
    public void Gh_Pr_Diff()
    {
        CommandCompleter.GetCompletions("gh pr di")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("diff");
    }

    [Fact]
    public void Tre()
    {
        CommandCompleter.GetCompletions("tre")
                        .Should().Contain(x => x.CompletionText == "-a")
                        .And.Contain(x => x.CompletionText == "--all")
                        .And.Contain(x => x.CompletionText == "-d")
                        .And.Contain(x => x.CompletionText == "--directories")
                        .And.Contain(x => x.CompletionText == "-j")
                        .And.Contain(x => x.CompletionText == "--json");
    }

    [Fact]
    public void Tre_All()
    {
        CommandCompleter.GetCompletions("tre --a")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("--all");
    }

    [Fact]
    public void Tre_Color()
    {
        CommandCompleter.GetCompletions("tre --color")
                        .Should().Contain(x => x.CompletionText == "automatic")
                        .And.Contain(x => x.CompletionText == "always")
                        .And.Contain(x => x.CompletionText == "never");
    }

    [Fact]
    public void Lsd()
    {
        CommandCompleter.GetCompletions("lsd")
            .Should().Contain(x => x.CompletionText == "-a")
            .And.Contain(x => x.CompletionText == "--all")
            .And.Contain(x => x.CompletionText == "-l")
            .And.Contain(x => x.CompletionText == "--long")
            .And.Contain(x => x.CompletionText == "--tree")
            .And.Contain(x => x.CompletionText == "--help");
    }

    [Fact]
    public void Lsd_Color()
    {
        CommandCompleter.GetCompletions("lsd --color")
            .Should().Contain(x => x.CompletionText == "always")
            .And.Contain(x => x.CompletionText == "auto")
            .And.Contain(x => x.CompletionText == "never");
    }

    [Fact]
    public void Lsd_Icon()
    {
        CommandCompleter.GetCompletions("lsd --icon")
            .Should().Contain(x => x.CompletionText == "always")
            .And.Contain(x => x.CompletionText == "auto")
            .And.Contain(x => x.CompletionText == "never");
    }

    [Fact]
    public void Lsd_Sort()
    {
        CommandCompleter.GetCompletions("lsd --sort")
            .Should().Contain(x => x.CompletionText == "size")
            .And.Contain(x => x.CompletionText == "time")
            .And.Contain(x => x.CompletionText == "version")
            .And.Contain(x => x.CompletionText == "extension")
            .And.Contain(x => x.CompletionText == "git")
            .And.Contain(x => x.CompletionText == "none");
    }

    [Fact]
    public void Lsd_Permission()
    {
        CommandCompleter.GetCompletions("lsd --permission")
            .Should().Contain(x => x.CompletionText == "rwx")
            .And.Contain(x => x.CompletionText == "octal")
            .And.Contain(x => x.CompletionText == "attributes")
            .And.Contain(x => x.CompletionText == "disable");
    }
}
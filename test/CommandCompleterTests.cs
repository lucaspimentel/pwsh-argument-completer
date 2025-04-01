using FluentAssertions;

namespace PowerShellArgumentCompleter.Tests;

public class CommandCompleterTests
{
    [Fact]
    public void Scoop()
    {
        CommandCompleter.GetCompletions("scoop")
                        .Should().HaveCount(4)
                        .And.ContainSingle(x => x.CompletionText == "install")
                        .And.ContainSingle(x => x.CompletionText == "update")
                        .And.ContainSingle(x => x.CompletionText == "status")
                        .And.ContainSingle(x => x.CompletionText == "checkup");
    }

    [Fact]
    public void Scoop_Install()
    {
        CommandCompleter.GetCompletions("scoop in")
                        .Should().ContainSingle()
                        .Which.CompletionText.Should().Be("install");
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
}

using FluentAssertions;

namespace PowerShellArgumentCompleter.Tests;

public class CommandCompleterTests
{
    [Fact]
    public void Scoop()
    {
        CommandCompleter.GetCompletions("scoop")
                        .Should().HaveCount(4)
                        .And.ContainSingle(x => x.Name == "install")
                        .And.ContainSingle(x => x.Name == "update")
                        .And.ContainSingle(x => x.Name == "status")
                        .And.ContainSingle(x => x.Name == "checkup");
    }

    [Fact]
    public void Scoop_Install()
    {
        CommandCompleter.GetCompletions("scoop in")
                        .Should().ContainSingle()
                        .Which.Name.Should().Be("install");
    }

    [Fact]
    public void Scoop_Update()
    {
        CommandCompleter.GetCompletions("scoop up")
                        .Should().ContainSingle()
                        .Which.Name.Should().Be("update");
    }

    [Fact]
    public void Scoop_Update_All()
    {
        CommandCompleter.GetCompletions("scoop update")
                        .Should().ContainSingle()
                        .Which.Name.Should().Be("*");
    }

    [Fact]
    public void Scoop_Status()
    {
        CommandCompleter.GetCompletions("scoop st")
                        .Should().ContainSingle()
                        .Which.Name.Should().Be("status");
    }

    [Fact]
    public void Scoop_Checkup()
    {
        CommandCompleter.GetCompletions("scoop ch")
                        .Should().ContainSingle()
                        .Which.Name.Should().Be("checkup");
    }
}

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a PowerShell argument completer written in C# that provides tab completion for various CLI tools (scoop, winget, az, azd, func, chezmoi, git, gh, VS Code). It's compiled to a native executable using NativeAOT for fast startup times, making it suitable as a PowerShell argument completer.

The tool integrates with PowerShell via `Register-ArgumentCompleter -Native` and receives three arguments: `$wordToComplete`, `$commandAst`, and `$cursorPosition`.

## Build and Test Commands

**Build:**
```bash
dotnet build
```

**Test:**
```bash
dotnet test
```

**Publish (native executable):**
```bash
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r win-x64 -o src/publish
```

The native executable will be at `src/publish/pwsh-argument-completer.exe`.

**Run tests with a specific filter:**
```bash
dotnet test --filter "FullyQualifiedName~ScoopCommand"
```

## Architecture

### Core Components

**Program.cs (Entry Point)**
- Receives 3 arguments from PowerShell: `wordToComplete`, `commandAst`, `cursorPosition`
- Truncates `commandAst` to `cursorPosition` to handle mid-line completions
- Calls `CommandCompleter.GetCompletions()` and outputs results in format: `completionText|toolTip`
- Supports debug mode via `DEBUG=1` environment variable

**CommandCompleter.cs (Main Logic)**
- Parses the command line to identify the root command (scoop, winget, az, etc.)
- Splits arguments and walks the completion tree to find the current node
- Returns appropriate completions based on current position

**Completion System (`src/Completions/`)**
- `ICompletion`: Base interface with `CompletionText`, `Tooltip`, and `FindNode()`
- `ICompletionWithChildren`: Extends ICompletion with `GetCompletions()`
- `Command`: Main completion node, supports SubCommands, Parameters, and DynamicArguments
- `CommandParameter`: For flags/parameters (e.g., `--local`, `-l`)
- `StaticArgument`: Fixed completion values
- `DynamicArgument`: Runtime-generated completions (e.g., from `scoop list`)

**Known Completions (`src/KnownCompletions/`)**
- Each supported CLI tool has its own file (e.g., `ScoopCommand.cs`, `WingetCommand.cs`)
- Azure tools are in `KnownCompletions/Azure/` subdirectory
- Command definitions use C# collection expressions for clean, declarative syntax
- Dynamic completions execute commands via `Helpers.ExecuteCommand()` which shells out to `pwsh.exe`

**Helpers.cs**
- Case-insensitive string comparison helpers using `ReadOnlySpan<char>`
- `ExecuteCommand()`: Executes PowerShell commands to get dynamic completion data
- Performance-optimized with `AggressiveInlining` for hot paths

### Tree-Walking Algorithm

The completer builds a tree of completion nodes. When parsing `scoop update bat`:
1. Match "scoop" → Find `ScoopCommand`
2. Match "update" → Navigate to update subcommand
3. Return completions from update node (which includes DynamicArguments from installed packages)

### Adding New Commands

To add a new command completer:
1. Create a new file in `src/KnownCompletions/` (e.g., `MyToolCommand.cs`)
2. Implement a static `Create()` method returning a `Command` with SubCommands/Parameters
3. Add the command to the switch statement in `CommandCompleter.GetCompletions()` (line 22-32)
4. For dynamic completions, add a `DynamicArguments` property with a factory method

Example:
```csharp
new("update", "Update packages")
{
    Parameters = [new("--all", "Update all packages")],
    DynamicArguments = GetInstalledPackages
}

private static IEnumerable<DynamicArgument> GetInstalledPackages()
{
    foreach (var line in Helpers.ExecuteCommand("mytool list"))
    {
        yield return new DynamicArgument(line.Trim());
    }
}
```

## Project Configuration

- **Target Framework:** .NET 9.0
- **Output:** `pwsh-argument-completer.exe` (NativeAOT)
- **Solution:** Uses Visual Studio `.slnx` format
- **Project Name:** `PowerShellArgumentCompleter` (C# namespace)
- **Assembly Name:** `pwsh-argument-completer` (output filename)

### NativeAOT Settings

The project uses aggressive size and speed optimizations:
- `PublishAot=true`
- `InvariantGlobalization=true` (no localization)
- `DebuggerSupport=false`
- Various trimming options to minimize binary size

## Testing

Tests use xUnit and FluentAssertions. Tests verify:
- Command matching (exact and prefix)
- Subcommand navigation
- Dynamic argument completion
- Parameter suggestions

Test pattern:
```csharp
CommandCompleter.GetCompletions("scoop up")
    .Should().ContainSingle()
    .Which.CompletionText.Should().Be("update");
```

## Important Notes

- All string comparisons are **case-insensitive**
- Command parsing uses `ReadOnlySpan<char>` for performance (zero-copy slicing)
- Commands are matched by exact name, not prefix (handled by `FindNode()`)
- The `.exe` suffix is stripped when detecting the command name
- Debug logs go to `Logger.Write()` which outputs to a log file when `DEBUG=1` is set

## Supported Commands

Currently implemented completions:
- **scoop** - Package manager for Windows
- **winget** - Windows Package Manager
- **az** - Azure CLI
- **azd** - Azure Developer CLI (✅ already supported)
- **func** - Azure Functions Core Tools (✅ already supported)
- **chezmoi** - Dotfiles manager
- **git** - Version control
- **gh** - GitHub CLI
- **code** - VS Code

## Future Command Candidates

Based on PowerShell history analysis, these commands are recommended for future implementation:

### High Priority

1. **dotnet** (1,530+ uses) - Most used command
   - Subcommands: `build`, `publish`, `restore`, `run`, `tool`, `nuget`, `new`, `--info`, `--list-sdks`, `--list-runtimes`
   - Parameters: `-c`, `-tl`, `-f`, `--help`, `--global`

2. **docker** (213+ uses)
   - Subcommands: `build`, `run`, `login`, `buildx`, `image`
   - Common flags: `--file`, `--tag`, `--progress`, `--no-cache`, `--rm`, `--name`, `-e`, `-p`

3. **python** (40+ uses)
   - Modules: `-m venv`, `-m pytest`
   - Common flags: `-n`, `-S`, `--version`

### Medium Priority

4. **dd-trace** (102+ uses) - Datadog tracer tool
   - Subcommands: `run`
   - Flags: `--help`, `--tracer-home`

5. **ssh** (35+ uses) - Could complete hostnames from SSH config

### Lower Priority (Niche/Less Complex)

- **tre** (254+ uses) - Tree viewer with improved output (TODO: add support)
- **lsd** (237+ uses) - LSDeluxe, modern `ls` replacement (TODO: add support)
- **gt** (126+ uses) - Graphite CLI for stacked diffs (TODO: add support)
- **dust** (120+ uses) - Modern `du` replacement (TODO: add support)
- **lazygit** (76+ uses) - No arguments typically
- **xh** (132+ uses) - HTTP client, could complete HTTP methods
- **fzf** (48+ uses) - Interactive, less benefit from completion

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a cross-platform PowerShell argument completer written in C# that provides tab completion for various CLI tools (git, gh, az, azd, func, chezmoi, VS Code, and Windows-only: scoop, winget). It's compiled to a native executable using NativeAOT for fast startup times, making it suitable as a PowerShell argument completer.

The tool integrates with PowerShell via `Register-ArgumentCompleter -Native` and receives three arguments: `$wordToComplete`, `$commandAst`, and `$cursorPosition`.

**Supported platforms:** Windows, Linux, macOS

## Build and Test Commands

**Build:**
```bash
dotnet build
```

**Test:**
```bash
dotnet test
```

**Install (automated):**
```bash
./install.ps1
```

The automated installation script (`install.ps1`) will:
- Detect your platform automatically (Windows, macOS Intel/ARM, Linux)
- Build the native executable for the appropriate runtime identifier
- Install both the executable and module files to `~/.local/pwsh-modules/PwshArgumentCompleter`
- Provide instructions for adding the module to your PowerShell profile

**Publish (manual):**
```bash
# Windows
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r win-x64 -o src/publish

# Linux
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r linux-x64 -o src/publish

# macOS (Intel)
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r osx-x64 -o src/publish

# macOS (Apple Silicon)
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r osx-arm64 -o src/publish
```

The native executable will be at `src/publish/pwsh-argument-completer` (or `.exe` on Windows).

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
- Dynamic completions execute commands via `Helpers.ExecuteCommand()` which executes commands directly (e.g., `git branch`, `scoop list`)

**Helpers.cs**
- Case-insensitive string comparison helpers using `ReadOnlySpan<char>`
- `ExecuteCommand()`: Executes commands directly to get dynamic completion data (cross-platform, no PowerShell dependency)
- Performance-optimized with `AggressiveInlining` for hot paths

**PowerShell Module (`module/`)**
- `PwshArgumentCompleter.psm1`: PowerShell module that registers argument completers for supported commands
- Loads the executable from the module directory (no PATH dependency)
- Platform-aware: automatically detects Windows vs Unix and uses appropriate executable name
- Registers completers only for commands that are actually installed on the system
- Self-contained: executable is bundled with module files for easy distribution

### Tree-Walking Algorithm

The completer builds a tree of completion nodes. When parsing `scoop update bat`:
1. Match "scoop" → Find `ScoopCommand`
2. Match "update" → Navigate to update subcommand
3. Return completions from update node (which includes DynamicArguments from installed packages)

### Adding New Commands

**Step-by-step process for implementing a new command:**

1. **Gather command documentation**
   - Run `<command> --help` to get main subcommands
   - For each subcommand, run `<command> <subcommand> --help` to get parameters
   - Note all short/long aliases (e.g., `-h`/`--silent`)
   - Identify parameters with static argument values (e.g., `--scope user|machine`)

2. **Create the command file**
   - Create a new file in `src/KnownCompletions/` (e.g., `MyToolCommand.cs`)
   - Use the help output as comments at the top of the file for reference
   - Implement a static `Create()` method returning a `Command` with SubCommands/Parameters

3. **Register the command**
   - Add the command to the switch statement in `CommandCompleter.GetCompletions()` (lines 25-39)
   - Format: `"commandname" => MyToolCommand.Create(),`
   - If the command is Windows-only (like `scoop` or `winget`), add a platform guard: `"commandname" when OperatingSystem.IsWindows() => MyToolCommand.Create(),`

4. **Add tests**
   - Add comprehensive tests in `test/CommandCompleterTests.cs`
   - Test subcommand completion, parameter suggestions, and static arguments
   - Test prefix matching (e.g., `"command sub"` should complete to `"subcommand"`)

5. **Verify**
   - Run `dotnet test --filter "FullyQualifiedName~MyTool"` to run your tests
   - Build with `dotnet build -c Release`
   - Test manually: `src/bin/Release/net9.0/pwsh-argument-completer.exe "" "command " 8`

**For dynamic completions:**
- Add a `DynamicArguments` property with a factory method that yields `DynamicArgument` instances
- Use `Helpers.ExecuteCommand(executable, arguments)` to run commands and parse output
- Examples in the codebase:
  - `scoop update` → lists installed scoop packages via `ExecuteCommand("scoop", "list")`
  - `git checkout` → lists git branches via `ExecuteCommand("git", "branch --format='%(refname:short)'")`
  - `git push/fetch` → lists git remotes via `ExecuteCommand("git", "remote")`
  - `git tag` → lists git tags via `ExecuteCommand("git", "tag")`

**Important: Parameter Alias Pattern**

When a command has both short (`-x`) and long (`--xxx`) forms of the same parameter, **merge them into a single parameter** using the `Alias` property:

- Use the **long form** as the main `CompletionText`
- Add the **short form** to the `Alias` property
- Include the **short form in parentheses** at the end of the tooltip

This ensures:
- No duplicate entries in completion results
- Both forms work when the user types them
- Clear documentation of which short form corresponds to each long form

**Example:**
```csharp
// ❌ OLD WAY (duplicates):
new("-a", "Update all packages"),
new("--all", "Update all packages"),

// ✅ NEW WAY (merged):
new("--all", "Update all packages (-a)") { Alias = "-a" },
```

**Full example with nested properties:**
```csharp
new("update", "Update packages")
{
    Parameters =
    [
        new("--all", "Update all packages (-a)") { Alias = "-a" },
        new("--force", "Force update (-f)") { Alias = "-f" },
        new("--output", "Output format (-o)")
        {
            Alias = "-o",
            StaticArguments =
            [
                new("json", "JSON format"),
                new("text", "Text format")
            ]
        }
    ],
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

**How it works:**
- `CommandParameter.Alias` property stores the short form
- `Command.GetCompletions()` returns only the long form in completion results
- When user types a short form, the long form is suggested
- `Helpers.FindEquals()` checks both `CompletionText` and `Alias` when matching user input
- Result: cleaner completion lists with no duplicates, but both forms still work when typed

### Quick Reference: Common Patterns

**Basic command with subcommands:**
```csharp
public static Command Create()
{
    return new Command("mycommand")
    {
        SubCommands =
        [
            new("install", "Install a package"),
            new("remove", "Remove a package"),
            new("list", "List packages")
        ]
    };
}
```

**Command with parameters:**
```csharp
new("install", "Install a package")
{
    Parameters =
    [
        new("--force", "Force installation (-f)") { Alias = "-f" },
        new("--quiet", "Quiet mode (-q)") { Alias = "-q" }
    ]
}
```

**Parameters with static values:**
```csharp
new("--scope", "Installation scope")
{
    StaticArguments =
    [
        new("user", "User scope"),
        new("machine", "Machine scope")
    ]
}
```

**Parameters with nested arguments:**
```csharp
new("--color", "Color mode (-c)")
{
    Alias = "-c",
    StaticArguments =
    [
        new("auto", "Automatic color detection"),
        new("always", "Always use colors"),
        new("never", "Never use colors")
    ]
}
```

**Nested subcommands:**
```csharp
new("config", "Configuration management")
{
    SubCommands =
    [
        new("get", "Get configuration value"),
        new("set", "Set configuration value"),
        new("list", "List all configuration")
    ],
    Parameters =
    [
        new("--global", "Use global configuration (-g)") { Alias = "-g" }
    ]
}
```

**Dynamic completions:**
```csharp
new("update", "Update packages")
{
    Parameters =
    [
        new("--all", "Update all packages")
    ],
    DynamicArguments = GetInstalledPackages
}

private static IEnumerable<DynamicArgument> GetInstalledPackages()
{
    foreach (var line in Helpers.ExecuteCommand("mycommand", "list --short"))
    {
        var packageName = line.Trim();
        if (!string.IsNullOrWhiteSpace(packageName))
        {
            yield return new DynamicArgument(packageName);
        }
    }
}
```

## Project Configuration

- **Target Framework:** .NET 9.0
- **Output:** `pwsh-argument-completer` (NativeAOT, `.exe` extension on Windows)
- **Solution:** Uses Visual Studio `.slnx` format
- **Project Name:** `PowerShellArgumentCompleter` (C# namespace)
- **Assembly Name:** `pwsh-argument-completer` (output filename)
- **Platforms:** Windows (win-x64), Linux (linux-x64), macOS (osx-x64, osx-arm64)

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

Currently implemented completions with their completion level:

### Fully Implemented (with parameters and nested subcommands)
- **git** - Version control with common subcommands and parameters (cross-platform)
  - 🔄 Dynamic completions: branches, remotes, tags
- **gh** - GitHub CLI with subcommands and parameters (cross-platform)
- **tre** - Tree viewer with parameters and static arguments (cross-platform)
- **lsd** - LSDeluxe with parameters and static arguments (cross-platform)
- **dust** - Disk usage tool with parameters and static arguments (cross-platform)
- **winget** - Windows Package Manager (Windows-only)
  - All major subcommands: install, upgrade, uninstall, search, list, show, export, import, download
  - Nested subcommands: source (add/list/update/remove/reset/export), pin (add/remove/list/reset)
  - Parameters with aliases (e.g., `-h`/`--silent`, `-i`/`--interactive`)
  - Static arguments for scope (user/machine), architecture (x86/x64/arm/arm64), installer-type

### Basic Implementation (subcommands only, parameters needed)
- **az** - Azure CLI (subcommands only, cross-platform)
- **azd** - Azure Developer CLI (subcommands only, cross-platform)
- **func** - Azure Functions Core Tools (subcommands only, cross-platform)
- **chezmoi** - Dotfiles manager (subcommands only, cross-platform)
- **code** - VS Code (basic parameters, cross-platform)
- **scoop** - Package manager (Windows-only)
  - 🔄 Dynamic completions: installed packages for `scoop update`

**Note:** When adding any new command, remember to also register it in the PowerShell module (`module/PwshArgumentCompleter.psm1`). If the command is platform-specific, add appropriate platform guards. See README.md for the list of future command candidates.

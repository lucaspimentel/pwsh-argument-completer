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
- Use `Helpers.ExecuteCommand()` to run commands and parse output
- Example: installed packages, available plugins, configured sources

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
    foreach (var line in Helpers.ExecuteCommand("mycommand list --short"))
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

Currently implemented completions with their completion level:

### Fully Implemented (with parameters and nested subcommands)
- **winget** - Windows Package Manager
  - All major subcommands: install, upgrade, uninstall, search, list, show, export, import, download
  - Nested subcommands: source (add/list/update/remove/reset/export), pin (add/remove/list/reset)
  - Parameters with aliases (e.g., `-h`/`--silent`, `-i`/`--interactive`)
  - Static arguments for scope (user/machine), architecture (x86/x64/arm/arm64), installer-type
- **git** - Version control with common subcommands and parameters
- **gh** - GitHub CLI with subcommands and parameters
- **tre** - Tree viewer with parameters and static arguments
- **lsd** - LSDeluxe with parameters and static arguments
- **dust** - Disk usage tool with parameters and static arguments

### Basic Implementation (subcommands only, parameters needed)
- **scoop** - Package manager for Windows (has dynamic completions for installed packages)
- **az** - Azure CLI (subcommands only)
- **azd** - Azure Developer CLI (subcommands only)
- **func** - Azure Functions Core Tools (subcommands only)
- **chezmoi** - Dotfiles manager (subcommands only)
- **code** - VS Code (basic parameters)

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

- **gt** (126+ uses) - Graphite CLI for stacked diffs (TODO: add support)
- **lazygit** (76+ uses) - No arguments typically
- **xh** (132+ uses) - HTTP client, could complete HTTP methods
- **fzf** (48+ uses) - Interactive, less benefit from completion
- when adding support for a new command-line tool, also add it to C:\Users\Lucas.Pimentel\.config\powershell\Microsoft.PowerShell_profile.ps1

## What's Next

Unsupported CLI tools from current scoop installation (identified 2025-10-13):

### High-Value Candidates

**Most Useful Additions:**
- **bat** - Modern cat replacement with syntax highlighting (parameters: `-l`, `--language`, `--theme`, `--style`)
- **fd** - Modern find replacement (parameters: `-t`, `--type`, `-e`, `--extension`, `-d`, `--max-depth`, `-H`, `--hidden`)
- **jq** - JSON processor (filters, built-in functions)
- **ripgrep** (`rg`) - Fast grep alternative (parameters: `-i`, `-t`, `--type`, `-g`, `--glob`, `-A`, `-B`, `-C`)
- **zoxide** (`z`) - Smart directory jumper (subcommands: `add`, `remove`, `query`)
- **delta** - Git diff viewer (parameters: `--side-by-side`, `--line-numbers`, `--theme`)
- **gsudo** - Sudo for Windows (subcommands: `config`, `cache`)
- **aws-vault** - AWS credential manager (subcommands: `add`, `exec`, `list`, `login`, `remove`)

**Development Tools:**
- **1password-cli** (`op`) - 1Password CLI (subcommands: `item`, `document`, `user`, `vault`)
- **dive** - Docker image explorer (parameters: `--ci`, `--source`)
- **git-machete** - Git branch management (subcommands: `add`, `delete`, `discover`, `status`, `traverse`)
- **neovim** (`nvim`) - Text editor (parameters: `-o`, `-O`, `-p`, `--cmd`)
- **helix** (`hx`) - Modern text editor (parameters: `-g`, `--grammar`, `-w`, `--working-dir`)
- **micro** - Terminal text editor

**System Utilities:**
- **bottom** (`btm`) - System monitor (parameters: `-b`, `--basic`, `-t`, `--tree`, `-g`, `--group`)
- **broot** - Modern tree/directory navigator (parameters: `-h`, `--hidden`, `-s`, `--sizes`)
- **duf** - Modern df replacement (parameters: `-a`, `--all`, `-j`, `--json`, `--only`)
- **procs** - Modern ps replacement (parameters: `-a`, `--and`, `-o`, `--or`, `--tree`)
- **glow** - Markdown renderer (parameters: `-p`, `--pager`, `-s`, `--style`)

**Specialized Tools:**
- **kalk** - Calculator/scripting language
- **ffmpeg** - Video/audio processing (complex, many parameters)
- **speedtest-cli** - Internet speed test

### Lower Priority

**System/Archive Tools:**
- **7zip** - Archive manager (already has good native completion)
- **nuget** - .NET package manager (parameters: `install`, `list`, `push`, `sources`)
- **scoop-search** - Extended scoop search

**Utilities:**
- **dark** - Dark mode toggler
- **less** - Pager (interactive)
- **mediainfo** - Media file info
- **tesseract** - OCR engine
- **lazygit** - Git TUI (interactive, less benefit from completion)
- **fzf** - Fuzzy finder (interactive, less benefit from completion)

**Note:** When adding any new command, remember to also register it in `C:\Users\Lucas.Pimentel\.config\powershell\Microsoft.PowerShell_profile.ps1`
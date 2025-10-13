# PowerShell Argument Completer

A fast, native PowerShell argument completer for common CLI tools, written in C# and compiled to a native executable using NativeAOT for optimal performance.

## What is this?

This tool provides intelligent tab completion for various command-line tools in PowerShell. Instead of typing out full command names, options, and arguments, you can use Tab to complete them automatically with helpful tooltips showing what each option does.

### Supported Commands

**Fully supported with parameters and nested subcommands:**
- **git** - Version control (add, commit, push, pull, status, branch, checkout, etc.)
- **gh** - GitHub CLI (pr, issue, repo, auth, etc.)
- **tre** - Tree viewer with improved output
- **lsd** - LSDeluxe, modern `ls` replacement
- **dust** - Disk usage analyzer
- **winget** - Windows Package Manager (Windows only)

**Basic support (subcommands only, parameters coming soon):**
- **az** - Azure CLI
- **azd** - Azure Developer CLI
- **func** - Azure Functions Core Tools
- **chezmoi** - Dotfiles manager
- **code** - VS Code
- **scoop** - Package manager (Windows only)

### Features

- **Fast startup**: Native executable compiled with NativeAOT
- **Smart completions**: Shows only relevant options based on what you've typed
- **Helpful tooltips**: Describes what each command/parameter does
- **Alias support**: Both short (`-h`) and long (`--help`) forms work seamlessly
- **Static arguments**: Suggests valid values for parameters (e.g., `--scope` → `user` or `machine`)
- **Nested subcommands**: Handles complex command structures (e.g., `winget source add`)
- **Dynamic completions**: Context-aware suggestions based on your environment
  - `scoop update <Tab>` lists your installed scoop packages
  - `git checkout <Tab>` lists your git branches
  - `git push <Tab>` lists your git remotes

## Installation

### 1. Build the native executable

```bash
# Clone the repository
git clone https://github.com/lucaspimentel/pwsh-argument-completer.git
cd pwsh-argument-completer

# Publish as a native executable using NativeAOT
# Windows:
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r win-x64

# Linux:
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r linux-x64

# macOS (Intel):
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r osx-x64

# macOS (Apple Silicon):
dotnet publish src/PowerShellArgumentCompleter.csproj -c Release -r osx-arm64
```

> **Note:** Using `dotnet publish` with NativeAOT produces a self-contained native executable with no runtime dependencies and faster startup times. Regular `dotnet build` produces a managed executable that requires the .NET runtime.

### 2. Install the files

Copy the executable and PowerShell module to your local directories:

```powershell
# Windows - Copy the executable to your PATH
Copy-Item src/bin/Release/net9.0/win-x64/publish/pwsh-argument-completer.exe ~/.local/bin/

# Linux/macOS - Copy the executable to your PATH (adjust RID as needed)
# cp src/bin/Release/net9.0/linux-x64/publish/pwsh-argument-completer ~/.local/bin/

# Copy the PowerShell module (all platforms)
Copy-Item module/* ~/.local/lib/PwshArgumentCompleter/ -Recurse -Force
```

### 3. Add to your PowerShell profile

Add a single line to your PowerShell profile (`$PROFILE`):

```powershell
Import-Module ~/.local/lib/PwshArgumentCompleter/PwshArgumentCompleter.psd1
```

**Alternative (manual registration):** If you prefer not to use the module, you can manually register the completions:

<details>
<summary>Click to expand manual registration code</summary>

```powershell
# Custom PowerShell parameter completion
if (Get-Command 'pwsh-argument-completer' -ErrorAction SilentlyContinue) {
   $commands = @('git', 'code', 'az', 'azd', 'func', 'chezmoi', 'gh', 'tre', 'lsd', 'dust')

   # Add Windows-specific commands
   if ($IsWindows -or $PSVersionTable.PSVersion.Major -lt 6) {
      $commands += @('scoop', 'winget')
   }

   foreach ($command in $commands) {
      if (Get-Command $command -ErrorAction SilentlyContinue) {
         Register-ArgumentCompleter -Native -CommandName $command -ScriptBlock {
            param($wordToComplete, $commandAst, $cursorPosition)

            pwsh-argument-completer "$wordToComplete" "$($commandAst.ToString())" "$cursorPosition" | ForEach-Object {
               $parts = $_.Split('|')
               [System.Management.Automation.CompletionResult]::new($parts[0], $parts[0], 'ParameterValue', $parts[1])
            }
         }
      }
   }

   Remove-Item Variable:command -Force -ErrorAction SilentlyContinue
   Remove-Item Variable:commands -Force -ErrorAction SilentlyContinue
}
```

</details>

### 4. Reload your profile

```powershell
. $PROFILE
```

## Usage

Simply start typing a supported command and press Tab to see completions:

```powershell
# Complete subcommands
winget ins<Tab>  # completes to "install"

# Complete parameters
winget install --sil<Tab>  # completes to "--silent"

# See available options
winget install <Tab>  # shows all available parameters with descriptions

# Complete nested subcommands
winget source <Tab>  # shows: add, list, update, remove, reset, export

# Complete static argument values
winget install --scope <Tab>  # shows: user, machine
```

### Examples

**Installing a package silently:**
```powershell
PS> winget install <package> --<Tab>
# Shows all parameters: --silent, --interactive, --scope, --architecture, etc.

PS> winget install <package> --scope <Tab>
# Shows: user, machine
```

**Managing git repositories:**
```powershell
PS> git <Tab>
# Shows: add, commit, push, pull, status, branch, checkout, etc.

PS> git commit --<Tab>
# Shows: --all, --amend, --message, etc.
```

**GitHub CLI operations:**
```powershell
PS> gh pr <Tab>
# Shows: create, list, view, checkout, checks, diff, etc.

PS> gh pr view --<Tab>
# Shows: --web, --comments, etc.
```

**Dynamic completions (context-aware):**
```powershell
PS> scoop update <Tab>
# Shows your installed scoop packages: git, python, nodejs, etc.

PS> git checkout <Tab>
# Shows your git branches: main, develop, feature/new-feature, etc.

PS> git push origin <Tab>
# Shows your git branches for pushing

PS> git fetch <Tab>
# Shows your git remotes: origin, upstream, etc.
```

## How It Works

The completer uses PowerShell's native argument completer API (`Register-ArgumentCompleter -Native`). When you press Tab:

1. PowerShell calls the registered script block with:
   - `$wordToComplete` - The partial word being completed
   - `$commandAst` - The full command line as typed
   - `$cursorPosition` - Current cursor position

2. The script block invokes `pwsh-argument-completer.exe` with these parameters

3. The executable parses the command line and returns completions in `text|tooltip` format

4. PowerShell displays the completions with their tooltips

## Performance

The tool is optimized for speed:
- **Native executable**: No JIT compilation overhead
- **Fast startup**: NativeAOT compilation
- **Efficient parsing**: Uses `ReadOnlySpan<char>` for zero-copy string operations
- **Minimal allocations**: Carefully designed for low memory footprint

Typical completion times are <10ms on modern hardware.

## Future Command Candidates

Want to help expand support? Here are commands that would benefit from completions, roughly prioritized by usefulness:

### High Priority

**Development Tools:**
1. **dotnet** - .NET SDK (most used command in PowerShell history analysis)
   - Subcommands: `build`, `publish`, `restore`, `run`, `test`, `add`, `remove`, `new`
   - Already has some native completion but could be enhanced
2. **npm** - Node package manager
   - Subcommands: `install`, `run`, `test`, `publish`, `init`, `update`, `uninstall`
   - Parameters: `--save-dev`, `--global`, `--production`, `--force`
3. **cargo** - Rust package manager
   - Subcommands: `build`, `run`, `test`, `publish`, `install`, `update`, `check`
   - Parameters: `--release`, `--target`, `--features`, `--all-features`
4. **docker** - Container management
   - Subcommands: `build`, `run`, `ps`, `images`, `pull`, `push`, `exec`, `logs`
   - Parameters: `--file`, `--tag`, `--detach`, `--volume`, `--env`, `--port`
5. **python** - Python interpreter
   - Modules: `-m venv`, `-m pip`, `-m pytest`, `-m http.server`
   - Parameters: `-c`, `-m`, `-u`, `-O`, `-B`

**Modern CLI Tools (from scoop):**
- **bat** - Cat with syntax highlighting (`--language`, `--theme`, `--style`, `--line-range`)
- **fd** - Modern find (`--type`, `--extension`, `--hidden`, `--max-depth`, `--exec`)
- **ripgrep** (`rg`) - Fast grep (`--type`, `--glob`, `--ignore-case`, `-A`, `-B`, `-C`)
- **jq** - JSON processor (complex but powerful filter language)
- **zoxide** (`z`) - Smart cd with history (`add`, `remove`, `query`, `import`)

**Security & Auth:**
- **1password-cli** (`op`) - 1Password CLI (`item`, `document`, `vault`, `user`, `signin`)
- **aws-vault** - AWS credential manager (`add`, `exec`, `list`, `login`, `remove`)

### Medium Priority

**System Tools:**
- **podman** - Docker alternative (similar commands to docker)
- **gsudo** - Sudo for Windows (`config`, `cache`, `status`)
- **ssh** - SSH client (could complete hostnames from SSH config)

**Development Utilities:**
- **git-machete** - Git branch management (`add`, `delete`, `discover`, `status`, `traverse`)
- **delta** - Git diff viewer (`--side-by-side`, `--line-numbers`, `--theme`)
- **dive** - Docker image explorer (`--ci`, `--source`)

**Modern Replacements:**
- **bottom** (`btm`) - System monitor (`--basic`, `--tree`, `--group`)
- **broot** - Directory navigator (`--hidden`, `--sizes`, `--dates`)
- **duf** - Disk usage (`--all`, `--json`, `--only`)
- **procs** - Process viewer (`--and`, `--or`, `--tree`, `--watch`)
- **glow** - Markdown renderer (`--pager`, `--style`, `--width`)

**Editors:**
- **neovim** (`nvim`) - Text editor (`-o`, `-O`, `-p`, `--cmd`, `--startuptime`)
- **helix** (`hx`) - Modern text editor (`--grammar`, `--working-dir`, `--config`)
- **micro** - Terminal text editor

### Lower Priority

**Specialized/Niche:**
- **7z** - Archive tool (has native completion but could be enhanced)
- **ffmpeg** - Media processing (extremely complex, many parameters)
- **gt** - Graphite stacked diffs
- **xh** - HTTP client (like httpie)
- **kalk** - Calculator/scripting language
- **speedtest-cli** - Internet speed test

**Interactive/GUI (less benefit):**
- **lazygit** - Git TUI (interactive, minimal benefit)
- **fzf** - Fuzzy finder (interactive, minimal benefit)

## Contributing

Contributions are welcome! See [CLAUDE.md](CLAUDE.md) for detailed information about the architecture and how to add support for new commands.

### Quick start for contributors:

1. Fork the repository
2. Add your command implementation in `src/KnownCompletions/`
3. Register it in `CommandCompleter.GetCompletions()`
4. Add tests in `test/CommandCompleterTests.cs`
5. Run tests: `dotnet test`
6. Submit a pull request

When adding a new command, remember to also register it in your PowerShell profile (`$PROFILE`).

## Requirements

- **.NET 9.0 SDK** (for building)
- **PowerShell 5.1+** (Windows) or **PowerShell Core 7+** (all platforms)
- **Supported platforms:** Windows, Linux, macOS

## Future Features

### Distribution & Installation
- **Scoop manifest** - Create a Scoop package using the `psmodule` installer type to handle both the executable and PowerShell module installation automatically (`scoop install pwsh-argument-completer`)
- **PowerShell Gallery** - Publish to PSGallery for easy installation via `Install-Module PwshArgumentCompleter`
- **Automated install script** - One-liner installation from GitHub releases
- **Homebrew formula** - macOS/Linux package manager support

## License

MIT License - see LICENSE file for details

## Credits

Created by [Lucas Pimentel](https://github.com/lucaspimentel)

## Related Projects

- [winget](https://github.com/microsoft/winget-cli) - Windows Package Manager
- [scoop](https://scoop.sh/) - Command-line installer for Windows
- [dotnet-suggest](https://github.com/dotnet/command-line-api) - Command line parser and suggestions for .NET

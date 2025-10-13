$ErrorActionPreference = 'SilentlyContinue'

# Find the pwsh-argument-completer executable
$completerPath = Get-Command 'pwsh-argument-completer' -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source

if (-not $completerPath) {
    Write-Warning "pwsh-argument-completer not found in PATH. Tab completion will not be registered."
    return
}

# List of commands to register completions for
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

Export-ModuleMember -Function @()

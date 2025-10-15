$ErrorActionPreference = 'SilentlyContinue'

# Find the pwsh-argument-completer executable in the module directory
$moduleDirectory = $PSScriptRoot
$executableName = if ($IsWindows -or $PSVersionTable.PSVersion.Major -lt 6) { 'pwsh-argument-completer.exe' } else { 'pwsh-argument-completer' }
$completerPath = Join-Path $moduleDirectory $executableName

if (-not (Test-Path $completerPath)) {
    Write-Warning "pwsh-argument-completer not found at '$completerPath'. Tab completion will not be registered."
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

            & $completerPath "$wordToComplete" "$($commandAst.ToString())" "$cursorPosition" | ForEach-Object {
                $parts = $_.Split('|')
                [System.Management.Automation.CompletionResult]::new($parts[0], $parts[0], 'ParameterValue', $parts[1])
            }
        }
    }
}

Export-ModuleMember -Function @()

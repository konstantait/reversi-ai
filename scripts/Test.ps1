param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$testAssemblyPaths = @(
    "tests\Reversi.Algebra.Tests\bin\$Configuration\Reversi.Algebra.Tests.dll"
)

try {
    $repositoryRoot = Split-Path -Parent $PSScriptRoot
    $testAssemblies = @($testAssemblyPaths | ForEach-Object { Join-Path $repositoryRoot $_ })
    foreach ($assembly in $testAssemblies) {
        if (-not (Test-Path -LiteralPath $assembly -PathType Leaf)) {
            throw "Test assembly was not found: $assembly. Run Build.ps1 first."
        }
    }

    $vswhere = Join-Path ([Environment]::GetEnvironmentVariable('ProgramFiles(x86)')) 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path -LiteralPath $vswhere -PathType Leaf)) {
        throw 'Visual Studio Installer locator was not found.'
    }
    $candidates = @(& $vswhere -latest -version '[17.0,18.0)' -products '*' -find '**\TestPlatform\vstest.console.exe')
    if ($LASTEXITCODE -ne 0) {
        throw "Visual Studio lookup failed with exit code $LASTEXITCODE."
    }
    if ($candidates.Count -eq 0 -or -not (Test-Path -LiteralPath $candidates[0] -PathType Leaf)) {
        throw 'VSTest for Visual Studio 2022 was not found.'
    }
    $vstest = $candidates[0]
    $resultsDirectory = Join-Path $repositoryRoot 'TestResults'
    $null = New-Item -ItemType Directory -Path $resultsDirectory -Force
    # A unique report prevents results from earlier runs from being counted.
    $reportName = 'tests-' + [Guid]::NewGuid().ToString('N') + '.trx'
    $reportPath = Join-Path $resultsDirectory $reportName
    & $vstest @testAssemblies "/Logger:trx;LogFileName=$reportName" "/ResultsDirectory:$resultsDirectory"
    $runnerExitCode = $LASTEXITCODE

    if (-not (Test-Path -LiteralPath $reportPath -PathType Leaf)) {
        throw "No tests were executed: VSTest did not create the expected TRX report (exit $runnerExitCode)."
    }
    [xml]$report = [IO.File]::ReadAllText($reportPath)
    $counters = $report.SelectSingleNode('/*[local-name()="TestRun"]/*[local-name()="ResultSummary"]/*[local-name()="Counters"]')
    if ($null -eq $counters) {
        throw 'No tests were executed: the TRX report has no result counters.'
    }
    $total = [int]::Parse($counters.GetAttribute('total'), [Globalization.CultureInfo]::InvariantCulture)
    $executed = [int]::Parse($counters.GetAttribute('executed'), [Globalization.CultureInfo]::InvariantCulture)
    $passed = [int]::Parse($counters.GetAttribute('passed'), [Globalization.CultureInfo]::InvariantCulture)
    $failed = [int]::Parse($counters.GetAttribute('failed'), [Globalization.CultureInfo]::InvariantCulture)
    Write-Output "Total: $total; Passed: $passed; Failed: $failed"
    if ($executed -eq 0) {
        throw 'No tests were executed'
    }
    if ($runnerExitCode -ne 0) {
        throw "VSTest failed with exit code $runnerExitCode."
    }
    if ($failed -gt 0) {
        throw "$failed tests failed."
    }
    exit 0
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}

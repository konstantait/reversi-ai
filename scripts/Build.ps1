param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',
    [switch]$NoRestore
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

try {
    $repositoryRoot = Split-Path -Parent $PSScriptRoot
    $solution = Join-Path $repositoryRoot 'Reversi.sln'
    if ($NoRestore) {
        foreach ($package in @('MSTest.TestAdapter.2.2.10', 'MSTest.TestFramework.2.2.10')) {
            if (-not (Test-Path -LiteralPath (Join-Path $repositoryRoot "packages\$package") -PathType Container)) {
                throw 'Packages are not restored; run without -NoRestore'
            }
        }
    }

    $vswhere = Join-Path ([Environment]::GetEnvironmentVariable('ProgramFiles(x86)')) 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path -LiteralPath $vswhere -PathType Leaf)) {
        throw 'Visual Studio Installer locator was not found.'
    }
    $candidates = @(& $vswhere -latest -version '[17.0,18.0)' -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe')
    if ($LASTEXITCODE -ne 0) {
        throw "Visual Studio lookup failed with exit code $LASTEXITCODE."
    }
    if ($candidates.Count -eq 0 -or -not (Test-Path -LiteralPath $candidates[0] -PathType Leaf)) {
        throw 'MSBuild for Visual Studio 2022 was not found.'
    }
    $msbuild = $candidates[0]

    if (-not $NoRestore) {
        & $msbuild $solution -t:Restore -p:RestorePackagesConfig=true -m:2 -nr:false -v:minimal
        if ($LASTEXITCODE -ne 0) {
            throw "Package restore failed with exit code $LASTEXITCODE."
        }
    }

    & $msbuild $solution "-p:Configuration=$Configuration" -m:2 -nr:false -v:minimal
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE."
    }
    exit 0
}
catch {
    [Console]::Error.WriteLine($_.Exception.Message)
    exit 1
}

param(
    [string]$SolutionRoot = "..\.."
)

$root = Resolve-Path $SolutionRoot
$jsonPath = Join-Path $root 'service-usage-analysis.json'
if (!(Test-Path $jsonPath)) {
    Write-Error "Analysis file not found"
    exit 1
}

$data = Get-Content $jsonPath -Raw | ConvertFrom-Json
$output = foreach ($service in $data.Services) {
    foreach ($method in ($service.Methods | Where-Object { -not $_.IsUsed })) {
        [pscustomobject]@{
            FilePath = $service.FilePath
            Service = $service.Name
            Method = $method.Name
        }
    }
}

$outFile = Join-Path $root 'unused-service-methods.txt'
$output | Sort-Object Service, Method | Format-Table -AutoSize | Out-String | Set-Content -Encoding utf8 $outFile
Write-Host "Unused list -> $outFile"

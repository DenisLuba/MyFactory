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
foreach ($service in $data.Services) {
    $unusedMethods = $service.Methods | Where-Object { -not $_.IsUsed }
    foreach ($method in $unusedMethods) {
        Write-Output ([pscustomobject]@{
            FilePath = $service.FilePath
            Service = $service.Name
            Method = $method.Name
        })
    }
}

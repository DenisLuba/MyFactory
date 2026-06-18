param(
    [string]$SolutionRoot = "..\.."
)

$root = Resolve-Path $SolutionRoot
$jsonPath = Join-Path $root 'service-usage-analysis.json'
if (!(Test-Path $jsonPath)) {
    Write-Error "Analysis file not found: $jsonPath"
    exit 1
}

$data = Get-Content $jsonPath -Raw | ConvertFrom-Json
$vmLines = @('ViewModels:')
foreach ($vm in ($data.ViewModels | Sort-Object Name)) {
    $vmLines += "  $($vm.Name):"
    foreach ($method in $vm.Methods) {
        if ($method.ServiceCalls.Count -gt 0) {
            $calls = ($method.ServiceCalls | ForEach-Object { "{0}.{1}" -f $_.ServiceType, $_.MethodName }) -join ', '
        }
        else {
            $calls = 'no service calls'
        }
        $vmLines += "    - $($method.Name)() ($calls)"
    }
}

$serviceLines = @('Services:')
foreach ($service in ($data.Services | Sort-Object Name)) {
    $iface = if ($service.Interfaces.Count -gt 0) { ($service.Interfaces -join ', ') } else { 'none' }
    $serviceLines += "  $($service.Name) (interfaces: $iface):"
    foreach ($method in $service.Methods) {
        $flag = if ($method.IsUsed) { '' } else { ' [UNUSED]' }
        $serviceLines += "    - $($method.Name)()$flag"
    }
}

$unusedLines = @('Unused service methods:', '  (marked with [UNUSED] above)')

$outPath = Join-Path $root 'src\MyFactory.MauiClient\Methods.txt'
$vmLines + '' + $serviceLines + '' + $unusedLines | Set-Content -Encoding utf8 $outPath
Write-Host "Methods summary written to $outPath"

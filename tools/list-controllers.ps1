param(
    [string]$ControllersPath = "c:\Projects\MyFactory\src\MyFactory.WebApi\Controllers"
)

$controllers = @()

Get-ChildItem -Path $ControllersPath -Filter '*Controller.cs' | Sort-Object Name | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $classMatch = [regex]::Match($content, 'class\s+(\w+Controller)\b')
    if (-not $classMatch.Success) { return }

    $className = $classMatch.Groups[1].Value
    $lines = [regex]::Split($content, "`r?`n")
    $methods = @()

    for ($i = 0; $i -lt $lines.Length; $i++) {
        $line = $lines[$i].Trim()
        if ($line.StartsWith('[Http')) {
            $attr = $line
            $j = $i + 1
            while ($j -lt $lines.Length -and -not $lines[$j].Trim().StartsWith('public')) {
                $j++
            }
            if ($j -ge $lines.Length) { continue }

            $sigLines = @()
            $endIndex = $j

            for ($k = $j; $k -lt $lines.Length; $k++) {
                $sigLine = $lines[$k].Trim()
                if ($sigLine.Length -gt 0) { $sigLines += $sigLine }

                if ($sigLine.Contains(')')) {
                    $trimmed = $sigLine.Trim()
                    if ($sigLine.Contains('=>') -or $sigLine.Contains('{') -or $trimmed.EndsWith(')') -or $trimmed.EndsWith(');')) {
                        $endIndex = $k
                        break
                    }
                }
            }

            if ($sigLines.Count -eq 0) { continue }

            $signature = ($sigLines -join ' ')
            $signature = [regex]::Replace($signature, '\s+', ' ')
            foreach ($sep in @('=>', '{')) {
                if ($signature.Contains($sep)) {
                    $signature = $signature.Split($sep)[0].Trim()
                }
            }

            if (-not $signature.EndsWith(';')) {
                $signature += ';'
            }

            $methods += [pscustomobject]@{
                Attr = $attr
                Sig  = $signature
            }

            $i = $endIndex
        }
    }

    $controllers += [pscustomobject]@{
        Class   = $className
        Methods = $methods
    }
}

$outLines = @()

foreach ($controller in $controllers) {
    $outLines += "class $($controller.Class)"
    $outLines += '{'

    foreach ($method in $controller.Methods) {
        $outLines += "    $($method.Attr)"
        $outLines += "    $($method.Sig)"
        $outLines += ''
    }

    $outLines += '}'
    $outLines += ''
}

$outText = ($outLines -join [Environment]::NewLine).TrimEnd()
$outText

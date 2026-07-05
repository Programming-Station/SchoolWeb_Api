$path = 'e:\GIT\SchoolSAAS\SchoolWeb_Api\School.Domain'
$files = Get-ChildItem -Path $path -Filter '*.cs' -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match 'public\s+(?:abstract\s+)?(?:partial\s+)?class\s+(\w+)') {
        $className = $matches[1]
        Write-Output "Entity: $className"
        
        $matches2 = [regex]::Matches($content, 'public\s+(virtual\s+)?([\w\?\[\]<>]+)\s+(\w+)\s*\{\s*get;')
        foreach ($m in $matches2) {
            $isVirtual = [string]::IsNullOrEmpty($m.Groups[1].Value) -eq $false
            $type = $m.Groups[2].Value
            $name = $m.Groups[3].Value
            
            $beforeText = $content.Substring(0, $m.Index)
            $lastIndex = $beforeText.LastIndexOf("`n")
            if ($lastIndex -gt 50) {
                $lines = $beforeText.Substring($lastIndex - 50)
            } else {
                $lines = $beforeText
            }
            $hasFk = $lines -match '\[ForeignKey'
            
            $flags = ""
            if ($isVirtual) { $flags += "[Virtual] " }
            if ($hasFk) { $flags += "[FK] " }
            
            Write-Output "  - $name ($type) $flags"
        }
    }
}

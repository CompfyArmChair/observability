param(
    [Parameter(Mandatory=$true)]
    [string]$serviceName,

    [Parameter(Mandatory=$true)]
    [ValidateSet("memory", "cpu", "both")]
    [string]$throttleType,

    [Parameter(Mandatory=$false)]
    [string]$memoryLimit = "100m",
	
	[Parameter(Mandatory=$false)]
    [float]$cpuLimit = 0.2,

    [Parameter(Mandatory=$false)]
    [bool]$restore = $false
)

# Ensure Docker is available
if (-not (Get-Command "docker" -ErrorAction SilentlyContinue)) {
    Write-Error "docker is not found. Please ensure it's installed and available in PATH."
    exit 1
}

# Get the full container name
$fullContainerName = docker ps --format '{{.Names}}' | Where-Object { $_ -like "*$serviceName*" } | Select-Object -First 1

if (-not $fullContainerName) {
    Write-Error "No container found matching the service name $serviceName."
    exit 1
}

# Apply Throttling based on type
if ($restore) {
    switch ($throttleType) {
        "cpu" {
            docker update --cpus=0 $fullContainerName
        }
        "memory" {
            docker update --memory="" $fullContainerName
        }
        "both" {
            docker update --cpus=0 --memory="" $fullContainerName
        }
    }
    Write-Output "$serviceName has been updated to restore resources."
} else {
    switch ($throttleType) {
        "cpu" {
            docker update --cpus=$cpuLimit $fullContainerName
        }
        "memory" {
            docker update --memory=$memoryLimit $fullContainerName
        }
        "both" {
            docker update --cpus=$cpuLimit --memory=$memoryLimit $fullContainerName
        }
    }
    Write-Output "$serviceName has been updated with throttled resources."
}

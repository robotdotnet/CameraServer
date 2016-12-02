param (
  [switch]$release = $false
)

If (Test-Path Env:APPVEYOR_REPO_TAG_NAME) {
  if (($env:APPVEYOR_REPO_TAG_NAME).Contains("-") -eq $false) {
     $release = $true
     echo "Tagged Release"
    }
    echo "Tag but not release"
}

$version = "-Pversion=2017.0.1"


If ($release) {
 $releaseString = "-Prelease"
 $revision =  "-PbuildNumber=0"
} Else {
 $releaseString = ""
 $revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
 $revision = "-PbuildNumber=`"{0:D4}`"" -f [convert]::ToInt32($revision, 10)
}

echo $revision

./gradlew build $version $releaseString $revision

If (($env:APPVEYOR_REPO_BRANCH -eq "master") -and (!$env:APPVEYOR_PULL_REQUEST_NUMBER)) {
  if ($env:APPVEYOR) {
    Get-ChildItem .\build\distributions\*.nupkg | % { Push-AppveyorArtifact $_.FullName -FileName $_.Name }
  }
}

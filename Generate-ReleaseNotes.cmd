rem https://github.com/StefH/GitHubReleaseNotes

SET version=1.2.1

GitHubReleaseNotes --output CHANGELOG.md --skip-empty-releases --exclude-labels question invalid doc --version %version%

rem GitHubReleaseNotes --output PackageReleaseNotes.txt --skip-empty-releases --exclude-labels question invalid doc --template PackageReleaseNotes.template --version %version%
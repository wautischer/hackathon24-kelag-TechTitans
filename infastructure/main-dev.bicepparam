using './main.bicep'

// TODO: <app-name> durch den Namen der Applikation
param appName = '<app-name>'
param postfix = 'main'
// TODO: <repository-name> durch korrekten Namen des Repositories ersetzen
param containerImageWithVersion = 'ghcr.io/kelag-hackathon-24/<repository-name>:main' 
param registryUsername = readEnvironmentVariable('USERNAME')
param registryToken = readEnvironmentVariable('GH_TOKEN')

// Parameter können beliebig erweitert werden, wenn benötigt.
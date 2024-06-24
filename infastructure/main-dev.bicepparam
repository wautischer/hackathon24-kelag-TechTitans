using './main.bicep'

param appName = 'AI_Ticketmanager'
param postfix = 'main'
param containerImageWithVersion = 'ghcr.io/kelag-hackathon-24/kelag-hackathon-2024-team-4:main' 
param registryUsername = readEnvironmentVariable('USERNAME')
param registryToken = readEnvironmentVariable('GH_TOKEN')

// Parameter können beliebig erweitert werden, wenn benötigt.
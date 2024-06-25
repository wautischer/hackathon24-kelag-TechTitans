using './main.bicep'

param appName = 'ai-ticketmanager'
param postfix = 'main'
param containerImageWithVersion = 'ghcr.io/kelag-hackathon-24/kelag-hackathon-2024-team-4:main' 
param registryUsername = readEnvironmentVariable('USERNAME')
param registryToken = readEnvironmentVariable('GH_TOKEN')

param passwordDBURL = 'https://kv-hackathon-team-4.vault.azure.net/secrets/passwordDB/22523b6d13934ecd9036e2a54a723346'
param openaikeyURL = 'https://kv-hackathon-team-4.vault.azure.net/secrets/OpenAIKey/0e91d9647fba454597a8a8bbf268fd96'

// Parameter können beliebig erweitert werden, wenn benötigt.

# 🤖 Omni
Omni is a small multifunctional Discord bot built with **Discord.Net**.
> **Note** : The application is still in developement. Bugs may occur.

## ✨ Modules
- Temporary voice channels
- GitHub

## ⚙️ Prerequisites
- .NET 8 (Download [here](https://dotnet.microsoft.com/en-us/download))
- Git (Download [here](https://git-scm.com/))
- Discord bot token (Generate a token [here](https://discord.com/developers/applications))
- Discord server identifier (Enable developer mode on Discord, then right click on your server and `Copy Server ID`)
- GitHub token (Generate a token [here](https://github.com/settings/tokens))

## 🚀 Installation
Clone the repository
```bash
git clone https://github.com/mehdiboudakhani/omni-discordbot
```
Configure environment variables.
```bash
setx OMNI_DISCORD_BOT_TOKEN "YOUR_DISCORD_BOT_TOKEN_HERE"
setx OMNI_DISCORD_GUILD_IDENTIFIER "YOUR_DISCORD_GUILD_IDENTIFIER_HERE"
setx OMNI_GITHUB_TOKEN "YOUR_GITHUB_TOKEN_HERE"
```
Launch the bot from the project folder.
```bash
dotnet run
```

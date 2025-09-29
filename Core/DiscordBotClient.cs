using Discord;
using Discord.WebSocket;

namespace Omni.Core
{
    class DiscordBotClient(DiscordSocketClient discordSocketClient, IServiceProvider serviceProvider)
    {
        public async Task InitializeAsync()
        {
            await discordSocketClient.LoginAsync(TokenType.Bot, GetDiscordBotToken());
            await discordSocketClient.StartAsync();
        }

        private string GetDiscordBotToken()
        {
            var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("The Discord bot token cannot be found.");
            return token;
        }
    }
}

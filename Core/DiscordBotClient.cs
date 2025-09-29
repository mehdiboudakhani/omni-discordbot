using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Omni.Core
{
    class DiscordBotClient(DiscordSocketClient discordSocketClient, IServiceProvider serviceProvider, InteractionService interactionService)
    {
        public async Task InitializeAsync()
        {
            discordSocketClient.InteractionCreated += async interaction =>
            {
                await interactionService.ExecuteCommandAsync(new SocketInteractionContext(discordSocketClient, interaction), serviceProvider);
            };
            await discordSocketClient.LoginAsync(TokenType.Bot, GetDiscordBotToken());
            await discordSocketClient.StartAsync();
            discordSocketClient.Ready += async () =>
            {
                await interactionService.RegisterCommandsToGuildAsync(ulong.Parse(GetDiscordServerIdentifier()));
            };
        }

        private string GetDiscordBotToken()
        {
            var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("The Discord bot token cannot be found.");
            return token;
        }

        private string GetDiscordServerIdentifier()
        {
            var identifier = Environment.GetEnvironmentVariable("DISCORD_SERVER_IDENTIFIER");
            if (string.IsNullOrWhiteSpace(identifier))
                throw new Exception("The Discord server identifier cannot be found.");
            return identifier;
        }
    }
}

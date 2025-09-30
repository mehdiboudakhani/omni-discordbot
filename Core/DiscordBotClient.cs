namespace Omni.Core
{
    class DiscordBotClient(DiscordSocketClient discordSocketClient, IServiceProvider serviceProvider, InteractionService interactionService)
    {
        public async Task InitializeAsync()
        {
            await interactionService.AddModulesAsync(typeof(GithubModule).Assembly, serviceProvider);
            discordSocketClient.InteractionCreated += OnInteractionCreatedAsync;
            discordSocketClient.Ready += OnReadyAsync;
            await discordSocketClient.LoginAsync(TokenType.Bot, EnvironmentVariableHelper.DiscordBotToken);
            await discordSocketClient.StartAsync();
        }

        private async Task OnInteractionCreatedAsync(SocketInteraction socketInteraction) =>
            await interactionService.ExecuteCommandAsync(new SocketInteractionContext(discordSocketClient, socketInteraction), serviceProvider);

        private async Task OnReadyAsync() =>
                await interactionService.RegisterCommandsToGuildAsync(ulong.Parse(EnvironmentVariableHelper.DiscordServerIdentifier));
    }
}

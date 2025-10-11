﻿namespace Omni
{
    class Bot
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionService _interactionService;

        public Bot()
        {
            _discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged
                                 | GatewayIntents.GuildMembers
                                 | GatewayIntents.GuildVoiceStates
                                 | GatewayIntents.MessageContent
            });
            _serviceProvider = new ServiceCollection()
                .AddSingleton(_discordSocketClient)
                .AddSingleton(serviceProvider => new InteractionService(serviceProvider.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<EmbedFactory>()
                .AddSingleton<SecretProvider>()
                .BuildServiceProvider();
            _interactionService = _serviceProvider.GetRequiredService<InteractionService>();
        }

        public async Task RunAsync()
        {
            await _interactionService.AddModulesAsync(typeof(TemporaryVoiceChannelsModule).Assembly, _serviceProvider);
            _discordSocketClient.InteractionCreated += OnInteractionCreatedAsync;
            _discordSocketClient.Ready += OnReadyAsync;
            await _discordSocketClient.LoginAsync(TokenType.Bot, _serviceProvider.GetRequiredService<SecretProvider>().DiscordBotToken);
            await _discordSocketClient.StartAsync();
            await Task.Delay(-1);
        }

        private async Task OnInteractionCreatedAsync(SocketInteraction socketInteraction) =>
            await _interactionService.ExecuteCommandAsync(new SocketInteractionContext(_discordSocketClient, socketInteraction), _serviceProvider);

        private async Task OnReadyAsync() =>
            await _interactionService.RegisterCommandsToGuildAsync(ulong.Parse(_serviceProvider.GetRequiredService<SecretProvider>().DiscordGuildIdentifier));
    }
}

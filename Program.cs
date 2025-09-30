namespace Omni
{
    class Program
    {
        static async Task Main()
        {
            await ConfigureServices().GetRequiredService<DiscordBotClient>().InitializeAsync();
            await Task.Delay(-1);
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged
                                 | GatewayIntents.GuildMembers
                                 | GatewayIntents.GuildVoiceStates
                                 | GatewayIntents.MessageContent
            }));
            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(client);
            });
            services.AddSingleton<DiscordBotClient>();
            return services.BuildServiceProvider();
        }
    }
}
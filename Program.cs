using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main()
    {
        await Task.Delay(-1);
    }

    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged
                             | GatewayIntents.GuildMembers
                             | GatewayIntents.GuildVoiceStates
                             | GatewayIntents.MessageContent
        }));
        return services.BuildServiceProvider();
    }
}
namespace Omni.Modules
{
    [Group("test", "Testing the application.")]
    public class TestModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Checking if the server responds.")]
        public async Task PingAsync()
        {
            await RespondAsync("Pong !");
        }
    }
}

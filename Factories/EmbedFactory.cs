namespace Omni.Factories
{
    public class EmbedFactory
    {
        public Embed Error(string reason)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription("An error has occurred.\n> **Reason**: " + reason)
                .WithColor(Color.Red)
                .Build();
            return embed;
        }

        public Embed Success(string message)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Success")
                .WithDescription(message)
                .WithColor(Color.Green)
                .Build();
            return embed;
        }

        public Embed Hubs(HashSet<ulong> hubs)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Hubs:")
                .WithDescription(hubs.Count == 0 ? "No hubs." : string.Join("\n", hubs.Select(id => $"<#{id}>")))
                .WithColor(Color.Green)
                .Build();
            return embed;
        }
    }
}

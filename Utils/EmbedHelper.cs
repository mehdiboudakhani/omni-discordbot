namespace Omni.Utils
{
    internal class EmbedHelper
    {
        public static Embed ErrorEmbed(string reason)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Erreur")
                .WithDescription("An error has occured.\n> **Reason** : " + reason)
                .WithColor(Color.Red)
                .WithFooter("This message will be deleted automatically.");
            return embedBuilder.Build();
        }
    }
}

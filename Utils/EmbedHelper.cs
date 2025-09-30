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

        public static Embed GithubProfileEmbed(JsonElement user)
        {
            var name = user.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
            var bio = user.TryGetProperty("bio", out var bioProp) ? bioProp.GetString() : null;
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"GitHub profile : {user.GetProperty("login").GetString()}")
                .WithUrl(user.GetProperty("html_url").GetString())
                .WithThumbnailUrl(user.GetProperty("avatar_url").GetString())
                .WithColor(Color.Green);
            if (!string.IsNullOrWhiteSpace(name))
                embedBuilder.AddField("Nom", name, true);
            if (!string.IsNullOrWhiteSpace(bio))
                embedBuilder.AddField("Bio", bio, true);
            return embedBuilder.Build();
        }

        public static Embed GithubRepositoryEmbed(JsonElement repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"GitHub repository : {repository.GetProperty("owner").GetProperty("login").GetString()}/{repository.GetProperty("name").GetString()}")
                .WithUrl(repository.GetProperty("html_url").GetString())
                .WithDescription(repository.GetProperty("description").GetString())
                .WithColor(Color.Green)
                .AddField("Dernière mise à jour", repository.GetProperty("updated_at").GetString())
                .AddField("Issues ouvertes", repository.GetProperty("open_issues_count").GetInt32());
            return embedBuilder.Build();
        }

        public static Embed GithubLastIssuesRepositoryEmbed(JsonElement issues, string owner, string repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"5 last open issues : {owner}/{repository}")
                .WithUrl($"https://github.com/{owner}/{repository}/issues")
                .WithColor(Color.Green);
            int count = 0;
            foreach (var issue in issues.EnumerateArray())
            {
                if (count++ >= 5) break;
                embedBuilder.AddField($"{issue.GetProperty("number").GetInt32()} - {issue.GetProperty("title").GetString()}", $"{issue.GetProperty("user").GetProperty("login").GetString()} * [Voir l'issue]({issue.GetProperty("html_url").GetString})");
            }
            return embedBuilder.Build();
        }

        public static Embed GithubLastCommitsRepositoryEmbed(JsonElement commits, string owner, string repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"5 last open commits : {owner}/{repository}")
                .WithUrl($"https://github.com/{owner}/{repository}/commits")
                .WithColor(Color.Green);
            int count = 0;
            foreach (var commit in commits.EnumerateArray())
            {
                if (count++ >= 5) break;
                embedBuilder.AddField($"{commit.GetProperty("commit").GetProperty("author").GetProperty("name").GetString()}", $"[{commit.GetProperty("commit").GetProperty("message").GetString()?.Split('\n')[0]}({commit.GetProperty("html_url").GetString()})]");
            }
            return embedBuilder.Build();
        }
    }
}

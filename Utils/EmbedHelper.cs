using System.Runtime.InteropServices.JavaScript;

namespace Omni.Utils
{
    class EmbedHelper
    {
        internal static Embed Error(string reason)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Erreur")
                .WithDescription("An error has occured.\n> **Reason** : " + reason)
                .WithColor(Color.Red);
            return embedBuilder.Build();
        }

        internal static Embed GithubProfile(JsonElement user)
        {
            var bio = user.TryGetProperty("bio", out var bioProp) ? bioProp.GetString() : null;
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"👤 GitHub profile : {user.GetProperty("login").GetString()}")
                .WithDescription(user.GetProperty("bio").GetString())
                .AddField("Account created", DateTime.Parse(user.GetProperty("created_at").GetString()!).ToString("dd-MM-yyyy"))
                .AddField("Public repositories", user.GetProperty("public_repos").GetInt32())
                .WithUrl(user.GetProperty("html_url").GetString())
                .WithThumbnailUrl(user.GetProperty("avatar_url").GetString())
                .WithColor(Color.Green);
            return embedBuilder.Build();
        }

        internal static Embed GithubRepository(JsonElement repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"📦 GitHub repository : {repository.GetProperty("owner").GetProperty("login").GetString()}/{repository.GetProperty("name").GetString()}")
                .WithUrl(repository.GetProperty("html_url").GetString())
                .WithDescription(repository.GetProperty("description").GetString())
                .WithColor(Color.Green)
                .AddField("Last update", DateTime.Parse(repository.GetProperty("updated_at").GetString()!).ToString("dd-MM-yyyy"), true)
                .AddField("Open issues", repository.GetProperty("open_issues_count").GetInt32(), true);
            return embedBuilder.Build();
        }

        internal static Embed GithubLastIssues(JsonElement issues, string owner, string repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"🐛 5 last open issues : {owner}/{repository}")
                .WithUrl($"https://github.com/{owner}/{repository}/issues")
                .WithColor(Color.Green);
            foreach (var issue in issues.EnumerateArray())
            {
                embedBuilder.AddField(
                    $"{issue.GetProperty("number").GetInt32()} • {issue.GetProperty("title").GetString()}", 
                    $"{issue.GetProperty("user").GetProperty("login").GetString()} • {DateTime.Parse(issue.GetProperty("created_at").GetString()!).ToString("dd/MM/yyyy")} • [View issue]({issue.GetProperty("html_url").GetString()})"
                );
            }
            return embedBuilder.Build();
        }

        internal static Embed GithubLastCommits(JsonElement commits, string owner, string repository)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"📝 5 last open commits : {owner}/{repository}")
                .WithUrl($"https://github.com/{owner}/{repository}/commits")
                .WithColor(Color.Green);
            foreach (var commit in commits.EnumerateArray())
            {
                embedBuilder.AddField(
                    $"{commit.GetProperty("commit").GetProperty("author").GetProperty("name").GetString()} • {commit.GetProperty("commit").GetProperty("author").GetProperty("date").GetDateTime().ToString("dd/MM/yyyy HH:mm:ss")}", 
                    $"{commit.GetProperty("commit").GetProperty("message").GetString()} • [View commit]({commit.GetProperty("html_url").GetString()})"
                );
            }
            return embedBuilder.Build();
        }

        internal static Embed GeminiAsk(string prompt, JsonElement response)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("🤖 Gemini response")
                .WithDescription($"Question: {prompt}\n " + response
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString())
                .WithColor(Color.Green);
            return embedBuilder.Build();
        }
    }
}

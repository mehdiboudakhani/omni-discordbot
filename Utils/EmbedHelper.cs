namespace Omni.Utils
{
    /// <summary>
    /// Provides a collection of helper methods for building standardized <see cref="Embed"/> objects.
    /// These embeds are used across the Omni bot to format responses.
    /// </summary>
    class EmbedHelper
    {
        /// <summary>
        /// Creates an error embed with a descriptive reason.
        /// </summary>
        /// <param name="reason">
        /// The reason for the error.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> with a red error message.
        /// </returns>
        internal static Embed Error(string reason)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Erreur")
                .WithDescription("An error has occured.\n> **Reason** : " + reason)
                .WithColor(Color.Red);
            return embedBuilder.Build();
        }

        /// <summary>
        /// Builds an embed containing details of a GitHub user's profile.
        /// </summary>
        /// <param name="user">
        /// The JSON response representing a GitHub user.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> showing the user's profile information.
        /// </returns>
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

        /// <summary>
        /// Builds an embed displaying information about a GitHub repository.
        /// </summary>
        /// <param name="repository">
        /// The JSON response representing a GitHub repository.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> with repository details such as description and open issues.
        /// </returns>
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

        /// <summary>
        /// Builds an embed listing the last 5 open issues of a GitHub repository.
        /// </summary>
        /// <param name="issues">
        /// The JSON response representing a collection of GitHub issues.
        /// </param>
        /// <param name="owner">
        /// The owner of the repository.
        /// </param>
        /// <param name="repository">
        /// The name of the repository.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> containing a formatted list of issues.
        /// </returns>
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

        /// <summary>
        /// Builds an embed displaying the last five commits of a GitHub repository.
        /// </summary>
        /// <param name="commits">
        /// A JSON array containing commit data from the GitHub API.
        /// </param>
        /// <param name="owner">
        /// The owner of the repository.
        /// </param>
        /// <param name="repository">
        /// The name of the repository.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> object showing the commit author, commit date, commit message, and a direct link to each commit.
        /// </returns>
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

        /// <summary>
        /// Builds an embed displaying the response of the Gemini AI model to a given prompt.
        /// </summary>
        /// <param name="prompt">
        /// The input question or text sent to Gemini.
        /// </param>
        /// <param name="response">
        /// The JSON response returned by the Gemini API.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> object containing the prompt and the AI-generated response text.
        /// </returns>
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

        /// <summary>
        /// Builds an embed confirming that a hub voice channel has been added for temporary channel creation.
        /// </summary>
        /// <param name="hubChannel">
        /// The voice channel that has been registered as a hub.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> object confirming the addition of the hub channel.
        /// </returns>
        internal static Embed TemporaryVoiceChannelsAdd(IVoiceChannel hubChannel)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Add hub channel")
                .WithDescription($"Hub added: {hubChannel.Name}")
                .WithColor(Color.Green);
            return embedBuilder.Build();
        }

        /// <summary>
        /// Builds an embed confirming that a hub voice channel has been removed from the configuration.
        /// </summary>
        /// <param name="hubChannel">
        /// The voice channel that has been removed as a hub.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> object confirming the removal of the hub channel.
        /// </returns>
        internal static Embed TemporaryVoiceChannelsRemove(IVoiceChannel hubChannel)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Remove hub channel")
                .WithDescription($"Hub removed: {hubChannel.Name}")
                .WithColor(Color.Green);
            return embedBuilder.Build();
        }

        /// <summary>
        /// Builds an embed listing all configured hub voice channels.
        /// </summary>
        /// <param name="hubChannels">
        /// A set of hub voice channel IDs.
        /// </param>
        /// <returns>
        /// An <see cref="Embed"/> object containing a formatted list of hub channels.
        /// </returns>
        internal static Embed TemporaryVoiceChannelsList(HashSet<ulong> hubChannels)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("List hub channels")
                .WithDescription(string.Join("\n", hubChannels.Select(id => $"<#{id}>")))
                .WithColor(Color.Green);
            return embedBuilder.Build();
        }
    }
}

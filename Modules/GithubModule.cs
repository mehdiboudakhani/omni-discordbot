namespace Omni.Modules
{
    [Group("github", "Interacting with GitHub.")]
    public class GithubModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;

        public GithubModule(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GithubHttpClient");
        }

        [SlashCommand("profile", "Viewing a GitHub user's profile.")]
        public async Task UserAsync(string username) =>
            await GithubRequestAsync($"users/{username}", EmbedHelper.GithubProfile);

        [SlashCommand("repository", "Viewing a user's GitHub repository.")]
        public async Task RepositoryAsync(string owner, string repository) =>
            await GithubRequestAsync($"repos/{owner}/{repository}", EmbedHelper.GithubRepository);

        [SlashCommand("last-issues", "Displaying the last 5 open issues of a GitHub repository.")]
        public async Task IssuesAsync(string owner, string repository) =>
            await GithubRequestAsync(
                $"repos/{owner}/{repository}/issues?state=open&per_page=5",
                root => EmbedHelper.GithubLastIssues(root, owner, repository)
            );

        [SlashCommand("last-commits", "Displaying the last 5 commits of a GitHub repository.")]
        public async Task CommitsAsync(string owner, string repository) =>
            await GithubRequestAsync(
                $"repos/{owner}/{repository}/commits?per_page=5", 
                root => EmbedHelper.GithubLastCommits(root, owner, repository)
            );

        private async Task GithubRequestAsync(string url, Func<JsonElement, Embed> embedFactory)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await RespondAsync(embed: EmbedHelper.Error("Unable to find the requested resource on GitHub."), ephemeral: true);
                    return;
                }
                var content = await response.Content.ReadAsStringAsync();
                await RespondAsync(embed: embedFactory(JsonDocument.Parse(content).RootElement));
            }
            catch (Exception)
            {
                await RespondAsync(embed: EmbedHelper.Error("A technical problem occurred while contacting GitHub."), ephemeral: true);
            }
        }
    }
}

namespace Omni.Modules
{
    /// <summary>
    /// Provides interaction commands for retrieving data from GitHub's REST API.
    /// </summary>
    [Group("github", "Interacting with GitHub.")]
    public class GithubModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubModule"/> class.
        /// Configures the HTTP client used to communicate with the GitHub API.
        /// </summary>
        /// <param name="httpClientFactory">
        /// The factory used to create named <see cref="HttpClient"/> instances.
        /// </param>
        public GithubModule(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GithubHttpClient");
        }

        /// <summary>
        /// Displays the profile of a specified GitHub user.
        /// </summary>
        /// <param name="username">
        /// The GitHub username to retrieve information for.
        /// </param>
        /// <returns></returns>
        [SlashCommand("profile", "Viewing a GitHub user's profile.")]
        public async Task UserAsync(string username) =>
            await GithubRequestAsync($"users/{username}", EmbedHelper.GithubProfile);

        /// <summary>
        /// Displays details about a specific GitHub repository.
        /// </summary>
        /// <param name="owner">The owner of the repository.</param>
        /// <param name="repository">The name of the repository.</param>
        /// <returns>
        /// A task representing the asynchronous operation, send an embed with repository details.
        /// </returns>
        [SlashCommand("repository", "Viewing a user's GitHub repository.")]
        public async Task RepositoryAsync(string owner, string repository) =>
            await GithubRequestAsync($"repos/{owner}/{repository}", EmbedHelper.GithubRepository);

        /// <summary>
        /// Displays the last five open issues from a GitHub repository.
        /// </summary>
        /// <param name="owner">
        /// The owner of the repository.
        /// </param>
        /// <param name="repository">
        /// The name of the repository.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, sending an embed with the last five open issues.
        /// </returns>
        [SlashCommand("last-issues", "Displaying the last 5 open issues of a GitHub repository.")]
        public async Task IssuesAsync(string owner, string repository) =>
            await GithubRequestAsync(
                $"repos/{owner}/{repository}/issues?state=open&per_page=5",
                root => EmbedHelper.GithubLastIssues(root, owner, repository)
            );

        /// <summary>
        /// Displays the last five commits of a GitHub repository.
        /// </summary>
        /// <param name="owner">
        /// The owner of the repository.
        /// </param>
        /// <param name="repository">
        /// The name of the repository.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, sending an embed with the last five commits.
        /// </returns>
        [SlashCommand("last-commits", "Displaying the last 5 commits of a GitHub repository.")]
        public async Task CommitsAsync(string owner, string repository) =>
            await GithubRequestAsync(
                $"repos/{owner}/{repository}/commits?per_page=5", 
                root => EmbedHelper.GithubLastCommits(root, owner, repository)
            );

        /// <summary>
        /// Sends an HTTP request to the GitHub API and builds a response embed.
        /// </summary>
        /// <param name="url">
        /// The relative GitHub API endpoint to query.
        /// </param>
        /// <param name="embedFactory">
        /// A factory method that builds an embed from the API response.
        /// </param>
        /// <returns>
        /// A task representing the asynchrnous operation.
        /// Sends either a success embed with the requested data, or an error embed if the request fails.
        /// </returns>
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

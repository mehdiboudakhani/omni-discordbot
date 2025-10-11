namespace Omni.Modules
{
    [Group("github", "GitHub related commands.")]
    public class GitHubModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly GitHubClient _gitHubClient;
        private readonly EmbedFactory _embedFactory;

        public GitHubModule(EmbedFactory embedFactory, SecretProvider secretProvider)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue("Omni"));
            if (!string.IsNullOrEmpty(secretProvider.GitHubToken))
                _gitHubClient.Credentials = new Credentials(secretProvider.GitHubToken);
            _embedFactory = embedFactory;
        }

        [SlashCommand("profile", "View a GitHub profile.")]
        public async Task ProfileAsync(string username)
        {
            await DeferAsync();
            var user = await _gitHubClient.User.Get(username);
            await FollowupAsync(embed: _embedFactory.GitHubProfile(user));
        }

        [SlashCommand("repository", "View a GitHub repository.")]
        public async Task RepositoryAsync(string owner, string repository)
        {
            await DeferAsync();
            var repo = await _gitHubClient.Repository.Get(owner, repository);
            await FollowupAsync(embed: _embedFactory.GitHubRepository(repo));
        }

        [SlashCommand("last-commits", "Show the last 5 commits of a GitHub repository.")]
        public async Task LastCommitsAsync(string owner, string repository)
        {
            await DeferAsync();
            var commits = await _gitHubClient.Repository.Commit.GetAll(owner, repository, new ApiOptions
            {
                PageCount = 1,
                PageSize = 5
            });
            await FollowupAsync(embed: _embedFactory.GitHubCommits(commits, owner, repository));
        }

        [SlashCommand("last-issues", "Show the last 5 issues of a GitHub repository.")]
        public async Task LastIssuesAsync(string owner, string repository)
        {
            await DeferAsync();
            var issues = await _gitHubClient.Issue.GetAllForRepository(owner, repository, new ApiOptions
            {
                PageCount = 1,
                PageSize = 5
            });
            await FollowupAsync(embed: _embedFactory.GitHubIssues(issues, owner, repository));
        }

        [SlashCommand("last-pull-requests", "Show the last 5 pull requests of a GitHub repository.")]
        public async Task LastPullRequests(string owner, string repository)
        {
            await DeferAsync();
            var pullRequests = await _gitHubClient.PullRequest.GetAllForRepository(owner, repository, new ApiOptions
            {
                PageCount = 1,
                PageSize = 5
            });
            await FollowupAsync(embed: _embedFactory.GitHubPullRequests(pullRequests, owner, repository));
        }
    }
}

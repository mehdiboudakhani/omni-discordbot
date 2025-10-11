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

        public Embed GitHubProfile(User user)
        {
            var embed = new EmbedBuilder()
                .WithTitle(user.Name ?? user.Login)
                .WithUrl(user.HtmlUrl)
                .WithThumbnailUrl(user.AvatarUrl)
                .WithDescription(user.Bio ?? "No bio.")
                .AddField("Public repositories", user.PublicRepos, true)
                .AddField("Followers", user.Followers, true)
                .AddField("Created at", user.CreatedAt.ToString("dd-MM-yyyy"), true)
                .WithColor(Color.Green)
                .Build();
            return embed;
        }

        public Embed GitHubRepository(Repository repository)
        {
            var embed = new EmbedBuilder()
                .WithTitle(repository.FullName)
                .WithUrl(repository.HtmlUrl)
                .WithDescription(repository.Description ?? "No description.")
                .AddField("Owner", repository.Owner.Login, true)
                .AddField("Stars", repository.StargazersCount, true)
                .AddField("Forks", repository.ForksCount, true)
                .AddField("Open issues", repository.OpenIssuesCount, true)
                .AddField("Created at", repository.CreatedAt.ToString("dd-MM-yyyy"), true)
                .AddField("Updated at", repository.UpdatedAt.ToString("dd-MM-yyyy"), true)
                .AddField("Language", repository.Language ?? "N/A", true)
                .WithColor(Color.Green)
                .Build();
            return embed;
        }

        public Embed GitHubCommits(IReadOnlyList<GitHubCommit> commits, string owner, string repository)
        {
            var embed = new EmbedBuilder()
                .WithTitle($"Commits ({owner}/{repository}):")
                .WithDescription(string.Join(string.Empty, commits.Select(commit => 
                    $"- [{commit.Commit.Message}]({commit.HtmlUrl}) by {commit.Commit.Author.Name} on {commit.Commit.Author.Date:dd-MM-yyyy}\n")))
                .WithColor(Color.Green)
                .Build();
            return embed;
        }

        public Embed GitHubIssues(IReadOnlyList<Issue> issues, string owner, string repository)
        {
            var embed = new EmbedBuilder()
                .WithTitle($"Issues ({owner}/{repository}):")
                .WithDescription(string.Join(string.Empty, issues.Select(issue => 
                    $"- [{issue.Title}]({issue.HtmlUrl}) #{issue.Number}\n")))
                .WithColor(Color.Green)
                .Build();
            return embed;
        }

        public Embed GitHubPullRequests(IReadOnlyList<PullRequest> pullRequests, string owner, string repository)
        {
            var embed = new EmbedBuilder()
                .WithTitle($"Pull requests ({owner}/{repository}):")
                .WithDescription(string.Join(string.Empty, pullRequests.Select(pullRequest =>
                    $"- [{pullRequest.Title}]({pullRequest.HtmlUrl}) #{pullRequest.Number} ({pullRequest.State})\n")))
                .WithColor(Color.Green)
                .Build();
            return embed;
        }
    }
}

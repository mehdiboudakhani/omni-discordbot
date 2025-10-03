namespace Omni.Configuration
{
    public interface ISecretProvider
    {
        string DiscordBotToken { get; }
        string DiscordServerIdentifier { get; }
        string GithubToken { get; }
        string GeminiApiKey { get; }
    }
}

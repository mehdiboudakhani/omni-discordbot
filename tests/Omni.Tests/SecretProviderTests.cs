namespace Omni.Tests
{
    public class SecretProviderTests
    {
        private readonly Mock<IEnvironmentVariableService> _environmentVariableServiceMock;

        public SecretProviderTests()
        {
            _environmentVariableServiceMock = new Mock<IEnvironmentVariableService>();
        }

        [Theory]
        [InlineData("OMNI_DISCORD_BOT_TOKEN", "discord-bot-token", "DiscordBotToken")]
        [InlineData("OMNI_DISCORD_SERVER_IDENTIFIER", "discord-server-identifier", "DiscordServerIdentifier")]
        [InlineData("OMNI_GITHUB_TOKEN", "github-token", "GithubToken")]
        [InlineData("OMNI_GEMINI_API_KEY", "gemini-api-key", "GeminiApiKey")]
        public void SecretProperties_ShouldReturnEnvironmentValue(string key, string expected, string propertyName)
        {
            _environmentVariableServiceMock.Setup(environmentVariableService => environmentVariableService.GetEnvironmentVariable(key)).Returns(expected);
            var provider = new SecretProvider(_environmentVariableServiceMock.Object);
            var property = typeof(ISecretProvider).GetProperty(propertyName);
            var result = property!.GetValue(provider);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("OMNI_DISCORD_BOT_TOKEN", "DiscordBotToken")]
        [InlineData("OMNI_DISCORD_SERVER_IDENTIFIER", "DiscordServerIdentifier")]
        [InlineData("OMNI_GITHUB_TOKEN", "GithubToken")]
        [InlineData("OMNI_GEMINI_API_KEY", "GeminiApiKey")]
        public void SecretProperties_WhenMissing_ShouldThrowException(string key, string propertyName)
        {
            _environmentVariableServiceMock.Setup(environmentVariableService => environmentVariableService.GetEnvironmentVariable(key)).Returns((string?)null);
            var provider = new SecretProvider(_environmentVariableServiceMock.Object);
            var property = typeof(ISecretProvider).GetProperty(propertyName);
            Action action = () => property!.GetValue(provider);
            action.Should().Throw<TargetInvocationException>().WithInnerException<Exception>().WithMessage($"Secret '{key}' is missing.");
        }
    }
}

namespace Omni.Tests
{
    public class HttpClientProviderTests
    {

        [Fact]
        public void GetGitHubHttpClient_ShouldHaveCorrectBaseUrlUserAgentAndAuthToken()
        {
            var mockSecretProvider = new Mock<ISecretProvider>();
            mockSecretProvider.Setup(secretProvider => secretProvider.GithubToken).Returns("fake-token");
            var provider = new HttpClientProvider(mockSecretProvider.Object);
            var client = provider.GetGithubHttpClient();
            Assert.Equal("https://api.github.com/", client.BaseAddress!.ToString());
            Assert.Contains("Omni", client.DefaultRequestHeaders.UserAgent.ToString());
            Assert.NotNull(client.DefaultRequestHeaders.Authorization);
            Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization!.Scheme);
            Assert.Equal("fake-token", client.DefaultRequestHeaders.Authorization.Parameter);
        }

        [Fact]
        public void GetGeminiHttpClient_ShouldHaveCorrectBaseUrlWithoutUserAgentOrAuthToken()
        {
            var mockSecretProvider = new Mock<ISecretProvider>();
            var provider = new HttpClientProvider(mockSecretProvider.Object);
            var client = provider.GetGeminiHttpClient();
            Assert.Equal("https://generativelanguage.googleapis.com/", client.BaseAddress!.ToString());
            Assert.Empty(client.DefaultRequestHeaders.UserAgent);
            Assert.Null(client.DefaultRequestHeaders.Authorization);
        }
    }
}

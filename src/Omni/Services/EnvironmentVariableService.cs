namespace Omni.Services
{
    public interface IEnvironmentVariableService
    {
        string? GetEnvironmentVariable(string key);
    }

    internal class EnvironmentVariableService : IEnvironmentVariableService
    {
        public string? GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}

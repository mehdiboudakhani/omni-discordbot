namespace Omni
{
    class Program
    {
        static async Task Main()
        {
            Bot bot = new();
            await bot.RunAsync();
        }
    }
}

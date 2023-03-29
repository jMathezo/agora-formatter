using CandidateTesting.JoaoMatheus.AgoraFormatter.Application.Microsoft.Extensions.DependencyInjection;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service;
using CandidateTesting.JoaoMatheus.AgoraFormatter.FileAdapter.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var _formatterService = serviceProvider.GetService<IFormatterService>();

            var sourceURI = args[0];
            var targetPath = args[1];

            var minhaCDNLogs = await _formatterService.GetLogs(sourceURI);

            await _formatterService.FormatMinhaCDN<Program>(minhaCDNLogs, targetPath);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationService();
            services.AddFileAdapter();
        }
    }
}
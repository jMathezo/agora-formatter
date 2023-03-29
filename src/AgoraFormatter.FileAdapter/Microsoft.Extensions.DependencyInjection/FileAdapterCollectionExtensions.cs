using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter.FileAdapter.Microsoft.Extensions.DependencyInjection
{
    public static class FileAdapterCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddFileAdapter(
           this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IFileAdapter, FileAdapter>();

            return services;
        }
    }
}

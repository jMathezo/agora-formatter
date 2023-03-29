using AgoraFormatter.Application;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Application.Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddApplicationService(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IFormatterService, FormatterService>();
            return services;
        }
    }
}

using FluentValidation;
using System.Reflection;
using AutoMapper;
using Checkout.Application.Common.Interfaces;
using Checkout.Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Checkout.Application.Common.Behaviours;
using System;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using Checkout.Application.Common.Mappings;
using System.Net.Http.Headers;

namespace Checkout.Application
{
    public static class Configuration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddScoped<ICardsService, CardsService>();
            services.AddScoped<ITransactionsHistoryService, TransactionsHistoryService>();

            services.AddScoped<IBankAuthProvider, DevBankAuthProvider>();

            services.AddHttpClient<ITelemetryService, TelemetryService>(
                    client =>
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.BaseAddress = new Uri($"https://api.applicationinsights.io/v1/apps/{configuration["ApplicationInsights:ApplicationId"]}/events/$all?$top=10");
                        client.Timeout = TimeSpan.FromMinutes(180);
                        client.DefaultRequestHeaders.Add("x-api-key", configuration["ApplicationInsights:ApiKey"]);
                    })
                .AddPolicyHandler(BaseRetryPolicy)
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(300));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> BaseRetryPolicy => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5));
    }
}

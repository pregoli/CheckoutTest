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

namespace Checkout.Application
{
    public static class Configuration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.AddScoped<ICardsService, CardsService>();
            services.AddScoped<ITransactionsHistoryService, TransactionsHistoryService>();

            services.AddScoped<ITransactionsAuthProvider, TransactionsAuthProvider>();

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> BaseRetryPolicy => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5));
    }
}

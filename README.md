# Checkout
This is a test API service built on top of [.Net Core Framework](https://docs.microsoft.com/en-us/dotnet/core/) **3.1**.

The implementation applies the [Mediatr](https://en.wikipedia.org/wiki/Mediator_pattern) pipeline with a command query pattern as shown below:


![Checkout Api](https://github.com/pregoli/CheckoutTest/blob/master/diagram.png)

Requests validation have been decoupled by using [FluentValidation](https://fluentvalidation.net/).

For test purpose, data will be stored in memory by using [Entity Framework Core InMemory](https://entityframeworkcore.com/providers-inmemory).


## Project dependencies
   * <a href="https://fluentvalidation.net/" target="_blank">FluentValidation.AspNetCore 8.6.2</a>
   * <a href="https://github.com/jbogard/MediatR" target="_blank">MediatR 8.0.1</a>
   * <a href="https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/3.1.4" target="_blank">Microsoft.EntityFrameworkCore.InMemory 3.1.4</a>
   * <a href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore" target="_blank">Swashbuckle.AspNetCore.Swagger</a>
   * <a href="https://docs.microsoft.com/it-it/azure/azure-monitor/app/app-insights-overview" target="_blank">Microsoft.ApplicationInsight.AspNetCore</a>
   * <a href="https://github.com/App-vNext/Polly" target="_blank">Microsoft.Extensions.Http.Polly</a>
   * <a href="https://automapper.org/" target="_blank">AutoMapper</a>
   * <a href="https://nunit.org/" target="_blank">NUnit</a>
   * <a href="https://github.com/moq/moq4" target="_blank">Moq</a>


## Usage

Running the API project locally will launch the [Swagger](https://swagger.io/) client exposing 5 endpoints as shown below:


![swagger client](https://github.com/pregoli/CheckoutTest/blob/master/apiclient2.png)


Each endpoint exposes its request/response schema.

An example of test flow could be:

1. Submit one or more Transactions via the **../api/beta/Ttransactions** endpoint.
2. Use the merchantId from previous response to invoke the **../api/beta/merchants/{id}/transactions** and fetch all its transactions (successful and non).
3. Use the transactionId from previous response to invoke the **../api/beta/transactions/{id}/** and fetch the transaction detail.
4. After playng around with above endpoints try out and have a look at telemetries fetched by [Azure Application Insights](https://dev.applicationinsights.io/): 
    * **../api/beta/Telemetries/events/all/top/10** to check out what kind of events have bee recored.
    * **../api/beta/Telemetries/requests/count** to check out how many requests have been submitted.


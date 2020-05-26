# CheckoutTest
>This is a test API service built on top of [.Net Core Framework](https://docs.microsoft.com/en-us/dotnet/core/) **3.1**.

The implementation applies the [Mediatr](https://en.wikipedia.org/wiki/Mediator_pattern) pattern as shown below:


![Checkout Api](https://github.com/pregoli/CheckoutTest/blob/master/diagram.png)

For test purpose, data will be stored in memory by using [Entity Framework Core InMemory](https://entityframeworkcore.com/providers-inmemory).

## Usage

Running the API project locally will launch the [Swagger](https://swagger.io/) client exposing 5 endpoints as shown below:


![swagger client](https://github.com/pregoli/CheckoutTest/blob/master/apiclient2.png)


Each endpoint exposes its request/response schema.

An example of test flow could be:

1. Submit one or more Transactions via the **../api/beta/Ttransactions** endpoint.
2. Use the merchantId from previous response to invoke the **../api/beta/merchants/{id}/transactions** and fetch all its transactions (successful and non).
3. Use the transactionId from previous response to invoke the **../api/beta/transactions/{id}/** and fetch the transaction detail.
4. After playng around with above endpoints try out to have a look at telemetries fetched by [Azure Application Insights](https://dev.applicationinsights.io/): 
    * **../api/beta/Telemetries/events/all/top/10** to check out what kind of events have bee recored.
    * **../api/beta/Telemetries/requests/count** to check out how many requests have been submitted.


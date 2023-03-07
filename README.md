# Medirect Exchange API
Simply designed with onion architecture in .NET 6 Web API to get latest currency rates and convert currencies

MeDirect.Exchange.API - Presentation layer 
## Endpoints
GET ExecuteCurrencyExchange ; Getting data from Exchange rate API and execute the currency operation
GET GetAllCurrencies; Getting all currency list from Exchange rate API. Also fill the database and cache using this endpoint
GET GetTransactionHistoryByUserId; Getting all transactions record with related user id
MeDirect.Exchange.Application - Business layer 
All business operations are generated inside this layer with specific services.
Mostly used services are;
CacheService; To handle cache operation acting as a repository with built in Microsoft Memory CacheService
CurrencyExchangeService ; To call database ops and serve to endpoints about currency process.
ExchangeRateApiService; Communicate between Currency API and backend.
#MeDirect.Exchange.Domain - Domain Layer
To store database end dto objects
#MeDirect.Exchange.Persistence - Database Layer
To manage database operations and repositories 
#MeDirect.Exchange.Test
To create unit tests for testing endpoints with Xunit and Moq library


# Used 
• C# 

•.NET Core 6

• Entity Framework Core

• MS SQL Server

• Microsoft Memory Caching

• RESTful APIs

• Unit Tests -Xunit

• Logging - SeriLog

• Exchange Rates Data API to get currency rates

• Swagger

# Introduction

Attension Please! To run the application here are the sample steps;

• Firstly, need to create database. To do that; you need to run-database command with the existing migration file
• Please keep in mind, all secret values(such as db password in connection string and api key value) are protected with dotnet secret, so you need to be replaced with your values.
• To fill Currency table and related Cache key; you need to call GetAllCurrencies endpoint firstly

• In project I used .NET 6 the create web apis and MS SQL server database to store data.

• 4 tables are created with Code-first approach
• User; Dummy users are created to assume that specific clients could be use 
• Currency; Storing all currency data end gathering all data from ExchangeRate API
• ExchangeRate; Storing all currency exchange rate data 
• Transaction; Storing all transaction logs if client want to complete the exchange operation.

• Used Entity Framework Core to access data.

• Used simple memory cache to handle requirements


• Added log table to sql server to store process and errors.

• Used Exchange Rates Data API to get currency rates.

• Used Swagger UI to visualize and interact with the API's resources.


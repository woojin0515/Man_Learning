# Man Learning

Man Learning is a gamified AI learning web application.

## Status

Initial project setup.

## Technology

- Blazor Web App
- ASP.NET Core
- .NET 10

## Deployment

The production deployment target is Azure App Service.

The current staging deployment is available at https://manlearning-woojin-krc.azurewebsites.net.
It runs in the `rg-manlearning-krc` resource group on the `asp-manlearning-krc` Linux B1 App Service Plan in Korea Central.

## External APIs

External APIs will be determined through future technical spikes.

## Common commands

```bash
dotnet restore ManLearning.sln
dotnet build ManLearning.sln
dotnet test ManLearning.sln
dotnet test tests/ManLearning.Domain.Tests/ManLearning.Domain.Tests.csproj --filter "FullyQualifiedName~TestName"
dotnet run --project src/ManLearning.Web/ManLearning.Web.csproj
```

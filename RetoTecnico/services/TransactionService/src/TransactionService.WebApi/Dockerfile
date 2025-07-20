FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY TransactionService.sln ./
COPY src/TransactionService.Application ./src/TransactionService.Application/
COPY src/TransactionService.Domain ./src/TransactionService.Domain/
COPY src/TransactionService.Infrastructure ./src/TransactionService.Infrastructure/
COPY src/TransactionService.WebApi ./src/TransactionService.WebApi/

WORKDIR /app/src/TransactionService.WebApi
RUN dotnet restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
COPY --from=build /out .
ENTRYPOINT ["dotnet", "TransactionService.WebApi.dll"]

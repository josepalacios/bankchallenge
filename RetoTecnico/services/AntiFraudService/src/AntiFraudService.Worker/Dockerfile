FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY AntiFraudService.sln ./
COPY src/AntiFraudService.Application ./src/AntiFraudService.Application/
COPY src/AntiFraudService.Domain ./src/AntiFraudService.Domain/
COPY src/AntiFraudService.Infrastructure ./src/AntiFraudService.Infrastructure/
COPY src/AntiFraudService.Worker ./src/AntiFraudService.Worker/

WORKDIR /app/src/AntiFraudService.Worker
RUN dotnet restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "AntiFraudService.Worker.dll"]

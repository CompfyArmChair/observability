FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt-get update && apt-get install -y stress

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIs/WarehouseApi/WarehouseApi.csproj", "API/WarehouseApi/"]
COPY ["Shared/Shared.ServiceBus/Shared.ServiceBus.csproj", "Shared/Shared.ServiceBus/"]
COPY ["Shared/Shared.Instrumentation/Shared.Instrumentation.csproj", "Shared/Shared.Instrumentation/"]
WORKDIR "/src/API/WarehouseApi/"
COPY APIs/WarehouseApi/ .
COPY Shared/Shared.ServiceBus/ .
COPY Shared/Shared.Instrumentation/ .

RUN dotnet build "WarehouseApi.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "WarehouseApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseApi.dll"]
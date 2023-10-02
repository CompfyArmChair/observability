FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt-get update && apt-get install -y stress

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIs/CatalogueApi/CatalogueApi.csproj", "API/CatalogueApi/"]
COPY ["Shared/Shared.ServiceBus/Shared.ServiceBus.csproj", "Shared/Shared.ServiceBus/"]
COPY ["Shared/Shared.Instrumentation/Shared.Instrumentation.csproj", "Shared/Shared.Instrumentation/"]
WORKDIR "/src/API/CatalogueApi/"
COPY APIs/CatalogueApi/ .
COPY Shared/Shared.ServiceBus/ .
COPY Shared/Shared.Instrumentation/ .

RUN dotnet build "CatalogueApi.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "CatalogueApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogueApi.dll"]
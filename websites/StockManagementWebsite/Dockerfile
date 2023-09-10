FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY NuGet.Config  /root/.nuget/NuGet/
COPY "localnugets/" "/var/nugets/"

COPY ["websites/StockManagementWebsite/Server/StockManagementWebsite.Server.csproj", "websites/StockManagementWebsite/Server/"]
COPY ["websites/StockManagementWebsite/Client/StockManagementWebsite.Client.csproj", "websites/StockManagementWebsite/Client/"]
COPY ["websites/StockManagementWebsite/Shared/StockManagementWebsite.Shared.csproj", "websites/StockManagementWebsite/Shared/"]
COPY ["Shared/Shared.ServiceBus/Shared.ServiceBus.csproj", "Shared/Shared.ServiceBus/"]
COPY ["Shared/Shared.Instrumentation/Shared.Instrumentation.csproj", "Shared/Shared.Instrumentation/"]
WORKDIR "/src/website/StockManagementWebsite/Client/"
COPY websites/StockManagementWebsite/Client/ .
WORKDIR "/src/website/StockManagementWebsite/Shared/"
COPY websites/StockManagementWebsite/Shared/ .
WORKDIR "/src/website/StockManagementWebsite/Server/"
COPY websites/StockManagementWebsite/Server/ .
COPY Shared/Shared.ServiceBus/ .
COPY Shared/Shared.Instrumentation/ .

RUN dotnet restore "StockManagementWebsite.Server.csproj"

RUN dotnet build "StockManagementWebsite.Server.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "StockManagementWebsite.Server.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockManagementWebsite.Server.dll"]
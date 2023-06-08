FROM mcr.microsoft.com/dotnet/aspnet:7.0.5 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build
WORKDIR /src
COPY ["websites/ShopWebsite/Server/ShopWebsite.Server.csproj", "website/ShopWebsite/Server/"]
COPY ["websites/ShopWebsite/Client/ShopWebsite.Client.csproj", "website/ShopWebsite/Client/"]
COPY ["websites/ShopWebsite/Shared/ShopWebsite.Shared.csproj", "website/ShopWebsite/Shared/"]
COPY ["Shared/Shared.ServiceBus.csproj", "Shared/"]
WORKDIR "/src/website/ShopWebsite/Client/"
COPY websites/ShopWebsite/Client/ .
WORKDIR "/src/website/ShopWebsite/Shared/"
COPY websites/ShopWebsite/Shared/ .
WORKDIR "/src/website/ShopWebsite/Server/"
COPY websites/ShopWebsite/Server/ .
COPY Shared/ .
RUN dotnet build "ShopWebsite.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  "ShopWebsite.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopWebsite.Server.dll"]
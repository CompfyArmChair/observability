#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["websites/ShopWebsite/Server/ShopWebsite.Server.csproj", "websites/ShopWebsite/Server/"]
COPY ["Shared/Shared.ServiceBus/Shared.ServiceBus.csproj", "Shared/Shared.ServiceBus/"]
COPY ["websites/ShopWebsite/Client/ShopWebsite.Client.csproj", "websites/ShopWebsite/Client/"]
COPY ["websites/ShopWebsite/Shared/ShopWebsite.Shared.csproj", "websites/ShopWebsite/Shared/"]


RUN dotnet restore "websites/ShopWebsite/Server/ShopWebsite.Server.csproj"
COPY . .
WORKDIR "/src/websites/ShopWebsite/Server"
RUN dotnet build "ShopWebsite.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopWebsite.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopWebsite.Server.dll"]
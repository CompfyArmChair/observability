#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIs/BillingApi/BillingApi.csproj", "API/BasketApi/"]
COPY ["Shared/Shared.ServiceBus.csproj", "Shared/"]
WORKDIR "/src/APIs/BillingApi/"
COPY APIs/BillingApi/ .
COPY Shared/ .

RUN dotnet restore "BillingApi.csproj" --disable-parallel

RUN dotnet build "BillingApi.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "BillingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingApi.dll"]
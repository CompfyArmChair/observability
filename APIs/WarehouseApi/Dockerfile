#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIs/WarehouseApi/WarehouseApi.csproj", "API/WarehouseApi/"]
COPY ["Shared/Shared.ServiceBus.csproj", "Shared/"]
WORKDIR "/src/API/WarehouseApi/"
COPY APIs/WarehouseApi/ .
COPY Shared/ .

RUN dotnet restore "WarehouseApi.csproj" --disable-parallel

RUN dotnet build "WarehouseApi.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "WarehouseApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseApi.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APIs/OrderApi/OrderApi.csproj", "API/OrderApi/"]
COPY ["Shared/Shared.ServiceBus/Shared.ServiceBus.csproj", "Shared/Shared.ServiceBus/"]
COPY ["Shared/Shared.Instrumentation/Shared.Instrumentation.csproj", "Shared/Shared.Instrumentation/"]
WORKDIR "/src/API/OrderApi/"
COPY APIs/OrderApi/ .
COPY Shared/Shared.ServiceBus/ .
COPY Shared/Shared.Instrumentation/ .

RUN dotnet restore "OrderApi.csproj" --disable-parallel

RUN dotnet build "OrderApi.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "OrderApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderApi.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 5010

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WeatherZapto.WebServer/WeatherZapto.WebServer.csproj", "WeatherZapto.WebServer/"]
COPY ["Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/Serilog.Sinks.SQLite.csproj", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
COPY ["WeatherZapto.Infrastructure.Services/WeatherZapto.Infrastructure.Services.csproj", "WeatherZapto.Infrastructure.Services/"]
COPY ["Framework.Core/Framework.Core.csproj", "Framework.Core/"]
COPY ["WeatherZapto.Application.Services/WeatherZapto.Application.Services.csproj", "WeatherZapto.Application.Services/"]
COPY ["Framework.Infrastructure.Services/Framework.Infrastructure.Services.csproj", "Framework.Infrastructure.Services/"]
COPY ["WeatherZapto.Domain/WeatherZapto.Domain.csproj", "WeatherZapto.Domain/"]
COPY ["WeatherZapto.WebServer.Services/WeatherZapto.WebServer.Services.csproj", "WeatherZapto.WebServer.Services/"]
COPY ["WeatherZapto.Data/WeatherZapto.Data.csproj", "WeatherZapto.Data/"]
COPY ["WeatherZapto.Data.Services/WeatherZapto.Data.Services.csproj", "WeatherZapto.Data.Services/"]
COPY ["Framework.Data.Services/Framework.Data.Services.csproj", "Framework.Data.Services/"]
RUN dotnet restore "WeatherZapto.WebServer/WeatherZapto.WebServer.csproj"
COPY ["WeatherZapto.WebServer/*", "WeatherZapto.WebServer/"]
COPY ["Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/*", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
COPY ["WeatherZapto.Infrastructure.Services/*", "WeatherZapto.Infrastructure.Services/"]
COPY ["Framework.Core/*", "Framework.Core/"]
COPY ["WeatherZapto.Application.Services/*", "WeatherZapto.Application.Services/"]
COPY ["Framework.Infrastructure.Services/*", "Framework.Infrastructure.Services/"]
COPY ["WeatherZapto.Domain/*", "WeatherZapto.Domain/"]
COPY ["WeatherZapto.WebServer.Services/*", "WeatherZapto.WebServer.Services/"]
COPY ["WeatherZapto.Data/*", "WeatherZapto.Data/"]
COPY ["WeatherZapto.Data.Services/*", "WeatherZapto.Data.Services/"]
COPY ["Framework.Data.Services/*", "Framework.Data.Services/"]
WORKDIR "/src/WeatherZapto.WebServer"
RUN dotnet build "WeatherZapto.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherZapto.WebServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherZapto.WebServer.dll"]
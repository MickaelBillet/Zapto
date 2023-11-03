#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 6001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AirZapto.WebServer/AirZapto.WebServer.csproj", "AirZapto.WebServer/"]
COPY ["AirZapto.WebServer.Services/AirZapto.WebServer.Services.csproj", "AirZapto.WebServer.Services/"]
COPY ["AirZapto.Application.Services/AirZapto.Application.Services.csproj", "AirZapto.Application.Services/"]
COPY ["AirZapto.Domain/AirZapto.Domain.csproj", "AirZapto.Domain/"]
COPY ["Framework.Core/Framework.Core.csproj", "Framework.Core/"]
COPY ["Framework.Infrastructure.Services/Framework.Infrastructure.Services.csproj", "Framework.Infrastructure.Services/"]
COPY ["AirZapto.Data/AirZapto.Data.csproj", "AirZapto.Data/"]
COPY ["AirZapto.Data.Services/AirZapto.Data.Services.csproj", "AirZapto.Data.Services/"]
COPY ["Framework.Data.Services/Framework.Data.Services.csproj", "Framework.Data.Services/"]
COPY ["Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/Serilog.Sinks.SQLite.csproj", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
RUN dotnet restore "AirZapto.WebServer/AirZapto.WebServer.csproj"
COPY ["AirZapto.WebServer/*", "AirZapto.WebServer/"]
COPY ["AirZapto.WebServer.Services/*", "AirZapto.WebServer.Services/"]
COPY ["AirZapto.Application.Services/*", "AirZapto.Application.Services/"]
COPY ["AirZapto.Domain/*", "AirZapto.Domain/"]
COPY ["Framework.Core/*", "Framework.Core/"]
COPY ["Framework.Infrastructure.Services/*", "Framework.Infrastructure.Services/"]
COPY ["AirZapto.Data.Services/*", "AirZapto.Data.Services/"]
COPY ["AirZapto.Data/*", "AirZapto.Data/"]
COPY ["Framework.Data.Services/*", "Framework.Data.Services/"]
COPY ["Serilog.Sqlite.Sinks/src/*", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
WORKDIR "/src/AirZapto.WebServer"
RUN dotnet build "AirZapto.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirZapto.WebServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirZapto.WebServer.dll"]
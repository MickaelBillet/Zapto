#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001/udp
EXPOSE 5002/udp
EXPOSE 5003/udp
EXPOSE 5004/udp
EXPOSE 5005/udp
EXPOSE 5007/udp

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Connect.WebServer/Connect.WebServer.csproj", "Connect.WebServer/"]
COPY ["Connect.WebServer.Services/Connect.WebServer.Services.csproj", "Connect.WebServer.Services/"]
COPY ["Connect.Application.Services/Connect.Application.Services.csproj", "Connect.Application.Services/"]
COPY ["Connect.Domain/Connect.Domain.csproj", "Connect.Domain/"]
COPY ["Framework.Core/Framework.Core.csproj", "Framework.Core/"]
COPY ["Framework.Common.Services/Framework.Common.Services.csproj", "Framework.Common.Services/"]
COPY ["Connect.Data.Repositories/Connect.Data.Repositories.csproj", "Connect.Data.Repositories/"]
COPY ["Connect.Data.Supervisors/Connect.Data.Supervisors.csproj", "Connect.Data.Supervisors/"]
COPY ["Connect.Data.Services/Connect.Data.Services.csproj", "Connect.Data.Services/"]
COPY ["Framework.Data.Services/Framework.Data.Services.csproj", "Framework.Data.Services/"]
COPY ["Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/Serilog.Sinks.SQLite.csproj", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
RUN dotnet restore "Connect.WebServer/Connect.WebServer.csproj"
COPY ["Connect.WebServer/*", "Connect.WebServer/"]
COPY ["Connect.WebServer.Services/*", "Connect.WebServer.Services/"]
COPY ["Connect.Application.Services/*", "Connect.Application.Services/"]
COPY ["Connect.Domain/*", "Connect.Domain/"]
COPY ["Framework.Core/*", "Framework.Core/"]
COPY ["Framework.Common.Services/*", "Framework.Common.Services/"]
COPY ["Connect.Data.Services/*", "Connect.Data.Services/"]
COPY ["Connect.Data.Repositories/*", "Connect.Data.Repositories/"]
COPY ["Connect.Data.Supervisors/*", "Connect.Data.Supervisors/"]
COPY ["Framework.Data.Services/*", "Framework.Data.Services/"]
COPY ["Serilog.Sqlite.Sinks/src/*", "Serilog.Sqlite.Sinks/src/Serilog.Sinks.SQLite/"]
WORKDIR "/src/Connect.WebServer"
RUN dotnet build "Connect.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Connect.WebServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Connect.WebServer.dll"]
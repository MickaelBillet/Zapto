#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 5010

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/WeatherZapto.WebServer"
RUN dotnet build "WeatherZapto.WebServer/WeatherZapto.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherZapto.WebServer/WeatherZapto.WebServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherZapto.WebServer.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app
RUN mkdir -p /app/DataProtection-Keys

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["rss-reader/rss-reader.csproj", "rss-reader/"]
RUN dotnet restore "rss-reader/rss-reader.csproj"
COPY . .
WORKDIR "/src/rss-reader"
RUN dotnet build "rss-reader.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "rss-reader.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rss-reader.dll"]
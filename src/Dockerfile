ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "Directory.Packages.props", "NuGet.config", ".editorconfig", "./"]
COPY ["src/Orchid.Service.Clipboard.csproj", "src/"]
RUN dotnet restore "src/Orchid.Service.Clipboard.csproj"

COPY ["src/", "src/"]
RUN dotnet publish "src/Orchid.Service.Clipboard.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false -p:EnforceCodeStyleInBuild=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orchid.Service.Clipboard.dll"]
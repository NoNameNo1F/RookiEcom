FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Hosts/RookiEcom.FrontStore/RookiEcom.FrontStore.csproj", "src/Hosts/RookiEcom.FrontStore/"]
RUN dotnet restore "src/Hosts/RookiEcom.FrontStore/RookiEcom.FrontStore.csproj"
COPY . .
WORKDIR "/src/src/Hosts/RookiEcom.FrontStore"
RUN dotnet build "RookiEcom.FrontStore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RookiEcom.FrontStore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RookiEcom.FrontStore.dll"]

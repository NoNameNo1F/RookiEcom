# Stage 1: Restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk_base
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Assuming your RookiEcom.sln file is at the root of your repository:
# And your build context in Azure DevOps is $(Build.SourcesDirectory)
COPY ["RookiEcom.sln", "."]

# Copy project file for IdentityServer
COPY ["src/Hosts/RookiEcom.IdentityServer/RookiEcom.IdentityServer.csproj", "src/Hosts/RookiEcom.IdentityServer/"]

# Copy project files for referenced BuildingBlocks
# Ensure these paths are correct relative to your $(Build.SourcesDirectory)
COPY ["src/BuildingBlocks/RookiEcom.Application/RookiEcom.Application.csproj", "src/BuildingBlocks/RookiEcom.Application/"]
COPY ["src/BuildingBlocks/RookiEcom.Domain/RookiEcom.Domain.csproj", "src/BuildingBlocks/RookiEcom.Domain/"]
COPY ["src/BuildingBlocks/RookiEcom.Infrastructure/RookiEcom.Infrastructure.csproj", "src/BuildingBlocks/RookiEcom.Infrastructure/"]

# If you have a NuGet.config, copy it:
# COPY ["NuGet.config", "."]

# Restore dependencies using the solution file for robustness
# This ensures all project references within the solution are resolved correctly.
# RUN dotnet restore "RookiEcom.sln"
# Or, if you prefer to restore only the specific project and its dependencies:
RUN dotnet restore "src/Hosts/RookiEcom.IdentityServer/RookiEcom.IdentityServer.csproj"

# Stage 2: Build and publish the application
# Copy the rest of the source code now that dependencies are restored
FROM sdk_base AS publish
COPY . .
WORKDIR "/src/src/Hosts/RookiEcom.IdentityServer"
# RUN dotnet build "RookiEcom.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# FROM build AS publish
# ARG BUILD_CONFIGURATION is inherited
RUN dotnet publish "RookiEcom.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

COPY ["tempkey.jwk", "/app/publish/tempkey.jwk"]

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
# EXPOSE 8081 # Default HTTPS port

# Create a non-root user and group
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=publish /app/publish .

# Copy the tempkey.jwk for Duende IdentityServer development signing credential
# Ensure this path is correct relative to the IdentityServer project directory in the publish output
# If tempkey.jwk is in the root of the publish output for IdentityServer, this is correct.
# COPY --from=publish /app/publish/tempkey.jwk ./tempkey.jwk

# Copy Views and wwwroot if they are not embedded or handled by SDK publish for Razor pages/MVC
# The default `dotnet publish` for web projects usually includes these.
# If you find views are missing, you might need explicit COPY for /app/publish/Views and /app/publish/wwwroot
# COPY --from=publish /app/publish/Views ./Views
# COPY --from=publish /app/publish/wwwroot ./wwwroot

USER appuser
ENTRYPOINT ["dotnet", "RookiEcom.IdentityServer.dll"]

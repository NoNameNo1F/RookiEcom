# Stage 1: Restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all .csproj files and the .sln file first for NuGet restore.
# This pattern helps Docker cache the restore layer effectively.
# Paths are relative to the build context, which is $(Build.SourcesDirectory) in your pipeline.

# Assuming your RookiEcom.sln file is at the root of your repository:
COPY ["RookiEcom.sln", "."]

# Copy project files for WebAPI
COPY ["src/Hosts/RookiEcom.WebAPI/RookiEcom.WebAPI.csproj", "src/Hosts/RookiEcom.WebAPI/"]

# Copy project files for BuildingBlocks
COPY ["src/BuildingBlocks/RookiEcom.Infrastructure/RookiEcom.Infrastructure.csproj", "src/BuildingBlocks/RookiEcom.Infrastructure/"]
COPY ["src/BuildingBlocks/RookiEcom.Application/RookiEcom.Application.csproj", "src/BuildingBlocks/RookiEcom.Application/"]
COPY ["src/BuildingBlocks/RookiEcom.Domain/RookiEcom.Domain.csproj", "src/BuildingBlocks/RookiEcom.Domain/"] 

# Copy project files for Cart Module
COPY ["src/Modules/Cart/RookiEcom.Modules.Cart.Application/RookiEcom.Modules.Cart.Application.csproj", "src/Modules/Cart/RookiEcom.Modules.Cart.Application/"]
COPY ["src/Modules/Cart/RookiEcom.Modules.Cart.Domain/RookiEcom.Modules.Cart.Domain.csproj", "src/Modules/Cart/RookiEcom.Modules.Cart.Domain/"]
COPY ["src/Modules/Cart/RookiEcom.Modules.Cart.Infrastructure/RookiEcom.Modules.Cart.Infrastructure.csproj", "src/Modules/Cart/RookiEcom.Modules.Cart.Infrastructure/"]
COPY ["src/Modules/Cart/RookiEcom.Modules.Cart.Contracts/RookiEcom.Modules.Cart.Contracts.csproj", "src/Modules/Cart/RookiEcom.Modules.Cart.Contracts/"]

# Copy project files for Product Module
COPY ["src/Modules/ProductModule/RookiEcom.Modules.Product.Application/RookiEcom.Modules.Product.Application.csproj", "src/Modules/ProductModule/RookiEcom.Modules.Product.Application/"]
COPY ["src/Modules/ProductModule/RookiEcom.Modules.Product.Contracts/RookiEcom.Modules.Product.Contracts.csproj", "src/Modules/ProductModule/RookiEcom.Modules.Product.Contracts/"]
COPY ["src/Modules/ProductModule/RookiEcom.Modules.Product.Domain/RookiEcom.Modules.Product.Domain.csproj", "src/Modules/ProductModule/RookiEcom.Modules.Product.Domain/"]
COPY ["src/Modules/ProductModule/RookiEcom.Modules.Product.Infrastructure/RookiEcom.Modules.Product.Infrastructure.csproj", "src/Modules/ProductModule/RookiEcom.Modules.Product.Infrastructure/"]

# Restore dependencies for the WebAPI project (or the entire solution if preferred)
# This will ensure all project references are resolved.
RUN dotnet restore "src/Hosts/RookiEcom.WebAPI/RookiEcom.WebAPI.csproj"
# Alternatively, to restore the entire solution (often more robust):
# RUN dotnet restore "RookiEcom.sln"

# Stage 2: Build and publish the application
# Copy the rest of the source code now that dependencies are restored
COPY . .
WORKDIR "/src/src/Hosts/RookiEcom.WebAPI"
RUN dotnet build "RookiEcom.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
# ARG BUILD_CONFIGURATION is inherited from the 'build' stage
RUN dotnet publish "RookiEcom.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 7670
EXPOSE 7671 # ASP.NET Core default HTTPS port in containers; adjust if your app uses different ports

# Create a non-root user and group for security
# The $APP_UID from your original Dockerfile is not standardly defined inside Docker build
# unless passed as a build-arg. Creating a specific user is more self-contained.
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=publish /app/publish .

# It's good practice to change to the non-root user *after* all file operations (like COPY)
# and before setting the ENTRYPOINT.
USER appuser

ENTRYPOINT ["dotnet", "RookiEcom.WebAPI.dll"]

# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj file and restore dependencies
COPY *.csproj .
RUN dotnet restore --verbosity normal

# Copy the rest of the source code
COPY . .

# Build and publish the application in one step
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser
USER appuser

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Expose port 8080 (Azure Container Apps default)
EXPOSE 8080

# Configure the app to listen on the correct port for Azure Container Apps
ENV ASPNETCORE_URLS=http://+:8080

# Set the entry point
ENTRYPOINT ["dotnet", "SK8DotNet.API.dll"]
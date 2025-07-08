FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj", "src/Api/B3DeveloperEvaluation.Api/"]
COPY ["src/Application/B3DeveloperEvaluation.Application/B3DeveloperEvaluation.Application.csproj", "src/Application/B3DeveloperEvaluation.Application/"]
COPY ["src/Domain/B3DeveloperEvaluation.Domain/B3DeveloperEvaluation.Domain.csproj", "src/Domain/B3DeveloperEvaluation.Domain/"]

# Restore dependencies
RUN dotnet restore "src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj"

# Copy the rest of the code
COPY . .

# Build the application
WORKDIR "/src/src/Api/B3DeveloperEvaluation.Api"
RUN dotnet build "B3DeveloperEvaluation.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "B3DeveloperEvaluation.Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
RUN apt-get update && apt-get install -y wget && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory and set permissions
RUN mkdir -p /app/logs && chmod 755 /app/logs

EXPOSE 5100
ENTRYPOINT ["dotnet", "B3DeveloperEvaluation.Api.dll"]
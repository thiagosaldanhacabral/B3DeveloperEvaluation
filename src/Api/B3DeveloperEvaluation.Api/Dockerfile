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
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3DeveloperEvaluation.Api.dll"]
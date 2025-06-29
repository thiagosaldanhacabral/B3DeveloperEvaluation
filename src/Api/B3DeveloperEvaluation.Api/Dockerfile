# Etapa 1: build
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5100
ENV ASPNETCORE_URLS=http://+:5100

# Copia arquivos de projeto e restaura dependências
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj", "src/Api/B3DeveloperEvaluation.Api/"]
COPY ["src/Application/B3DeveloperEvaluation.Application/B3DeveloperEvaluation.Application.csproj", "src/Application/B3DeveloperEvaluation.Application/"]
RUN dotnet restore ".src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/B3DeveloperEvaluation.Api"
RUN dotnet build "./B3DeveloperEvaluation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: runtime
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./B3DeveloperEvaluation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3DeveloperEvaluation.Api.dll"]
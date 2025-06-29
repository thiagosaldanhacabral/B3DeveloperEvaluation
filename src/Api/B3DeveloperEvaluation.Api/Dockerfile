# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie apenas os arquivos de projeto e restore as dependências
COPY src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj src/Api/B3DeveloperEvaluation.Api/
COPY src/Application/B3DeveloperEvaluation.Application/B3DeveloperEvaluation.Application.csproj src/Application/B3DeveloperEvaluation.Application/
RUN dotnet restore src/Api/B3DeveloperEvaluation.Api/B3DeveloperEvaluation.Api.csproj

# Copie o restante dos arquivos necessários
COPY src/Api/B3DeveloperEvaluation.Api/ src/Api/B3DeveloperEvaluation.Api/
COPY src/Application/B3DeveloperEvaluation.Application/ src/Application/B3DeveloperEvaluation.Application/

WORKDIR /src/src/Api/B3DeveloperEvaluation.Api
RUN dotnet build B3DeveloperEvaluation.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish B3DeveloperEvaluation.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5100
ENV ASPNETCORE_URLS=http://+:5100

# Criar e usar um usuário
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "B3DeveloperEvaluation.Api.dll"]
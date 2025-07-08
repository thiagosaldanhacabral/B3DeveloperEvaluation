@echo off
echo  Iniciando ambiente B3 Developer Evaluation...
echo =================================================

REM Verificar se o Docker esta rodando
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo Docker nao esta rodando. Por favor, inicie o Docker e tente novamente.
    pause
    exit /b 1
)

REM Verificar configuracao do sistema
echo Verificando configuracao do sistema...
wsl --list --verbose >nul 2>&1
if %errorlevel% equ 0 (
    echo Configurando WSL2 para SonarQube...
    wsl -d docker-desktop sysctl -w vm.max_map_count=262144 >nul 2>&1
    echo WSL2 configurado.
)

echo Parando containers existentes...
docker-compose down -v --remove-orphans

echo Removendo volumes antigos (se existirem)...
for /f "tokens=*" %%i in ('docker volume ls -q ^| findstr /i "sonarqube b3" 2^>nul') do docker volume rm %%i 2>nul

echo Limpando imagens antigas do SonarQube...
docker rmi sonarqube:latest >nul 2>&1
docker rmi sonarqube:10.7-community >nul 2>&1

echo Construindo imagens...
docker-compose build --no-cache

echo.
echo Iniciando servicos em sequencia:
echo    1. SonarQube sera iniciado primeiro
echo    2. Aguardar o SonarQube estar saudavel
echo    3. API sera iniciada apos o SonarQube
echo    4. UI sera iniciada apos a API
echo.

REM Iniciar todos os servicos (com dependencias automaticas)
docker-compose up -d

echo.
echo Monitorando progresso da inicializacao...
echo.

REM Aguardar e mostrar progresso
:CHECK_SONAR
echo Verificando status do SonarQube...
docker-compose ps sonarqube-b3 | findstr "healthy" >nul
if %errorlevel% neq 0 (
    echo    SonarQube ainda nao esta pronto. Aguardando...
    timeout /t 15 /nobreak >nul
    goto CHECK_SONAR
)
echo  SonarQube esta pronto!

echo  API esta pronta!

echo  UI esta pronta!

echo.
echo  Todos os servicos foram iniciados com sucesso!
echo.
echo Status atual dos containers:
docker-compose ps

echo.
echo URLs dos servicos:
echo    SonarQube: http://localhost:9000
echo    API: http://localhost:5100
echo    UI: http://localhost:4200
echo.
echo Comandos uteis:
echo    docker-compose logs -f                    # Ver logs de todos os servicos
echo    docker-compose logs -f sonarqube-b3       # Ver logs do SonarQube
echo    docker-compose logs -f sonarqube-config   # Ver logs da configuracao
echo    docker-compose ps                         # Ver status dos containers
echo.
echo Ambiente pronto para uso!
echo.
echo Pressione qualquer tecla para continuar...
pause >nul
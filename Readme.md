# API de Avaliação de Desenvolvedor B3

## Visão Geral
Este projeto implementa uma API de cálculo de investimentos usando princípios de Clean Architecture e tecnologias modernas do .NET.

## Stack Tecnológico
- .NET 9.0
- Docker & Docker Compose
- Swagger/OpenAPI
- Serilog para logging
- MediatR para padrão CQRS
- AutoMapper para mapeamento de objetos
- XUnit para testes unitários
- NSubstitute para mocking
- SonarQube para análise de qualidade de código

## UI
- **Angular 20**: Framework para construção de aplicações web modernas.
- **Node.js**: Ambiente de execução para JavaScript no backend.
- **Docker**: Plataforma para criar, rodar e gerenciar containers.

## Arquitetura
A solução segue os princípios de Clean Architecture com as seguintes camadas:
- **API**: Interface HTTP e configuração
- **Application**: Lógica de negócio, comandos e DTOs
- **Domain**: Regras de negócio principais e entidades
- **Tests**: Testes unitários cobrindo a lógica de negócio

## Pré-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/downloads)

## 1. Clonar o Repositório
```bash
git clone https://github.com/thiagosaldanhacabral/B3DeveloperEvaluation.git
cd B3DeveloperEvaluation
```

## 2. Executar com Docker
A aplicação está containerizada e pode ser executada usando Docker Compose. Executar o arquivo start-environment.bat na raiz do solution
no Terminal como administrador se for no Windows.
Se for no Linux executar o comando

```bash
chmod +x start-environment.sh
```

Depois

```bash
./start-environment.sh
```

Após ser executado o arquivo pela primeira vez, nás próximas pode ser usado o comando abaixo na raiz do solution:

```bash
docker-compose up -d
```

### 2.1. Configuração

Principais configurações no docker-compose.yml:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ASPNETCORE_URLS=http://+:5100
  - Cdi=0.009
  - Tb=1.08
```

Onde Cdi é a taxa do CDI e Tb é a taxa do banco

### 2.2 Endpoints de Health Check

/health/ready - Verificação de prontidão (para Kubernetes readiness probe)
/health/live - Verificação de vida (para Kubernetes liveness probe)

## 3. Configuração do SonarQube (.NET 9)

Estas instruções permitem instalar e configurar o SonarQube do zero em ambiente local

---

### 3.1. Pré-requisitos

- **Java 17 ou superior** (SonarQube precisa do Java para rodar)
- **Docker Desktop** (recomendado para facilitar a instalação)
- **.NET SDK 9** (já utilizado no projeto)
- **Git** (para clonar o repositório)

---

### 3.2. Primeiro acesso ao SonarQube

- Acesse [http://localhost:9000](http://localhost:9000) no navegador.
- Login padrão:
  - **Usuário:** `admin`
  - **Senha:** `admin`
- No primeiro acesso, será solicitado que você altere a senha do usuário `admin`.

---

2.5. Gerando um Token de Autenticação

1. No menu superior direito, clique no seu usuário e depois em **"My Account"**.
2. Vá até a aba **"Security"**.
3. Em **"Generate Tokens"**, digite um nome (ex: `token-dotnet`), selecione o Type User Token e clique em **"Generate"**.
4. Copie o token gerado e guarde em local seguro (ele será usado nos próximos passos).

---

### 3.3. Executando Análise

3.3.1. Executar a análise:
No diretório raiz do solution executar:

```bash
dotnet sonarscanner begin /k:"B3DeveloperEvaluation" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="seu-token"
dotnet build
dotnet sonarscanner end /d:sonar.login="seu-token"
```

3.3.2. Visualizar resultados no painel do SonarQube: http://localhost:9000

**Obs:** Substituir a URL e o token pelas informações do seu servidor do SonarQube

## 4. Configuração

Principais configurações no docker-compose.yml:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ASPNETCORE_URLS=http://+:5100
  - Cdi=0.009
  - Tb=1.08
```

Onde Cdi é a taxa do CDI e Tb é a taxa do banco


## 5. Verificar a Instalação
Uma vez em execução, você pode acessar:
- Documentação da API: http://localhost:5100/swagger
- Health Check: Executar o curl abaixo:

```PowerShell
curl -Method GET 'http://localhost:5100/health/ready' -Headers @{ Accept = '*/*' }
```

```bash
curl -X GET 'http://localhost:5100/health/ready' -H 'accept: */*'

```

## 6. Executar Testes
Para executar os testes unitários:

```bash
cd tests/Api.Tests
dotnet test
```

Para relatório de cobertura de testes:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

Obs: Recomendado executar o comando abaixo para criar um certificado .NET para ambiente de desenvolvimento

```bash
dotnet dev-certs https --trust
```

## 7. Solução de Problemas
1. Se o container Docker falhar ao iniciar:
   - Verifique se o Docker Desktop está em execução
   - Confirme que a porta 5100 não está em uso
   - Verifique os logs: `docker-compose logs`

2. Se os testes falharem ao executar:
   - Certifique-se de que o .NET 9 SDK está instalado
   - Tente limpar a solução: `dotnet clean`
   - Restaure os pacotes: `dotnet restore`

---

## Suporte

Para problemas ou dúvidas, entre em contato pelo e-mail [thiagosaldanhacabral@gmail.com](mailto:thiagosaldanhacabral@gmail.com).

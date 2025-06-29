# B3 Developer Evaluation API

## Visão Geral do Negócio

Esta API foi desenvolvida para simular investimentos baseados em taxas do mercado financeiro brasileiro, como CDI e TB. O objetivo é fornecer um endpoint que calcula o retorno bruto, o imposto devido e o valor líquido de um investimento, considerando o valor inicial e o prazo em meses. É ideal para aplicações que desejam oferecer simulações de investimentos de renda fixa para seus usuários.

## Arquitetura

A solução segue uma arquitetura em camadas, separando responsabilidades de forma clara:

- **Domain**: Contém as entidades e regras de negócio puras.
- **Application**: Implementa os serviços de aplicação, comandos, DTOs, mapeamentos e integrações com MediatR e AutoMapper.
- **API**: Camada de apresentação, responsável por expor os endpoints HTTP, configurar DI, logging, Swagger e orquestrar as chamadas de aplicação.
- **Tests**: Projeto de testes automatizados para garantir a qualidade dos serviços de aplicação.

### Principais Frameworks e Bibliotecas

- **.NET 9**: Plataforma principal para desenvolvimento da API.
- **MediatR**: Implementação do padrão Mediator para desacoplamento entre camadas.
- **AutoMapper**: Mapeamento automático entre entidades e DTOs.
- **Serilog**: Logging estruturado e persistência de logs em arquivos.
- **Swashbuckle.AspNetCore**: Geração automática de documentação Swagger/OpenAPI.
- **xUnit/NSubstitute**: Testes automatizados e mocks.

## Como Executar com Docker Compose

Siga os passos abaixo para subir a aplicação utilizando Docker Compose:

1. **Pré-requisitos**  
   - Docker instalado ([Download Docker](https://www.docker.com/products/docker-desktop/))
   - Docker Compose instalado (já incluso no Docker Desktop)

2. **Clone o repositório**
	gh repo clone thiagosaldanhacabral/B3DeveloperEvaluation

3. **Suba o docker-compose**
	docker compose up --build

6. **Acesse a API**
   - Swagger: [http://localhost:5100/swagger](http://localhost:5100/swagger)
   - Endpoint principal: `POST /calculate`

## Observações

- Os logs da aplicação serão salvos na pasta `logs` do projeto.
- As configurações de taxas podem ser ajustadas no arquivo `appsettings.json`.
- Para rodar os testes, utilize o comando:
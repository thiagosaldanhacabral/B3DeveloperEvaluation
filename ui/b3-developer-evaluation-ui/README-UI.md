# B3 Developer Evaluation UI

> Guia simples para rodar o projeto Angular em Docker

---

## Tecnologias Utilizadas

- **Angular**: Framework para construção de aplicações web modernas.
- **Node.js**: Ambiente de execução para JavaScript no backend.
- **Docker**: Plataforma para criar, rodar e gerenciar containers.

---

## Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado no seu computador (Windows, Mac ou Linux).

---

## Como rodar o projeto usando Docker

1. **Baixe o código do projeto**
   - Faça o download ou clone este repositório para uma pasta no seu computador.

2. **Abra o terminal**
   - No Windows, você pode usar o PowerShell. No Mac ou Linux, use o Terminal.
   - Navegue até a pasta do projeto ui/b3-developer-evaluation-ui. Exemplo:

     ```bash
     cd caminho/para/a/pasta/ui/b3-developer-evaluation-ui
     ```

3. **Construa a imagem Docker**
   - Execute o comando abaixo para criar a imagem do projeto:

     ```bash
     docker build -t b3-angular-app .
     ```

4. **Rode o container Docker**
   - Após a imagem ser criada, execute:

     ```bash
     docker run --name b3-angular-app -p 4200:4200 b3-angular-app
     ```

   - Isso fará o site ficar disponível no seu computador.

5. **Abra o navegador**
   - Acesse: [http://localhost:4200](http://localhost:4200)
   - Você verá a aplicação Angular rodando.

---

## Dicas Úteis

- Para parar o container:

  ```bash
  docker stop b3-angular-app
  ```

- Para remover o container:

  ```bash
  docker rm b3-angular-app
  ```

- Para remover a imagem:

  ```bash
  docker rmi b3-angular-app
  ```

---

## Sobre o Dockerfile

O arquivo `Dockerfile` já está configurado para:

- Instalar dependências do Node.js
- Gerar a versão de produção do Angular
- Servir a aplicação usando um servidor web leve (Nginx ou similar)

Você não precisa alterar nada para rodar o projeto.

---

## Suporte

Para problemas ou dúvidas, entre em contato pelo e-mail [thiagosaldanhacabral@gmail.com](mailto:thiagosaldanhacabral@gmail.com).

# C# DDD Boilerplate

Este projeto é um **boilerplate para APIs em .NET**, estruturado com base nos princípios do **Domain-Driven Design (DDD)**. Seu objetivo é acelerar o desenvolvimento de novos projetos, promover boas práticas e servir como referência técnica.

<p>
  <a href="https://www.docker.com/"><img src="https://img.shields.io/badge/Docker-Container-blue?logo=docker" alt="Docker"></a>
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet" alt=".NET"></a>
  <a href="https://www.microsoft.com/sql-server"><img src="https://img.shields.io/badge/Database-SQL_Server-4479A1?logo=microsoft-sql-server" alt="SQL Server"></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License: MIT"></a>
</p>

## 🎯 Por que usar este Boilerplate?

-   **Acelere o Desenvolvimento:** Comece novos projetos com uma estrutura robusta e pré-configurada, economizando horas de setup inicial.
-   **Foco nas Regras de Negócio:** Com a estrutura pronta, você pode se concentrar as regras de negócio da sua aplicação.
-   **Fácil de Manter:** A arquitetura facilita a manutenção e escalabilidade, sem aumentar consideravelmente a complexidade da solução.

## 📌 Funcionalidades Inclusas
- 🗑️ **Soft Delete**: Registros nunca são realmente excluídos do banco. Eles são apenas marcados com um timestamp `DeletedAt`.
- 🕒 **Timestamps Automáticos**: Todas as entidades herdam `CreatedAt` e `UpdatedAt`, que são gerenciados automaticamente.
- 📦 **Repositório Genérico**: Abstração de métodos CRUD com suporte simplificado a filtros, includes, projeções e paginação.
- 🔗 **Padrão Unit of Work**: Garante que as transações sejam executadas de forma conjunta, mantendo a consistência dos dados.
- 🛡️ **Validação de requisição:** Regras de validação para DTOs de entrada usando FluentValidation.
- ✉️ **Padronização de Respostas**: Respostas da API seguem um padrão, facilitando o consumo no frontend.
- 🔐 **Autenticação JWT**: Estrutura de autenticação e autorização via JWT pré-configurada.

## 📁 Estrutura do Projeto

Este projeto segue uma arquitetura em camadas inspirada no Domain-Driven Design (DDD), garantindo uma clara separação de responsabilidades.

```text
├── src/
│   ├── API/
│   │   ├── Controllers/
│   │   ├── Extensions/
│   │   └── SeedWork/
│   │       └── Filters/
│   │
│   ├── Application/
│   │   ├── Services/
│   │   └── SeedWork/
│   │       └── Responses/
│   │
│   ├── Infrastructure/
│   │   ├── Contexts/
│   │   ├── Extensions/
│   │   ├── Migrations/
│   │   ├── Options/
│   │   ├── Repositories/
│   │   └── SeedWork/
│   │       └── UnitOfWork/
│   │
│   ├── Core/
│   │   ├── SeedWork/
│   │   └── Enums/
│
├── tests/
│   ├── IntegrationTests/
│   └── UnitTests/
│
└── docker-compose.yml
```

## 🚀 Começando

Siga os passos abaixo para configurar e executar o projeto em seu ambiente.

### Pré-requisitos

-   [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
-   [Docker](https://www.docker.com/) (Para execução com contêineres)
-   Um SGBD como [SQL Server](https://www.microsoft.com/sql-server/sql-server-2022) (Para execução local)

### 1. Clone o Repositório

```bash
git clone https://github.com/LucasSimionatoIsTaken/csharp-ddd-boilerplate.git
cd csharp-ddd-boilerplate
```

### 2. Configuração (Apenas para execução local)

Se você não for usar o Docker, precisará configurar a string de conexão no arquivo `src/API/appsettings.json`. Altere a string `Default` para apontar para o seu banco de dados.

```json
"ConnectionStrings": {
  "Default": "Data Source=SEU_SERVIDOR;Initial Catalog=NOME_DO_BANCO;TrustServerCertificate=true;Integrated Security=true;"
},
```

### 3. Execute o Projeto

-   **Com Docker (Recomendado):**
    O Docker Compose irá criar e configurar os contêineres para a API e para o banco de dados.

    ```bash
    docker-compose up -d
    ```

-   **Localmente:**
    Execute o comando na raiz do projeto. As migrations serão aplicadas na inicialização.

    ```bash
    dotnet run --project src/API/API.csproj
    ```

## 📚 Como Usar

Após iniciar a aplicação, a API estará disponível localmente.

1.  **Acesse a Documentação da API (Swagger)**:
    Abra seu navegador e acesse `http://localhost:5000/swagger`. Lá você encontrará todos os endpoints documentados e poderá testá-los diretamente.

2.  **Exemplo de fluxo**:
    * Use o endpoint `POST /api/auth/register` para criar um novo usuário.
    * Use `POST /api/auth/login` para autenticar e obter um token JWT.
    * Use o token obtido no cabeçalho `Authorization: Bearer <token>` para acessar endpoints protegidos.

## 🔧 Customização do Banco de Dados

Caso deseje migrar para outro SGBD (ex: MySQL, PostgreSQL):

1.  Instale o provider do Entity Framework para o banco desejado e remova o do SQL Server. Exemplo para MySQL:
    ```bash
    dotnet add package Pomelo.EntityFrameworkCore.MySql --project Infrastructure
    dotnet remove package Microsoft.EntityFrameworkCore.SqlServer --project Infrastructure
    ```
2.  Em `src/API/Extensions/IServiceCollectionExtensions.cs`, na função `AddDbContext`, substitua `UseSqlServer` pelo método de extensão correspondente ao seu novo provider (ex: `UseMySql`).

## 📚 Próximos Passos
-   [x] 🧾 Melhorar a cobertura da documentação Swagger
-   [x] 🔽 Melhorar a paginação com ordenação
-   [x] 🧩 Configurar adapters para serviços externos
-   [x] 🧪 Adicionar testes de integração
-   [ ] 🔐 Adicionar features de segurança (refresh token, forgot password)
-   [ ] 🗄️ Adicionar seed de dados para o banco
-   [ ] 📧 Adicionar serviço de envio de e-mail
-   [ ] 📄 Adicionar serviço de upload de arquivos
-   [ ] 🧪 Adicionar testes unitários

## 📝 Feedback

Este é um projeto pessoal, mas feedbacks são bem-vindos. Se você encontrar um bug ou tiver uma sugestão, sinta-se à vontade para abrir uma [Issue](https://github.com/LucasSimionatoIsTaken/csharp-ddd-boilerplate/issues).

## 🧑‍💻 Autor

Desenvolvido por Lucas Simionato — [@LucasSimionatoIsTaken](https://github.com/LucasSimionatoIsTaken)  

Este projeto é open-source e você pode usá-lo livremente como base para seus próprios projetos.

# C# DDD Boilerplate


Este projeto é um **boilerplate para APIs em .NET**, estruturado com base nos princípios do **Domain-Driven Design (DDD)**. Seu objetivo é acelerar o desenvolvimento de novos projetos, promover boas práticas e servir como referência técnica.

<p>

[![Docker](https://img.shields.io/badge/Docker-Container-blue?logo=docker)](https://www.docker.com/)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/Database-SQL_Server-4479A1?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
</p>
<br>

## 🚀 Como Executar

### Pré-requisitos

#### Execução local:
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-2022)

#### Execução com Docker:
- [Docker](https://www.docker.com/)

<br>

### Passos

#### 🔹 Local: Use o comando
```bash
dotnet run
```

#### 🔹 Docker: Use o comando
```bash
docker-compose up -d
```
<br>

# 📁 Estrutura do Projeto

Este projeto segue uma arquitetura baseada em DDD (Domain-Driven Design), com separação clara de responsabilidades entre as camadas.

```text
src/
│
├── API/
│   ├── Controllers/
│   ├── Extensions/
│   └── SeedWork/
│       └── Filters/
│
├── Application/
│   ├── Services/
│   └── SeedWork/
│       └── Responses/
│
├── Infrastructure/
│   ├── Contexts/
│   ├── Extensions/
│   ├── Migrations/
│   ├── Options/
│   ├── Repositories/
│   └── SeedWork/
│       └── UnitOfWork/
│
├── Core/ 
│   ├── SeedWork/
│   └── Enums/
│
└──  docker-compose.yml
```
<br>


## 📌 Notas Importantes

- 🗑️ **Soft Delete**: registros excluídos não são removidos do banco, apenas marcados com `DeletedAt`.
- 🕒 **Timestamps automáticos**: entidades sempre terão `CreatedAt` e `UpdatedAt` atualizados automaticamente.
- 📦 **Services**: cada ação do controller tem um service dedicado, com:
    - DTOs para entrada e saída
    - Validações
    - Mapeamento entre entidade e resposta

<br>

## 📚 Próximos Passos

- [x] 🧾 Melhorar a cobertura da documentação Swagger
- [ ] 🔽️ Melhorar a paginação com ordenação
- [ ] 🧩 Configurar adapters
- [ ] 🔐 Adicionar features de segurança como atualização de token existente e esquecimento de senha
- [ ] 🗄️ Adicionar seed para o banco de dados
- [ ] 📧 Adicionar envio de email
- [ ] 📄 Adicionar upload de arquivos
- [ ] 🔍 Adicionar testes unitários e de integração

<br>

## 🧑‍💻 Autor

Desenvolvido por Lucas Simionato — [@LucasSimionatoIsTaken](https://github.com/LucasSimionatoIsTaken)  
Este projeto é open-source e você pode usá-lo livremente como base para seus próprios projetos.
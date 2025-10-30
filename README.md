# Ambev Developer Evaluation - OMNIA (API de Vendas)

Este projeto implementa uma API para o **teste técnico da Ambev/ABI (DeveloperStore Use Case)**, focado no gerenciamento de registros de vendas. A solução foi desenvolvida utilizando **.NET 8 (LTS)** e segue rigorosamente os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, visando alta testabilidade, manutenibilidade e separação de conceitos.

---

## 🎯 Caso de Uso

O objetivo é implementar uma API **CRUD completa** para gerenciar registros de vendas (`Sales`), incluindo:
* Registro e consulta de vendas (data, cliente, vendedor, filial).
* Gerenciamento de itens de venda (produtos, quantidades, preços).
* Cálculo de descontos com base em regras de negócio (ex: 10% para 4+ itens, 20% para 10-20 itens).
* Geração de eventos de domínio (ex: `SaleCreated`).
* Gerenciamento de `Users` e `Products`.

---

## 🏗️ Arquitetura: Clean Architecture

A solução adota a **Clean Architecture** para garantir uma clara separação entre a lógica de negócio e as dependências externas (UI, banco de dados, serviços). Isso promove:

* **Independência:** O Core (Domain + Application) não depende de frameworks ou tecnologias externas.
* [cite_start]**Testabilidade:** As regras de negócio podem ser testadas isoladamente[cite: 16].
* **Manutenibilidade:** As camadas podem ser modificadas ou substituídas com menor impacto.

---

### 🧩 Estrutura do Projeto

/backend
├── src/
│   ├── Adapters/
│   │   ├── Driven/
│   │   │   └── Infrastructure/ (Ambev.DeveloperEvaluation.ORM)
│   │   │       → Implementação da persistência (EF Core, Repositories)
│   │   └── Drivers/
│   │       └── WebApi/ (Ambev.DeveloperEvaluation.WebApi)
│   │           → Ponto de entrada (API Controllers, Middlewares, Swagger)
│   │
│   ├── Core/
│   │   ├── Application/ (Ambev.DeveloperEvaluation.Application)
│   │   │   → Orquestração dos casos de uso (CQRS Handlers, DTOs, Validation)
│   │   └── Domain/ (Ambev.DeveloperEvaluation.Domain)
│   │       → Coração do negócio (Entities, Value Objects, Repository Interfaces, Domain Logic)
│   │
│   ├── Crosscutting/
│   │   ├── Common/ (Ambev.DeveloperEvaluation.Common)
│   │   │   → Utilitários compartilhados (Logging, Security, HealthChecks)
│   │   └── IoC/ (Ambev.DeveloperEvaluation.IoC)
│   │       → Configuração central de Injeção de Dependência
│
├── tests/
│   ├── Unit/
│   │   → Testes de lógica de negócio (isolados)
│   ├── Integration/
│   │   → Testes entre camadas (com banco em memória)
│   └── Functional/
│       → Testes End-to-End (simulando chamadas HTTP)
│
└── Ambev.DeveloperEvaluation.sln

---

## 🛠️ Tecnologias Utilizadas (Tech Stack)

* **Backend:** .NET 8 (LTS), C#
* **Framework API:** ASP.NET Core
* **ORM:** Entity Framework Core 8
* **Banco de Dados:** PostgreSQL (Pronto para Docker)
* **Padrões:** Clean Architecture, DDD, CQRS (com MediatR), Repository Pattern
* **Validação:** FluentValidation
* **Mapeamento:** AutoMapper
* **Autenticação:** JWT Bearer Token
* **Testes:** xUnit, NSubstitute, FluentAssertions, Bogus
* **Containerização:** Docker, Docker Compose [cite: 17]
* **Documentação API:** Swagger (OpenAPI)

---

## ⚙️ Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker](https://www.docker.com/products/docker-desktop/) (Recomendado para fácil execução)
* Um cliente de API (como Postman, Insomnia ou `curl`) para testar os endpoints.

---

## 🚀 Como Executar o Projeto (Passo a Passo)

Instruções claras são essenciais para a avaliação[cite: 14].

---

### 🐳 Via Docker (Recomendado)

Esta é a forma mais simples e garante um ambiente consistente.

1.  Clone o repositório.
2.  Navegue até a pasta `template/backend`.
3.  Execute o comando:
    ```bash
    docker-compose up -d --build
    ```
4.  Aguarde os containers subirem. A API estará disponível em:
    * **Swagger UI (Documentação Interativa):** `https://localhost:7181/swagger`

---

### 💻 Localmente

1.  Clone o repositório.
2.  Certifique-se de que um servidor PostgreSQL esteja rodando e acessível.
3.  **Configure a Connection String:** Edite o arquivo `template/backend/src/Ambev.DeveloperEvaluation.WebApi/appsettings.Development.json` e ajuste a `DefaultConnection` com os dados do seu banco PostgreSQL (usuário `postgres`, senha `postgres`, etc.).
4.  Navegue até a pasta `template/backend`.
5.  **Restaure as dependências:**
    ```bash
    dotnet restore Ambev.DeveloperEvaluation.sln
    ```
6.  **Aplique as Migrations** (Cria o schema do banco e insere dados iniciais):
    ```bash
    dotnet ef database update --project src\Ambev.DeveloperEvaluation.ORM --startup-project src\Ambev.DeveloperEvaluation.WebApi
    ```
7.  **Execute a Aplicação:**
    ```bash
    dotnet run --project src/Ambev.DeveloperEvaluation.WebApi --launch-profile https
    ```
8.  A API estará disponível em:
    * **Swagger UI (Documentação Interativa):** `https://localhost:7181/swagger`

---

## 🧪 Testes

A qualidade é garantida por uma suíte de testes automatizados[cite: 16].

Para executar todos os testes (Unitários e Integração), rode:
```bash
dotnet test Ambev.DeveloperEvaluation.sln

📚 Comandos e Exemplos de API
Para um guia detalhado com todos os comandos de dotnet ef, docker, curl e exemplos de payloads JSON para cada endpoint, consulte o GUIA DE COMANDOS. 

💡 Versão do .NET
O projeto utiliza .NET 8 (LTS). A escolha pela versão de Suporte de Longo Prazo (LTS) em vez de versões mais recentes foi intencional, priorizando a estabilidade, confiabilidade e compatibilidade com o ecossistema corporativo. Manter a versão LTS reflete a busca por maturidade técnica e segurança, alinhada às boas práticas de engenharia de software em ambientes de produção.

URL da API: https://localhost:7181

Swagger: https://localhost:7181/swagger

Executar com Docker
Navegar até a pasta `template/backend` e executar:

🧪 Testes
Executar Todos os Testes
Executar Testes de Integração
Executar Testes Unitários

🔧 Build e Manutenção
Build da Solução
Restaurar Pacotes
Limpar Solução

🗄️ Banco de Dados (Entity Framework Core)
Aplicar Migrações
Cria ou atualiza o banco de dados com o schema mais recente.

Criar Nova Migração
Gera um novo script de migração com base nas mudanças do DbContext ou Entities.

🌐 Endpoints da API (Exemplos)
Base URL: https://localhost:7181/api

👥 Users
POST /api/Users (Criar usuário)
GET /api/Users (Listar usuários)

📦 Products
GET /api/Products (Listar todos os produtos)
Exemplo de Resposta:

🛒 Sales
POST /api/Sales (Criar nova venda)
Exemplo de Request:

Exemplo de Response (Sucesso):

GET /api/Sales (Listar todas as vendas)
Exemplo de Response:

Outros Endpoints de Vendas
(Assumindo que sua branch se chama feature/sales-crud. Se for outro nome, apenas troque-o no comando push).
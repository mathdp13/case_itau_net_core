# Case Fundos — Itaú

API RESTful para gerenciamento de fundos de investimento com frontend Angular, desenvolvida como case técnico com foco em arquitetura limpa, observabilidade e boas práticas de desenvolvimento .NET.

---

## 🏗 Arquitetura

O projeto segue os princípios de **Clean Architecture**, organizado em camadas com dependências unidirecionais:

```
CaseItau.API          → Camada de apresentação (Controllers, Middleware, Swagger)
CaseItau.Application  → Camada de aplicação (Services, DTOs, Validators, AutoMapper)
CaseItau.Domain       → Camada de domínio (Entidades, Interfaces, Exceções)
CaseItau.Infrastructure → Camada de infraestrutura (EF Core, Repositórios, Migrations)
CaseItau.UnitTests    → Testes unitários
CaseItau.Front        → Frontend Angular (standalone components)
```

**Fluxo de dependências:**

```
API → Application → Domain
Infrastructure → Domain / Application
Tests → qualquer camada sob teste
Front → API (HTTP/REST)
```

---

## 🛠 Stack Tecnológica

| Categoria              | Tecnologia                                      |
|------------------------|-------------------------------------------------|
| Runtime                | .NET 8                                          |
| Banco de dados         | SQLite                                          |
| ORM                    | Entity Framework Core 8                         |
| Mapeamento             | AutoMapper 16                                   |
| Validação              | FluentValidation 12                             |
| Autenticação           | JWT Bearer                                      |
| Observabilidade        | OpenTelemetry (OTLP → Seq)                      |
| Documentação           | Swagger / Swashbuckle                           |
| Testes unitários       | xUnit + Moq                                     |
| Frontend               | Angular 21 (standalone components)              |
| Containerização        | Docker + Docker Compose                         |
| CI                     | GitHub Actions                                  |

---

## 📦 Estrutura do Projeto

### CaseItau.Domain
Coração da aplicação — sem dependências externas.

- `Entities/Fundo` — entidade principal mapeada para a tabela `FUNDO`
- `Entities/TipoFundo` — entidade de tipo de fundo mapeada para `TIPO_FUNDO`
- `Interfaces/` — contratos de repositório (`IFundoRepository`, `ITipoFundoRepository`, `IUnitOfWork`)

### CaseItau.Application
Lógica de aplicação e orquestração dos casos de uso.

- `Services/FundoService` — CRUD completo + movimentação de patrimônio, com validações de duplicidade de `Codigo` e `CNPJ`
- `Services/TipoFundoService` — leitura de tipos de fundo
- `DTOs/` — `CreateFundoDto`, `UpdateFundoDto`, `FundoDto` (resposta)
- `Validators/` — `CreateFundoDtoValidator` e `UpdateFundoDtoValidator` com FluentValidation
- `Mappings/FundoProfile` — perfil AutoMapper entre entidades e DTOs
- `Exceptions/NotFoundException` — exceção lançada quando um recurso não é encontrado
- `ApplicationServiceExtensions` — registro dos serviços da camada

### CaseItau.Infrastructure
Integração com recursos externos (SQLite).

- `Data/AppDbContext` — contexto EF Core com mapeamento das entidades
- `Repositories/FundoRepository` — operações de `Fundo`
- `Repositories/TipoFundoRepository` — operações de `TipoFundo`
- `Repositories/UnitOfWork` — abstração de `SaveChangesAsync` para controle transacional
- `Migrations/` — migration inicial com seed dos tipos de fundo
- `InfrastructureServiceExtensions` — registro do DbContext, repositórios e migration automática

### CaseItau.API
Camada de apresentação.

- `Controllers/AuthController` — emissão de token JWT
- `Controllers/FundoController` — 6 endpoints REST protegidos por JWT
- `Controllers/TipoFundoController` — consulta de tipos de fundo
- `Middleware/ExceptionHandlingMiddleware` — intercepta `NotFoundException` (→ 404) e exceções genéricas (→ 500) retornando `ProblemDetails`
- `Program.cs` — configura OpenTelemetry, Swagger, DI de todas as camadas e migration automática

### CaseItau.Front
Frontend Angular 21 com componentes standalone.

- `core/services/` — `AuthService`, `FundoService`
- `core/guards/` — `authGuard` (proteção de rotas via JWT)
- `core/interceptors/` — `authInterceptor` (Bearer token), `loggingInterceptor` (spans OpenTelemetry)
- `core/telemetry/` — inicialização do OpenTelemetry Web (OTLP → Collector → Seq)
- `features/login/` — tela de autenticação
- `features/fundos/` — listagem e formulário de fundos

---

## 🚀 Como Executar

### Pré-requisitos

- [Docker](https://www.docker.com/) com Docker Compose

### Subindo o ambiente completo

```bash
docker compose up --build
```

Esse comando sobe quatro containers:

| Container                  | Porta externa             | Descrição                              |
|----------------------------|---------------------------|----------------------------------------|
| caseItau_seq               | 5341 (UI) / 5342 (ingest) | Visualizador de logs e traces          |
| caseItau_otel_collector    | —                         | Converte traces JSON → Protobuf p/ Seq |
| caseItau_api               | 5000                      | API REST                               |
| caseItau_front             | 4200                      | Frontend Angular                       |

A migration é aplicada automaticamente na inicialização da API.

### Acessando os serviços

| Serviço         | URL                          |
|-----------------|------------------------------|
| Frontend        | http://localhost:4200        |
| Swagger UI      | http://localhost:5000/swagger |
| Seq (logs)      | http://localhost:5341        |

> Credenciais padrão do Seq: usuário `admin`, senha definida em `.env` (padrão: `admin123`)

### Credenciais de acesso (frontend / API)

As credenciais são definidas no arquivo `.env` (variáveis `AUTH_USERNAME` e `AUTH_PASSWORD`) e injetadas no container via Docker Compose. Utilize essas credenciais para autenticar via `/api/auth/login` ou pela tela de login do frontend.

---

## 🔌 Endpoints

### Autenticação

| Método | Rota             | Descrição                   | Status de Retorno |
|--------|------------------|-----------------------------|-------------------|
| POST   | /api/auth/login  | Autentica e retorna JWT     | 200, 401          |

### Tipos de Fundo *(requer JWT)*

| Método | Rota                      | Descrição                       | Status de Retorno |
|--------|---------------------------|---------------------------------|-------------------|
| GET    | /api/tipos-fundo          | Lista todos os tipos de fundo   | 200, 500          |
| GET    | /api/tipos-fundo/{codigo} | Busca tipo de fundo pelo código | 200, 404, 500     |

### Fundos *(requer JWT)*

| Método | Rota                              | Descrição                        | Status de Retorno       |
|--------|-----------------------------------|----------------------------------|-------------------------|
| GET    | /api/fundos                       | Lista todos os fundos            | 200, 500                |
| GET    | /api/fundos/{codigo}              | Busca fundo pelo código          | 200, 404, 500           |
| POST   | /api/fundos                       | Cria um novo fundo               | 201, 400, 422, 500      |
| PUT    | /api/fundos/{codigo}              | Atualiza nome, CNPJ e/ou tipo   | 204, 400, 404, 422, 500 |
| DELETE | /api/fundos/{codigo}              | Remove um fundo                  | 204, 404, 500           |
| PUT    | /api/fundos/{codigo}/patrimonio   | Movimenta o patrimônio líquido  | 204, 404, 422, 500      |

---

## ✅ Testes

### Tipos de testes

| Tipo                        | Localização             | Ferramentas |
|-----------------------------|-------------------------|-------------|
| Unitários — Services        | `CaseItau.UnitTests/`   | xUnit, Moq  |
| Unitários — Validators      | `CaseItau.UnitTests/`   | xUnit       |

### Executando os testes

```bash
dotnet test
```

Os testes unitários cobrem:

- `FundoService` — GetAll, GetByCodigo, Create, Update, Delete e MovimentarPatrimonio, incluindo cenários de erro e `NotFoundException`
- `CreateFundoDtoValidator` — validação de CNPJ (dígitos verificadores), tamanho dos campos e campos obrigatórios
- `UpdateFundoDtoValidator` — validações específicas do fluxo de atualização

---

## ⚙️ Configuração

As configurações são lidas de `appsettings.json` e sobrepostas por variáveis de ambiente (padrão .NET). Ao rodar via Docker Compose, as variáveis já estão definidas no `docker-compose.yml`.

| Variável de Ambiente                   | Descrição                                        |
|----------------------------------------|--------------------------------------------------|
| `ConnectionStrings__DefaultConnection` | Connection string do SQLite                      |
| `Seq__Endpoint`                        | Endpoint do Seq para envio de logs               |
| `JwtSettings__Secret`                  | Chave secreta para assinatura do JWT             |
| `Auth__Username`                       | Usuário para autenticação na API                 |
| `Auth__Password`                       | Senha para autenticação na API                   |
| `SEQ_ADMIN_PASSWORD`                   | Senha do admin do Seq                            |

Todas as variáveis sensíveis são definidas no arquivo `.env` (ver `.env.example`).

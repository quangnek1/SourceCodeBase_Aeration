# MyApp — Aeration Sterilize Control System

Hệ thống quản lý và điều phối quy trình hấp tiệt trùng (Aeration Sterilize), bao gồm quản lý cột hấp, vị trí hấp, lô sản xuất, kế hoạch sản xuất và theo dõi lịch sử vận hành.

## Table of Contents

- [Elasticsearch](#elasticsearch)
- [Mục tiêu dự án](#mục-tiêu-dự-án)
- [Tech Stack](#tech-stack)
- [Yêu cầu môi trường](#yêu-cầu-môi-trường)
- [Cấu trúc Source Code](#cấu-trúc-source-code)
- [Chạy Local](#chạy-local)
- [Chạy Docker](#chạy-docker)
- [EF Core Migrations](#ef-core-migrations)
- [API Endpoints](#api-endpoints-v1)
- [Chạy Tests](#chạy-tests)
- [Documentation](#documentation)

---

## Elasticsearch

Local Elasticsearch and Kibana can be started with:

```powershell
docker compose -f deploy/compose/elasticsearch.yml up -d
```

Enable product search in `src/Services/AerationSterilize/AerationSterilize.API/appsettings.json` by setting `ElasticsearchOptions:Enabled` to `true` after Elasticsearch is running. When disabled, product search falls back to the existing SQL query.

---

## Mục tiêu dự án

- Số hóa quy trình điều phối hấp tiệt trùng tại xưởng sản xuất thiết bị y tế.
- Quản lý trạng thái thời gian thực của từng cột hấp và vị trí hấp.
- Tích hợp đọc kế hoạch sản xuất từ file Excel (CAG / PTCA / AMI-Q411).
- Sinh QR Code và PDF cho từng lô sản xuất.
- Cung cấp REST API versioned (v1/v2) cho frontend và hệ thống tích hợp.
- Hỗ trợ xác thực JWT, phân quyền theo Role/Permission.

---

## Tech Stack

### Backend

| Thành phần | Thư viện / Framework |
|---|---|
| Runtime | .NET 8 (ASP.NET Core 8) |
| Architecture | Clean Architecture + CQRS + DDD |
| Mediator | MediatR 12 |
| ORM | Entity Framework Core 8 (SQL Server) |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Identity | ASP.NET Core Identity |
| Validation | FluentValidation 12 |
| Mapping | AutoMapper 16 |
| Caching | Redis (`StackExchange.Redis`) |
| Background Jobs | Hangfire 1.8 (SQL Server storage) |
| Logging | Serilog (Console + Debug sinks) |
| Excel | EPPlus 8 |
| PDF | RazorLight 2 + PuppeteerSharp 25 |
| QR Code | QRCoder 1.8 |
| Messaging | MassTransit 8 + RabbitMQ |
| API Versioning | Asp.Versioning 7 |
| OpenAPI | Swashbuckle 6 + FluentValidation rules |
| Smart Enum | Ardalis.SmartEnum 8 |
| Testing | xUnit 2.5 + Architecture Tests |
| Code Analysis | SonarAnalyzer.CSharp + StyleCop.Analyzers |

### Frontend

| Thành phần | Thư viện / Framework |
|---|---|
| Framework | React 19 + TypeScript |
| Build Tool | Vite 4 |
| Routing | TanStack Router v1 |
| Data Fetching | TanStack Query v5 |
| State Management | Zustand v5 |
| UI Components | shadcn/ui + Radix UI |
| Styling | TailwindCSS v4 |
| Form | React Hook Form v7 + Zod v4 |
| HTTP Client | Axios v1 |
| Package Manager | pnpm |

---

## Yêu cầu môi trường

### Backend

| Tool | Phiên bản tối thiểu |
|---|---|
| .NET SDK | 8.0 |
| SQL Server | 2019+ (hoặc Azure SQL) |
| Redis | 6.0+ |
| RabbitMQ | 3.12+ *(tùy chọn, nếu dùng Messaging)* |
| dotnet-ef (CLI) | 8.0+ |

Cài `dotnet-ef` nếu chưa có:
```bash
dotnet tool install --global dotnet-ef
```

### Frontend

| Tool | Phiên bản tối thiểu |
|---|---|
| Node.js | 20 LTS+ |
| pnpm | 9+ |

---

## Cấu trúc Source Code

```
MyApp/
├── Directory.Build.props              # Shared MSBuild properties
├── Directory.Packages.props           # Central package version management
├── global.json                        # .NET SDK version lock (8.0)
├── MyApp.sln
│
├── deploy/
│   ├── compose/                       # Docker Compose files
│   └── env/                           # Environment variable files (.env)
│
├── src/
│   ├── BuildingBlocks/                # Shared infrastructure packages
│   │   ├── Common.Logging/            # Serilog configuration
│   │   ├── Contracts/                 # Abstractions: CQRS messages, repositories,
│   │   │   ├── Common/Messages/       #   ICommand, IQuery, ICommandHandler, IQueryHandler
│   │   │   ├── Common/Repositories/   #   IRepositoryBase, IUnitOfWork
│   │   │   ├── Domains/               #   EntityBase<T>, EntityAuditBase<T>
│   │   │   ├── Exceptions/            #   BadRequest, NotFound, Validation, Domain
│   │   │   ├── Identity/              #   ITokenService
│   │   │   ├── Mapping/               #   IMapFrom, MappingProfile
│   │   │   ├── Responses/             #   Result<T>, PagedResult<T>
│   │   │   └── Services/              #   ICache, IExcel, IPdf, IQrCode
│   │   ├── Infrastructure/            # Implementations: RepositoryBase, UnitOfWork,
│   │   │   ├── Common/                #   ExceptionHandlingMiddleware,
│   │   │   ├── Middlewares/           #   CacheService, EpplusExcelReaderService,
│   │   │   └── Services/              #   PdfService, QrCodeServices
│   │   └── Shared/                    # DTOs, Options, Paging, Enumerations
│   │       ├── Options/               #   DatabaseOptions, JwtOptions, RedisOptions...
│   │       ├── Paging/                #   PagedResult, PagingExtensions
│   │       └── Emumerations/          #   SortOrder
│   │
│   ├── Services/
│   │   └── AerationSterilize/
│   │       ├── AerationSterilize.Domain/
│   │       │   ├── Entities/          # AerationColumn, AerationPosition, Batch,
│   │       │   │                      # BatchInAerationPosition, BatchItem,
│   │       │   │                      # DataAmiQ411, DataPlan, Product, LogHistory
│   │       │   ├── Enums/             # AerationStatus (SmartEnum)
│   │       │   └── Exceptions/
│   │       │
│   │       ├── AerationSterilize.Application/
│   │       │   ├── Behaviors/         # ValidationPipelineBehavior,
│   │       │   │                      # TransactionPipelineBehavior,
│   │       │   │                      # PerformanceBehaviour
│   │       │   └── Features/V1/
│   │       │       ├── AerationColumn/   # CRUD: Commands + Queries + Dtos + Mapping
│   │       │       ├── AerationPosition/ # CRUD: Commands + Queries + Dtos + Mapping
│   │       │       ├── Auth/
│   │       │       ├── Batch/
│   │       │       ├── DataAmi/
│   │       │       ├── DataPlan/
│   │       │       └── Products/
│   │       │
│   │       ├── AerationSterilize.Infrastructure/
│   │       │   └── DependencyInjection/ # Đăng ký Redis, Excel, PDF, QrCode services
│   │       │
│   │       ├── AerationSterilize.Persistence/
│   │       │   ├── ApplicationDbContext.cs
│   │       │   ├── Configurations/    # EF Core entity configurations
│   │       │   ├── Migrations/
│   │       │   ├── Repositories/      # ApplicationRepository<T, K>
│   │       │   └── DependencyInjection/
│   │       │
│   │       └── AerationSterilize.API/
│   │           ├── Controllers/V1/    # AerationColumnController, AerationPositionController,
│   │           │                      # AuthController, BatchController,
│   │           │                      # DataPlanController, ProductsController
│   │           ├── DependencyInjection/ # Swagger, CORS, API Versioning setup
│   │           └── Program.cs
│   │
│   └── WebApp/
│       └── AerationWebApp/            # React 19 + Vite + TanStack + shadcn/ui
│           ├── src/
│           │   ├── components/
│           │   ├── features/
│           │   ├── pages/
│           │   ├── routes/
│           │   ├── services/          # Axios API clients
│           │   ├── stores/            # Zustand stores
│           │   └── types/
│           └── package.json
│
└── tests/
    └── AerationSterilize.Architecture.Tests/  # Architecture rule tests (xUnit)
```

---

## Chạy Local

### 1. Clone & chuẩn bị cấu hình

```bash
git clone <repo-url>
cd MyApp
```

Cập nhật connection string trong `src/Services/AerationSterilize/AerationSterilize.API/appsettings.json`:

```json
{
  "DatabaseOptions": {
    "ConnectionString": "Server=<host>;Database=AerationSterilizeControl;User Id=<user>;Password=<pass>;TrustServerCertificate=True;"
  },
  "RedisOptions": {
    "DBProvider": "redis",
    "ConnectionString": "<redis-host>:6379"
  },
  "JwtOptions": {
    "SecretKey": "<min-32-chars-secret>",
    "Issuer": "AerationSterilize",
    "Audience": "AerationSterilize",
    "ExpiryMinutes": 60
  }
}
```

### 2. Apply Migration & chạy API

```bash
cd src/Services/AerationSterilize

# Tạo migration mới (nếu cần)
dotnet ef migrations add "Add_BoxingSession"  --project AerationSterilize.Persistence  --startup-project AerationSterilize.API  --output-dir Migrations

# Áp dụng migration
dotnet ef database update --project AerationSterilize.Persistence  --startup-project AerationSterilize.API

# Remove migration
dotnet ef migrations remove --project AerationSterilize.Persistence --startup-project AerationSterilize.API

# Chạy API
dotnet run --project AerationSterilize.API
```

API mặc định tại: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

### 3. Chạy Frontend

```bash
cd src/WebApp/AerationWebApp

pnpm install
pnpm dev
```

Frontend mặc định tại: `http://localhost:5173`

---

## Chạy Docker

> Docker Compose files đặt tại `deploy/compose/`. Tạo file `.env` tương ứng tại `deploy/env/`.

Ví dụ cấu trúc file `deploy/env/api.env`:

```env
DATABASE_CONNECTION_STRING=Server=sqlserver;Database=AerationSterilizeControl;User Id=sa;Password=YourPassword!;TrustServerCertificate=True;
REDIS_CONNECTION_STRING=redis:6379
JWT_SECRET_KEY=AeartionSterilize_SuperSecretKey_PhaiDaiHon_32KyTu_Nhe_123456!
```

Ví dụ `deploy/compose/docker-compose.yml`:

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourPassword!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"

  api:
    build:
      context: ../..
      dockerfile: src/Services/AerationSterilize/AerationSterilize.API/Dockerfile
    env_file:
      - ../env/api.env
    ports:
      - "5000:8080"
    depends_on:
      - sqlserver
      - redis

  webapp:
    build:
      context: ../../src/WebApp/AerationWebApp
    ports:
      - "3000:80"
    depends_on:
      - api
```

Chạy toàn bộ stack:

```bash
cd deploy/compose
docker compose up -d
```

---

## EF Core Migrations

Tất cả lệnh chạy từ thư mục `src/Services/AerationSterilize/`.

```bash
# Tạo migration
dotnet ef migrations add "<MigrationName>" \
  --project AerationSterilize.Persistence \
  --startup-project AerationSterilize.API \
  --output-dir Migrations

# Áp dụng migration
dotnet ef database update \
  --project AerationSterilize.Persistence \
  --startup-project AerationSterilize.API

# Xóa migration cuối (chưa apply)
dotnet ef migrations remove \
  --project AerationSterilize.Persistence \
  --startup-project AerationSterilize.API
```

---

## API Endpoints (v1)

| Resource | GET (list) | GET (by id) | POST | PUT | DELETE |
|---|---|---|---|---|---|
| AerationColumn | `GET /api/v1/aerationcolumn` | `GET /api/v1/aerationcolumn/{id}` | `POST /api/v1/aerationcolumn` | `PUT /api/v1/aerationcolumn/{id}` | `DELETE /api/v1/aerationcolumn/{id}` |
| AerationPosition | `GET /api/v1/aerationposition` | `GET /api/v1/aerationposition/{id}` | `POST /api/v1/aerationposition` | `PUT /api/v1/aerationposition/{id}` | `DELETE /api/v1/aerationposition/{id}` |
| Batch | `GET /api/v1/batch` | `GET /api/v1/batch/{id}` | `POST /api/v1/batch` | `PUT /api/v1/batch/{id}` | `DELETE /api/v1/batch/{id}` |
| Products | `GET /api/v1/products` | `GET /api/v1/products/{id}` | `POST /api/v1/products` | `PUT /api/v1/products/{id}` | `DELETE /api/v1/products/{id}` |
| Auth | — | — | `POST /api/v1/auth/login` | — | — |

Tất cả list endpoints hỗ trợ query params: `searchTerm`, `sortColumn`, `sortOrder` (asc/desc), `pageIndex`, `pageSize`.

---

## Chạy Tests

```bash
dotnet test tests/AerationSterilize.Architecture.Tests/AerationSterilize.Architecture.Tests.csproj
```

---

## Documentation

- **[Boxing Position Workflow](docs/boxing-flow.md)** — Detailed flow for boxing position scanning and job assignment
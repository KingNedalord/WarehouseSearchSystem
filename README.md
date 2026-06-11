# WareHouse Search System V2.0

A console-based clothing and footwear inventory management system built with .NET 10, Entity Framework Core, PostgreSQL. Supports two modes — **User** (read-only search) and **Admin** (full CRUD) — with a clean layered architecture and full async I/O throughout.

---

## Tech Stack

| Layer | Technology                               |
|---|------------------------------------------|
| Runtime | .NET 10                                  |
| Database | PostgreSql via Entity Framework Core     |
| DI Container | Microsoft.Extensions.DependencyInjection |
| Testing | NUnit + Moq + EF Core InMemory           |

---

## Project Structure

```
WareHouseSearchSystem/
├── Models/             # Domain entities: Item, Clothing, Footwear, enums
├── DAL/                # EF Core DbContext, PostgreSqlSource, ItemDao, interfaces
├── Services/           # UserService, AdminService, business logic
├── Controller/         # Commands (FindCommand, AdminCommands, HelpCommand), parsers
├── Presentation/       # ConsoleView, ColorConsole, AnimatedLoading
├── Main/               # Program.cs
└── Tests/              # Unit tests per layer
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) installed and running

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://autocode.git.epam.com/azizbek_tolipov/warehouse-search-system-v2.0.git
cd WareHouseSearchSystem
```

### 2. Configure the connection string

Edit (or create) `Main/appsettings.json`:

```json
{
    "sqlConnectionString": "Host=localhost;Database=warehouse;Username=postgres;Password=yourpassword"
}
```
### 3. Create the database and apply migrations

First, create the database in PostgreSQL:
```bash
psql -U postgres -c "CREATE DATABASE warehouse;"
```

Then apply migrations:
```bash
cd .\DAL\
dotnet ef migrations add Initial
dotnet ef database update
```

### 4. Build and run

```bash
cd Main
dotnet run
```

---

## Available Commands

### User mode

| Command | Description |
|---|---|
| `find clothing` | List all clothing |
| `find footwear` | List all footwear |
| `find all` | List all items |
| `find all price=min;max` | Filter by price range |
| `find clothing size=M` | Filter clothing by size |
| `find clothing gender=Male` | Filter clothing by gender |
| `switch admin` | Switch to admin mode |
| `help` | Show available commands |
| `exit` | Exit the application |

### Admin mode

| Command | Description |
|---|---|
| `add clothing` | Add new clothing (interactive form) |
| `add footwear` | Add new footwear (interactive form) |
| `update clothing 1` | Update clothing by ID |
| `update footwear 1` | Update footwear by ID |
| `delete clothing 1` | Delete clothing by ID |
| `delete footwear 1` | Delete footwear by ID |
| `switch user` | Switch back to user mode |
| `help` | Show admin commands |
| `exit` | Exit the application |

---

## Running Tests

```bash
cd Tests
dotnet test
```

### Test coverage by layer

| Test Class | Layer | Approach |
|---|---|---|
| `SqliteSourceTests` | DAL | EF Core InMemory database |
| `ItemDaoTests` | DAO | Moq `ISourceReader` / `ISourceWriter` |
| `UserServiceTests` | Service | Moq `IItemReader` |
| `AdminServiceTests` | Service | Moq `IItemWriter` |
| `FindCommandTests` | Controller | Moq `IUserService` |
| `AdminCommandTests` | Controller | Moq `IAdminService` + `IUserService` |
| `ConsoleViewTests` | View | Moq `IController` + `Console.SetIn` |

---

## Architecture Notes

- **Async throughout** — all I/O from `SqliteSource` up to `ConsoleView` uses `async/await`; no `.Result` or `.Wait()` anywhere
- **SOLID design** — commands implement `ICommand`, composition happens only in `DAL/Configuration.cs`
- **No config in tests** — `ItemContext.OnConfiguring` checks `options.IsConfigured` before reading `appsettings.json`, so tests using in-memory options never touch the filesystem

---

## Helpful Notes

- Always run `dotnet ef database update` after pulling if new migrations are present
- `appsettings.json` is excluded from source control — copy `appsettings.example.json` and fill in your PostgreSQL credentials

```{
  "admin": {
    "password": "password"
  },
  "sqlConnectionString": "Host=localhost;Database=warehouse;Username=postgres;Password=postgresPassword"
}
```
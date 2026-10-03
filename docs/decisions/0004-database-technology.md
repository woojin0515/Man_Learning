# ADR 0004: Database Technology — Azure SQL Database via EF Core

## Status

Accepted.

## Context

Production deployment is fixed to Azure App Service (see `.github/copilot-instructions.md` §3).
Every persistence implementation so far (`ManLearning.Infrastructure.Persistence.InMemory*`) is
an explicit, documented temporary stand-in, because the database technology was deliberately left
as an open decision pending a dedicated spike (per `.github/copilot-instructions.md` §3: "Do not
assume a database provider without documenting the decision").

Spike 0001 (`docs/spikes/0001-database-technology.md`) compared Azure SQL Database, Azure
Database for PostgreSQL Flexible Server, Azure Cosmos DB, and a SQLite file on App Service against
this application's relational domain model and its Azure App Service deployment target.

## Decision

Use **Azure SQL Database (Serverless compute tier)** as the production database, accessed through
**EF Core** (`Microsoft.EntityFrameworkCore.SqlServer`) from `ManLearning.Infrastructure`.

- `ManLearning.Application`'s repository interfaces (`ICourseRepository`,
  `ILessonProgressRepository`, `IXpAwardRepository`, `IStreakRepository`) remain the contract;
  only the Infrastructure-side implementation changes.
- A single `ManLearningDbContext` in `ManLearning.Infrastructure.Persistence` will own entity
  configuration (mapping Domain types to tables), replacing the in-memory dictionaries/bags.
- Local development and the existing test suite may use an EF Core provider suited to fast,
  isolated runs (e.g. SQLite in-memory or a local container), while production uses Azure SQL
  Database — the repository contract is what both must satisfy, not a specific provider.
- Connection strings are never committed to source control: local development uses .NET user
  secrets; production is configured through Azure App Service Configuration (Connection strings),
  per `.github/copilot-instructions.md` §7.

## Consequences

- Domain and Application layers are unaffected; this decision is fully contained in
  Infrastructure, which is the layer responsible for external-system implementations.
- The in-memory repositories will be removed once their EF Core replacements pass the existing
  Application-layer test suite, rather than being kept as a second, divergent implementation.
- EF Core migrations become part of the deployment process. The initial migration strategy
  (apply-on-startup versus a separate release step) is deferred to implementation time and should
  be documented when it is decided, rather than assumed here.
- If a future requirement emerges that this domain cannot express relationally (none is known
  today), this ADR should be superseded by a new one rather than silently reworked in code.

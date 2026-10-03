# Spike 0001: Database Technology for Azure App Service Deployment

## Trigger

Production deployment is fixed to Azure App Service (Azure Portal). Every repository in
`ManLearning.Infrastructure.Persistence` today is an in-memory singleton
(`InMemoryCourseRepository`, `InMemoryLessonProgressRepository`, `InMemoryXpAwardRepository`,
`InMemoryStreakRepository`). This is explicitly documented as a temporary stand-in in each
type's XML doc comment ("temporary stand-in until the database technology spike is completed").

In-memory storage is incompatible with a real Azure App Service deployment for two concrete
reasons:

1. **Process restarts lose all data.** App Service recycles instances routinely (deploys,
   scaling, patching). Every learner's XP, progress, and streak would reset unpredictably.
2. **Horizontal scaling breaks consistency.** If the App Service plan scales to more than one
   instance, each instance holds its own in-memory dictionary, so a learner could see different
   progress depending on which instance handled the request.

This spike exists to close that gap before further feature work increases the amount of code
that will need to be migrated off in-memory storage.

## Candidates considered

| Option | Fit for this app | Azure App Service integration | Cost at this stage | EF Core support |
| --- | --- | --- | --- | --- |
| Azure SQL Database (Serverless tier) | Strong — the domain is relational (Course → Lesson → Quiz → Question → AnswerChoice, plus per-learner progress/XP/streak rows) | First-class; connection string via App Service configuration, supports Easy Auth / managed identity | Serverless auto-pauses when idle; free-tier-equivalent cost for a small learning app | First-class (`Microsoft.EntityFrameworkCore.SqlServer`) |
| Azure Database for PostgreSQL (Flexible Server) | Equally strong relationally | Good, but slightly more setup (firewall rules, no auto-pause on cheapest tiers at the time of writing) | Comparable to SQL Database but without a serverless auto-pause option at the lowest tier | First-class via Npgsql provider |
| Azure Cosmos DB | Poor fit — the domain has normalized relational structure (foreign keys between courses/lessons/questions/answers); Cosmos's strengths (schema-flexible documents, global distribution) are not needed here | Good | Higher baseline cost than a small SQL/Postgres instance for this workload | Supported but the "aggregate as document" modeling the domain does not need |
| SQLite file on App Service | Not viable in production | App Service's file system is not guaranteed persistent across restarts/scale-out; multiple instances cannot safely share one file | Free | Supported via `Microsoft.EntityFrameworkCore.Sqlite`, but only safe for local/dev use |

## Decision

**Azure SQL Database, Serverless compute tier, accessed through EF Core** (see ADR 0004 for the
formal record).

Rationale:

- The domain model is relational by nature (ordered lessons within a course, questions within a
  quiz, answer choices within a question, per-learner progress/XP/streak rows referencing a
  learner id). Azure SQL Database models this directly without fighting the shape of a document
  or key-value store.
- Serverless compute auto-pauses during idle periods, which matches an early-stage learning app
  with intermittent traffic and keeps cost low without a separate provisioning step later.
- EF Core's SQL Server provider is the most mature and well-documented combination with ASP.NET
  Core / Blazor on Azure App Service, which keeps the Infrastructure layer's learning curve low
  for contributors.
- Azure Portal deployment of App Service already has a first-class, documented path to
  provisioning and connecting an Azure SQL Database (connection string via App Service
  Configuration → Connection strings, no custom networking required for a Basic/Serverless
  instance in the same region).

## Consequences

- `ManLearning.Infrastructure` will gain an EF Core `DbContext` and SQL-backed repository
  implementations that satisfy the existing `Application.Abstractions` interfaces
  (`ICourseRepository`, `ILessonProgressRepository`, `IXpAwardRepository`, `IStreakRepository`).
  The Application and Domain layers do not change shape as a result of this decision — the whole
  point of the repository abstractions was to make this swap possible without touching use cases.
- Local development will use either a local SQL Server/Azure SQL Edge container or EF Core's
  in-memory/SQLite provider for fast tests; the in-memory repositories under
  `ManLearning.Infrastructure.Persistence` will be removed once EF Core implementations exist and
  are verified, rather than kept indefinitely alongside them.
- Connection strings must never be committed to source control: local development uses .NET user
  secrets, and production uses Azure App Service Configuration (Connection strings section) per
  `.github/copilot-instructions.md` §7.
- EF Core migrations become part of the deployment process; this needs a documented migration
  strategy (apply-on-startup for this stage, versus a separate release step later) as a follow-up
  decision once implementation starts.

## Follow-up work (tracked separately, not part of this spike)

1. Add `ManLearning.Infrastructure` EF Core packages (`Microsoft.EntityFrameworkCore.SqlServer`,
   `Microsoft.EntityFrameworkCore.Design`) and a `ManLearningDbContext`.
2. Implement SQL-backed repositories and an initial migration.
3. Provision the Azure SQL Database (Serverless) alongside the App Service instance in the Azure
   Portal, and wire the connection string through App Service Configuration.
4. Retire the in-memory repositories once parity is verified by the existing Application-layer
   test suite running against the EF Core implementations (e.g. via SQLite in-memory for fast
   CI runs, mirroring production behavior closely enough for the current test depth).

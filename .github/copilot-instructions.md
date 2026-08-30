# Man Learning — GitHub Copilot Instructions

## 1. Project Overview

Man Learning is a web application designed to make learning about Artificial Intelligence engaging and accessible through a game-like learning experience.

The core idea is:

> Learn AI concepts through short lessons, quizzes, progression, and interactive experiences.

The application should make AI education feel more like a game than a traditional online course.

The target audience is primarily students and beginners who want to understand AI concepts without starting with overly technical material.

The product should gradually guide users from basic AI concepts to practical AI development concepts.

---

## 2. Core Product Principles

### Learning First

Every feature must contribute to learning.

Avoid adding features simply because they are technically interesting.

### Short Learning Sessions

Lessons should be understandable and completable in short sessions.

### Gamification

The product should encourage continued learning through:

* XP
* Levels
* Streaks
* Achievements
* Progress
* Course progression

Gamification must support learning rather than distract from it.

### Progressive Difficulty

Learning content should progress from beginner concepts to more advanced concepts.

Example progression:

AI Fundamentals
→ Machine Learning
→ Deep Learning
→ Generative AI
→ LLM
→ AI Development
→ Agents and Tools

### Maintainability

The project must remain easy to understand and modify as the application grows.

Prefer simple, explicit architecture over unnecessary abstraction.

---

# 3. Technology Stack

## Application

* Web application
* Blazor Web App
* ASP.NET Core
* .NET 10

The application must remain a web application.

Do not introduce a separate mobile application during the initial development phase.

---

## Deployment

Production deployment is fixed to:

* Microsoft Azure
* Azure Portal
* Azure App Service

The application must be designed so that deployment to Azure App Service is straightforward.

Do not introduce another production hosting platform unless explicitly requested.

---

## UI

The UI framework should be selected during the UI/UX implementation phase.

Do not introduce a UI framework solely during initial project setup unless required by the provided UI/UX specification.

The UI should ultimately be:

* Responsive
* Accessible
* Mobile-friendly
* Desktop-friendly
* Consistent
* Simple
* Modern

---

## Database

The database technology should be selected through a dedicated technical spike before implementation.

Do not assume a database provider without documenting the decision.

---

## External APIs

External APIs must NOT be selected during initial project setup.

API selection will be handled through technical spikes.

When an external dependency is required:

1. Identify the requirement.
2. Create a spike.
3. Compare suitable solutions.
4. Document the decision.
5. Record important trade-offs.
6. Only then implement the integration.

Never invent API credentials, endpoints, or SDK configuration.

---

# 4. Architecture

Use a layered architecture.

```text
ManLearning.Web
        ↓
ManLearning.Application
        ↓
ManLearning.Domain

ManLearning.Infrastructure
        ↓
External systems / Database / APIs
```

## ManLearning.Web

Responsible for:

* Blazor pages
* Components
* Layouts
* UI state
* User interaction
* Dependency injection composition

The Web project should not contain business rules.

---

## ManLearning.Application

Responsible for:

* Use cases
* Application services
* Commands
* Queries
* DTOs
* Interfaces
* Application-level validation

This layer coordinates application behavior.

---

## ManLearning.Domain

Responsible for:

* Entities
* Value objects
* Domain rules
* Domain enums
* Domain events where necessary

The Domain layer must not depend on infrastructure or UI.

---

## ManLearning.Infrastructure

Responsible for:

* Database implementation
* External API integrations
* Authentication infrastructure
* AI providers
* File storage
* Notifications
* Other external dependencies

Infrastructure implementations should be exposed to the Application layer through interfaces.

---

## ManLearning.Shared

Responsible only for genuinely shared models or constants.

Do not turn Shared into a dumping ground.

If something belongs clearly to Domain or Application, keep it there.

---

# 5. Documentation

All important project decisions must be documented.

```text
docs/
├── product/
├── architecture/
├── decisions/
└── spikes/
```

### product/

Product-related documentation.

Examples:

* product vision
* problem definition
* personas
* requirements
* learning philosophy

### architecture/

Technical architecture documentation.

### decisions/

Architecture Decision Records.

Use ADRs when an important technical or architectural decision is made.

### spikes/

Technical investigations.

Examples:

* AI provider investigation
* database investigation
* authentication investigation
* analytics investigation

A spike must investigate before implementation.

---

# 6. Coding Principles

Follow:

* SOLID principles
* Separation of concerns
* Dependency inversion
* Async/await for I/O
* Dependency injection
* Nullable reference types
* Clear naming
* Small focused classes
* Reusable components

Avoid:

* God classes
* Large methods
* Duplicate logic
* Magic strings
* Hardcoded credentials
* Hardcoded production configuration
* Business logic inside UI components
* Unnecessary abstractions
* Premature optimization

---

# 7. Configuration and Secrets

Never commit:

* API keys
* passwords
* connection strings containing credentials
* OAuth secrets
* Azure secrets
* private tokens

Use appropriate configuration mechanisms.

Development secrets should use local development secret storage.

Production secrets should be configured through Azure configuration/environment settings.

Never place secrets directly in source code.

---

# 8. Error Handling

Errors must be handled intentionally.

Do not silently swallow exceptions.

Use structured logging where appropriate.

User-facing errors should be understandable.

Internal errors must not expose sensitive implementation details.

---

# 9. Testing

Tests should be added for important business logic.

Prioritize:

* Domain rules
* Application use cases
* Critical services
* External integration behavior where practical

Do not create meaningless tests solely to increase coverage numbers.

---

# 10. Git and Commit Rules

Use clear, conventional commit messages.

Examples:

* `feat: add learning course model`
* `feat: add quiz system`
* `fix: correct lesson progress calculation`
* `refactor: simplify learning service`
* `docs: add database spike`
* `test: add quiz scoring tests`
* `chore: configure Azure deployment`

Commits should represent coherent changes.

Do not mix unrelated changes into one commit.

---

# 11. Development Workflow

Follow this workflow for significant features:

```text
Requirement
    ↓
Design
    ↓
Technical Spike if necessary
    ↓
Architecture Decision
    ↓
Implementation
    ↓
Testing
    ↓
Documentation
    ↓
Commit
```

Do not immediately implement an uncertain external dependency.

When requirements are ambiguous, identify the ambiguity before making a major architectural decision.

---

# 12. AI Development Rules

AI functionality must be designed as a replaceable capability.

Do not tightly couple the application to one AI provider.

Use an abstraction such as an application-level AI service interface when AI functionality is introduced.

The specific AI provider, model, SDK, pricing, limits, and API architecture must be investigated through a technical spike.

Do not assume a specific provider during initial setup.

---

# 13. Product Scope

The initial product focuses on:

* AI education
* Short lessons
* Quizzes
* Learning progression
* XP
* Levels
* Streaks
* Achievements
* Learning history
* Progress tracking

The initial version should remain focused.

Do not introduce social networking, payments, messaging, or unrelated productivity features unless explicitly approved.

---

# 14. Agent Behavior

Before making major changes:

1. Inspect the existing project.
2. Read relevant documentation.
3. Determine dependencies.
4. Explain the planned change when appropriate.
5. Implement the smallest coherent solution.
6. Test the change.
7. Update documentation when necessary.

Never overwrite existing work without understanding it.

Prefer incremental changes.

Do not generate large amounts of code before the architecture and requirements are understood.

---

# 15. Initial Repository Structure and Commands

The repository is organized as:

```text
src/
├── ManLearning.Web/            # Blazor Web App and ASP.NET Core host
├── ManLearning.Application/    # Use cases, DTOs, interfaces, and validation
├── ManLearning.Domain/         # Entities and domain rules
├── ManLearning.Infrastructure/ # External-system implementations
└── ManLearning.Shared/         # Only genuinely shared models or constants
tests/
├── ManLearning.Application.Tests/
├── ManLearning.Domain.Tests/
└── ManLearning.Infrastructure.Tests/
```

Dependency direction is Web → Application → Domain and Infrastructure → Application → Domain. Shared is intentionally not referenced until a genuine shared concern exists.

Use these commands from the repository root:

```bash
dotnet restore ManLearning.sln
dotnet build ManLearning.sln
dotnet test ManLearning.sln
dotnet test tests/ManLearning.Domain.Tests/ManLearning.Domain.Tests.csproj --filter "FullyQualifiedName~TestName"
dotnet run --project src/ManLearning.Web/ManLearning.Web.csproj
```

Before implementing or modifying UI, read `.github/design.md`. Its token-based styling, accessibility, responsive behavior, interaction-state, motion, and Korean typography rules apply to all UI work.

---

# 16. Definition of Done

A feature is considered complete when:

* It satisfies its requirements.
* It follows the architecture.
* It builds successfully.
* Relevant tests pass.
* No secrets are committed.
* Documentation is updated when necessary.
* The implementation is maintainable.
* The change is ready for deployment to Azure App Service when applicable.

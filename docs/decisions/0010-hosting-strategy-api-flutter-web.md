# ADR 0010: Hosting Strategy for `ManLearning.Api` and Flutter Web

## Status

Accepted (provisional) — hosting direction only; no Azure resources are created, modified, or
reconfigured by this ADR.

## Context

Production infrastructure today (per ADR 0004 and the project's current deployment) consists of:

- A single Azure App Service, `manlearning-woojin-krc` (Linux, B1 plan), in resource group
  `rg-manlearning-krc`, currently hosting `ManLearning.Web` (Blazor Server).
- Azure SQL Database (Serverless compute tier), accessed via Managed Identity from the App
  Service's system-assigned identity, per ADR 0004.

ADR 0005 introduces a new client (Flutter Web) and ADR 0006 introduces a new server component
(`ManLearning.Api`). Both need a hosting location. The project's general preference, reflected in
earlier decisions (e.g. ADR 0004 reusing the existing resource group rather than provisioning a
new one), is to reuse existing Azure infrastructure rather than introduce new resources unless a
concrete need is demonstrated.

## Decision

Reuse the existing Azure App Service as the hosting target for both `ManLearning.Api` and the
compiled Flutter Web static assets, as the **default/baseline plan**:

```text
Azure App Service (manlearning-woojin-krc)
 ├── ManLearning.Api        — serves /api/* via ASP.NET Core Controllers (ADR 0006)
 └── Flutter Web build output — served as static files for all other routes

Azure SQL Database — unchanged, same Serverless database and Managed Identity connection as today
```

Key points:

1. **Baseline plan: single App Service serves both the API and the Flutter Web static build.**
   ASP.NET Core's static file hosting (serving the Flutter Web build's output alongside the API
   controllers in the same process) is the default approach, because it requires no new Azure
   resource and reuses the App Service, App Service Plan, and deployment pipeline that already
   exist and are already proven in production.
2. **A separate static hosting service (e.g. Azure Static Web Apps, a CDN-backed static host) is
   recorded as a considered alternative, not adopted now.** It may become appropriate later if
   static-asset traffic or scaling characteristics justify separating it from the API, but no such
   need has been demonstrated yet.
3. **Azure SQL Database is unchanged.** The existing Serverless database and its Managed
   Identity–based connection (ADR 0004) continue to be used exactly as today; this ADR does not
   alter the database, its tier, or its connection method.
4. **This is a direction, not a verified deployment.** Before implementation, the actual hosting
   structure (how ASP.NET Core serves both API routes and Flutter Web static assets in one app,
   how routing between the two is configured, how this interacts with the existing CI/deployment
   process) must be concretely validated against the real App Service, not assumed from this
   document alone.

## Alternatives Considered

- **Flutter Web hosted separately (e.g. a second App Service or Azure Static Web Apps), API kept
  on the existing App Service.** Recorded as a viable alternative, not adopted as the baseline:
  it offers clearer separation of concerns (static assets vs. API) and CDN-style performance
  benefits, but requires provisioning and paying for an additional Azure resource that the current
  scale has not been shown to need.
- **Replacing Azure SQL Database or its connection method as part of this hosting change.**
  Rejected: out of scope. ADR 0004's decision (Azure SQL Serverless + Managed Identity) is
  unaffected by where the API/Flutter Web are hosted and is explicitly retained.
- **Provisioning an entirely new resource group or App Service Plan for the API/Flutter Web,
  rather than reusing `rg-manlearning-krc`/`manlearning-woojin-krc`.** Rejected: contradicts the
  project's demonstrated preference (ADR 0004) to reuse existing Azure infrastructure rather than
  create parallel resources without a specific justification.

## Consequences

- No Azure resource is created, deleted, or modified as a result of this ADR.
- When `ManLearning.Api` and the Flutter Web build are actually implemented and deployed, the
  deployment pipeline will need updating to build and publish both alongside (or in place of)
  `ManLearning.Web`'s current publish step; this is implementation work, not performed here.
- If actual implementation reveals that a single App Service cannot adequately serve both the API
  and Flutter Web static assets (for example, due to scaling, caching, or routing conflicts), this
  ADR should be superseded by a new one that adopts the separate-hosting alternative, rather than
  silently changed in infrastructure.
- `ManLearning.Web` continues to run on the same App Service throughout the migration window
  described in ADR 0005; this ADR does not require removing it or reconfiguring the App Service
  ahead of that migration being validated.

## Migration / Implementation Notes

- Before implementation, validate concretely (in a non-production slot or a short spike) how
  ASP.NET Core in `ManLearning.Api` would serve Flutter Web's static build output alongside API
  controllers on the same App Service, including routing precedence between `/api/*` and
  client-side Flutter routes.
- Any change to the App Service's configuration (runtime stack, deployment slots, scaling plan)
  required to support this hosting shape must be made deliberately and documented at
  implementation time, not inferred from this ADR.
- Azure SQL Database, its firewall rules, and its Managed Identity configuration (ADR 0004) are
  unaffected and require no changes under this ADR.

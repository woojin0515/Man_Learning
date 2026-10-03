# ADR 0007: Microsoft Entra External ID for Authentication

## Status

Accepted (provisional) — authentication is not implemented yet; this ADR records the chosen
identity provider and integration shape only.

## Context

Authentication is currently **not implemented** anywhere in the codebase. The only stand-in is
`ManLearning.Web.Learners.CurrentLearnerContext`, which issues a new `LearnerId.New()` for every
Blazor Server circuit. Its own doc comment already states this "must be replaced once real
sign-in exists; it is not a security boundary" — this ADR does not change that code, but records
it as the thing that must eventually be replaced.

ADR 0005 and ADR 0006 establish that Flutter will call a new `ManLearning.Api` over HTTPS/JSON.
Once a network boundary exists between the client and the server, identity can no longer be a
value the client simply holds in memory (as `CurrentLearnerContext` does today) — it must be
established by an authentication step the client cannot forge, and verified by the server on
every request.

The project's production infrastructure is already entirely Azure-based: Azure App Service
(`manlearning-woojin-krc`) and Azure SQL Database with Managed Identity–based passwordless
authentication (ADR 0004). This existing Azure footprint is a relevant, factual constraint when
choosing an identity provider, per the project's general preference to reuse existing Azure
infrastructure rather than introduce a new vendor.

## Decision

Adopt **Microsoft Entra External ID** as the identity provider for learner authentication, with
the following integration shape. Importantly, **the API does not issue its own JWTs** — it only
validates tokens issued by Entra External ID:

```text
Flutter
  ↓
Microsoft Entra External ID        (handles sign-up / sign-in / token issuance)
  ↓
Entra-issued access token
  ↓
ManLearning.Api
  ↓
ASP.NET Core JWT bearer validation  (validates the Entra-issued token; issues nothing itself)
  ↓
authenticated user identity        (claims from the validated token, e.g. `sub`/`oid`)
  ↓
Learner mapping                    (server-side lookup/creation of the corresponding LearnerId)
```

Key points of this decision:

1. **Entra External ID is the authority for sign-up, sign-in, and token issuance.** Flutter
   authenticates the user against Entra External ID (via its hosted UI or an MSAL-based SDK) and
   receives an Entra-issued access token. The client never constructs or signs its own token.
2. **`ManLearning.Api` only validates tokens; it does not mint them.** The API uses standard
   ASP.NET Core JWT bearer authentication middleware to validate the signature, issuer, and
   audience of the Entra-issued token on every request.
3. **The server, not the client, determines `LearnerId`.** The API extracts a stable identity
   claim (e.g. `sub` or `oid`) from the validated token and uses it to look up or create the
   corresponding `LearnerId` server-side. A client-supplied `LearnerId` in a request body or query
   string must never be trusted as the acting learner's identity — the security boundary is the
   validated token, not client input.
4. **`CurrentLearnerContext`'s current behavior is explicitly acknowledged as temporary and
   insufficient.** It issues an unauthenticated, per-circuit random identifier and performs no
   identity verification. This ADR does not modify that code, but records that it must eventually
   be replaced by the token-based identity flow above once authentication is implemented for
   either Blazor or the new API.

This ADR does **not** implement sign-up, sign-in, token validation middleware, or any Learner
mapping code. It records the decision so later implementation work has a concrete target.

## Alternatives Considered

- **Self-built email/password authentication.** Rejected: requires the project to directly own
  password hashing, credential storage, password-reset flows, and associated security risk, with
  no corresponding benefit over a managed identity provider the project can otherwise rely on.
- **Third-party identity providers unrelated to Azure (e.g. Auth0, Supabase Auth).** Rejected:
  introduces a new vendor relationship with no integration advantage over an identity provider
  already native to the Azure subscription and resource group the project depends on (App
  Service, Azure SQL, Managed Identity per ADR 0004).
- **Issuing JWTs directly from `ManLearning.Api`** (i.e., the API acting as its own authority).
  Rejected: this would require the API to own credential verification and token signing itself,
  duplicating what a managed identity provider already does safely, and was explicitly excluded
  by the task defining this ADR.

## Consequences

- No authentication code exists yet; `ManLearning.Web` continues to operate with
  `CurrentLearnerContext`'s temporary anonymous identifier until authentication is actually
  implemented for either client.
- Once implemented, `ManLearning.Api` will require a `LearnerId` mapping mechanism (e.g. a mapping
  table or Learner aggregate keyed by the Entra identity claim) that does not exist today in
  `ManLearning.Domain`/`ManLearning.Infrastructure`. Designing that mapping is implementation work,
  not covered by this ADR.
- Any future API endpoint (per ADR 0006) must treat the validated token's identity claim, not a
  client-supplied `LearnerId`, as the only trustworthy source of the acting learner's identity.
- Azure AD tenant and app registration setup for Entra External ID is Azure configuration work
  that has not been performed and is out of scope for this ADR.
- If a future requirement makes Entra External ID unsuitable (cost, missing Flutter/mobile SDK
  support, or a product decision to support non-Microsoft identity federation), this ADR should be
  superseded by a new one rather than silently changed in code.

## Migration / Implementation Notes

- This ADR does not implement authentication. Implementation requires, at minimum: an Entra
  External ID tenant/app registration, JWT bearer validation middleware added to
  `ManLearning.Api` (once that project exists per ADR 0006), a Learner-identity mapping mechanism,
  and an MSAL-based sign-in flow in the Flutter client (once that project exists per ADR 0005).
- `CurrentLearnerContext` is not modified by this ADR. Its replacement is deferred to the
  implementation phase and should be tracked as its own change, not bundled silently into
  unrelated work.

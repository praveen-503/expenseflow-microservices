# ExpenseFlow Copilot Instructions

## Project Shape

ExpenseFlow is a .NET 10 microservices system with an Angular frontend. The repository currently contains the Expense, Identity, and Notification services under `src/Services/`, plus the Angular client under `src/Web/expenseflow-client/`.

## Working Rules

- Keep changes aligned with Clean Architecture and microservice boundaries.
- Prefer small, focused edits that fit the existing layer structure.
- Preserve the separation between API, Application, Domain, and Persistence layers.
- Do not introduce secrets, credentials, or environment values into source files.
- Use existing package versions, conventions, and service registration patterns when extending code.
- Favor readable, maintainable code over clever implementations.

## Backend Expectations

- Keep controllers thin and push business logic into Application or Domain layers.
- Use async methods and pass cancellation tokens where the call chain supports them.
- Validate inputs early and return clear error responses.
- Keep entity and domain behavior in the Domain layer, not in controllers or infrastructure code.
- Use EF Core migrations for schema changes and keep database access in Persistence or Infrastructure.

## Frontend Expectations

- Keep Angular components focused and composable.
- Prefer TypeScript strictness, reactive patterns, and existing Angular Material conventions.
- Put UI styling in SCSS and avoid introducing ad hoc styling systems.
- Match the current client scripts, Node version, and build tooling in `src/Web/expenseflow-client/`.

## Testing Expectations

- Add or update tests for behavior changes whenever practical.
- Prefer focused tests that cover the touched slice rather than broad end-to-end changes.
- Keep test names descriptive and aligned with the existing project language.

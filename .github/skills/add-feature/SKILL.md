---
name: add-feature
description: Use when implementing a feature that spans API, Application, Domain, Persistence, and possibly the Angular client.
---

# Add Feature

Use this skill when a change needs to move through more than one layer of ExpenseFlow.

## What to do

1. Trace the feature to the owning service and use case.
2. Update the domain model or business rule first if the feature changes core behavior.
3. Add the application command, query, DTO, or handler that exposes the use case.
4. Update persistence or infrastructure only if the feature needs storage or external integration changes.
5. Wire the API endpoint or frontend entry point that triggers the change.
6. Add or update tests for the touched slice.
7. Check whether documentation, OpenAPI, or local setup notes need a small update.

## Constraints

- Keep the feature incremental.
- Do not expand scope into unrelated services.
- Preserve the current architecture and naming conventions.

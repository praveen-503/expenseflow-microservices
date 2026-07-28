---
description: Testing guidance for backend and frontend changes
applyTo: "src/**"
---

# Testing Guidance

- Add tests that exercise the changed behavior directly.
- Keep tests narrow and deterministic.
- Prefer unit tests for business logic and integration tests for persistence, messaging, or API boundaries.
- Use existing test frameworks and patterns already present in the repo.
- When a change affects a contract, update the relevant test coverage on both sides of the contract if needed.
- Keep test data explicit and avoid large shared fixtures unless the project already uses them.

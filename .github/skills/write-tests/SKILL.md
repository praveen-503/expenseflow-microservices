---
name: write-tests
description: Use when adding or updating unit, integration, or frontend tests for ExpenseFlow changes.
---

# Write Tests

Use this skill when you need to cover a behavior change with tests.

## What to do

1. Identify the smallest layer that proves the behavior.
2. Prefer a unit test for pure logic and an integration test for EF Core, API, or messaging behavior.
3. Keep test names descriptive and aligned with the feature being verified.
4. Build test data explicitly so failures are easy to understand.
5. Add assertions for the outcome and any important side effects.
6. Run the narrowest relevant test command for the touched project.

## Constraints

- Do not overfit tests to internal implementation details.
- Keep fixtures small and local to the test when practical.
- Match the repo's existing test framework and style.

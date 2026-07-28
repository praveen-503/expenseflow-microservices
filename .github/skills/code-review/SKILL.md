---
name: code-review
description: Use when reviewing ExpenseFlow changes for correctness, architecture fit, security, and test coverage.
---

# Code Review

Use this skill when reviewing a change before merge.

## What to check

1. Correctness of the behavior and edge cases.
2. Layering and architecture boundaries.
3. Security, authorization, and tenant isolation.
4. Error handling, logging, and observability.
5. Test coverage for the changed behavior.
6. Performance or reliability regressions.

## Output

- Call out concrete findings first.
- Separate must-fix issues from smaller risks.
- Reference the impacted file and line when possible.

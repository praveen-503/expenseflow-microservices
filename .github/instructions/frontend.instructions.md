---
description: Frontend guidance for the Angular client
applyTo: "src/Web/expenseflow-client/**"
---

# Frontend Guidance

- Keep Angular code consistent with the existing Angular 22, TypeScript, and Angular Material stack.
- Prefer small, focused components and feature-oriented folders.
- Use SCSS for styling and keep component styles local when possible.
- Favor reactive state and signals over ad hoc mutable state.
- Keep templates readable and avoid pushing complex logic into HTML.
- Match the current npm scripts, Node engine range, and build setup in the client package.
- Add or update unit tests when changing component behavior, services, or state logic.

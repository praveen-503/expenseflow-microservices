---
description: Architecture guidance for the ExpenseFlow codebase
applyTo: "src/**"
---

# Architecture Guidance

- Respect the microservice boundaries between Expense, Identity, and Notification.
- Keep shared contracts intentionally small and stable.
- Use the layered structure already present in each service: API, Application, Domain, Infrastructure, and Persistence.
- Prefer domain-centric naming and behavior over technical naming.
- Keep cross-cutting concerns such as telemetry, messaging, and validation in the appropriate outer layer.
- If a change needs a new service or integration point, fit it into the existing repo structure instead of inventing a new pattern.
- Match the current deployment and local development model that uses Docker, Docker Compose, and the existing GitHub Actions workflows.

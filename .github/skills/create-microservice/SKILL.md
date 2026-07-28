---
name: create-microservice
description: Use when creating a new backend microservice under src/Services with the standard API, Application, Domain, Infrastructure, and Persistence layers.
---

# Create Microservice

Use this skill when introducing a new backend service to the ExpenseFlow solution.

## What to do

1. Choose a service name that matches the repository naming style.
2. Create the standard layered projects for API, Application, Domain, Infrastructure, and Persistence.
3. Register the projects in the solution and connect the project references in the correct direction.
4. Add configuration, health checks, logging, and middleware consistent with the existing services.
5. Add persistence and migration setup if the service owns data.
6. Add Docker and local startup support if the service must run with the rest of the stack.
7. Add CI workflow updates if the new service needs build or deployment coverage.

## Constraints

- Keep boundaries clear between services.
- Prefer reusing shared abstractions only when they are truly cross-cutting.
- Avoid creating a new integration pattern if the repo already has one that fits.

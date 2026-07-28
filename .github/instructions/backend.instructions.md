---
description: Backend guidance for ExpenseFlow microservices
applyTo: "src/Services/**/*.cs"
---

# Backend Guidance

- Keep API projects thin. Controllers should translate HTTP concerns, not hold business rules.
- Put use cases in the Application layer and domain rules in the Domain layer.
- Keep Persistence focused on EF Core configuration, repositories, and migrations.
- Keep Infrastructure focused on external integrations such as messaging, telemetry, and hosted services.
- Use consistent naming for commands, queries, handlers, DTOs, validators, and specifications.
- Prefer explicit dependency injection via extension methods and service registration helpers.
- Preserve API versioning, Swagger, health checks, logging, and validation conventions already in the service.
- Use cancellation tokens for async I/O, database calls, and messaging handlers when available.
- Avoid leaking EF Core types, database models, or infrastructure details into API responses.

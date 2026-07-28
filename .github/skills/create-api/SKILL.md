---
name: create-api
description: Use when adding a new API endpoint, controller action, request or response DTO, validator, or versioned route in an ExpenseFlow service.
---

# Create API

Use this skill when you need to add or extend an HTTP endpoint inside one of the backend services.

## What to do

1. Identify the owning service and API version.
2. Add or update the request and response models in the Application layer.
3. Add the command, query, or handler that owns the behavior.
4. Add validation rules before the handler reaches persistence.
5. Wire the endpoint in the controller with the existing routing and versioning style.
6. Update Swagger/OpenAPI annotations if the project uses them for the endpoint.
7. Add focused tests for the endpoint or handler behavior.

## Constraints

- Keep controllers thin.
- Do not place business rules directly in the API project.
- Reuse existing exception and response handling patterns.
- Keep tenant and identity concerns explicit when the endpoint touches protected data.

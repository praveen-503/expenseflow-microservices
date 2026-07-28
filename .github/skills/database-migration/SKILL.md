---
name: database-migration
description: Use when creating, updating, or applying Entity Framework Core migrations for an ExpenseFlow service.
---

# Database Migration

Use this skill when the schema or persistence model changes.

## What to do

1. Update the persistence model or configuration first.
2. Create the EF Core migration in the owning persistence project.
3. Review the generated migration for correctness before applying it.
4. Update seed data or conversion rules if the schema change requires it.
5. Apply the migration locally and confirm the service still starts.
6. Update tests or fixtures if they depend on the affected schema.

## Constraints

- Do not hand-edit the database without a matching migration when the repo uses EF Core migrations.
- Keep migrations focused and reversible when possible.
- Avoid mixing unrelated schema changes into one migration.

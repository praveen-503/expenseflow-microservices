---
description: Security and reliability guidance for ExpenseFlow
applyTo: "src/**"
---

# Security and Reliability Guidance

- Never hardcode secrets, access tokens, or connection strings.
- Treat tenant, identity, and authorization boundaries as security-sensitive.
- Validate user input and external messages before using them in persistence or downstream calls.
- Do not weaken authentication, authorization, or audit logging behavior.
- Prefer safe defaults for configuration and fail fast on invalid environment setup.
- Be careful with logging so sensitive data is not written to console or traces.
- Keep dependency and package updates conservative unless the change is specifically about dependency maintenance.

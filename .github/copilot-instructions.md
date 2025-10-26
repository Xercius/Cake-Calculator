# GitHub Copilot Instructions — Cake Pricing

## Purpose
Local-only app. Compute cost and three suggested prices from ingredients, templates, extras, labor by role, and fixed per-order overhead (incl. packaging and delivery). Export CSV. No external paid services.

## Stack
Backend: .NET 9 ASP.NET Core Web API + EF Core 9 + SQLite
Frontend: React 19 + Vite + TypeScript
UI: Tailwind (+ optional shadcn/ui)
Data: TanStack Query
IDE: VS 2022

## Structure
/api → backend (Features/Ingredients, Templates, Roles, Orders, Pricing, Settings)
/client → frontend

## Domain
- Ingredient(id, name, unit, costPerUnit, isActive)
- Template(id, name, size, type)
- TemplateIngredient(templateId, ingredientId, qty, unit?)
- Role(id, name, hourlyRate)
- Order(id, templateId, name, date,
        rolesTime[{roleId, minutes}],
        extraItems[{ingredientId, qty, unit?}],
        overheadFixed, yieldLossNotes?)
- PricingSettings(margins[], roundingRule="ceil-dollar", allowPerQuoteOverride=true)

## Pricing Rules (must follow)
- ingredientCost = Σ((template + extras) qty × costPerUnit)  // qty and costPerUnit must use the same unit (e.g., grams, pieces); no automatic unit conversions are performed
- laborCost = Σ(minutes × role.hourlyRate / 60)
- overheadCost = overheadFixed  // includes packaging and delivery
- totalCost = ingredientCost + laborCost + overheadCost
- prices = margins.map(m => ceil_to_dollar(totalCost × (1 + m)))
- No min price. Rounding is always “ceil to next whole dollar.”

Keep pricing math out of React components. Implement in backend services or a shared backend module used by API.

## Endpoints
- CRUD: /api/ingredients, /api/templates, /api/roles, /api/orders
- GET /api/settings/pricing
- PUT /api/settings/pricing  // update margins only; roundingRule is always "ceil-dollar" (not editable)
- GET /api/pricing/{orderId}?margins=0.2,0.4,0.6
  Returns:
  {
    "breakdown": { "ingredients": number, "labor": number, "overhead": number, "totalCost": number },
    "prices": [number, number, number],
    "inputs": { "margins": [..], "roundingRule": "ceil-dollar" }
  }
- GET /api/export/orders.csv

## Backend Conventions
- Feature folders: Controllers, Dtos, Mapping, Validation.
- REST, async/await, ProblemDetails, Swagger in dev.
- EF Core migrations + seed. CORS allow http://localhost:5173.

## Frontend Guidelines
- Pages: Ingredients, Templates, Roles, Orders, Price Summary.
- Fetch via TanStack Query; axios baseURL /api.
- Tailwind layouts. Responsive.
- Display math only; no pricing logic in UI.

## Copilot — Do
- Provide a short plan, exact file paths, code edits.
- Add/update tests for pricing and controllers.
- Include run and verify steps for each change.
- Use PricingSettings; allow margins override per request.

## Copilot — Don’t
- Don’t introduce external APIs or paid services.
- Don’t move pricing math into React.
- Don’t change rounding away from “ceil-dollar” unless asked.

## Verification Checklist (attach in PR)
- `dotnet build` and `dotnet test` pass.
- `pnpm i` and `pnpm dev` run.
- Seed an Order and Roles. Call:
  GET `/api/pricing/{orderId}?margins=0.2,0.4,0.6`
  Confirm: ingredients + labor + overhead = totalCost.
  Confirm: prices are whole dollars and ≥ totalCost.
  Paste JSON response or screenshot.
- Export check: GET `/api/export/orders.csv` downloads and includes the seeded order.

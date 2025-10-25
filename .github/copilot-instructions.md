# GitHub Copilot Instructions — Cake Pricing

## Purpose
Local-only Cake Pricing app. Compute cost and three suggested prices from ingredients, templates, extras, labor, and other expenses. Educational use. No external paid services.

## Stack
Backend: .NET 9 ASP.NET Core Web API + EF Core 9 + SQLite
Frontend: React 19 + Vite + TypeScript
UI: Tailwind CSS (+ optional shadcn/ui)
Data: TanStack Query
IDE: Visual Studio 2022

## Structure
/api → backend (Features/Ingredients, Features/Templates, Features/Cakes, Features/Pricing)
/client → frontend

## Domain (guidance)
- Ingredient(id, name, unit, costPerUnit, yieldPct?, lossPct?, isActive)
- Template(id, name, size, type)
- TemplateIngredient(templateId, ingredientId, qty, unit?)
- Cake(id, name, templateId, laborHours, laborRate, otherCosts)
- CakeExtraIngredient(cakeId, ingredientId, qty, unit?)
- PricingSettings(margins[], roundingRule, minPrice, overheadPct?, packagingFixed?)

## Pricing Rules (must follow)
- Ingredient unit cost = costPerUnit × (1 / effective_yield), where effective_yield accounts for yieldPct/lossPct if present.
- Recipe cost = Σ(qty × unit cost) across template + extras.
- Labor = laborHours × laborRate.
- Overhead = recipe cost × overheadPct + packagingFixed + otherCosts.
- Total cost = ingredients + labor + overhead.
- Prices = apply margins to Total cost → apply roundingRule → enforce minPrice.

Do not put pricing math inside React components. Keep it in backend services or a shared module used by the API.

## Endpoints
- CRUD: /api/ingredients, /api/templates, /api/cakes
- GET /api/pricing/{cakeId}?margins=0.2,0.4,0.6
  Returns:
  {
    "costBreakdown": {
      "ingredients": number,
      "labor": number,
      "overhead": number,
      "totalCost": number
    },
    "prices": [number, number, number],
    "inputs": { "margins": [0.2,0.4,0.6], "roundingRule": "nearest-0.25", "minPrice": 25 }
  }
- GET /api/health
- GET/PUT /api/settings/pricing

## Backend Conventions
- Feature folders with Controllers, Dtos, Mapping, Validation.
- RESTful naming, async/await, Swagger on dev.
- CORS allow http://localhost:5173.
- EF Core migrations included. Seed minimal sample data.

## Frontend Guidelines
- Pages: Ingredients, Templates, Cakes, Price Summary (React Router v7).
- axios baseURL /api; all fetching via TanStack Query.
- Tailwind for layout; responsive by default.
- No business math in components. Display only.

## Copilot — Do
1) When implementing or fixing:
   - Provide a short plan.
   - Output exact file paths and code edits.
   - Add or update tests for pricing and controllers.
   - Include run/verify steps.

2) Keep logic pure and testable:
   - Pricing service class with unit tests.
   - Deterministic seed data for snapshot checks.

3) Respect settings:
   - Use PricingSettings for margins, roundingRule, minPrice, overheadPct, packagingFixed.

## Copilot — Don’t
- Don’t add external APIs or paid services.
- Don’t move pricing math into React.
- Don’t change defaults or domain unless asked.

## Verification Checklist (always include in PR)
- `dotnet build` and `dotnet test` pass.
- `pnpm install` and `pnpm dev` start successfully.
- Sample verification:
  - Given seed Cake `Sample 8" Round`, GET `/api/pricing/{cakeId}?margins=0.2,0.4,0.6`
  - Confirm `ingredients + labor + overhead = totalCost`.
  - Confirm `prices.length === 3` and roundingRule + minPrice applied.
  - Attach screenshot or paste JSON of the response.

## Example Tasks
- Implement PricingSettings GET/PUT and persist with EF Core.
- Add rounding modes: nearest-0.25, nearest-0.50, charm-.99.
- Add overheadPct and packagingFixed to pricing pipeline with tests.
- Build Price Summary page consuming `/api/pricing/{cakeId}` with live refresh.

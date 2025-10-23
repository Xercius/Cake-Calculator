# GitHub Copilot Instructions

## Project Purpose
Local-only **Cake Pricing** app. Compute three suggested prices for a custom cake using costs of ingredients, base templates, extra items, labor, and other expenses. No paid services. Educational use.

## Tech Stack
- Backend: .NET 9 ASP.NET Core Web API
- DB: SQLite via EF Core 9
- Frontend: React 19 + Vite + TypeScript
- UI: Tailwind CSS (+ optional shadcn/ui)
- Data layer: TanStack Query
- IDE: Visual Studio 2022

## Structure
/api        → backend
  /Features
    /Ingredients
    /Templates
    /Cakes
/client     → frontend

## Domain
- Ingredient(id, name, unit, costPerUnit)
- Template(id, name, size, type) with TemplateIngredient(templateId, ingredientId, qty)
- Cake(id, name, templateId, LaborHours, LaborRate, OtherCosts)
- CakeExtraIngredient(cakeId, ingredientId, qty)

## Endpoints
- CRUD: /api/ingredients, /api/templates, /api/cakes
- GET /api/pricing/{cakeId}?margins=0.1,0.2,0.3
  - Returns { costBreakdown, prices: [p1,p2,p3] }
- GET /api/health

## Backend Conventions
- Feature folders with Controllers, Dtos, Mapping.
- REST names. Async/await. Swagger enabled. CORS for http://localhost:5173.
- EF Core migrations included. Seed minimal sample data.

## Frontend Guidelines
- React Router v7 pages: Ingredients, Templates, Cakes, Price Summary.
- axios baseURL `/api`. All fetching via TanStack Query.
- Tailwind for layout. Responsive by default.

## Expectations for Copilot
- Keep code local-only. No external APIs.
- Follow stack and structure above.
- Provide descriptive commit messages.
- Verify `dotnet run` and `npm run dev` both work.
- Add basic tests where changes touch controllers or pricing logic.

## Testing
- xUnit for pricing calculator and controller smoke tests.
- Optional: React Testing Library for Price Summary.

## License
MIT.

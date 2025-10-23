# Cake Calculator

An app to help us plan and track cake costs, and help determine sale price.

## Architecture

This application consists of:

- **Backend**: .NET 9 Web API with EF Core SQLite (`/api`)
- **Frontend**: React 19 + Vite + TypeScript + Tailwind CSS (`/client`)

## Features

### Backend API Endpoints

#### Ingredients CRUD
- `GET /api/ingredients` - List all ingredients
- `GET /api/ingredients/{id}` - Get specific ingredient
- `POST /api/ingredients` - Create new ingredient
- `PUT /api/ingredients/{id}` - Update ingredient
- `DELETE /api/ingredients/{id}` - Delete ingredient

#### Templates CRUD
- `GET /api/templates` - List all templates
- `GET /api/templates/{id}` - Get specific template
- `POST /api/templates` - Create new template
- `PUT /api/templates/{id}` - Update template
- `DELETE /api/templates/{id}` - Delete template

#### Cakes CRUD
- `GET /api/cakes` - List all cakes
- `GET /api/cakes/{id}` - Get specific cake
- `POST /api/cakes` - Create new cake
- `PUT /api/cakes/{id}` - Update cake
- `DELETE /api/cakes/{id}` - Delete cake

#### Pricing
- `GET /api/pricing/{id}?margins=0.1,0.2,0.3` - Calculate cake pricing with multiple margin options

### Frontend Pages

1. **Ingredients** - Manage ingredients with name and cost per unit
2. **Templates** - Manage cake templates with size, type, and base ingredients
3. **Cakes** - Manage cakes with template selection, extra ingredients, labor, and other costs
4. **Price Summary** - Calculate and view pricing for cakes with configurable profit margins

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js 18+ and npm

### Running the Backend

```bash
cd api
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000`

### Running the Frontend

```bash
cd client
npm install
npm run dev
```

The frontend will be available at `http://localhost:5173` with Vite proxy configured to forward `/api` requests to the backend.

### Building for Production

#### Backend
```bash
cd api
dotnet publish -c Release
```

#### Frontend
```bash
cd client
npm run build
```

The production build will be in `client/dist/`

## Data Models

### Ingredient
- `id`: number
- `name`: string
- `costPerUnit`: decimal

### Template
- `id`: number
- `name`: string
- `size`: string
- `type`: string
- `baseIngredients`: JSON string (e.g., `{"1": 2.0, "2": 1.5}`)

### Cake
- `id`: number
- `name`: string
- `templateId`: number
- `extraIngredients`: JSON string (optional)
- `labor`: decimal
- `otherCosts`: decimal

## Screenshots

### Ingredients Page
![Ingredients](https://github.com/user-attachments/assets/2c27f6be-e6a9-4cc1-8f1a-4c34ecc23866)

### Templates Page
![Templates](https://github.com/user-attachments/assets/b4796012-2b87-4bfb-9cdc-2447ed260e02)

### Cakes Page
![Cakes](https://github.com/user-attachments/assets/b8eb0f89-5e2d-4981-8158-cd47025b3515)

### Price Summary Page
![Price Summary](https://github.com/user-attachments/assets/6a7b7533-46bd-41e3-a421-fdff6145d559)


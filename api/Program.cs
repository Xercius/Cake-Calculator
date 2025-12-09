using Microsoft.EntityFrameworkCore;
using CakeCalculatorApi.Data;
using CakeCalculatorApi.Models;
using CakeCalculatorApi.DTOs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CakeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=cakecalculator.db"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CakeDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// Validation helpers
static IResult? ValidateRequiredString(string? value, string fieldName, out string? trimmedValue)
{
    trimmedValue = value?.Trim();
    if (string.IsNullOrWhiteSpace(trimmedValue))
    {
        return Results.Problem(
            title: "Validation failed",
            detail: $"{fieldName} is required and cannot be empty or whitespace",
            statusCode: 400);
    }
    return null;
}

static IResult? ValidateNonNegativeDecimal(decimal value, string fieldName)
{
    if (value < 0)
    {
        return Results.Problem(
            title: "Validation failed",
            detail: $"{fieldName} must be greater than or equal to 0",
            statusCode: 400);
    }
    return null;
}

static IResult? ValidateIngredient(string? name, decimal costPerUnit, out string? trimmedName)
{
    var nameValidation = ValidateRequiredString(name, "Name", out trimmedName);
    if (nameValidation != null) return nameValidation;

    return ValidateNonNegativeDecimal(costPerUnit, "CostPerUnit");
}

static IResult? ValidateTemplate(string? name, string? size, string? type, string? baseIngredients,
    out string? trimmedName, out string? trimmedSize, out string? trimmedType)
{
    var nameValidation = ValidateRequiredString(name, "Name", out trimmedName);
    if (nameValidation != null)
    {
        trimmedSize = null;
        trimmedType = null;
        return nameValidation;
    }

    var sizeValidation = ValidateRequiredString(size, "Size", out trimmedSize);
    if (sizeValidation != null)
    {
        trimmedType = null;
        return sizeValidation;
    }

    var typeValidation = ValidateRequiredString(type, "Type", out trimmedType);
    if (typeValidation != null) return typeValidation;

    if (string.IsNullOrWhiteSpace(baseIngredients))
    {
        return Results.Problem(
            title: "Validation failed",
            detail: "BaseIngredients is required and cannot be empty",
            statusCode: 400);
    }

    return null;
}

static IResult? ValidateRole(string? name, decimal hourlyRate, out string? trimmedName)
{
    var nameValidation = ValidateRequiredString(name, "Name", out trimmedName);
    if (nameValidation != null) return nameValidation;

    return ValidateNonNegativeDecimal(hourlyRate, "HourlyRate");
}

// Ingredients CRUD
app.MapGet("/api/ingredients", async (CakeDbContext db) =>
    await db.Ingredients.ToListAsync());

app.MapGet("/api/ingredients/{id}", async (int id, CakeDbContext db) =>
    await db.Ingredients.FindAsync(id) is Ingredient ingredient
        ? Results.Ok(ingredient)
        : Results.NotFound());

app.MapPost("/api/ingredients", async (Ingredient ingredient, CakeDbContext db) =>
{
    var validationError = ValidateIngredient(ingredient.Name, ingredient.CostPerUnit, out var trimmedName);
    if (validationError != null) return validationError;

    ingredient.Name = trimmedName!;
    db.Ingredients.Add(ingredient);
    await db.SaveChangesAsync();
    return Results.Created($"/api/ingredients/{ingredient.Id}", ingredient);
});

app.MapPut("/api/ingredients/{id}", async (int id, Ingredient inputIngredient, CakeDbContext db) =>
{
    var ingredient = await db.Ingredients.FindAsync(id);
    if (ingredient is null) return Results.NotFound();

    var validationError = ValidateIngredient(inputIngredient.Name, inputIngredient.CostPerUnit, out var trimmedName);
    if (validationError != null) return validationError;

    ingredient.Name = trimmedName!;
    ingredient.CostPerUnit = inputIngredient.CostPerUnit;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/ingredients/{id}", async (int id, CakeDbContext db) =>
{
    if (await db.Ingredients.FindAsync(id) is Ingredient ingredient)
    {
        db.Ingredients.Remove(ingredient);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Templates CRUD
app.MapGet("/api/templates", async (CakeDbContext db) =>
    await db.Templates.ToListAsync());

app.MapGet("/api/templates/{id}", async (int id, CakeDbContext db) =>
    await db.Templates.FindAsync(id) is Template template
        ? Results.Ok(template)
        : Results.NotFound());

app.MapPost("/api/templates", async (Template template, CakeDbContext db) =>
{
    var validationError = ValidateTemplate(template.Name, template.Size, template.Type, template.BaseIngredients,
        out var trimmedName, out var trimmedSize, out var trimmedType);
    if (validationError != null) return validationError;

    template.Name = trimmedName!;
    template.Size = trimmedSize!;
    template.Type = trimmedType!;
    db.Templates.Add(template);
    await db.SaveChangesAsync();
    return Results.Created($"/api/templates/{template.Id}", template);
});

app.MapPut("/api/templates/{id}", async (int id, Template inputTemplate, CakeDbContext db) =>
{
    var template = await db.Templates.FindAsync(id);
    if (template is null) return Results.NotFound();

    var validationError = ValidateTemplate(inputTemplate.Name, inputTemplate.Size, inputTemplate.Type, inputTemplate.BaseIngredients,
        out var trimmedName, out var trimmedSize, out var trimmedType);
    if (validationError != null) return validationError;

    template.Name = trimmedName!;
    template.Size = trimmedSize!;
    template.Type = trimmedType!;
    template.BaseIngredients = inputTemplate.BaseIngredients;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/templates/{id}", async (int id, CakeDbContext db) =>
{
    if (await db.Templates.FindAsync(id) is Template template)
    {
        db.Templates.Remove(template);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Cakes CRUD
app.MapGet("/api/cakes", async (CakeDbContext db) =>
    await db.Cakes.Include(c => c.Template).ToListAsync());

app.MapGet("/api/cakes/{id}", async (int id, CakeDbContext db) =>
    await db.Cakes.Include(c => c.Template).FirstOrDefaultAsync(c => c.Id == id) is Cake cake
        ? Results.Ok(cake)
        : Results.NotFound());

app.MapPost("/api/cakes", async (Cake cake, CakeDbContext db) =>
{
    db.Cakes.Add(cake);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cakes/{cake.Id}", cake);
});

app.MapPut("/api/cakes/{id}", async (int id, Cake inputCake, CakeDbContext db) =>
{
    var cake = await db.Cakes.FindAsync(id);
    if (cake is null) return Results.NotFound();

    cake.Name = inputCake.Name;
    cake.TemplateId = inputCake.TemplateId;
    cake.ExtraIngredients = inputCake.ExtraIngredients;
    cake.Labor = inputCake.Labor;
    cake.OtherCosts = inputCake.OtherCosts;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/cakes/{id}", async (int id, CakeDbContext db) =>
{
    if (await db.Cakes.FindAsync(id) is Cake cake)
    {
        db.Cakes.Remove(cake);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Pricing endpoint
app.MapGet("/api/pricing/{id}", async (int id, string? margins, CakeDbContext db, ILogger<Program> logger) =>
{
    var cake = await db.Cakes.Include(c => c.Template).FirstOrDefaultAsync(c => c.Id == id);
    if (cake is null) return Results.NotFound();

    // Calculate base cost
    decimal totalCost = cake.Labor + cake.OtherCosts;

    // Add template ingredients cost
    if (!string.IsNullOrEmpty(cake.Template?.BaseIngredients))
    {
        try
        {
            var baseIngredients = JsonSerializer.Deserialize<Dictionary<int, decimal>>(cake.Template.BaseIngredients);
            if (baseIngredients != null)
            {
                foreach (var item in baseIngredients)
                {
                    var ingredient = await db.Ingredients.FindAsync(item.Key);
                    if (ingredient != null)
                    {
                        totalCost += ingredient.CostPerUnit * item.Value;
                    }
                }
            }
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to parse base ingredients JSON for template {TemplateId}. Skipping base ingredients in cost calculation.", cake.Template.Id);
        }
    }

    // Add extra ingredients cost
    if (!string.IsNullOrEmpty(cake.ExtraIngredients))
    {
        try
        {
            var extraIngredients = JsonSerializer.Deserialize<Dictionary<int, decimal>>(cake.ExtraIngredients);
            if (extraIngredients != null)
            {
                foreach (var item in extraIngredients)
                {
                    var ingredient = await db.Ingredients.FindAsync(item.Key);
                    if (ingredient != null)
                    {
                        totalCost += ingredient.CostPerUnit * item.Value;
                    }
                }
            }
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to parse extra ingredients JSON for cake {CakeId}. Skipping extra ingredients in cost calculation.", cake.Id);
        }
    }

    // Parse margins and calculate prices
    var marginList = new List<decimal> { 0.1m, 0.2m, 0.3m }; // Default margins
    if (!string.IsNullOrEmpty(margins))
    {
        var marginStrings = margins.Split(',');
        marginList = marginStrings
            .Select(m => decimal.TryParse(m.Trim(), out var margin) ? margin : 0)
            .ToList();
    }

    var prices = marginList.Select(margin => new
    {
        Margin = margin,
        Price = totalCost * (1 + margin)
    }).ToList();

    return Results.Ok(new
    {
        CakeId = cake.Id,
        CakeName = cake.Name,
        TotalCost = totalCost,
        Prices = prices
    });
});

// Health endpoint
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }));

// CakeTypes CRUD
app.MapGet("/api/caketypes", async (CakeDbContext db) =>
    await db.CakeTypes.Where(ct => ct.IsActive).OrderBy(ct => ct.SortOrder).ToListAsync());

app.MapGet("/api/caketypes/{id}", async (int id, CakeDbContext db) =>
    await db.CakeTypes.FindAsync(id) is CakeType cakeType
        ? Results.Ok(cakeType)
        : Results.NotFound());

app.MapPost("/api/caketypes", async (CakeType cakeType, CakeDbContext db) =>
{
    db.CakeTypes.Add(cakeType);
    await db.SaveChangesAsync();
    return Results.Created($"/api/caketypes/{cakeType.Id}", cakeType);
});

// CakeShapes CRUD
app.MapGet("/api/cakeshapes", async (CakeDbContext db) =>
    await db.CakeShapes.Where(cs => cs.IsActive).OrderBy(cs => cs.SortOrder).ToListAsync());

app.MapGet("/api/cakeshapes/{id}", async (int id, CakeDbContext db) =>
    await db.CakeShapes.FindAsync(id) is CakeShape cakeShape
        ? Results.Ok(cakeShape)
        : Results.NotFound());

app.MapPost("/api/cakeshapes", async (CakeShape cakeShape, CakeDbContext db) =>
{
    db.CakeShapes.Add(cakeShape);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cakeshapes/{cakeShape.Id}", cakeShape);
});

// CakeSizes CRUD
app.MapGet("/api/cakesizes", async (int? shapeId, CakeDbContext db) =>
{
    var query = db.CakeSizes.Where(cs => cs.IsActive);
    if (shapeId.HasValue)
    {
        query = query.Where(cs => cs.ShapeId == shapeId.Value);
    }
    return await query.OrderBy(cs => cs.SortOrder).ToListAsync();
});

app.MapGet("/api/cakesizes/{id}", async (int id, CakeDbContext db) =>
    await db.CakeSizes.FindAsync(id) is CakeSize cakeSize
        ? Results.Ok(cakeSize)
        : Results.NotFound());

app.MapPost("/api/cakesizes", async (CakeSize cakeSize, CakeDbContext db) =>
{
    db.CakeSizes.Add(cakeSize);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cakesizes/{cakeSize.Id}", cakeSize);
});

// Fillings CRUD
app.MapGet("/api/fillings", async (CakeDbContext db) =>
    await db.Fillings.Where(f => f.IsActive).OrderBy(f => f.SortOrder).ToListAsync());

app.MapGet("/api/fillings/{id}", async (int id, CakeDbContext db) =>
    await db.Fillings.FindAsync(id) is Filling filling
        ? Results.Ok(filling)
        : Results.NotFound());

app.MapPost("/api/fillings", async (Filling filling, CakeDbContext db) =>
{
    db.Fillings.Add(filling);
    await db.SaveChangesAsync();
    return Results.Created($"/api/fillings/{filling.Id}", filling);
});

// Frostings CRUD
app.MapGet("/api/frostings", async (CakeDbContext db) =>
    await db.Frostings.Where(f => f.IsActive).OrderBy(f => f.SortOrder).ToListAsync());

app.MapGet("/api/frostings/{id}", async (int id, CakeDbContext db) =>
    await db.Frostings.FindAsync(id) is Frosting frosting
        ? Results.Ok(frosting)
        : Results.NotFound());

app.MapPost("/api/frostings", async (Frosting frosting, CakeDbContext db) =>
{
    db.Frostings.Add(frosting);
    await db.SaveChangesAsync();
    return Results.Created($"/api/frostings/{frosting.Id}", frosting);
});

// Roles CRUD
app.MapGet("/api/roles", async (CakeDbContext db) =>
    await db.Roles.ToListAsync());

app.MapGet("/api/roles/{id}", async (int id, CakeDbContext db) =>
    await db.Roles.FindAsync(id) is Role role
        ? Results.Ok(role)
        : Results.NotFound());

app.MapPost("/api/roles", async (Role role, CakeDbContext db) =>
{
    var validationError = ValidateRole(role.Name, role.HourlyRate, out var trimmedName);
    if (validationError != null) return validationError;

    role.Name = trimmedName!;
    db.Roles.Add(role);
    await db.SaveChangesAsync();
    return Results.Created($"/api/roles/{role.Id}", role);
});

app.MapPut("/api/roles/{id}", async (int id, Role inputRole, CakeDbContext db) =>
{
    var role = await db.Roles.FindAsync(id);
    if (role is null) return Results.NotFound();

    var validationError = ValidateRole(inputRole.Name, inputRole.HourlyRate, out var trimmedName);
    if (validationError != null) return validationError;

    role.Name = trimmedName!;
    role.HourlyRate = inputRole.HourlyRate;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/roles/{id}", async (int id, CakeDbContext db) =>
{
    if (await db.Roles.FindAsync(id) is Role role)
    {
        db.Roles.Remove(role);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Pricing constants - these would ideally come from configuration or database
// Cost per square inch of cake base
const decimal CostPerSquareInch = 0.50m;
// Cost per square inch for filling layer
const decimal FillingCostPerSquareInch = 0.15m;
// Cost per square inch for frosting
const decimal FrostingCostPerSquareInch = 0.20m;
// Base labor cost in USD
const decimal BaseLaborCost = 20m;
// Labor cost per square inch
const decimal LaborCostPerSquareInch = 0.10m;
// Labor cost per layer
const decimal LaborCostPerLayer = 5m;
// Overhead percentage (as decimal: 0.30 = 30%)
const decimal OverheadPercentage = 0.30m;

// Pricing Preview endpoint
app.MapPost("/api/pricing/preview", async (PricingPreviewRequest request, CakeDbContext db, ILogger<Program> logger) =>
{
    try
    {
        // Base cost calculation (simplified for now - would use actual ingredient costs in production)
        decimal ingredientsCost = 0;
        decimal laborCost = 0;
        decimal overheadCost = 0;

        // Calculate based on size (using rough estimates)
        decimal cakeArea = 0;
        if (!string.IsNullOrEmpty(request.SizeId) && int.TryParse(request.SizeId, out int sizeId))
        {
            var cakeSize = await db.CakeSizes.FindAsync(sizeId);
            if (cakeSize != null && !string.IsNullOrEmpty(cakeSize.Dimensions))
            {
                try
                {
                    var dims = JsonSerializer.Deserialize<Dictionary<string, decimal>>(cakeSize.Dimensions);
                    if (dims != null)
                    {
                        if (dims.ContainsKey("roundDiameterIn"))
                        {
                            var diameter = dims["roundDiameterIn"];
                            var radius = (double)(diameter / 2);
                            cakeArea = (decimal)(Math.PI * radius * radius);
                        }
                        else if (dims.ContainsKey("lengthIn") && dims.ContainsKey("widthIn"))
                        {
                            cakeArea = dims["lengthIn"] * dims["widthIn"];
                        }
                    }
                }
                catch (JsonException ex)
                {
                    logger.LogWarning(ex, "Failed to parse dimensions for size {SizeId}", sizeId);
                }
            }
        }
        else if (request.CustomSize != null)
        {
            if (request.CustomSize.DiameterIn.HasValue)
            {
                var diameter = request.CustomSize.DiameterIn.Value;
                var radius = (double)(diameter / 2);
                cakeArea = (decimal)(Math.PI * radius * radius);
            }
            else if (request.CustomSize.LengthIn.HasValue && request.CustomSize.WidthIn.HasValue)
            {
                cakeArea = request.CustomSize.LengthIn.Value * request.CustomSize.WidthIn.Value;
            }
        }

        // Calculate ingredient costs based on cake area
        ingredientsCost = cakeArea * CostPerSquareInch * request.Layers;

        // Add cost for filling (if layers > 1)
        if (request.Layers > 1 && !string.IsNullOrEmpty(request.FillingId))
        {
            ingredientsCost += cakeArea * FillingCostPerSquareInch * (request.Layers - 1);
        }

        // Add cost for frosting
        if (!string.IsNullOrEmpty(request.FrostingId))
        {
            ingredientsCost += cakeArea * FrostingCostPerSquareInch;
        }

        // Labor: base cost + area complexity + layer complexity
        laborCost = BaseLaborCost + (cakeArea * LaborCostPerSquareInch) + (request.Layers * LaborCostPerLayer);

        // Overhead: percentage of ingredients and labor
        overheadCost = (ingredientsCost + laborCost) * OverheadPercentage;

        var response = new PricingPreviewResponse
        {
            CostBreakdown = new CostBreakdown
            {
                Ingredients = Math.Round(ingredientsCost, 2),
                Labor = Math.Round(laborCost, 2),
                Overhead = Math.Round(overheadCost, 2)
            },
            TotalCost = Math.Round(ingredientsCost + laborCost + overheadCost, 2),
            Currency = "USD"
        };

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error calculating pricing preview");
        return Results.Problem("Failed to calculate pricing", statusCode: 500);
    }
});

app.Run();

// Make Program accessible for testing
public partial class Program { }

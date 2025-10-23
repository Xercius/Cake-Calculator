using Microsoft.EntityFrameworkCore;
using CakeCalculatorApi.Data;
using CakeCalculatorApi.Models;
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

// Ingredients CRUD
app.MapGet("/api/ingredients", async (CakeDbContext db) =>
    await db.Ingredients.ToListAsync());

app.MapGet("/api/ingredients/{id}", async (int id, CakeDbContext db) =>
    await db.Ingredients.FindAsync(id) is Ingredient ingredient
        ? Results.Ok(ingredient)
        : Results.NotFound());

app.MapPost("/api/ingredients", async (Ingredient ingredient, CakeDbContext db) =>
{
    db.Ingredients.Add(ingredient);
    await db.SaveChangesAsync();
    return Results.Created($"/api/ingredients/{ingredient.Id}", ingredient);
});

app.MapPut("/api/ingredients/{id}", async (int id, Ingredient inputIngredient, CakeDbContext db) =>
{
    var ingredient = await db.Ingredients.FindAsync(id);
    if (ingredient is null) return Results.NotFound();

    ingredient.Name = inputIngredient.Name;
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
    db.Templates.Add(template);
    await db.SaveChangesAsync();
    return Results.Created($"/api/templates/{template.Id}", template);
});

app.MapPut("/api/templates/{id}", async (int id, Template inputTemplate, CakeDbContext db) =>
{
    var template = await db.Templates.FindAsync(id);
    if (template is null) return Results.NotFound();

    template.Name = inputTemplate.Name;
    template.Size = inputTemplate.Size;
    template.Type = inputTemplate.Type;
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
app.MapGet("/api/pricing/{id}", async (int id, string? margins, CakeDbContext db) =>
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
        catch { }
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
        catch { }
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

app.Run();

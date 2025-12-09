using Microsoft.EntityFrameworkCore;
using CakeCalculatorApi.Models;

namespace CakeCalculatorApi.Data;

public class CakeDbContext : DbContext
{
    public CakeDbContext(DbContextOptions<CakeDbContext> options) : base(options)
    {
    }

    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Cake> Cakes { get; set; }
    public DbSet<CakeType> CakeTypes { get; set; }
    public DbSet<CakeShape> CakeShapes { get; set; }
    public DbSet<CakeSize> CakeSizes { get; set; }
    public DbSet<Filling> Fillings { get; set; }
    public DbSet<Frosting> Frostings { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.CostPerUnit).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Size).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.BaseIngredients).IsRequired();
        });

        modelBuilder.Entity<Cake>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Labor).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OtherCosts).HasColumnType("decimal(18,2)");
            
            entity.HasOne(e => e.Template)
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CakeType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<CakeShape>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<CakeSize>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            
            entity.HasOne(e => e.Shape)
                .WithMany()
                .HasForeignKey(e => e.ShapeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Filling>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Frosting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed CakeTypes
        modelBuilder.Entity<CakeType>().HasData(
            new CakeType { Id = 1, Name = "Vanilla", SortOrder = 1, IsActive = true },
            new CakeType { Id = 2, Name = "Chocolate", SortOrder = 2, IsActive = true },
            new CakeType { Id = 3, Name = "Red Velvet", SortOrder = 3, IsActive = true },
            new CakeType { Id = 4, Name = "Lemon", SortOrder = 4, IsActive = true },
            new CakeType { Id = 5, Name = "Carrot", SortOrder = 5, IsActive = true }
        );

        // Seed CakeShapes
        modelBuilder.Entity<CakeShape>().HasData(
            new CakeShape { Id = 1, Name = "Round", SortOrder = 1, IsActive = true },
            new CakeShape { Id = 2, Name = "Square", SortOrder = 2, IsActive = true },
            new CakeShape { Id = 3, Name = "Rectangle", SortOrder = 3, IsActive = true }
        );

        // Seed CakeSizes
        modelBuilder.Entity<CakeSize>().HasData(
            new CakeSize { Id = 1, ShapeId = 1, Name = "6\" Round", Dimensions = "{\"roundDiameterIn\":6}", SortOrder = 1, IsActive = true },
            new CakeSize { Id = 2, ShapeId = 1, Name = "8\" Round", Dimensions = "{\"roundDiameterIn\":8}", SortOrder = 2, IsActive = true },
            new CakeSize { Id = 3, ShapeId = 1, Name = "10\" Round", Dimensions = "{\"roundDiameterIn\":10}", SortOrder = 3, IsActive = true },
            new CakeSize { Id = 4, ShapeId = 2, Name = "8\" Square", Dimensions = "{\"lengthIn\":8,\"widthIn\":8}", SortOrder = 4, IsActive = true },
            new CakeSize { Id = 5, ShapeId = 3, Name = "9x13\" Sheet", Dimensions = "{\"lengthIn\":13,\"widthIn\":9}", SortOrder = 5, IsActive = true }
        );

        // Seed Fillings
        modelBuilder.Entity<Filling>().HasData(
            new Filling { Id = 1, Name = "Buttercream", SortOrder = 1, IsActive = true },
            new Filling { Id = 2, Name = "Chocolate Ganache", SortOrder = 2, IsActive = true },
            new Filling { Id = 3, Name = "Raspberry", SortOrder = 3, IsActive = true },
            new Filling { Id = 4, Name = "Lemon Curd", SortOrder = 4, IsActive = true },
            new Filling { Id = 5, Name = "Cream Cheese", SortOrder = 5, IsActive = true }
        );

        // Seed Frostings
        modelBuilder.Entity<Frosting>().HasData(
            new Frosting { Id = 1, Name = "Buttercream", SortOrder = 1, IsActive = true },
            new Frosting { Id = 2, Name = "Cream Cheese", SortOrder = 2, IsActive = true },
            new Frosting { Id = 3, Name = "Chocolate", SortOrder = 3, IsActive = true },
            new Frosting { Id = 4, Name = "Fondant", SortOrder = 4, IsActive = true },
            new Frosting { Id = 5, Name = "Whipped Cream", SortOrder = 5, IsActive = true }
        );

        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Baker", HourlyRate = 25.00m },
            new Role { Id = 2, Name = "Decorator", HourlyRate = 30.00m },
            new Role { Id = 3, Name = "Delivery", HourlyRate = 20.00m }
        );
    }
}

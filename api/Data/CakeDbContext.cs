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
    }
}

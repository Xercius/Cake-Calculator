namespace CakeCalculatorApi.Models;

public class Cake
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int TemplateId { get; set; }
    public Template? Template { get; set; }
    public string? ExtraIngredients { get; set; } // JSON string of additional ingredient IDs and quantities
    public decimal Labor { get; set; }
    public decimal OtherCosts { get; set; }
}

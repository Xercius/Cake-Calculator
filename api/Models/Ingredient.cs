namespace CakeCalculatorApi.Models;

public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal CostPerUnit { get; set; }
}

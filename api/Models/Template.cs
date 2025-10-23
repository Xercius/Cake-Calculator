namespace CakeCalculatorApi.Models;

public class Template
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Size { get; set; }
    public required string Type { get; set; }
    public required string BaseIngredients { get; set; } // JSON string of ingredient IDs and quantities
}

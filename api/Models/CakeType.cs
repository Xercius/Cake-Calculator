namespace CakeCalculatorApi.Models;

public class CakeType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ImagePath { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

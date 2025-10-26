namespace CakeCalculatorApi.Models;

public class CakeSize
{
    public int Id { get; set; }
    public int ShapeId { get; set; }
    public CakeShape? Shape { get; set; }
    public required string Name { get; set; }
    public string? Dimensions { get; set; } // JSON string: { "roundDiameterIn": 10 } or { "lengthIn": 12, "widthIn": 9 }
    public string? ImagePath { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

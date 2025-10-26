namespace CakeCalculatorApi.DTOs;

public class CustomSizeDto
{
    public decimal? DiameterIn { get; set; }
    public decimal? LengthIn { get; set; }
    public decimal? WidthIn { get; set; }
}

public class PricingPreviewRequest
{
    public string? TypeId { get; set; }
    public string? ShapeId { get; set; }
    public string? SizeId { get; set; }
    public CustomSizeDto? CustomSize { get; set; }
    public int Layers { get; set; } = 1;
    public string? FillingId { get; set; }
    public string? FrostingId { get; set; }
}

public class CostBreakdown
{
    public decimal Ingredients { get; set; }
    public decimal Labor { get; set; }
    public decimal Overhead { get; set; }
}

public class PricingPreviewResponse
{
    public CostBreakdown CostBreakdown { get; set; } = new();
    public decimal TotalCost { get; set; }
    public string Currency { get; set; } = "USD";
}

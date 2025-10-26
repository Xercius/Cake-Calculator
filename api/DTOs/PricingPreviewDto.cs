namespace CakeCalculatorApi.DTOs;

/// <summary>
/// Custom dimensions for a cake when using non-standard sizes.
/// </summary>
public class CustomSizeDto
{
    /// <summary>
    /// Diameter in inches for round cakes.
    /// </summary>
    public decimal? DiameterIn { get; set; }
    
    /// <summary>
    /// Length in inches for rectangular or square cakes.
    /// </summary>
    public decimal? LengthIn { get; set; }
    
    /// <summary>
    /// Width in inches for rectangular or square cakes.
    /// </summary>
    public decimal? WidthIn { get; set; }
}

/// <summary>
/// Request payload for calculating a cake cost preview.
/// Contains all cake configuration selections used to estimate the total cost.
/// At least one of SizeId or CustomSize must be provided along with required fields.
/// </summary>
public class PricingPreviewRequest
{
    /// <summary>
    /// ID of the selected cake type (e.g., Vanilla, Chocolate).
    /// </summary>
    public string? TypeId { get; set; }
    
    /// <summary>
    /// ID of the selected cake shape (e.g., Round, Square, Rectangle).
    /// </summary>
    public string? ShapeId { get; set; }
    
    /// <summary>
    /// ID of a preset cake size. Mutually exclusive with CustomSize.
    /// </summary>
    public string? SizeId { get; set; }
    
    /// <summary>
    /// Custom cake dimensions. Mutually exclusive with SizeId.
    /// </summary>
    public CustomSizeDto? CustomSize { get; set; }
    
    /// <summary>
    /// Number of layers in the cake. Defaults to 1.
    /// </summary>
    public int Layers { get; set; } = 1;
    
    /// <summary>
    /// ID of the selected filling. Required when Layers > 1.
    /// </summary>
    public string? FillingId { get; set; }
    
    /// <summary>
    /// ID of the selected frosting. Required field.
    /// </summary>
    public string? FrostingId { get; set; }
}

/// <summary>
/// Breakdown of cost components for a cake.
/// </summary>
public class CostBreakdown
{
    /// <summary>
    /// Cost of all ingredients in USD.
    /// </summary>
    public decimal Ingredients { get; set; }
    
    /// <summary>
    /// Labor cost in USD based on time and complexity.
    /// </summary>
    public decimal Labor { get; set; }
    
    /// <summary>
    /// Overhead costs in USD (utilities, packaging, etc.).
    /// </summary>
    public decimal Overhead { get; set; }
}

/// <summary>
/// Response containing the cost preview for a configured cake.
/// Does not include profit margins - this is the base cost only.
/// </summary>
public class PricingPreviewResponse
{
    /// <summary>
    /// Detailed breakdown of cost components.
    /// </summary>
    public CostBreakdown CostBreakdown { get; set; } = new();
    
    /// <summary>
    /// Total base cost (sum of all breakdown components) in the specified currency.
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// Currency code for all cost values. Defaults to "USD".
    /// </summary>
    public string Currency { get; set; } = "USD";
}

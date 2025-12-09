namespace CakeCalculatorApi.Models;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal HourlyRate { get; set; }
}

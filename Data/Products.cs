namespace Logistica.API.Data;

public class Products
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
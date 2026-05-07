namespace Logistica.API.Data;

public class Orders
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }

    public virtual Products? Product { get; set; }
}
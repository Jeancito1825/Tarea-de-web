using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logistica.API.Data;

namespace Logistica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly LogisticaDbContext _context;

    public OrdersController(LogisticaDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.Product)
            .ToListAsync();
    }

    // GET: api/Orders/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Orders>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();
        return Ok(order);
    }


    [HttpGet("byproduct/{productId}")]
    public async Task<ActionResult<IEnumerable<Orders>>> GetByProduct(int productId)
    {
        var orders = await _context.Orders
            .Include(o => o.Product)
            .Where(o => o.ProductId == productId)
            .ToListAsync();

        return Ok(orders);
    }


    [HttpPost]
    public async Task<ActionResult<Orders>> PostOrder(Orders order)
    {
        var product = await _context.Products.FindAsync(order.ProductId);
        if (product == null)
            return BadRequest(new { message = "El producto no existe." });

        order.TotalPrice = product.UnitPrice * order.Quantity;
        order.OrderDate  = DateTime.Now;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
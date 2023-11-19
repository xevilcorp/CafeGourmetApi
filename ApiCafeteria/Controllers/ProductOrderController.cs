using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class ProductOrderController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public ProductOrderController(ApplicationDbContext context)
  {
    _context = context;
  }

  [HttpPost]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<ProductOrder>> CreateProductOrder(ProductOrder productOrder)
  {
    var userId = User.FindFirstValue(ClaimTypes.Name);
    productOrder.UserId = userId;

    _context.ProductOrders.Add(productOrder);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetProductOrder), new { id = productOrder.Id }, productOrder);
  }

  [HttpPatch("{id}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<IActionResult> UpdateProductOrderStatus(int id, int status)
  {
    if (!User.IsInRole("Admin"))
    return Unauthorized();

    var productOrder = await _context.ProductOrders.FindAsync(id);

    if (productOrder == null)
    return NotFound();

    productOrder.Status = (OrderStatus)status;

    try
    {
      await _context.SaveChangesAsync();
      return Ok();
    }
    catch (Exception)
    {
      return NoContent();
    }
  }


  [HttpGet]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<IEnumerable<ProductOrder>>> GetProductOrders(bool includeAll = false)
  {
    var userId = User.FindFirstValue(ClaimTypes.Name);

    if (User.IsInRole("Admin") && includeAll)
    {
      return await _context.ProductOrders
        .Include(o => o.Address)
        .Include(o => o.OrderItems)
        .ThenInclude(i => i.Product)
        .Include(o => o.User)
        .OrderByDescending(p => p.Id)
        .ToListAsync();
    }
    else
    {
      return await _context.ProductOrders
        .Include(o => o.Address)
        .Include(o => o.OrderItems)
        .ThenInclude(i => i.Product)
        .Include(o => o.User)
        .Where(p => p.UserId == userId)
        .OrderByDescending(p => p.Id)
        .ToListAsync();
    }
  }

  [HttpGet("{id}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<ProductOrder>> GetProductOrder(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.Name);
    var productOrder = await _context.ProductOrders
    .Include(o => o.Address)
    .Include(o => o.OrderItems)
    .ThenInclude(i => i.Product)
    .Include(o => o.User)
    .Where(o => o.Id == id).FirstOrDefaultAsync();

    if (productOrder.UserId != userId.ToString() && !User.IsInRole("Admin"))
    {
      return Unauthorized();
    };

    if (productOrder == null)
    {
      return NotFound();
    }

    return productOrder;
  }
}
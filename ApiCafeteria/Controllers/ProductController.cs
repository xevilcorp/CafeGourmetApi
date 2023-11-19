using System.Security.Claims;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public ProductController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
      var products = await _context.Products.Where(p => !p.Title.Contains("[REMOVED]")).OrderByDescending(o => o.Id).ToListAsync();
      return products;
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      if (!User.IsInRole("Admin"))
      {
        return Unauthorized();
      }

      var product = await _context.Products.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      product.Title += " [REMOVED]";
      await _context.SaveChangesAsync();

      return Ok();
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromForm]Product product, [FromForm]IFormFile image)
    {
        if (!User.IsInRole("Admin"))
        {
          return Unauthorized();
        }
        
        var connectionString = _configuration.GetConnectionString("BlobStorage");
        var containerName = "product-images"; 
        var blobName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        var blobClient = new BlobClient(connectionString, containerName, blobName);

        await using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        stream.Position = 0;
        await blobClient.UploadAsync(stream, overwrite: true);

        product.ImageUrl = blobClient.Uri.AbsoluteUri;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

}
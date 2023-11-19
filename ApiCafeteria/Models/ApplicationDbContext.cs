using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Address> Addresses { get; set; }
  public DbSet<Transaction> Transactions { get; set; }
  public DbSet<ProductOrder> ProductOrders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }



  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
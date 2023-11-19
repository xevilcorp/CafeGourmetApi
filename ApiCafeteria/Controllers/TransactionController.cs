using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public TransactionController(ApplicationDbContext context)
  {
    _context = context;
  }
  
  [HttpPost("{orderId}")]
  public async Task<ActionResult> CaptureTransaction(int orderId)
  {
    var order = await _context.ProductOrders.FindAsync(orderId);

    if (order == null)
    {
      return NotFound();
    }

    var transaction = order.Transaction;

    if (transaction == null)
    {
      return BadRequest("No transaction associated with this order.");
    }

    var paymentData = new
    {
      MerchantOrderId = "2014111701",
      Customer = new
      {
        Name = "Comprador crédito completo",
        // ... other customer properties ...
      },
      Payment = new
      {
        Type = "CreditCard",
        Amount = (int)transaction.Amount * 100, // Cielo uses cents
                                                // ... other payment properties ...
        CreditCard = new
        {
          CardNumber = "1234123412341234", // Replace with actual card number
          Holder = "Test Holder",
          ExpirationDate = "12/2030", // Replace with actual expiration date
          SecurityCode = "123", // Replace with actual security code
          Brand = "Visa" // Replace with actual brand
        }
      }
    };

    var httpClient = new HttpClient();
    var content = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8, "application/json");
    var response = await httpClient.PostAsync("https://api.cieloecommerce.cielo.com.br/1/sales/", content);

    if (response.IsSuccessStatusCode)
    {
      var responseContent = await response.Content.ReadAsStringAsync();
      var paymentResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

      if (paymentResponse.Payment.Status == 1) // 1 = Payment authorized
      {
        transaction.Status = TransactionStatus.Succeeded;
        _context.Update(transaction);
        await _context.SaveChangesAsync();
      }
      else
      {
        transaction.Status = TransactionStatus.Failed;
        _context.Update(transaction);
        await _context.SaveChangesAsync();
      }

      return Ok(paymentResponse);
    }
    else
    {
      return BadRequest("Failed to capture transaction.");
    }
  }
}
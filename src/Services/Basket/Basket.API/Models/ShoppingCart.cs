namespace Basket.API.Models;
public class ShoppingCart
{
  public string UserName { get; set; }
  public List<ShoppingCarItem> Items { get; set; }
  public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
  public ShoppingCart(string userName)
  {
    UserName = userName;
  }

  //required for mapping
  public ShoppingCart()
  {
        
  }
}
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infraestructure.Data.Extentions;
internal class InitialData
{
  public static IEnumerable<Customer> Customers =>
    new List<Customer>()
    {
      Customer.Create(CustomerId.Of(new Guid("fc359c8e-371e-4ecd-bdd2-90cd5d9be23e")), "Gio", "ariza@gmail.com"),
      Customer.Create(CustomerId.Of(new Guid("4aaf0959-138e-4003-8f62-fe2f25aa165d")), "Mehmet", "ozkaya@gmail.com")
    };

  public static IEnumerable<Product> Products =>
    new List<Product>()
    {
      Product.Create(ProductId.Of(new Guid("f35511ed-4137-4d03-b4a5-648b8347e1a4")), "Iphone X", 500),
      Product.Create(ProductId.Of(new Guid("7f273b3d-f6d3-4fcc-873a-7baef3952467")), "Samsung 10", 400),
      Product.Create(ProductId.Of(new Guid("1d381245-692d-4dee-8237-2bb03f03cc8a")), "Huawei Plus", 650),
      Product.Create(ProductId.Of(new Guid("5719dab1-50cd-4c82-b79c-5483a35fcb7b")), "Xiami Mi", 450)
    };

  public static IEnumerable<Order> Orders
  {
    get
    {
      var address1 = Address.Of("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
      var address2 = Address.Of("Gio", "Ariza", "ariza@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

      var payment1 = Payment.Of("mehmet", "5555555555554444", "12/28", "355", 1);
      var payment2 = Payment.Of("Gio", "8885555555554444", "06/30", "222", 2);

      var order1 = Order.Create(
                      OrderId.Of(Guid.NewGuid()),
                      CustomerId.Of(new Guid("4aaf0959-138e-4003-8f62-fe2f25aa165d")),
                      OrderName.Of("ORD_1"),
                      shippingAddress: address1,
                      billingAddress: address1,
                      payment1);
      order1.Add(ProductId.Of(new Guid("f35511ed-4137-4d03-b4a5-648b8347e1a4")), 2, 500);
      order1.Add(ProductId.Of(new Guid("7f273b3d-f6d3-4fcc-873a-7baef3952467")), 1, 400);

      var order2 = Order.Create(
                      OrderId.Of(Guid.NewGuid()),
                      CustomerId.Of(new Guid("fc359c8e-371e-4ecd-bdd2-90cd5d9be23e")),
                      OrderName.Of("ORD_2"),
                      shippingAddress: address2,
                      billingAddress: address2,
                      payment2);
      order2.Add(ProductId.Of(new Guid("1d381245-692d-4dee-8237-2bb03f03cc8a")), 1, 650);
      order2.Add(ProductId.Of(new Guid("5719dab1-50cd-4c82-b79c-5483a35fcb7b")), 2, 450);

      return new List<Order> { order1, order2 };
    }
  }
}

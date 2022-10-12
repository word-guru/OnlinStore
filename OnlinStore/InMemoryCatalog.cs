using System.Collections.Concurrent;
using OnlinStore.Interface;
using OnlinStore.Models;

namespace OnlinStore;

public class InMemoryCatalog : ICatalog
{
    private readonly IClock _clock;
    private ConcurrentBag<Product> Products { get; set; } = new()
    {
        new Product(1, "Чистый код", 123),
        new Product(2, "Lift", 231),
        new Product(3, "Maus", 312)
    };

    public InMemoryCatalog(IClock clock)
    {
        _clock = clock;
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
    public void ClearProduct()
    {
            Products.Clear();
    }

    public IReadOnlyList<Product> GetProducts()
    {
       
        if (_clock.GetUTCTime().DayOfWeek == DayOfWeek.Monday)
        {
            return Products.Select(product => new Product(product.Id,product.Name,product.Price * 1.5m)).ToList();
        }
            return Products.ToList(); //копирование
    }
}
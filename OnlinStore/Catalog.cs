using System.Collections.Concurrent;
using OnlinStore.Models;

namespace OnlinStore;

public class Catalog
{
    private ConcurrentBag<Product> Products { get; set; } = new()
    {
        new Product(1, "Чистый код", 123),
        new Product(2, "Lift", 231),
        new Product(3, "Maus", 312)
    };
    private readonly object _syncObj = new();

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
            return Products.ToList(); //копирование
    }
}
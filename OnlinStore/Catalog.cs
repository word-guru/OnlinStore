using OnlinStore.Models;

namespace OnlinStore;

public class Catalog
{
    private List<Product> Products { get; set; } = new()
    {
        new Product(1, "Чистый код", 123),
        new Product(2, "Lift", 231),
        new Product(3, "Maus", 312)
    };
    private readonly object _syncObj = new();

    public void AddProduct(Product product)
    {
        lock (_syncObj)
        {
            Products.Add(product);
        }
    }
    public void ClearProduct()
    {
        lock (_syncObj)
        {
            Products.Clear();
        }
    }

    public List<Product> GetProducts()
    {
        lock (_syncObj)
        {
            return Products;
        }
    }
}
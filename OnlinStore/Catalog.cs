using OnlinStore.Models;

namespace OnlinStore;

public class Catalog
{
    public List<Product> Products { get; set; } = new()
    {
        new Product(1, "Чистый код", 123),
        new Product(2, "Lift", 231),
        new Product(3, "Maus", 312)
    };

}
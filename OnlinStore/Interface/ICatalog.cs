using OnlinStore.Models;

namespace OnlinStore.Interface;

public interface ICatalog
{
    public void AddProduct(Product product);
    public void ClearProduct();
    public IReadOnlyList<Product> GetProducts();
}
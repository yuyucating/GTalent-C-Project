using InventorySystem.Models;
namespace InventorySystem.Repositories;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    
    int GetNextProductID();
    void UpdateProduct(Product product);
    List<Product> CheckOutOfStockProducts();
}
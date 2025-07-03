using InventorySystem.Models;
namespace InventorySystem.Repositories;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(string name, decimal price, int quantity);
}
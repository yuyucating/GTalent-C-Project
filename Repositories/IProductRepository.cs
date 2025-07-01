using InventorySystem.Models;
namespace InventorySystem.Repositories;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
}
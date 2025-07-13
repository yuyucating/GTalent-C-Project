using InventorySystem.Models;
using InventorySystem.Utils;

namespace InventorySystem.Services;

using InventorySystem.Repositories;

public class InventoryService
{
    private readonly IProductRepository _productRepository;

    // 透過建構子 注入介面
    public InventoryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public List<Product> GetAllProducts()
    {
        try
        {
            List<Product> products = _productRepository.GetAllProducts(); //呼叫介面 (不是呼叫實作物件) - DI
            if (!products.Any())
            {
                Console.WriteLine("No products found!");
            }
            
            // //使用 EmailNotifier
            // INotifier emailNotifier = new EmailNitifier();
            // NotificationService emailService = new NotificationService(emailNotifier);
            // emailService.NotifyUser("Mickey", "Finish.");
            
            return products;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in reading all products:{e.Message}");
            return new List<Product>(); //回傳空 Product: 不會抱錯
        }
    }

    public Product GetProductByID(int id)
    {
        try
        {
            Product product = _productRepository.GetProductById(id);
            if (product == null)
            {
                Console.WriteLine("No products found!");
            }

            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in reading product with ID {id}:{e.Message}");
            return new Product();
        }
    }

    public void AddProduct(string? name, decimal price, int quantity)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required!");
            }

            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero!");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("Quantity can not be lower than zero!");
            }
            // 嘗試透過 Repo 處理 add product
            var product = new Product(_productRepository.GetNextProductID(), name, price, quantity); // 準備好 status
            _productRepository.AddProduct(product);
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError in adding product: {e.Message}");
        }
    }

    public void UpdateProduct(Product product, string? name, decimal price, int quantity)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required!");
            }
            if (price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero!");
            }
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity can not be lower than zero!");
            }
            // 嘗試透過 Repo 處理 UpdateProduct
            product.Name = name;
            product.Price = price;
            product.Quantity = quantity;
            _productRepository.UpdateProduct(product);
            Console.WriteLine($"Complete product with ID {product.Id} updating.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in updating product: {e.Message}");
        }
    }

    public List<Product> SearchProduct(string? input)
    {
        try
        {
            List<Product> products = _productRepository.GetAllProducts(); //呼叫介面 (不是呼叫實作物件) - DI
            if (string.IsNullOrWhiteSpace(input))
            {
                return products;
                // 如果是空白或是 null 就列出所有的 products
            }
            
            var results = products.Where(product => product.Name.ToLower().Contains(input.ToLower()))
                .OrderBy(product => product.Name)
                .ToList();
            
            if (!results.Any())
            {
                Console.WriteLine("No products found!");
            }
            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in reading all products:{e.Message}");
            return new List<Product>(); //回傳空 Product: 不會抱錯
        }
    }

    public List<Product> CheckOutOfStockProducts()
    {
        try
        {
            List<Product> OutOfStockProducts = _productRepository.GetAllProducts();
            
            var results = OutOfStockProducts.Where(product => product.Status == Product.ProductStatus.LowStock || product.Quantity < 10).ToList();
            
            if (!results.Any())
            {
                Console.WriteLine("No products found!");
            }
            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in checking low-stock product:{e.Message}");
            return new List<Product>();
        }
    }
}
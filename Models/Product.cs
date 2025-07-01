namespace InventorySystem.Models;

public class Product
{
    public enum ProductStatus
    {
        InStock, //有庫存
        LowStock,//庫存偏低
        OutOfStock//沒有庫存
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; } // enum 可以做為型別 (像是 static)

    //連接 SQL 的建構子
    public Product()
    {
    }
    
    //建構子
    public Product(int id, string name, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
        UpdateStatus();
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price}, Quantity: {Quantity}, Status: {Status}";
    }

    public void UpdateStatus()
    {
        if (Quantity <= 0)
        {
            Status = ProductStatus.OutOfStock;
        }
        else if (Quantity < 10)
        {
            Status = ProductStatus.LowStock;
        }
        else
        {
            Status = ProductStatus.InStock;
        }
    }
}
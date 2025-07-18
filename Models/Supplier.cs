namespace InventorySystem.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public Supplier(int id, string name, string address, string phone, string email)
    {
        Id = id;
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
    }
    
    // 空的建構子 for Database 套件使用
    public Supplier()
    {
    }

    public override string ToString()
    {
        return $"id: {Id}, name: {Name}, address: {Address}, phone: {Phone}, email: {Email}";
    }
}
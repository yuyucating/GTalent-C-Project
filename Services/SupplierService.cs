using InventorySystem.Models;
using InventorySystem.Repositories;

namespace InventorySystem.Services;

public class SupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public void AddSupplier(string name, string address, string phone, string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Supplier name cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Supplier address cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Supplier's phone cannot be null or empty");
            }

            if (phone.Length != 10)
            {
                throw new ArgumentException("Length of supplier's phone number should be 10");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Supplier's email cannot be null or empty");
            }
            
            if (!email.Contains("@"))
            {
                throw new ArgumentException("Supplier's email should contain '@'");
            }

            Supplier supplier = new Supplier(_supplierRepository.GetNextSupplierID(), name, address, phone, email);
            _supplierRepository.AddSupplier(supplier);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in adding supplier: {e.Message}");
        }
    }

    public List<Supplier> GetAllSuppliers()
    {
        try
        {
            List<Supplier> suppliers = _supplierRepository.GetAllSuppliers();
            if (!suppliers.Any()) // 如果沒有取到任何資料
            {
                Console.WriteLine("No supplier were found");
            }
            return suppliers;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in getting all suppliers: {e.Message}");
            return new List<Supplier>(); //回傳空的 List... 不報錯
        }
    }

    public List<Supplier> SearchSupplierByKeywords(string? input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<Supplier>();
            }

            List<Supplier> suppliers = _supplierRepository.SearchSupplierByKeywords(input);
            
            if (!suppliers.Any()) // 如果 List 裡面沒有東西的話
            {
                Console.WriteLine("No supplier were found!");
            }
            return suppliers;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in searching suppliers: {e.Message}");
            return new List<Supplier>();
        }
    }
}
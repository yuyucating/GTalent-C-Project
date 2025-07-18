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
}
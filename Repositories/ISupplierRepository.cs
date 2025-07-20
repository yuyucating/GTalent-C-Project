using InventorySystem.Models;

namespace InventorySystem.Repositories;

public interface ISupplierRepository
{
    List<Supplier> GetAllSuppliers();
    int GetNextSupplierID();
    void CreateSupplierTable();
    void AddSupplier(Supplier supplier);
    
    Supplier GetSupplierById(int id);
    void UpdateSupplier(Supplier supplier);
    void DeleteSupplier(Supplier supplier);
    void ExistSupplier(int id);
    List<Supplier> SearchSupplierByKeywords(string input);
}
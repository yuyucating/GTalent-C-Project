using InventorySystem.Models;
using MySql.Data.MySqlClient;

namespace InventorySystem.Repositories;

public class MySqlSupplierRepository:ISupplierRepository
{
    private readonly string _connectionString;
    public MySqlSupplierRepository(string connectionString)
    {
        _connectionString = connectionString; // 接收外界的資料
        CreateSupplierTable(); // new 物件出來的時候執行 funciton!!
    }

    public void CreateSupplierTable()
    {
        // using: 以防忘記關掉, 而占用資源
        using (var connection = new MySqlConnection(_connectionString)) // MySqlConnection 接收連接需要的物件
        {
            try
            {
                connection.Open(); //開啟連結 (進行連線)
                //建立 SQL 語句
                string createTableSql = @"
                    create table if not exists suppliers(
                        id INT PRIMARY KEY AUTO_INCREMENT,
                        name VARCHAR(50) NOT NULL,
                        address VARCHAR(50) NOT NULL,
                        phone VARCHAR(10) NOT NULL,
                        email VARCHAR(30) NOT NULL,
                        is_deleted BOOLEAN DEFAULT false
                    )
     
                ";
                using MySqlCommand cmd = new MySqlCommand(createTableSql, connection); // MySqlCommand 執行 SQL 語句(?)
                {
                    cmd.ExecuteNonQuery(); //執行
                }
                Console.WriteLine("[Suppliers] MySql 初始化成功或已存在");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"[Suppliers] 初始化 MySql 失敗: {e.Message}");
            }
        }
    }
    

    public void AddSupplier(Supplier supplier)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string insertSql = "INSERT INTO suppliers (name, address, phone, email) VALUES (@name, @address, @phone, @email)";
            using (MySqlCommand cmd = new MySqlCommand(insertSql, connection))
            {
                cmd.Parameters.AddWithValue("@name", supplier.Name);
                cmd.Parameters.AddWithValue("@address", supplier.Address);
                cmd.Parameters.AddWithValue("@phone", supplier.Phone);
                cmd.Parameters.AddWithValue("@email", supplier.Email);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<Supplier> GetAllSuppliers()
    {
        List<Supplier> suppliers = new List<Supplier>(); // 為了存放回傳資料
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string selectSql = "SELECT * FROM suppliers WHERE is_deleted = false";
            using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier(reader.GetInt32("id"),
                            reader.GetString("name"),
                            reader.GetString("address"),
                            reader.GetString("phone"),
                            reader.GetString("email")
                            ));
                    }
                }
            }
        }
        return suppliers;
    }
    
    public int GetNextSupplierID()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string selectSql = @"SELECT IFNULL(MAX(id), 0) FROM suppliers"; // 取得最大的 id
            using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
            {
                var result = cmd.ExecuteScalar(); // 取得第一欄的第一筆: MAX(id) 的結果中的第一欄第一筆(其實只有一筆)
                if (result != null)
                {
                    return Convert.ToInt32(result)+1; // 回傳新加入的 product 的 id
                }

                return 0;
            }
        }
        
    }

    public List<Supplier> SearchSupplierByKeywords(string input)
    {
        List<Supplier> suppliers = new List<Supplier>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string searchSql = "SELECT * FROM suppliers WHERE name LIKE @input";
            using (MySqlCommand cmd = new MySqlCommand(searchSql, connection))
            {
                // cmd.Parameters.AddWithValue("@input", input);
                cmd.Parameters.AddWithValue("@input", "%" + input + "%");
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suppliers.Add(new Supplier(reader.GetInt32("id"),
                            reader.GetString("name"),
                            reader.GetString("address"),
                            reader.GetString("phone"),
                            reader.GetString("email")));
                    }
                }
            }
        }
        return suppliers;
    }

    public Supplier GetSupplierById(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateSupplier(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public void DeleteSupplier(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public void ExistSupplier(int id)
    {
        throw new NotImplementedException();
    }
}
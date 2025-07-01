using InventorySystem.Models;
using MySql.Data.MySqlClient;

namespace InventorySystem.Repositories;

public class MySqlProductRepository : IProductRepository
{   
    // for 連線的物件!!!
    private readonly string _connectionString;
    public MySqlProductRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // using: 以防忘記關掉, 而占用資源
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open(); //開啟連線
                string createTableSql = @"
                    create table if not exists product(
                        id INT PRIMARY KEY AUTO_INCREMENT,
                        name VARCHAR(10) NOT NULL,
                        price DECIMAL(10,2) NOT NULL,
                        quantity INT NOT NULL,
                        status INT NOT NULL
                    )
     
                "; // @可以換行的字串 (???) , 可以跟 $"" 做比較???
                // cmd 命令: 把我們的字串(連線) 執行 connection
                using MySqlCommand cmd = new MySqlCommand(createTableSql, connection);
                {
                    cmd.ExecuteNonQuery(); //執行
                }
                Console.WriteLine("MySql 初始化成功或已存在");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"初始化 MySql 失敗: {e.Message}");
            }
        }
    }

    public List<Product> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public Product GetProductById(int id)
    {
        throw new NotImplementedException();
    }
}
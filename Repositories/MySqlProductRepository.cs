using InventorySystem.Models;
using MySql.Data.MySqlClient;

namespace InventorySystem.Repositories;

public class MySqlProductRepository : IProductRepository
{   
    // for 連線的物件!!!
    private readonly string _connectionString;
    public MySqlProductRepository(string connectionString)
    {
        _connectionString = connectionString; // 接收外界的資料
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // using: 以防忘記關掉, 而占用資源
        using (var connection = new MySqlConnection(_connectionString)) // MySqlConnection 接收連接需要的物件
        {
            try
            {
                connection.Open(); //開啟連結 (進行連線)
                //建立 SQL 語句
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
                using MySqlCommand cmd = new MySqlCommand(createTableSql, connection); // MySqlCommand 執行 SQL 語句(?)
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
        List<Product> products = new List<Product>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string selectSql = "SELECT * FROM product"; // 所有 product 這個 table 裡的資料 (很多個 product)
            using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())  // 他會取得很多筆, 自己會迭代, 所以要搭配 while!!
                {   
                    // 一筆資料一筆資料讀取 (列)
                    while (reader.Read())  // 因為不知道有幾筆資料, 所以用 while()
                    {
                        // 將獨到的資料丟掉 Product 裡面!! 再丟到 List 裡面收起來 [obj initializer]
                        products.Add(new Product(reader.GetInt32("id"), 
                            reader.GetString("name"), 
                            reader.GetDecimal("price"), 
                            reader.GetInt32("quantity"))
                        {
                            Status = (Product.ProductStatus)reader.GetInt32("status")
                        });
                        
                        // 傳統寫法:
                        // Product product = new Product(reader.GetInt32("id"),
                        //     reader.GetString("name"),
                        //     reader.GetDecimal("price"),
                        //     reader.GetInt32("quantity"));
                        // product.Status = (Product.ProductStatus)reader.GetInt32("status");
                        // products.Add(product);
                        
                    }
                }
            }
        }
        return products;
    }

    public Product GetProductById(int id)
    {
        Product product = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string selectSql = "SELECT * FROM product WHERE id = @id";
            // string selectSql = "SELECT * FROM product WHERE id = " + id;
            using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
            {
                // 防止 SQL Injection
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader()) // 他會取得很多筆, 自己會迭代, 所以要搭配 while!!
                {
                    if (reader.Read())
                    {
                        // obj initializer
                        product = new Product(reader.GetInt32("id"),
                            reader.GetString("name"),
                            reader.GetDecimal("price"),
                            reader.GetInt32("quantity"))
                        {
                            Status = (Product.ProductStatus)reader.GetInt32("status")
                        };
                    }
                }
            }
        }

        return product;
    }

    
    public void AddProduct(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string insertSql = "INSERT INTO product (name, price, quantity, status) VALUES (@name, @price, @quantity, @status)";
            using (MySqlCommand cmd = new MySqlCommand(insertSql, connection))
            {
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@status", product.Status);
                
                cmd.ExecuteNonQuery();
            }
        }
    }

    public int GetNextProductID()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string selectSql = @"SELECT IFNULL(MAX(id), 0) FROM product"; // 取得最大的 id
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

    public void UpdateProduct(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string updateSql = @"UPDATE product SET name=@name, price=@price, quantity=@quantity WHERE id=@id";
            using (MySqlCommand cmd = new MySqlCommand(updateSql, connection))
            {
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@id", product.Id);
                
                cmd.ExecuteNonQuery();
            }
        }
    }

    // public List<Product> CheckOutOfStockProducts()
    // {
    //     List<Product> products = new List<Product>();
    //     using (var connection = new MySqlConnection(_connectionString))
    //     {
    //         connection.Open();
    //         string selectSql = "SELECT * FROM product WHERE status=2"; // 所有 product 這個 table 裡的資料 (很多個 product)
    //         using (MySqlCommand cmd = new MySqlCommand(selectSql, connection))
    //         {
    //             using (MySqlDataReader reader = cmd.ExecuteReader())  // 他會取得很多筆, 自己會迭代, 所以要搭配 while!!
    //             {   
    //                 while (reader.Read())  // 因為不知道有幾筆資料, 所以用 while()
    //                 {
    //                     products.Add(new Product(reader.GetInt32("id"), 
    //                         reader.GetString("name"), 
    //                         reader.GetDecimal("price"), 
    //                         reader.GetInt32("quantity"))
    //                     {
    //                         Status = (Product.ProductStatus)reader.GetInt32("status")
    //                     });
    //                 }
    //             }
    //         }
    //     }
    //     return products;
    // }
}
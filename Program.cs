
using InventorySystem.Models;
using InventorySystem.Repositories;

/* Server: mysql所在伺服器位址(localhost or ip xxx.xxx.xxx.xxx);
 Port: mysql端口 (預設 3306);
 Database: SQL 以 CREATE DATABASE 的名字;
 uid: mysql使用者名稱;
 pwd: mysql使用者密碼*/
const string MYSQL_CONNECTION_STRING = "Server=localhost;Port=3306;Database=inventory_db;uid=root;pwd=plutomarsD1206";

MySqlProductRepository productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);

RunMenu();

void RunMenu()
{
 while (true)
 {
  DisplayMenu();
  string input = Console.ReadLine();// 準備把介面 new 出來
  switch (input)
  {
   case "1": GetAllProducts();
    break;
   case "2": SearchProduct();
    break;
   case "3": AddProduct();
    break;
   case "0":
    Console.WriteLine("Goodbye");
    return;
  }
 }
}

void DisplayMenu()
{
 Console.WriteLine("Welcome to Inventory System!");
 Console.WriteLine("What would you like to do?");
 Console.WriteLine("1. Get all product");
 Console.WriteLine("2. Search product");
 Console.WriteLine("3. Add product");
 Console.WriteLine("0. Exit");
}

void GetAllProducts()
{
 Console.WriteLine("\n=== All product list ===");
 var products = productRepository.GetAllProducts();
 if (products.Any()) // .Any() 去了解 products 有沒有元素 (而不是單純 products 存不存在)
 {
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  foreach (var product in products)
  {
   Console.WriteLine(product);
  }
  Console.WriteLine("---------------------------------------");
 }
}

void SearchProduct()
{
 Console.WriteLine("\n=== Key in the product ID ===");
 int input = ReadIntInput();
 // string s = Console.ReadLine();
 // int input = ReadInt(s);
 

 var product = productRepository.GetProductById(input);
 if (product!=null)
 {
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine(product);
  Console.WriteLine("---------------------------------------");
 }
}

void AddProduct(){
 Console.WriteLine("\nPlease key in product name:");
 string name = Console.ReadLine();
 Console.WriteLine("\nPlease key in product price:");
 decimal price = ReadDecimalInput();
 Console.WriteLine("\nPlease key in product quantity:");
 int quantity = ReadIntInput();
 Console.WriteLine($"Product name:{name}, Price:{price}, Quantity:{quantity}");
 productRepository.AddProduct(name, price, quantity);
}

// 確定輸入為數字格式
int ReadIntInput(int defaultValue = 0)
{
 while (true)
 {
  // Console.WriteLine(s);
  String input = Console.ReadLine();
  if (string.IsNullOrWhiteSpace(input) && defaultValue != 0)
  {
   return defaultValue;
  }

  // string parsing to int
  if (int.TryParse(input, out int value))
  {
   return value;
  }
  else
  {
   // 如果收到的 s 不是數字
   Console.WriteLine("Please enter a valid integer");
  }
 }
 
}

int ReadInt(string input)
{
 try
 {
  return int.Parse(input);
 }
 catch (Exception e)
 {
  // Console.WriteLine(e);
  Console.WriteLine("Please enter a valid integer");
  return 0;
 }
}

decimal ReadDecimalInput(decimal defaultValue = 0.0m)
{
 while (true)
 {
  // Console.WriteLine(s);
  String input = Console.ReadLine();
  if (string.IsNullOrWhiteSpace(input) && defaultValue != 0.0m)
  {
   return defaultValue;
  }

  // string parsing to int
  if (decimal.TryParse(input, out decimal value))
  {
   return value;
  }
  else
  {
   // 如果收到的 s 不是數字
   Console.WriteLine("Please enter a valid integer");
  }
 }
 
}
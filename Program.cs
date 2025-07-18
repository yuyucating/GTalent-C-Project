// namespace InventorySystem;

using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.Utils;
using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using ZstdSharp.Unsafe;

/* Server: mysql所在伺服器位址(localhost or ip xxx.xxx.xxx.xxx);
 Port: mysql端口 (預設 3306);
 Database: SQL 以 CREATE DATABASE 的名字;
 uid: mysql使用者名稱;
 pwd: mysql使用者密碼*/

string connectionString = "";
string configFile = "appsettings.ini";

if (File.Exists(configFile))
{
 Console.WriteLine($"Readding {configFile} file");
 try
 {
  Dictionary<string, Dictionary<string, string>> config = ReadFile(configFile);

  if (config.ContainsKey("Database"))
  {
   var dbConfig = config["Database"];
   connectionString = $"Server={dbConfig["Server"]};Port={dbConfig["Port"]};Database={dbConfig["Database"]};uid={dbConfig["uid"]};pwd={dbConfig["pwd"]}";
   Console.WriteLine($"Readding file finished!!");
  }
 }
 catch (Exception ex)
 {
  Console.WriteLine($"Error in reading file: {ex.Message}");
  // thorw;
  // connectionString = MYSQL_CONNECTION_STRING;
 }
}
else
{
 Console.WriteLine($"Error in reading file: {configFile} is not exists.");
}

// MySqlProductRepository productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);
MySqlProductRepository productRepository = new MySqlProductRepository(connectionString);
MySqlSupplierRepository supplierRepository = new MySqlSupplierRepository(connectionString);
InventoryService inventoryService = new InventoryService(productRepository);
SupplierService supplierService = new SupplierService(supplierRepository);

//通知功能相關
// 使用 EmailNotifier
EmailNotifier emailNotifier = new EmailNotifier();
NotificationService emailService = new NotificationService(emailNotifier);
// 使用 SmsNotifier
SmsNotifier  smsNotifier = new SmsNotifier();
NotificationService smsService = new NotificationService(smsNotifier);

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
   case "2": SearchProductByID();
    break;
   case "3": AddProduct();
    break;
   case "4": UpdateProduct();
    break;
   case "5": SearchProduct();
    break;
   case "6": CheckLowStockProducts();
    break;
   case "7": CheckOutOfStockProducts();
    break;
   case "8": AdjustProductQuantity();
    break;
   case "9": DeleteProduct();
    break;
   case "10": RunSupplierMenu();
    break;
   case "0":
    Console.WriteLine("Goodbye");
    return;
  }
 }
}

void DisplayMenu()
{
 Console.WriteLine("\n。 。 。 。 。 。 。 。 。 。 。");
 Console.WriteLine("Welcome to Inventory System!");
 Console.WriteLine("What would you like to do?");
 Console.WriteLine("1. Get all product");
 Console.WriteLine("2. Search product by ID");
 Console.WriteLine("3. Add product");
 Console.WriteLine("4. Update product");
 Console.WriteLine("5. Search product");
 Console.WriteLine("6. Check products with low stock");
 Console.WriteLine("7. Check Products which are out of stock");
 Console.WriteLine("8. Adjust product quantity");
 Console.WriteLine("9. Delete product");
 Console.WriteLine("10. Manage Suppliers");
 Console.WriteLine("0. Exit");
}

void GetAllProducts()
{
 Console.WriteLine("\n=== All product list ===");
 // var products = productRepository.GetAllProducts();
 var products = inventoryService.GetAllProducts();
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

void SearchProductByID()
{
 Console.WriteLine("\n=== Key in the product ID ===");
 int input = ReadIntInput();
 
 // InventoryService inventoryService = new InventoryService(productRepository);
 // var product = inventoryService.GetProductByID(input);
 OperationResult<Product> product = inventoryService.GetProductByID(input);
 if (product!=null)
 {
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine(product.Data);
  Console.WriteLine("---------------------------------------");
 }
}

void SearchProduct()
{
 Console.WriteLine("\nPlease key in the keyword:");
 string input = Console.ReadLine();
 OperationResult<List<Product>> products = inventoryService.SearchProduct(input);
 if (products.Data.Any())
 {
  Console.WriteLine($"--- Searching condition is: {input} --");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  foreach (var product in products.Data)
  {
   Console.WriteLine(product);
  }
  Console.WriteLine("---------------------------------------");
 }
}

void AddProduct()
{
 Console.WriteLine("\nPlease key in product name:");
 string name = Console.ReadLine();
 Console.WriteLine("\nPlease key in product price:");
 decimal price = ReadDecimalInput();
 Console.WriteLine("\nPlease key in product quantity:");
 int quantity = ReadIntInput();
 Console.WriteLine($"Product name:{name}, Price:{price}, Quantity:{quantity}");
 // productRepository.AddProduct(name, price, quantity);
 
 smsService.NotifyUser("Admin", $"Finish adding product: {name}.");
 inventoryService.AddProduct(name, price , quantity);
}

void UpdateProduct()
{
 Console.WriteLine("\nPlease key in product id:");
 int id = ReadIntInput(1);
 
 // 找到對應產品
 // Product product = inventoryService.GetProductByID(id); -- 改使用泛型
 OperationResult<Product> product = inventoryService.GetProductByID(id);
 if (product.Success)
 {
  Console.WriteLine(product.Message);
  Console.WriteLine("\n======= Product to be Updated =======");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine(product.Data);
  Console.WriteLine("---------------------------------------");
  // return;
 }

 if (product != null)
 {
  Console.WriteLine("\nPlease key in new product name:");
  string name = Console.ReadLine();
  Console.WriteLine("\nPlease key in new product price:");
  decimal price = ReadDecimalInput();
  Console.WriteLine("\nPlease key in new product quantity:");
  int quantity = ReadIntInput();
  
  // 進行更新
  inventoryService.UpdateProduct(product.Data, name, price, quantity);
  Console.WriteLine("\n======= Updated Product =======");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine(product.Data);
  Console.WriteLine("---------------------------------------");
 }
}

void CheckLowStockProducts()
{
 var products = inventoryService.CheckLowStockProducts();
 if (products!=null)
 {
  Console.WriteLine("\n======= Product with Low Stock =======");
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

void CheckOutOfStockProducts()
{
 var products = inventoryService.CheckOutOfStockProducts();
 if (products!=null)
 {
  Console.WriteLine("\n======= Products which are Out of Stock =======");
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

void AdjustProductQuantity()
{
 Console.WriteLine("\nPlease key in product id to be adjusted:");
 int id = ReadIntInput(1);
 // 找到對應產品
 // Product product = inventoryService.GetProductByID(id);
 OperationResult<Product> product = inventoryService.GetProductByID(id);
 // Console.WriteLine($"\n{product.Name}'s quantity: {product.Quantity}");

 if (product != null)
 {
  Console.WriteLine("\nPlease key in quantity to be adjusted (positive: stock-in / negative: stock-out):");
  int quantity = ReadIntInput();
  // product = inventoryService.AdjustProductQuantity(product, quantity);
  inventoryService.AdjustProductQuantity(product.Data, quantity);
 }
 if (product!=null)
 {
  Console.WriteLine("======= Adjusted Product =======");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("---------------------------------------");
  Console.WriteLine(product.Data);
  Console.WriteLine("---------------------------------------");
  
  emailService.NotifyUser("Admin", $"The information of the product, {product.Data.Name}, has been adjusted.");
  smsService.NotifyUser("Admin", $"The information of the product, {product.Data.Name}, has been adjusted.");
 }
}

void DeleteProduct()
{
 bool deleteContinue = true;
 while (deleteContinue)
 {
  Console.WriteLine("\nPlease key in product to be deleted with its id:");
  int id = ReadIntInput();
  OperationResult<Product> product = inventoryService.GetProductByID(id);
  if (product.Success)
  {
   Console.WriteLine(product.Message);
   Console.WriteLine("\n======= Product to be deleted =======");
   Console.WriteLine("---------------------------------------");
   Console.WriteLine("ID | Name | Price | Quantity | Status");
   Console.WriteLine("---------------------------------------");
   Console.WriteLine(product.Data);
   Console.WriteLine("---------------------------------------");
   bool checkContinue = true;
   while (checkContinue)
   {
    Console.WriteLine("\nAre you sure to delete this product? y/n");
    string input = Console.ReadLine().ToLower();
    switch (input)
    {
     case "y":
      inventoryService.DeleteProduct(id);
      Console.WriteLine("Product deleted!");
      checkContinue = false;
      break;
     case "n":
      Console.WriteLine("Operation cancelled ");
      checkContinue = false;
      break;
     default:
      Console.WriteLine("Please try again.");
      break;
    }
   }
   deleteContinue = false;
  }
  else
  {
   deleteContinue =  true;
  }
 }
}

void RunSupplierMenu()
{
 while (true)
 {
  Console.WriteLine("\n[ Supplier Management Menu ]");
  Console.WriteLine("1. Get all supplier");
  Console.WriteLine("2. Search supplier by ID");
  Console.WriteLine("3. Add supplier");
  Console.WriteLine("4. Update supplier");
  Console.WriteLine("5. Search supplier by keywords");
  Console.WriteLine("6. Adjust product quantity");
  Console.WriteLine("7. Delete Supplier");
  Console.WriteLine("0. Back to previous menu");
  string choice = Console.ReadLine();
  switch (choice)
  {
   case "1": GetAllSuppliers();
    break;
   case "3": AddSupplier();
    break;
   case "0":
    return;
  };
 }
}

void GetAllSuppliers()
{
 Console.WriteLine("\n==== Get All Suppliers ====");
 List<Supplier> suppliers = supplierService.GetAllSuppliers();
 if (suppliers.Any()) // .Any() 去了解 products 有沒有元素 (而不是單純 products 存不存在)
 {
  Console.WriteLine("---------------------------------------");
  Console.WriteLine("ID | Name | Address | Phone | e-mail");
  Console.WriteLine("---------------------------------------");
  foreach (var supplier in suppliers)
  {
   Console.WriteLine(supplier);
  }
  Console.WriteLine("---------------------------------------");
 }
}

void AddSupplier()
{
 Console.WriteLine("\n==== Add Supplier ====");
 Console.WriteLine("\nPlease key in supplier name:");
 string name = Console.ReadLine();
 Console.WriteLine("\nPlease key in supplier address:");
 string address = Console.ReadLine();
 Console.WriteLine("\nPlease key in supplier phone:");
 string phone = Console.ReadLine();
 Console.WriteLine("\nPlease key in supplier email:");
 string email = Console.ReadLine();
 Console.WriteLine($"Supplier name:{name}, address:{address}, phone:{phone},  email:{email}");
 
 smsService.NotifyUser("Admin", $"Finish adding supplier: {name}.");
 supplierService.AddSupplier(name, address, phone, email);
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

Dictionary<string, Dictionary<string, string>> ReadFile(string s)
{
 var config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
 string currentSection = "";

 foreach (string line in File.ReadLines(s))
 {
  string trimmedLine = line.Trim(); //去掉所有空格!!
  if (trimmedLine.StartsWith("#") || string.IsNullOrWhiteSpace(trimmedLine)) // # 表示註釋; 跳過註釋 & 跳過空白行
  {
   continue; // 跳過註釋和空行
  }

  if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]")) // 找到有中括號的那一行!!
  {
   currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2); // 取得index 為 1 的字母開始到 index 為 倒數2 的字母
   config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
  }
  else if (currentSection != "" && trimmedLine.Contains("="))
   // 取得 [] 取出來的 key 對應的 value!!! (但這個 value 將被 = 拆分成作為 value 的 Dictionay 的 key 跟 value)
  {
   int equalsIndex = trimmedLine.IndexOf('=');
   string key = trimmedLine.Substring(0, equalsIndex).Trim();
   string value = trimmedLine.Substring(equalsIndex + 1).Trim();
   config[currentSection][key] = value;
  }
 }
 return config;
}
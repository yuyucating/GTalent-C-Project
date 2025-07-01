
using InventorySystem.Models;
using InventorySystem.Repositories;

/* Server: mysql所在伺服器位址(localhost or ip xxx.xxx.xxx.xxx);
 Port: mysql端口 (預設 3306);
 Database: SQL 以 CREATE DATABASE 的名字;
 uid: mysql使用者名稱;
 pwd: mysql使用者密碼*/
const string MYSQL_CONNECTION_STRING = "Server=localhost;Port=3306;Database=inventory_db;uid=root;pwd=plutomarsD1206";

MySqlProductRepository productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);
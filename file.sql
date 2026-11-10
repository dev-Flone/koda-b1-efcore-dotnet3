-- SELECT name FROM sys.databases
-- create DATABASE ecommercedb
-- USE ecommercedb
-- SELECT DB_NAME() As CurrentDatabase
-- CREATE TABLE dbo.Product(
--     ProductId INT IDENTITY(1,1) PRIMARY KEY,
--     CustomerId INT NOT NULL CONSTRAINT Fk_Product_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
--     ProductCode NVARCHAR(20) NOT NULL UNIQUE,
--     Name NVARCHAR(100) NOT NULL,
--     Price BIGINT NOT NULL,
--     Stock INT NOT NULL,
--     Category TEXT,
--     IsActive BIT NOT NULL DEFAULT 1,
--     CONSTRAINT CK_Products_Price
--         CHECK(Price >= 0),
--     CONSTRAINT CK_Product_Stock
--         CHECK(Stock >= 0)
-- );
-- INSERT INTO dbo.Product (ProductCode, Name, Price, Stock, Category)
-- VALUES 
--     ('PR001', 'Keyboard', 30000, 10, 'Elektronik'),
--     ('PR002', 'Keyboard Mechanical', 30000, 10, 'Elektronik')
-- UPDATE dbo.Product SET Price =  60000 WHERE ProductId = 2
-- SELECT *
-- FROM   dbo.Product;
-- CREATE TABLE Customer(
--     Id INT IDENTITY(1, 1) PRIMARY KEY,    
--     Name NVARCHAR(50) NOT NULL,
--     Email NVARCHAR(100) NOT NULL UNIQUE,
--     Phone VARCHAR(15) NOT NULL UNIQUE,
--     Address NVARCHAR(255) NOT NULL
-- );
-- SELECT * from Customer
-- ALTER TABLE dbo.Product ADD CustomerId INT NOT NULL CONSTRAINT Fk_Product_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id);
-- TRUNCATE TABLE dbo.Product;
-- SELECT * FROM dbo.Product
-- DECLARE @json NVARCHAR(MAX) = N'[
--     {
--         "productCode": "PR001",
--         "name": "Mouse",
--         "price": 25000,
--         "stock": 20,
--         "category": "Aksesoris"
--     }
-- ]';
-- DROP TABLE dbo.Product;
-- DROP TABLE Customer;
-- SELECT * FROM dbo.Product;
-- SELECT * FROM Customer;
-- SELECT @@SERVERNAME AS ToolServerInstance, DB_NAME() AS ToolDatabaseName;
-- DELETE dbo.Product WHERE ProductId = 2
-- SELECT * FROM dbo.Product WITH (NOLOCK);
-- SELECT * FROM Customer WITH(NOLOCK);
-- DROP TABLE [__EFMigrationsHistory];
SELECT TABLE_NAME
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE';

SELECT *
FROM   Product;

SELECT *
FROM   Customer;

SELECT *
FROM   Orders;

SELECT *
FROM   OrderItem;

DROP TABLE Customer;

DROP TABLE Product;

DROP TABLE Orders;

DROP TABLE OrderItem;

DROP TABLE __EFMigrationsHistory;

-- Hapus seluruh foreign key constraint
DECLARE @sql AS NVARCHAR (MAX) = N'';

SELECT @sql += N'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(schema_id)) + N'.' + QUOTENAME(name) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
FROM   sys.tables AS t
       INNER JOIN
       sys.foreign_keys AS fk
       ON t.object_id = fk.parent_object_id;

EXECUTE sp_executesql @sql;

-- Hapus seluruh tabel
SET @sql = N'';

SELECT @sql += N'DROP TABLE ' + QUOTENAME(SCHEMA_NAME(schema_id)) + N'.' + QUOTENAME(name) + N';'
FROM   sys.tables
WHERE  is_ms_shipped = 0;

EXECUTE sp_executesql @sql;
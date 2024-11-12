CREATE TABLE CustomerTable (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Surname NVARCHAR(50),
    Email NVARCHAR(50),
    Age INT,
    ImageURL NVARCHAR(150)
);

CREATE TABLE ProductTable (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(50),
    ProductDescription NVARCHAR(150),
    Price INT,
    Availability INT,
    ImageURL NVARCHAR(MAX)
);

CREATE TABLE OrderTable (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT,   
    Quantity INT,
    FOREIGN KEY (ProductID) REFERENCES ProductTable(ProductID)
);

SELECT*FROM [dbo].[OrderTable]
SELECT*FROM [dbo].[CustomerTable]
SELECT*FROM [dbo].[ProductTable]




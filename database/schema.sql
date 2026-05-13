-- database/schema.sql
CREATE DATABASE CleaningCRM;
GO

USE CleaningCRM;
GO

CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);

CREATE TABLE Services (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(10,2) NOT NULL
);

CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20) NOT NULL
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId INT NOT NULL,
    ServiceId INT NOT NULL,
    EmployeeId INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    Address NVARCHAR(200) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Новый',
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (ServiceId) REFERENCES Services(Id),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Ожидается',
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);

INSERT INTO Clients (Name, Phone, Email) VALUES 
('Иван Петров', '+7-999-123-4567', 'ivan@mail.com'),
('ООО "БизнесЦентр"', '+7-495-789-1234', 'office@biz.ru');

INSERT INTO Services (Name, Description, Price) VALUES 
('Генеральная уборка', 'Полная уборка квартиры', 5000),
('Поддерживающая уборка', 'Еженедельная уборка', 2500),
('Химчистка мебели', 'Чистка диванов', 3500);

INSERT INTO Employees (FullName, Position, Phone) VALUES 
('Анна Сидорова', 'Клинер', '+7-916-111-2233'),
('Ольга Кузнецова', 'Клинер', '+7-916-444-5566');
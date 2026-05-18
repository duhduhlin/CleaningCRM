cat > database/init.sql << 'EOF'
-- Удаляем старые таблицы
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Services;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Clients;
DROP TABLE IF EXISTS Users;

-- Таблица услуг
CREATE TABLE Services (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Price DECIMAL(18,2) NOT NULL
);

-- Вставляем услуги
INSERT INTO Services (Name, Description, Price) VALUES 
('Стандартная уборка', 'Уборка квартиры до 50 кв.м', 3000),
('Генеральная уборка', 'Полная уборка', 5000),
('Мытье окон', 'Мытье окон', 1500);

-- Таблица пользователей
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    EmployeeId INT NULL,
    ClientId INT NULL
);

-- Вставляем пользователей
INSERT INTO Users (Username, PasswordHash, Role) VALUES 
('admin', '123456', 'Admin'),
('anna', '123456', 'Employee'),
('accountant', '123456', 'Accountant'),
('director', '123456', 'Director'),
('client1', '123456', 'Customer');

-- Таблица клиентов
CREATE TABLE Clients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Address NVARCHAR(500) NULL,
    UserId INT NULL
);

-- Вставляем клиента
INSERT INTO Clients (Name, Phone, Email, Address) VALUES 
('Тестовый Клиент', '+79990001111', 'client@test.com', 'ул. Тестовая, д.1');

-- Связываем клиента с пользователем
UPDATE Users SET ClientId = (SELECT Id FROM Clients) WHERE Username = 'client1';
EOF
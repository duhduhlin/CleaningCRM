-- Посмотрим пользователей
SELECT * FROM Users;

-- Привяжем Анну к пользователю anna (предположим что у anna userId=1)
UPDATE Employees SET UserId = 1 WHERE Id = 1;

-- Проверим
SELECT * FROM Employees;
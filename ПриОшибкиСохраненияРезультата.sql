DECLARE @group_name NVARCHAR(10);
/*Если при попытке сохранения результата группы появляется ошибка или закрывается программа -
в эту переменную запишите полное название группы как на форме сохранения результатов и запустите скрипт*/
SET @group_name = '24-Д9-ИНБ';

DELETE FROM StudentResult
WHERE TestingId = 1 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 1) ;

DELETE FROM StudentResult
WHERE TestingId = 2 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 1);

DELETE FROM StudentResult
WHERE TestingId = 3 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 2);

DELETE FROM StudentResult
WHERE TestingId = 4 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 7
);

DELETE FROM StudentResult
WHERE TestingId = 5 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 2);

DELETE FROM StudentResult
WHERE TestingId = 6 AND (Surname + [Name]) IN (
    SELECT (Surname + [Name])
    FROM StudentResult AS SR
	WHERE @group_name = (SELECT [FullName] FROM [Group] WHERE Id = SR.GroupId)
    GROUP BY Surname, [Name]
    HAVING COUNT(*) != 5);
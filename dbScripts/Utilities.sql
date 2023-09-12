/*Удаление внешнего ключа таблицы dbo.Students на таблицу dbo.Classes */
ALTER TABLE dbo.Students 
DROP CONSTRAINT FK_Students_Classes_ClassId

/*Удаление внешнего ключа таблицы dbo.Students на таблицу dbo.TeacherClassSubjects */
ALTER TABLE dbo.TeacherClassSubjects 
DROP CONSTRAINT FK_TeacherClassSubjects_Classes_ClassId

/*Добавление внешнего ключа для таблицы dbo.Students на таблицу dbo.Classes*/
ALTER TABLE dbo.Students 
ADD CONSTRAINT FK_Students_Classes_ClassId 
FOREIGN KEY (ClassId) 
REFERENCES dbo.Classes(Id)

/*Добавление внешнего ключа для таблицы dbo.TeacherClassSubjects на таблицу dbo.Classes*/
ALTER TABLE dbo.TeacherClassSubjects 
ADD CONSTRAINT FK_TeacherClassSubjects_Classes_ClassId 
FOREIGN KEY (ClassId) 
REFERENCES dbo.Classes(Id)
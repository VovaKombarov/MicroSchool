﻿INSERT INTO skool.classes(Number, Letter) 
VALUES
	  ('9', 'A'),
	  ('10', 'B'),
	  ('11', 'C') 


INSERT INTO skool.Students(Name, Patronymic, Surname, BirthDate, ClassId) 
VALUES
      ('Аглая', 'Ивановна', 'Епанчина', '2008-07-2', 1),
	  ('Аделаида', 'Ивановна', 'Епанчина', '2007-08-2', 2),
      ('Александра', 'Ивановна', 'Епанчина', '2006-12-1', 3),
	  ('Николай', 'Ардалионович', 'Иволгин', '2008-07-2', 1),
	  ('Варвара', 'Ардалионовна', 'Иволгина', '2007-08-2', 2),
	  ('Гаврила', 'Ардалионович', 'Иволгин', '2006-6-1', 3)	 

INSERT INTO skool.Parents(Name, Patronymic, Surname) 
VALUES 
		('Лизавета', 'Прокофьевна', 'Епанчина'),
		('Иван', 'Федорович', 'Епанчин'),
		('Ардалион', 'Александрович', 'Иволгин'),
		('Нина', 'Александровна', 'Иволгина')

INSERT INTO skool.ParentStudent(ParentsId, StudentsId)
VALUES 
		(1, 1),
		(1, 2), 
		(1, 3),
		(2, 1), 
		(2, 2),
		(2, 3),
		(3, 4),
		(3, 5),
		(3, 6),
		(4, 4), 
		(4, 5), 
		(4, 6)


INSERT INTO skool.Teachers(Name, Patronymic, Surname) 
VALUES 
		('Лев', 'Николевич', 'Мышкин'),
		('Парфен', 'Семенович', 'Рогожин'),
		('Настасья', 'Филиповна', 'Барашкова')	

INSERT INTO skool.Subjects(SubjectName) 
VALUES 
		('Ораторское искусство'),
		('Владение ножом'),
		('Этикет')	


INSERT INTO skool.TeachersClassesSubjects(TeacherId, ClassId, SubjectId) 
VALUES 
		(1, 1, 1),
		(1, 2, 1),
		(1, 3, 1),
		(2, 1, 2),
		(2, 2, 2),
		(2, 3, 2),
		(3, 1, 3),
		(3, 2, 3),	
		(3, 3, 3)	

INSERT INTO skool.homeworkstatuses(Status)
VALUES 
	('Задана'),
	('В работе'),
	('Выполнена, требуется проверка родителя'),
	('На проверке родителя'),
	('Проверена родителем'),
	('Готова к проверке учителем'),
	('На проверке учителя'),
	('Проверена учителем')

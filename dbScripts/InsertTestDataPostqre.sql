/* Вставляем урок */
INSERT INTO skool.lessons(
	teacherclasssubjectid, lessondt, theme)
	VALUES (1, '2023-06-02', 'Тема 1. Введение в ораторское искусство часть 1')

/* Вставляем учеников на уроке */
INSERT INTO skool.studentsinlessons(
	studentid, lessonid, comment)
	VALUES (1, 1, 'Замечание 1'),
		(4, 1, null)

/* Вставляем домашнюю работу */
INSERT INTO skool.homeworks(
	lessonid, startdt, finishdt, howework)
	VALUES (1, '2023-06-02', '2023-05-17', 'Домашняя работа по уроку 1');

/* Вставляем готовность домашней работы */
INSERT INTO skool.completedhomeworks(
	studentinlessonid, work, grade)
	VALUES (1, null, 4),
		   (2, null, null);

/* Вставляем статусы прогресса */
INSERT INTO skool.homeworkprogressstatuses(
	studentinlessonid, statussetdt, homeworkstatusid)
	VALUES 
		(1, '2023-05-12', 1),
		(2, '2023-05-12', 1)
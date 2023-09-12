/* Вставляем урок */
INSERT INTO skool.lessons(
	id, teacherclasssubjectid, lessondt, theme)
	VALUES (1, 1, '2023-06-02', 'Тема 1. Введение в ораторское искусство часть 1')

/* Вставляем учеников на уроке */
INSERT INTO skool.studentsinlessons(
	id, studentid, lessonid, comment)
	VALUES (1, 1, 1, 'Замечание 1'),
		(2, 4, 1, null)

/* Вставляем домашнюю работу */
INSERT INTO skool.homeworks(
	id, lessonid, startdt, finishdt, howework)
	VALUES (1, 1, '2023-06-02', '2023-05-17', 'Домашняя работа по уроку 1');

/* Вставляем готовность домашней работы */
INSERT INTO skool.completedhomeworks(
	id, studentinlessonid, work, grade)
	VALUES (1, 1, null, 4),
		   (2, 2, null, null);

/* Вставляем статусы прогресса */
INSERT INTO skool.homeworkprogressstatuses(
	id, studentinlessonid, statussetdt, homeworkstatusid)
	VALUES 
		(1, 1, '2023-05-12', 1),
		(2, 2, '2023-05-12', 1)
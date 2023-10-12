MicroSchool это небольшой pet проект, представляющий собой backend часть школы, 
где основная логика взаимодействия происходит между микрослужбами учителя и родителя.

Состоит из трех микрослужб:
- Сервис аутентификации;
- Сервис учителя;
- Сервис родителя;

Ключевые моменты:
- Каждая микрослужба автономна, реализует свою бизнес логику и имеет собственную модель данных.
- Cервис учителя в качестве СУБД использует MSSQL, сервис родителя PostgreSQL. Доступ и взаимодействие с данными EntityFrameworkCore (создание CodeFirst).
- Обмен данными происходит через RabbitMq. Взаимодействие между микрослужбами происходит на основе событий интеграции. Сервер Rabbitmq крутится локально в образe docker.
- Логика работы с данными реализованы через связку шаблонов, репозитарий + спецификаций.
Плюс этой связки шаблонов в том, что логика работы с данными не просачивается в бизнес логику,
она инкапсулирована в спецификации (соблюдение принципа единой ответственности).
- Сервис аутентификации защищает остальные микрослужбы. Токены любезно поставляет IdentityServer4.
- тесты написаны на NUnit.
- Api описан с помощью Swagger.
- целевая платформа .Net 7.0

Функционал сервисов ( можно не читать ).
IdentityApi логика аутентификации и выдачи токенов с помощью IdentityServer4.
- Регистрация
- Вход пользователя в систему
- Выход пользователя из системы

TeacherApi представляет логику для работы учителя.
- Получение списка студентов для определенного класса
- Получение списка родителей для студента
- Получение статуса домашней работы
- Получение домашней работы
- Создание урока
- Создание домашней работы
- Создание встречи учителя и родителя
- Изменение статуса домашней работы
- Оценивание домашней работы
- Создание замечания студенту
- Оценивание студента на уроке
- Отмена встречи учителя и родителя

ParentApi представляет логику действий родителя.
- Получение списка замечаний студента
- Получение конкретного замечания по уроку
- Получение оценки по уроку
- Получение всех оценок по предмету
- Создание встречи родителя и учителя
- Отмена встречи родителя и учителя

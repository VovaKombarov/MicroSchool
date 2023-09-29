using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherApi.Models;

namespace TeacherApi.Services
{
    /// <summary>
    /// Интерфейс сервиса учителя.
    /// </summary>
    public interface ITeacherService
    {
        /// <summary>
        /// Асинхронное проверка существования класса.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        /// <returns>Класс.</returns>
        Task<Class> ClassExistsAsync(int classId);

        /// <summary>
        /// Асинхронное проверка существования студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Студент.</returns>
        Task<Student> StudentExistsAsync(int studentId);

        /// <summary>
        /// Асинхронное проверка существования учителя.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <returns>Учитель.</returns>
        Task<Teacher> TeacherExistsAsync(int teacherId);

        /// <summary>
        /// Асинхронное проверка существования предмета.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Предмет.</returns>
        Task<Subject> SubjectExistsAsync(int subjectId);

        /// <summary>
        /// Асинхронное проверка существования родителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <returns>Родитель.</returns>
        Task<Parent> ParentExistsAsync(int parentId);

        /// <summary>
        /// Асинхронное проверка существования встречи родителя и учителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        Task<TeacherParentMeeting> TeacherParentMeetingExists(
            int teacherParentMeetingId);

        /// <summary>
        /// Асинхронное проверка существования статуса домашней работы.
        /// </summary>
        /// <param name="homeworkStatusId">Идентификатор статуса домашней работы.</param>
        /// <returns>Статус домашней работы.</returns>
        Task<HomeworkStatus> HomeworkStatusExistsAsync(
            int homeworkStatusId);

        /// <summary>
        /// Асинхронное проверка существования урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Урок.</returns>
        Task<Lesson> LessonExistsAsync(int lessonId);

        /// <summary>
        /// Асинхронное проверка существования домашней работы.
        /// </summary>
        /// <param name="homeworkId">Идентификатор домашней работы.</param>
        /// <returns>Домашняя работа.</returns>
        Task<Homework> HomeworkExistsAsync(int homeworkId);

        /// <summary>
        /// Асинхронное проверка обьекта учитель/класс/предмет.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>обьекта учитель/класс/предмет.</returns>
        Task<TeacherClassSubject> TeacherClassSubjectExistsAsync(
            int teacherId,
            int classId,
            int subjectId);

        /// <summary>
        /// Асинхронное проверка студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Студента на уроке.</returns>
        Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId);

        /// <summary>
        /// Асинхронное проверка существования домашней работы по идентификатру урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Домашняя работа.</returns>
        Task<Homework> HomeworkExistsByLessonIdAsync(int lessonId);


        /// <summary>
        /// Асинхронное получение списка студентов по идентификатору класса.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        /// <returns>Коллекция студентов.</returns>
        Task<List<Student>> GetStudentsByClassIdAsync(int classId);

        /// <summary>
        /// Получение коллекции родителей по идентификатору студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекция родителей.</returns>
        Task<List<Parent>> GetParentsByStudentIdAsync(int studentId);

        /// <summary>
        /// Асинхронное получение статуса прогресса домашней работы.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        /// <returns>Статус прогресса домашней работы.</returns>
        Task<HomeworkProgressStatus> GetHomeworkProgressStatusAsync(
            int studentInLessonId);

        /// <summary>
        /// Асинхронное получение готовой домашней работы по идентификатору студента на уроке.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        /// <returns></returns>
        Task<CompletedHomework> GetCompletedHomeworkByStudentInLessonIdAsync(
            int studentInLessonId);

        /// <summary>
        /// Асинхронное добавление урока.
        /// </summary>
        /// <param name="teacherId">Идентификатор урока.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <param name="theme">Тема урока.</param>
        /// <param name="lessonDateTime">Время урока.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddLessonAsync(int teacherId,
             int classId,
             int subjectId,
             string theme,
             DateTime lessonDateTime);

        /// <summary>
        /// Асинхронное добавление домашней работы.
        /// </summary>
        /// <param name="lessonId"><Идентификатор урока./param>
        /// <param name="finishDateTime">Время окончания урок</param>
        /// <param name="homeWork">Домашняя работа.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddHomeworkAsync(
            int lessonId,
            DateTime finishDateTime,
            string homeWork);

        /// <summary>
        /// Асинхронное добавление встречи учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи родителя и учителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddTeacherParentMeetingAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        /// <summary>
        /// Асинхронное добавления статуса прогресса домашней работы. 
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="homeworkStatusId">Статус домашней работы.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddHomeworkProgressStatusAsync(
            int studentId,
            int lessonId,
            int homeworkStatusId);

        /// <summary>
        /// Асинхронное обновление оценки домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task UpdateGradeHomeworkAsync(
            int studentId,
             int lessonId,
             int grade);

        /// <summary>
        /// Асинхронное обновление замечания.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="comment">Замечание.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task UpdateCommentAsync(int studentId, int lessonId, string comment);

        /// <summary>
        /// Асинхронное обновление оценки студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task UpdateGradeStudentInLessonAsync(
            int studentId, int lessonId, int grade);

        /// <summary>
        /// Удаление встречи учителя и родителя.
        /// </summary>
        /// <param name="teacherParentMeeting">Встречя учителя и родителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task RemoveTeacherParentMeeting(int teacherParentMeeting);

    }
}

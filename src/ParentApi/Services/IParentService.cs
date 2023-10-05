using ParentApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParentApi.Services
{
    /// <summary>
    /// Сервис родителя.
    /// </summary>
    public interface IParentService
    {
        /// <summary>
        /// Асинхронная проверка существования студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Студент.</returns>
        Task<Student> StudentExistsAsync(int studentId);

        /// <summary>
        /// Асинхронная проверка существования учителя.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <returns>Учитель.</returns>
        Task<Teacher> TeacherExistsAsync(int teacherId);

        /// <summary>
        /// Асинхронная проверка существования родителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <returns>Родитель.</returns>
        Task<Parent> ParentExistsAsync(int parentId);

        /// <summary>
        /// Асинхронная проверка существования урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Урок.</returns>
        Task<Lesson> LessonExistsAsync(int lessonId);

        /// <summary>
        /// Асинхронная проверка существования родителя.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Предмет.</returns>
        Task<Subject> SubjectExistsAsync(int subjectId);

        /// <summary>
        /// Асинхронная проверка существования студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Студент на уроке.</returns>
        Task<StudentInLesson> StudentInLessonExistsAsync(
           int studentId, int lessonId);

        /// <summary>
        /// Асинхронная проверка существования встречи учителя и родителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int teacherParentMeetingId);

        /// <summary>
        /// Асинхронная проверка существования встречи учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        /// <summary>
        /// Асинхронное получение коллекции студентов на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекцию студентов на уроке.</returns>
        Task<List<StudentInLesson>> GetStudentInLessonsAsync(int studentId);

        /// <summary>
        /// Добавляет встречу учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task AddTeacherParentMeetingAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        /// <summary>
        /// Асинхронный возврат готовой домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Готовая домашняя работа.</returns>
        Task<CompletedHomework> GetCompletedHomeworkAsync(
            int studentId, int lessonId);

        /// <summary>
        /// Асинхронное получение оценок.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Коллекция оценок.</returns>
        Task<List<int>> GetGradesAsync(int studentId, int subjectId);

        /// <summary>
        /// Асинхронное удаление встречи родителя и учителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task RemoveParentTeacherMeetingAsync(int teacherParentMeetingId);
    }
}

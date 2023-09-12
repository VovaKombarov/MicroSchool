using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using ParentApi.Models;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace ParentApi.Services
{
    public interface IParentService
    {
        Task<Student> StudentExistsAsync(int studentId);

        Task<Teacher> TeacherExistsAsync(int teacherId);

        Task<Parent> ParentExistsAsync(int parentId);

        Task<Subject> SubjectExistsAsync(int subjectId);

        Task<StudentInLesson> StudentInLessonExistsAsync(
           int studentId, int lessonId);

        Task<Lesson> LessonExistsAsync(int lessonId);

        Task<List<StudentInLesson>> GetStudentInLessonsAsync(int studentId);

        Task AddTeacherParentMeetingAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        Task<CompletedHomework> GetCompletedHomeworkAsync(
            int studentId, int lessonId);

        Task<List<int>> GetGradesAsync(int studentId, int subjectId);

        Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int teacherParentMeetingId);

        Task RemoveParentTeacherMeetingAsync(int teacherParentMeetingId);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherApi.Models;

namespace TeacherApi.Services
{
    public interface ITeacherService
    {
        Task<Class> ClassExistsAsync(int classId);

        Task<Student> StudentExistsAsync(int studentId);

        Task<Teacher> TeacherExistsAsync(int teacherId);

        Task<Subject> SubjectExistsAsync(int subjectId);

        Task<Parent> ParentExistsAsync(int parentId);

        Task<TeacherParentMeeting> TeacherParentMeetingExists(
            int teacherParentMeetingId);

        Task<HomeworkStatus> HomeworkStatusExistsAsync(
            int homeworkStatusId);

        Task<TeacherClassSubject> TeacherClassSubjectExistsAsync(
            int teacherId,
            int classId,
            int subjectId);

        Task<List<Student>> GetStudentsByClassIdAsync(int classId);

        Task<List<Parent>> GetParentsByStudentIdAsync(int studentId);

        Task<Lesson> LessonExistsAsync(int lessonId);

        Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId);

        Task<Homework> HomeworkExistsAsync(int homeworkId);

        Task<HomeworkProgressStatus> GetHomeworkProgressStatusAsync(
            int studentInLessonId);

        Task<CompletedHomework> GetCompletedHomeworkByStudentInLessonIdAsync(
            int studentInLessonId);

        Task<Homework> HomeworkExistsByLessonIdAsync(int lessonId);


        Task AddLessonAsync(int teacherId,
             int classId,
             int subjectId,
             string theme,
             DateTime lessonDateTime);

        Task AddHomeworkAsync(
            int lessonId,
            DateTime finishDateTime,
            string himeWork);

        Task AddTeacherParentMeetingAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT);

        Task AddHomeworkProgressStatusAsync(
            int studentId,
            int lessonId,
            int homeworkStatusId);

        Task UpdateGradeHomeworkAsync(
            int studentId,
             int lessonId,
             int grade);

        Task UpdateCommentAsync(int studentId, int lessonId, string comment);

        Task UpdateGradeStudentInLessonAsync(
            int studentId, int lessonId, int grade);

        Task RemoveTeacherParentMeeting(int teacherParentMeeting);

    }
}

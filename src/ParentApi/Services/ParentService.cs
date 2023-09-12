using Common.Api;
using ParentApi.Data;
using ParentApi.Data.Specifications;
using ParentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentApi.Services
{
    public class ParentService : IParentService
    {
        #region Fields 

        private readonly IRepository<Student> _studentRepo;

        private readonly IRepository<Teacher> _teacherRepo;

        private readonly IRepository<Parent> _parentRepo;

        private readonly IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        private readonly IRepository<Lesson> _lessonRepo;

        private readonly IRepository<StudentInLesson>  _studentInLessonRepo;

        private readonly IRepository<CompletedHomework> _completedHomeworkRepo;

        private readonly IRepository<Subject> _subjectRepo;

        #endregion Fields

        public ParentService(
            IRepository<Student> studentRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<Parent> parentRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo,
            IRepository<Lesson> lessonRepo,
            IRepository<StudentInLesson> studentInLessonRepo,
            IRepository<CompletedHomework> completedHomeworkRepo,
            IRepository<Subject> subjectRepo)
        {
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _parentRepo = parentRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _lessonRepo = lessonRepo;
            _studentInLessonRepo = studentInLessonRepo;
            _completedHomeworkRepo = completedHomeworkRepo;
            _subjectRepo = subjectRepo;
        }

        #region Utilities

        private TeacherParentMeeting _CreateTeacherParentMeeting(
           Student student,
           Teacher teacher,
           Parent parent,
           DateTime meetingDT)
        {
            TeacherParentMeeting teacherParentMeeting =
                new TeacherParentMeeting()
                {
                    Student = student,
                    Parent = parent,
                    Teacher = teacher,
                    TeacherInitiative = false,
                    MeetingDT = meetingDT
                };
            return teacherParentMeeting;
        }

        #endregion Utilities

        #region Methods

        public async Task<Parent> ParentExistsAsync(int parentId)
        {
            return await _parentRepo.GetItemAsync(
                new ParentSpecification(parentId));
        }

        public async Task<Student> StudentExistsAsync(int studentId)
        {
            return await _studentRepo.GetItemAsync(
                new StudentSpecification(studentId));
        }

        public async Task<Lesson> LessonExistsAsync(int lessonId)
        {
            return await _lessonRepo.GetItemAsync(
                new LessonSpecification(lessonId));
        }

        public async Task<Teacher> TeacherExistsAsync(int teacherId)
        {
            return await _teacherRepo.GetItemAsync(
                new TeacherSpecification(teacherId));
        }

        public async Task<Subject> SubjectExistsAsync(int subjectId)
        {
            return await _subjectRepo.GetItemAsync(
                new SubjectSpecification(subjectId));
        }

        public async Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId)
        {
            return await _studentInLessonRepo.GetItemAsync(
                new StudentInLessonSpecification(studentId, lessonId));
        }

        public async Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
           int teacherParentMeetingId)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(teacherParentMeetingId));
        }

        public async Task<List<StudentInLesson>> GetStudentInLessonsAsync(
            int studentId)
        {
            return await _studentInLessonRepo.GetListAsync(
                new StudentInLessonSpecification(studentId));
        }

        public async Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(
                    studentId, teacherId, parentId, meetingDT)); 
        }

        public async Task AddTeacherParentMeetingAsync(
            int studentId, int teacherId, int parentId, DateTime meetingDT)
        {
            Student student;
            Teacher teacher;
            Parent parent;
            TeacherParentMeeting teacherParentMeeting;

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            teacher = await TeacherExistsAsync(teacherId);
            ResultHandler.CheckResult(teacher);

            parent = await ParentExistsAsync(parentId);
            ResultHandler.CheckResult(parent);

            teacherParentMeeting = _CreateTeacherParentMeeting(
                student, teacher, parent, meetingDT);

            await _teacherParentMeetingRepo.AddAsync(teacherParentMeeting);
            await _teacherParentMeetingRepo.SaveChangesAsync(); 
        }

        public async Task<CompletedHomework> GetCompletedHomeworkAsync(
            int studentId, int lessonId)
        {
            StudentInLesson studentInLesson;

            studentInLesson = await _studentInLessonRepo.GetItemAsync(
               new StudentInLessonSpecification(studentId, lessonId));

            ResultHandler.CheckResult(studentInLesson);

            CompletedHomework completedHomework =
                await _completedHomeworkRepo.GetItemAsync(
                    new CompletedHomeworkSpecification(studentInLesson.Id));

            ResultHandler.CheckResult(completedHomework);

            return completedHomework;
        }

        public async Task<List<int>> GetGradesAsync(
            int studentId, int subjectId)
        {
            Student student;
            Subject subject;

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            subject = await SubjectExistsAsync(subjectId);
            ResultHandler.CheckResult(subject);

            List<StudentInLesson> studentInLessons =
                await _studentInLessonRepo.GetListAsync(
                    new StudentInLessonSpecificationFilterBySubject(
                        studentId, subjectId));
                    
            ResultHandler.CheckResult(studentInLessons);

            List<int> grades = studentInLessons
                .Where(w => w.Grade != null)
                .Select(w => (int)w.Grade)
                .ToList();

            return grades;
        }

        public async Task RemoveParentTeacherMeetingAsync(
            int teacherParentMeetingId)
        {
            TeacherParentMeeting teacherParentMeeting = 
                await _teacherParentMeetingRepo.GetItemAsync(
                    new TeacherParentMeetingSpecification(
                        teacherParentMeetingId));

            ResultHandler.CheckResult(teacherParentMeeting);

            _teacherParentMeetingRepo.Remove(teacherParentMeeting);

            await _teacherParentMeetingRepo.SaveChangesAsync();   
        }

        #endregion Methods
    }
}

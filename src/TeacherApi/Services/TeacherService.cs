using Common.Api;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;
using TeacherApi.Data.Specifications;
using TeacherApi.Models;
using TeacherApi.Utilities;

namespace TeacherApi.Services
{
    public class TeacherService : ITeacherService
    {
        #region Fields

        /// <summary>
        /// Источник данных.
        /// </summary>
        private AppDbContext _context;

        private readonly IRepository<Class> _classRepo;

        private readonly IRepository<Student> _studentRepo;

        private readonly IRepository<Subject> _subjectRepo;

        private readonly IRepository<Teacher> _teacherRepo;

        private readonly IRepository<TeacherClassSubject> _teacherClassSubjectRepo;

        private readonly IRepository<Lesson> _lessonRepo;

        private readonly IRepository<Parent> _parentRepo;

        private readonly IRepository<StudentInLesson> _studentInLessonRepo;

        private readonly IRepository<Homework> _homeworkRepo;

        private readonly IRepository<HomeworkProgressStatus>
            _homeworkProgressStatusRepo;

        private readonly IRepository<CompletedHomework> _completedHomeworkRepo;

        private readonly IRepository<HomeworkStatus> _homeWorkStatusRepo;

        private readonly IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        #endregion Fields

        #region Constructors

        public TeacherService(
            AppDbContext context,
            IRepository<Class> classRepo,
            IRepository<Student> studentRepo,
            IRepository<TeacherClassSubject> teacherClassSubjectRepo,
            IRepository<Parent> parentRepo,
            IRepository<Lesson> lessonRepo,
            IRepository<StudentInLesson> studentInLessonRepo,
            IRepository<Homework> homeworkRepo,
            IRepository<HomeworkProgressStatus> homeworkProgressStatusRepo,
            IRepository<Subject> subjectRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<CompletedHomework> completedHomeworkRepo,
            IRepository<HomeworkStatus> homeworkStatusRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo)
        {
            _context = context;

            _classRepo = classRepo;
            _studentRepo = studentRepo;
            _teacherClassSubjectRepo = teacherClassSubjectRepo;
            _parentRepo = parentRepo;
            _lessonRepo = lessonRepo;
            _studentInLessonRepo = studentInLessonRepo;
            _homeworkRepo = homeworkRepo;
            _homeworkProgressStatusRepo = homeworkProgressStatusRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _completedHomeworkRepo = completedHomeworkRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _homeWorkStatusRepo = homeworkStatusRepo;
        }

        #endregion Constructors

        #region Utilities

        private Lesson _CreateLesson(
            DateTime lessonDateTime,
            TeacherClassSubject teacherClassSubject,
            string theme)
        {
            return new Lesson()
            {
                LessonDT = lessonDateTime,
                TeacherClassSubject = teacherClassSubject,
                Theme = theme
            };
        }

        private Homework _CreateHomework(
            Lesson lesson,
            DateTime finishDT,
            string hmwork)
        {
            return new Homework()
            {
                Lesson = lesson,
                StartDT = lesson.LessonDT,
                FinishDT = finishDT,
                Howework = hmwork
            };  
        }

        private StudentInLesson _CreateStudentInLesson(
            Lesson lesson,
            Student student)
        {
            return new StudentInLesson()
            {
                Lesson = lesson,
                Student = student
            };
        }

        private CompletedHomework _CreateCompletedHomework(
            StudentInLesson studentInLesson)
        {
            return new CompletedHomework()
            {
                StudentInLesson = studentInLesson
            };
        }

        private TeacherParentMeeting _CreateTeacherParentMeeting(
            Student student,
            Teacher teacher,
            Parent parent,
            DateTime meetingDT)
        {
            return new TeacherParentMeeting()
            {
                Student = student,
                Parent = parent,
                Teacher = teacher,
                TeacherInitiative = true,
                MeetingDT = meetingDT
            };
        }

        private async Task<List<StudentInLesson>> _GetStudentsInLessonByLessonId(
            int lessonId)
        {
            return await _studentInLessonRepo.GetListAsync(
                new StudentInLessonSpecification(lessonId));
        }

        private HomeworkProgressStatus _CreateHomeworkProgressStatus(
            StudentInLesson studentInLesson,
            HomeworkStatus homeworkStatus)
        {
            HomeworkProgressStatus homeworkProgressStatus =
                new HomeworkProgressStatus()
                {
                    HomeworkStatus = homeworkStatus,
                    StudentInLesson = studentInLesson,
                    StatusSetDT = DateTime.Now
                };

            return homeworkProgressStatus;
        }

        #endregion Utilities

        #region Methods

        public async Task<Class> ClassExistsAsync(int classId)
        {
            return await _classRepo.GetItemAsync(
                new ClassSpecification(classId));
        }

        public async Task<Lesson> LessonExistsAsync(int lessonId)
        {
            return await _lessonRepo.GetItemAsync(
                new LessonSpecification(lessonId));
        }

        public async Task<Student> StudentExistsAsync(int studentId)
        {
            return await _studentRepo.GetItemAsync(
                new StudentSpecification(studentId));
        }

        public async Task<Parent> ParentExistsAsync(int parentId)
        {
            return await _parentRepo.GetItemAsync(
                new ParentSpecification(parentId));
        }

        public async Task<HomeworkStatus> HomeworkStatusExistsAsync(int homeworkStatusId)
        {
            return await _homeWorkStatusRepo.GetItemAsync(
                new HomeworkStatusSpecification(homeworkStatusId));
        }

        public async Task<TeacherParentMeeting> TeacherParentMeetingExists(
          int teacherParentMeetingId)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(teacherParentMeetingId));
        }

        public async Task<List<Student>> GetStudentsByClassIdAsync(int classId)
        {
            return await _studentRepo.GetListAsync(
                new StudentsSpecification(classId));
        }

        public async Task<List<Parent>> GetParentsByStudentIdAsync(int studentId)
        {
            return await _parentRepo.GetListAsync(
                new ParentsSpecification(studentId));
        }

        public async Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId)
        {
            return await _studentInLessonRepo.GetItemAsync(
                new StudentInLessonSpecification(studentId, lessonId));
        }

        public async Task<Homework> HomeworkExistsAsync(int homeworkId)
        {
            return await _homeworkRepo.GetItemAsync(
                new HomeworkSpecification(homeworkId));
        }

        public async Task<HomeworkProgressStatus> GetHomeworkProgressStatusAsync(
            int studentInLessonId)
        {
            return await _homeworkProgressStatusRepo.GetItemAsync(
                new HomeworkProgressStatusSpecification(studentInLessonId));
        }

        public async Task<CompletedHomework> GetCompletedHomeworkByStudentInLessonIdAsync(
            int studentInLessonId)
        {
            return await _completedHomeworkRepo.GetItemAsync(
                new CompletedHomeworkSpecification(studentInLessonId));
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

        public async Task<Homework> HomeworkExistsByLessonIdAsync(int lessonId)
        {
            return await _homeworkRepo.GetItemAsync(
                new HomeworkSpecificationByLesson(lessonId));
        }

        public async Task<TeacherClassSubject> TeacherClassSubjectExistsAsync(
            int teacherId,
            int classId,
            int subjectId)
        {
            return await _teacherClassSubjectRepo.GetItemAsync(
                new TeacherClassSubjectSpecification(teacherId, classId, subjectId));
        }

        public async Task AddLessonAsync(
            int teacherId,
            int classId,
            int subjectId,
            string theme,
            DateTime lessonDateTime)
        {
            List<Student> students;
            TeacherClassSubject teacherClassSubject;

            students = await GetStudentsByClassIdAsync(classId);
            ResultHandler.CheckResult(students);

            teacherClassSubject = await TeacherClassSubjectExistsAsync(
                teacherId, classId, subjectId);

            ResultHandler.CheckResult(teacherClassSubject);

            Lesson lesson = _CreateLesson(
                lessonDateTime, teacherClassSubject, theme);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _lessonRepo.AddAsync(lesson);
                    await _lessonRepo.SaveChangesAsync();

                    foreach (Student student in students)
                    {
                        StudentInLesson studentInLesson =
                            _CreateStudentInLesson(lesson, student);

                        await _studentInLessonRepo.AddAsync(studentInLesson);
                    }

                    await _studentInLessonRepo.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task AddHomeworkAsync(
            int lessonId,
            DateTime finishDateTime,
            string homeWork)
        {
            Lesson lesson;
            List<StudentInLesson> studentsInLesson;
            HomeworkStatus appointedHomeworkStatus;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            studentsInLesson = await _GetStudentsInLessonByLessonId(lessonId);
            ResultHandler.CheckResult(studentsInLesson);

            HomeworkStatusSpecification homeworkStatusSpecification = 
                new HomeworkStatusSpecification((int)HomeworkStatuses.Appointed);

            appointedHomeworkStatus = 
                await _homeWorkStatusRepo.GetItemAsync(
                    homeworkStatusSpecification);

            ResultHandler.CheckResult(appointedHomeworkStatus);

            Homework homework = _CreateHomework(
                lesson, finishDateTime, homeWork);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _homeworkRepo.AddAsync(homework);
                    await _homeworkRepo.SaveChangesAsync();

                    foreach (var studentInLesson in studentsInLesson)
                    {
                        CompletedHomework completedHomework =
                            _CreateCompletedHomework(studentInLesson);

                        await _completedHomeworkRepo.AddAsync(completedHomework);

                        HomeworkProgressStatus homeworkProgressStatus =
                             _CreateHomeworkProgressStatus(
                                 studentInLesson, appointedHomeworkStatus);

                        await _homeworkProgressStatusRepo.AddAsync(
                            homeworkProgressStatus);
                    }

                    await _completedHomeworkRepo.SaveChangesAsync();
                    await _homeworkProgressStatusRepo.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task AddTeacherParentMeetingAsync(
            int studentId, 
            int teacherId, 
            int parentId, 
            DateTime meetingDT)
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

        public async Task AddHomeworkProgressStatusAsync(
            int studentId, int lessonId, int homeworkStatusId)
        {
            StudentInLesson studentInLesson;
            HomeworkStatus homeworkStatuses;
            HomeworkProgressStatus homeworkProgressStatus;

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            homeworkStatuses = await HomeworkStatusExistsAsync(homeworkStatusId);
            ResultHandler.CheckResult(homeworkStatusId);

            homeworkProgressStatus = _CreateHomeworkProgressStatus(
                studentInLesson, homeworkStatuses);

            await _homeworkProgressStatusRepo.AddAsync(homeworkProgressStatus);
            await _homeworkProgressStatusRepo.SaveChangesAsync(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="lessonId"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public async Task UpdateGradeHomeworkAsync(int studentId, int lessonId, int grade)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;
            CompletedHomework completedHomework;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            completedHomework = await GetCompletedHomeworkByStudentInLessonIdAsync(
                studentInLesson.Id);
            ResultHandler.CheckResult(completedHomework);

            completedHomework.Grade = grade;
            _completedHomeworkRepo.Update(completedHomework);

             await _completedHomeworkRepo.SaveChangesAsync();
        }

        public async Task UpdateGradeStudentInLessonAsync(
            int studentId, int lessonId, int grade)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            studentInLesson.Grade = grade;
            _studentInLessonRepo.Update(studentInLesson);

            await _studentInLessonRepo.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(
            int studentId, int lessonId, string comment)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            studentInLesson.Comment = comment;
            _studentInLessonRepo.Update(studentInLesson);

             await _studentInLessonRepo.SaveChangesAsync();
        }

        public async Task RemoveTeacherParentMeeting(int teacherParentMeetingId)
        {
            TeacherParentMeetingSpecification teacherParentMeetingSpecification = 
                new TeacherParentMeetingSpecification(teacherParentMeetingId);

            TeacherParentMeeting teacherParentMeeting =
                await _teacherParentMeetingRepo.GetItemAsync(
                    teacherParentMeetingSpecification);

            ResultHandler.CheckResult(teacherParentMeeting);

            _teacherParentMeetingRepo.Remove(teacherParentMeeting);

            await _teacherParentMeetingRepo.SaveChangesAsync();  
        }

        #endregion Methods
    }
}

using AutoMapper;
using Common.Api;
using Common.ErrorResponse;
using Common.EventBus;
using Common.TestsUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TeacherApi.Controllers;
using TeacherApi.Data.Dtos;
using TeacherApi.Models;
using TeacherApi.Services;
using TeacherApi.Tests.Utilities;
using TeacherApi.Utilities;

namespace TeachersService.Tests
{
    public class TeachersControllerTests
    {
        #region Fields

        private TeachersController _teachersController;

        private Mock<ITeacherService> _teacherService;

        private Mock<IMapper> _mapper;

        private Mock<IOptions<Dictionary<string, string>>> _options;

        private Mock<IEventBus> _eventBus;

        private Mock<ILogger> _logger;

        #endregion Fields

        #region Utilities

        private HttpStatusCode _GetHttpStatusCodeBySetupKey(SetupKey setupKey)
        {
           return setupKey switch
            {
                SetupKey.InternalServerError => HttpStatusCode.InternalServerError,
                SetupKey.NotFound => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.NotFound
            };
        }

        #endregion Utilities

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _teacherService = new Mock<ITeacherService>();
            _options = new Mock<IOptions<Dictionary<string, string>>>();
            _eventBus = new Mock<IEventBus>();
            _logger = new Mock<ILogger>();
           
            _teachersController = new TeachersController(
                _teacherService.Object,
                _mapper.Object,
                _eventBus.Object,
                _options.Object, 
                _logger.Object);

            Invoker.InitLogger(_logger.Object);

            _SetupOptions();
        }

        private void _SetupOptions()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "HomeworkStatusNotValid", "Статус домашней работы некорректный!"},
                { "GradeNotValid", "Оценка не валидна!"},
                { "DateTimeNotValid", "Время невалидно!"},
                {"CommentMissing", "Замечание отсутствует!" }
            };

            _options.Setup(w => w.Value).Returns(keyValuePairs);
        }

        private void _SetupClassExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                 _teacherService.Setup(w => w.ClassExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Class()));
            }
            else
            {
                _teacherService.Setup(w => w.ClassExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupStudentExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.StudentExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Student()));
            }
            else
            {
                _teacherService.Setup(w => w.StudentExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }           
        }

        private void _SetupTeacherExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.TeacherExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Teacher()));
            }
            else
            {
                _teacherService.Setup(w => w.TeacherExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }

        }

        private void _SetupSubjectExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.SubjectExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Subject()));
            }
            else
            {
                _teacherService.Setup(w => w.SubjectExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupHomeworkExistsByLessonIdAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.HomeworkExistsByLessonIdAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Homework()));
            }
            else
            {
                _teacherService.Setup(w => w.HomeworkExistsByLessonIdAsync(
                    It.IsAny<int>())).Throws(new HttpStatusException(
                        _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupLessonExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.LessonExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Lesson()));
            }
            else
            {
                _teacherService.Setup(w => w.LessonExistsAsync(It.IsAny<int>()))
                    .Throws(new HttpStatusException(
                        _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupParentsExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.ParentExistsAsync(
                    It.IsAny<int>())).Returns(
                        Task.FromResult(new Parent()));
            }
            else
            {
                _teacherService.Setup(w => w.ParentExistsAsync(It.IsAny<int>()))
                    .Throws(new HttpStatusException(
                        _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupTeacherClassSubjectExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.TeacherClassSubjectExistsAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Returns(Task.FromResult(new TeacherClassSubject()));
            }
            else
            {
                _teacherService.Setup(w => w.TeacherClassSubjectExistsAsync(
                   It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupTeacherParentMeetingExists(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.TeacherParentMeetingExists(
                    It.IsAny<int>()))
                        .Returns(Task.FromResult(new TeacherParentMeeting()));
            }
            else
            {
                _teacherService.Setup(w => w.TeacherParentMeetingExists(
                   It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupHomeworkExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.HomeworkExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Homework()));

            }
            else
            {
                _teacherService.Setup(w => w.HomeworkExistsAsync(
                   It.IsAny<int>())).Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            } 
        }

        private void _SetupStudentInLessonExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.StudentInLessonExistsAsync(

                    It.IsAny<int>(), It.IsAny<int>()))
                        .Returns(Task.FromResult(new StudentInLesson()));
            }
            else
            {
                _teacherService.Setup(w => w.StudentInLessonExistsAsync(
                   It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupGetParentsByStudentIdAsync(
            SetupKey setupKey, List<Parent> parents = null) 
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.GetParentsByStudentIdAsync(
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }

            if (setupKey == SetupKey.EmptyCollection)
            {
                _teacherService.Setup(w => w.GetParentsByStudentIdAsync(
                    It.IsAny<int>()))
                        .Returns(Task.FromResult(new List<Parent>(parents)));
            }

            if (setupKey == SetupKey.NotEmptyCollection)
            {
                _teacherService.Setup(w => w.GetParentsByStudentIdAsync(
                    It.IsAny<int>()))
                        .Returns(Task.FromResult(new List<Parent>(parents)));
            }
        }

        private void _SetupHomeworkStatusExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _teacherService.Setup(w => w.HomeworkStatusExistsAsync(
                    It.IsAny<int>())).Returns(
                        Task.FromResult(new HomeworkStatus()));
            }
            else
            {
                _teacherService.Setup(w => w.HomeworkStatusExistsAsync(
                    It.IsAny<int>())).Throws(new HttpStatusException(
                        _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupGetHomeworkProgressStatusAsync(SetupKey setupKey, 
            HomeworkStatuses  status = HomeworkStatuses.Appointed)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                HomeworkProgressStatus homeworkProgress = new HomeworkProgressStatus()
                {
                    HomeworkStatus = new HomeworkStatus()
                    {
                        Id = (int)status
                    }
                };

                _teacherService.Setup(w => w.GetHomeworkProgressStatusAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(homeworkProgress));

            }
            else
            {
                _teacherService.Setup(w => w.GetHomeworkProgressStatusAsync(
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupGetStudentsByClassIdAsync(
            SetupKey setupKey, List<Student> students = null)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.GetStudentsByClassIdAsync(
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }

            if(setupKey == SetupKey.EmptyCollection)
            {
                _teacherService.Setup(w => w.GetStudentsByClassIdAsync(
                    It.IsAny<int>()))
                        .Returns(Task.FromResult(
                            new List<Student>(students)));
            }

            if(setupKey == SetupKey.NotEmptyCollection)
            {
                _teacherService.Setup(w => w.GetStudentsByClassIdAsync(
                    It.IsAny<int>()))
                        .Returns(Task.FromResult(new List<Student>(students)));
            }
        }

        private void _SetupAddLessonAsync(SetupKey setupKey)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.AddLessonAsync(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(),
                    It.IsAny<string>(), 
                    It.IsAny<DateTime>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupUpdateGradeHomeworkAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.UpdateGradeHomeworkAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupUpdateGradeStudentInLesson(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.UpdateGradeStudentInLessonAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupUpdateCommentAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.UpdateCommentAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupGetCompletedHomeworkByStudentInLessonId(
            SetupKey setupKey, 
            string homework = null)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                CompletedHomework completedHomework = new CompletedHomework()
                {
                    Work = homework
                };

                _teacherService.Setup(w => w.GetCompletedHomeworkByStudentInLessonIdAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(completedHomework));
            }
            else
            {
                _teacherService.Setup(w => w.GetCompletedHomeworkByStudentInLessonIdAsync(
                     It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupAddHomeworkProgressStatusAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.AddHomeworkProgressStatusAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupAddTeacherParentMeetingAsync(SetupKey setupKey)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.AddTeacherParentMeetingAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupAddHomeworkAsync(SetupKey setupKey)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.AddHomeworkAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupRemoveTeacherParentMeeting(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _teacherService.Setup(w => w.RemoveTeacherParentMeeting(
                    It.IsAny<int>()))
                        .Throws(new HttpStatusException(
                            HttpStatusCode.InternalServerError));
            }
        }

        private void _SetupMappingList<T, Tdtos>(List<Tdtos> values)
        {
            _mapper.Setup(m => m.Map<List<Tdtos>>(
                It.IsAny<List<T>>()))
                    .Returns(values);
        }

        private void _SetupMapping<T, Tdtos>(Tdtos value)
        {
            _mapper.Setup(m => m.Map<Tdtos>(It.IsAny<T>())).Returns(value); 
        }

        #endregion Setup

        #region Tests

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetStudents_ClassExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupClassExistsAsync(SetupKey.InternalServerError);

             var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetStudents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetStudents_GetStudentsAsyncThrowException_ReturnsInternalServerErrror()
        {
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentsByClassIdAsync(SetupKey.InternalServerError);
            

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetStudents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetStudents_ClassExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupClassExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetStudents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetStudents_GetStudentsReturnsEmptyCollection_ReturnsOk()
        {
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentsByClassIdAsync(
                SetupKey.EmptyCollection, new List<Student>());

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetStudents(It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.OK);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_COLLECTION);
            });
        }

        [Test]
        public async Task GetStudents_ClassExistsAsync_ReturnsNotEmptyCollection()
        {
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentsByClassIdAsync(
                SetupKey.NotEmptyCollection, FakeData.GetStudents());
            _SetupMappingList<Student, StudentReadDto>(
                FakeData.GetStudentsReadDto(FakeData.GetStudents()));

            var result = await _teachersController.GetStudents(It.IsAny<int>());
            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(statusCode, HttpStatusCode.OK);
                Assert.IsNotEmpty(result.Value);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetParents_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetParents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetParents_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetParents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetParents_GetParentsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetParentsByStudentIdAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetParents(It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void GetParents_GetParentsAsyncReturnsEmptyCollection_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetParentsByStudentIdAsync(SetupKey.EmptyCollection, new List<Parent>());

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GetParents(It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.OK);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_COLLECTION);
            });
        }

        [Test]
        public async Task GetParents_GetParentsAsyncReturnsNotEmptyCollection_ReturnsNotEmptyCollection()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetParentsByStudentIdAsync(
                SetupKey.NotEmptyCollection, 
                FakeData.GetParents());
            _SetupMappingList<Parent, ParentReadDto>(
                FakeData.GetParentsReadDto(
                    FakeData.GetParents()));

            var result = await _teachersController.GetParents(It.IsAny<int>());

            var  statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(statusCode, HttpStatusCode.OK);
                Assert.IsNotEmpty(result.Value);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateLesson_TeacherExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupTeacherExistsAsync(SetupKey.InternalServerError);
           
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateLesson(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateLesson_ClassExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateLesson(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateLesson_SubjectExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateLesson(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateLesson_TeacherClassSubjectExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateLesson(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateLesson_TeacherExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupTeacherExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<string>(),
                   It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateLesson_ClassExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<string>(),
                   It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateLesson_SubjectExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<string>(),
                   It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateLesson_TeacherClassSubjectExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<string>(),
                   It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null)]
        [TestCase("")]
        public void CreateLesson_ThemeIsIncorrect_ReturnsBadRequest(string theme)
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateLesson(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    theme,
                    It.IsAny<DateTime>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_STRING);
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void CreateLesson_LessonDateTimeIsIncorrect_ReturnsBadRequest()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   FakeData.Values.NotEmptyString,
                   FakeData.Values.DateTimeMinusOneDay));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                    _options.Object.Value[MessageKey.DateTimeNotValid.ToString()]);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateLesson_AddLessonAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupAddLessonAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   FakeData.Values.NotEmptyString,
                   FakeData.Values.DateTimePlusOneDay));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task CreateLesson_AddLessonAsync_ReturnsOk()
        {
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupClassExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherClassSubjectExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.CreateLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   FakeData.Values.NotEmptyString,
                   FakeData.Values.DateTimePlusOneDay);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateHomework_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateHomework(
                    It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void CreateHomework_finishDateTimeIsIncorrect_ReturnsBadRequest()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateHomework(
                     It.IsAny<int>(), 
                     FakeData.Values.DateTimeMinusOneDay,
                     It.IsAny<string>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                    _options.Object.Value[MessageKey.DateTimeNotValid.ToString()]);
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null)]
        [TestCase("")]
        public void CreateHomework_HomeworkIsIncorrect_ReturnsBadRequest(
            string homework)
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateHomework(
                    It.IsAny<int>(), FakeData.Values.DateTimePlusOneDay, homework));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_STRING);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateHomework_AddHomeworkAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupAddHomeworkAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateHomework(
                    It.IsAny<int>(),
                    FakeData.Values.DateTimePlusOneDay,
                    FakeData.Values.NotEmptyString));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }


        [Test]
        public async Task CreateHomework_AddHomeworkAsync_ReturnsOk()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
           
            var result = await _teachersController.CreateHomework(
                It.IsAny<int>(),
                FakeData.Values.DateTimePlusOneDay,
                FakeData.Values.NotEmptyString);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetHomeworkStatus_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetHomeworkStatus_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetHomeworkStatus_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetHomeworkStatus_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetHomeworkStatus_HomeworkExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetHomeworkStatus_HomeworkExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetHomeworkStatus_StudentInLessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetHomeworkStatus_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetHomeworkStatus_GetHomeworkProgressStatusAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetHomeworkStatus_GetHomeworkProgressStatusAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetHomeworkStatus(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetHomeworkStatus_GetHomeworkProgressStatusAsync_ReturnsOk()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.GetHomeworkStatus(
                It.IsAny<int>(),
                It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateTeacherParentMeeting_StudentExistAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateTeacherParentMeeting(
                    It.IsAny<int>(),
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateTeacherParentMeeting_TeacherExistAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateTeacherParentMeeting(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateTeacherParentMeeting_ParentExistAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentsExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateTeacherParentMeeting(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateTeacherParentMeeting_StudentExistsAsyncNotFound_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateTeacherParentMeeting(
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateTeacherParentMeeting_TeacherExistsAsyncNotFound_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateTeacherParentMeeting(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateTeacherParentMeeting_ParentExistsAsyncNotFound_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentsExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateTeacherParentMeeting(
                      It.IsAny<int>(),
                      It.IsAny<int>(),
                      It.IsAny<int>(),
                      It.IsAny<DateTime>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void CreateTeacherParentMeeting_MeetingDateTimeIncorrect_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentsExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateTeacherParentMeeting(
                     It.IsAny<int>(),
                     It.IsAny<int>(),
                     It.IsAny<int>(),
                     FakeData.Values.DateTimeMinusOneDay));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                    _options.Object.Value[MessageKey.DateTimeNotValid.ToString()]);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateTeacherParentMeeting_AddTeacherParentMeetingAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentsExistsAsync(SetupKey.ReturnsValue);
            _SetupAddTeacherParentMeetingAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateTeacherParentMeeting(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    FakeData.Values.DateTimePlusOneDay));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        
        [Test]
        public async Task CreateTeacherParentMeeting_AddTeacherParentMeetingAsync_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentsExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.CreateTeacherParentMeeting(
                It.IsAny<int>(),
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                FakeData.Values.DateTimePlusOneDay);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_StudentInLessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_HomeworkByLessonIdExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.InternalServerError);
            
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_HomeworkStatusExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupHomeworkStatusExistsAsync(SetupKey.InternalServerError);
           
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void ChangeStatusHomework_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.NotFound);
           
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void ChangeStatusHomework_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void ChangeStatusHomework_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void ChangeStatusHomework_HomeworkExistsByLessonIdAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void ChangeStatusHomework_HomeworkStatusExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupHomeworkStatusExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void ChangeStatusHomework_AddHomeworkProgressStatusAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupHomeworkStatusExistsAsync(SetupKey.ReturnsValue);
            _SetupAddHomeworkProgressStatusAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.ChangeStatusHomework(
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task ChangeStatusHomework_AddHomeworkProgressStatusAsync_ReturnsOk()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupHomeworkStatusExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.ChangeStatusHomework(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_HomeworkExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_StudentInLessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }


        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCompletedHomework_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.NotFound);
           
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCompletedHomework_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCompletedHomework_HomeworkExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCompletedHomework_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_GetHomeworkProgressStatusAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        
        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(HomeworkStatuses.InProgress)]
        [TestCase(HomeworkStatuses.Appointed)]
        [TestCase(HomeworkStatuses.Rated)]
        [TestCase(HomeworkStatuses.TeacherCheck)]
        [TestCase(HomeworkStatuses.ParentCheck)]
        public void GetCompletedHomework_IncorrectHomeworkProgressStatus_ReturnsBadRequest(HomeworkStatuses status)
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue, status);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                    _options.Object.Value[MessageKey.HomeworkStatusNotValid.ToString()]);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCompletedHomework_GetCompletedHomeworkByStudentInLessonIdAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue, 
                HomeworkStatuses.Completed);
            _SetupGetCompletedHomeworkByStudentInLessonId(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCompletedHomework_GetCompletedHomeworkByStudentInLessonIdAsyncThrowException_ReturnsNotFound()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue,
                HomeworkStatuses.Completed);
            _SetupGetCompletedHomeworkByStudentInLessonId(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }


        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null)]
        [TestCase("")]
        public void GetCompletedHomework_GetCompletedHomeworkByStudentInLessonIncorrectCompletedHomework_ReturnsBadRequest(
            string completedHomework)
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue,
                HomeworkStatuses.Completed);
            _SetupGetCompletedHomeworkByStudentInLessonId(SetupKey.ReturnsValue, completedHomework);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GetCompletedHomework(
                     It.IsAny<int>(),
                     It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_STRING);
            });
        }

        [Test]
        public async Task GetCompletedHomework_GetCompletedHomeworkByStudentInLessonIdAsync_ReturnsOk()
        {
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue,
                HomeworkStatuses.Completed);
            _SetupGetCompletedHomeworkByStudentInLessonId(
                SetupKey.ReturnsValue, FakeData.Values.NotEmptyString);

            _SetupMapping<CompletedHomework, CompletedHomeworkReadDto>(
                FakeData.GetCompletedHomeworkReadDto(
                    FakeData.GetCompletedHomework()));

           var result = await _teachersController.GetCompletedHomework(
                It.IsAny<int>(),
                It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_StudentInLessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_HomeworkExistsByLessonIdAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.InternalServerError);    

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_GetHomeworkProgressStatusAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeHomework_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeHomework_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeHomework_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeHomework_HomeworkByLessonIdExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeHomework_GetHomeworkProgressStatusIdAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeHomework(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.NotValidGrades))]
        public void GradeHomework_IncorrectGradeValue_ReturnsBadRequest(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.GradeHomework(
                    It.IsAny<int>(), It.IsAny<int>(), grade));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                     _options.Object.Value[MessageKey.GradeNotValid.ToString()]);
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.ValidGrades))]
        public async Task GradeHomework_CorrectGradeValue_ReturnsOk(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.GradeHomework(
                  It.IsAny<int>(), It.IsAny<int>(), grade);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeHomework_UpdateGradeHomeworkThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue);
            _SetupUpdateGradeHomeworkAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _teachersController.GradeHomework(
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  FakeData.ValidGrades[0]));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GradeHomework_UpdateGradeHomework_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupHomeworkExistsByLessonIdAsync(SetupKey.ReturnsValue);
            _SetupGetHomeworkProgressStatusAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.GradeHomework(
                It.IsAny<int>(), It.IsAny<int>(), FakeData.ValidGrades[0]);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateComment_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateComment_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateComment_StudentInLessonsExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateComment_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateComment_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateComment_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null)]
        [TestCase("")]
        public void CreateComment_CommentIncorrect_ReturnsBadRequest(string comment)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.CreateComment(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    comment));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message, ResultHandler.EMPTY_STRING);
            });
        }

        [Test]
        public void CreateComment_UpdateCommentAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupUpdateCommentAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.CreateComment(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   FakeData.Values.NotEmptyString));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task CreateComment_UpdateCommentAsyncReturnsOk_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.CreateComment(
                It.IsAny<int>(), It.IsAny<int>(),FakeData.Values.NotEmptyString);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeStudentInLesson_StudentExistAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeStudentInLesson_StudentExistAsyncThrowsException_ReturnsItemNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeStudentInLesson_LessonExistAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeStudentInLesson_LessonExistAsyncThrowsException_ReturnsItemNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeStudentInLesson_StudentInLessonExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GradeStudentInLesson_StudentInLessonExistAsyncThrowsException_ReturnsItemNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _teachersController.GradeStudentInLesson(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.NotValidGrades))]
        public void GradeStudentInLesson_IncorrectGradeValue_ReturnsBadRequest(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _teachersController.GradeStudentInLesson(
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  grade));

            Assert.Multiple(() =>
            {
                Assert.AreEqual(exception.Status, HttpStatusCode.BadRequest);
                Assert.AreEqual(exception.Message,
                     _options.Object.Value[MessageKey.GradeNotValid.ToString()]);
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.ValidGrades))]
        public async Task GradeStudentInLesson_CorrectGradeValue_ReturnsOk(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.GradeStudentInLesson(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 grade);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GradeStudentInLesson_UpdateGradeStudentInLesson_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupUpdateGradeStudentInLesson(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _teachersController.GradeStudentInLesson(
                  It.IsAny<int>(),
                  It.IsAny<int>(),
                  FakeData.ValidGrades[0]));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GradeStudentInLesson_UpdateGradeStudentInLesson_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);

            var result = await _teachersController.GradeStudentInLesson(
                It.IsAny<int>(), It.IsAny<int>(), FakeData.ValidGrades[0]);

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void RemoveTeacherParentMeeting_TeacherParentMeetingExistsThrowsException_ReturnsInternalServerError()
        {
            _SetupTeacherParentMeetingExists(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _teachersController.RemoveTeacherParentMeeting(
                    It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void RemoveTeacherParentMeeting_TeacherParentMeetingExistsThrowsException_ReturnsNotFound()
        {
            _SetupTeacherParentMeetingExists(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _teachersController.RemoveTeacherParentMeeting(
                 It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.NotFound);
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void RemoveTeacherParentMeeting_RemoveTeacherParentMeetingThrowsException_ReturnsInternalServerError()
        {
            _SetupTeacherParentMeetingExists(SetupKey.ReturnsValue);
            _SetupRemoveTeacherParentMeeting(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _teachersController.RemoveTeacherParentMeeting(
                 It.IsAny<int>()));

            Assert.AreEqual(exception.Status, HttpStatusCode.InternalServerError);
        }

     
        [Test]
        public async Task RemoveTeacherParentMeeting_RemoveTeacherParentMeeting_ReturnsOk()
        {
            _SetupTeacherParentMeetingExists(SetupKey.ReturnsValue);

            var result = await _teachersController.RemoveTeacherParentMeeting(
                It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        #endregion Tests 
    }
}

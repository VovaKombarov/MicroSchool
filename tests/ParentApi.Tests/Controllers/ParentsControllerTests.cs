using AutoMapper;
using Common.ErrorResponse;
using Common.EventBus;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ParentApi.Controllers;
using ParentApi.Data.Dtos;
using ParentApi.Models;
using ParentApi.Services;
using ParentApi.Tests.Utilities;
using ParentApi.Utilities;
using System;
using System.Net;
using Common.TestsUtils;
using Common.Api;
using Microsoft.Extensions.Logging;

namespace ParentApi.Tests.Controllers
{
    public class ParentsControllerTests
    {
        #region Fields

        private ParentsController _parentsController;

        private Mock<IParentService> _parentService;

        private Mock<IMapper> _mapper;

        private Mock<IOptions<Dictionary<string, string>>> _options;

        private Mock <IEventBus> _eventBus;

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
            _parentService = new Mock<IParentService>();
            _options = new Mock<IOptions<Dictionary<string, string>>>();
            _eventBus = new Mock<IEventBus>();
            _logger = new Mock<ILogger>();

            _parentsController = new ParentsController(
                _mapper.Object,
                _parentService.Object,
                _eventBus.Object,
                _options.Object, 
                _logger.Object);

            Invoker.InitLogger(_logger.Object);

            _SetupOptions();
        }

        private void _SetupGetCompletedHomeworkAsync(
            SetupKey setupKey, 
            CompletedHomework completedHomework)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.GetCompletedHomeworkAsync(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(completedHomework));

            }
            else
            {
                _parentService.Setup(w => w.GetCompletedHomeworkAsync(
                     It.IsAny<int>(),
                     It.IsAny<int>()))
                    .Throws(new HttpStatusException(_GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupRemoveParentTeacherMeetingAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _parentService.Setup(w => w.RemoveParentTeacherMeetingAsync(
                    It.IsAny<int>()))
                    .Throws(new HttpStatusException(HttpStatusCode.InternalServerError));
            }
            else
            {
                _parentService.Setup(w => w.RemoveParentTeacherMeetingAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(0));
                    
            }

        }

        private void _SetupAddTeacherParentMeetingAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _parentService.Setup(w => w.AddTeacherParentMeetingAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()))
                    .Throws(new HttpStatusException(HttpStatusCode.InternalServerError));
            }

        }

        private void _SetupStudentInLessonExistsAsync(
            SetupKey setupKey, 
            StudentInLesson studentInLesson = null)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                if(studentInLesson == null)
                {
                    studentInLesson = new StudentInLesson();
                }

                _parentService.Setup(w => w.StudentInLessonExistsAsync(
                    It.IsAny<int>(), It.IsAny<int>()))
                        .Returns(Task.FromResult(studentInLesson));
            }
            else
            {
                _parentService.Setup(w => w.StudentInLessonExistsAsync(
                    It.IsAny<int>(), It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupStudentExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.StudentExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Student()));
            }
            else
            {
                _parentService.Setup(w => w.StudentExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupTeacherExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.TeacherExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Teacher()));
            }
            else
            {
                _parentService.Setup(w => w.TeacherExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupSubjectExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.SubjectExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Subject()));
            }
            else
            {
                _parentService.Setup(w => w.SubjectExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupParentExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.ParentExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Parent()));
            }
            else
            {
                _parentService.Setup(w => w.ParentExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupTeacherParentMeetingExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.NotFound)
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                   It.IsAny<int>())).Returns(Task.FromResult<TeacherParentMeeting>(null));
            }

            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new TeacherParentMeeting()));
            }
            else
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                    It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupTeacherParentMeetingExistsAsyncWithFourArguments(SetupKey setupKey)
        {
            if (setupKey == SetupKey.NotFound)
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<DateTime>())).Returns(Task.FromResult<TeacherParentMeeting>(null));
            }

            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<DateTime>())).Returns(Task.FromResult(new TeacherParentMeeting()));
            }
            else
            {
                _parentService.Setup(w => w.TeacherParentMeetingExistsAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupLessonExistsAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.ReturnsValue)
            {
                _parentService.Setup(w => w.LessonExistsAsync(
                    It.IsAny<int>())).Returns(Task.FromResult(new Lesson()));
            }
            else
            {
                _parentService.Setup(w => w.LessonExistsAsync(It.IsAny<int>()))
                   .Throws(new HttpStatusException(
                       _GetHttpStatusCodeBySetupKey(setupKey)));
            }
        }

        private void _SetupGetStudentInLessonsAsync(
            SetupKey setupKey, List<StudentInLesson> studentInLessons = null)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _parentService.Setup(w => w.GetStudentInLessonsAsync(It.IsAny<int>()))
                    .Throws(new HttpStatusException(HttpStatusCode.InternalServerError));
            }
            else
            {
                _parentService.Setup(w => w.GetStudentInLessonsAsync(It.IsAny<int>()))
                   .Returns(Task.FromResult(new List<StudentInLesson>(studentInLessons)));
            }
        }

        private void _SetupGetGrades(SetupKey setupKey, List<int> grades = null)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _parentService.Setup(w => w.GetGradesAsync(It.IsAny<int>(), It.IsAny<int>()))
                    .Throws(new HttpStatusException(HttpStatusCode.InternalServerError));
            }
            else
            {
                _parentService.Setup(w => w.GetGradesAsync(It.IsAny<int>(), It.IsAny<int>()))
                   .Returns(Task.FromResult(new List<int>(grades)));
            }
        }

        private void _SetupMapping<T, Tdtos>(Tdtos value)
        {
            _mapper.Setup(m => m.Map<Tdtos>(It.IsAny<T>())).Returns(value);
        }

        private void _SetupMappingList<T, Tdtos>(List<Tdtos> values)
        {
            _mapper.Setup(m => m.Map<List<Tdtos>>(
                It.IsAny<List<T>>()))
                    .Returns(values);
        }

        private void _SetupOptions()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "HomeworkStatusNotValid", "Статус домашней работы некорректный!"},
                { "GradeNotValid", "Оценка не валидна!"},
                { "DateTimeNotValid", "Время невалидно!"},
                {  "CommentMissing", "Замечание отсутствует!"},
                 { "NoRating", "Нет оценки!"},
                  { "DuplicateItem", "Повторяющийся элемент!"},

            };

            _options.Setup(w => w.Value).Returns(keyValuePairs);
        }

        #endregion Setup

        #region Tests 

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCommentsAsync_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _parentsController.GetComments(It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCommentsAsync_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _parentsController.GetComments(It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCommentsAsync_GetStudentInLessonsAsync_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentInLessonsAsync(SetupKey.InternalServerError, null);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _parentsController.GetComments(It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public void GetCommentsAsync_GetStudentInLessonsAsync_ReturnsEmptyCollection()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentInLessonsAsync(
                SetupKey.EmptyCollection, new List<StudentInLesson>());

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComments(It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(exception.Message, Is.EqualTo(ResultHandler.EMPTY_COLLECTION));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void GetCommentsAsync_GetStudentInLessonsAsyncNoComments_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentInLessonsAsync(
                SetupKey.NotEmptyCollection, FakeData.GetStudentInLessonsWithoutComments());

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComments(It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo(
                    _options.Object.Value[MessageKey.CommentMissing.ToString()]));
            });
        }

        [Test]
        public async Task GetCommentsAsync_GetStudentInLessonsAsyncAreComments_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupGetStudentInLessonsAsync(
                SetupKey.NotEmptyCollection, 
                FakeData.GetStudentInLessonsWithComments());
            _SetupMappingList<StudentInLesson, CommentReadDto>(
                FakeData.GetCommentsReadDto(
                    FakeData.GetStudentInLessonsWithComments()));

            var result = await _parentsController.GetComments(It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.Multiple(() =>
            {
                Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.IsNotEmpty(result.Value);
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test] 
        public void GetCommentAsync_StudentExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCommentAsync_StudentExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCommentAsync_LessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCommentAsync_LessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetCommentAsync_StudentInLessonExistsAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetCommentAsync_StudentInLessonExistsAsyncThrowException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null)]
        [TestCase("")]
        public void GetCommentAsync_InсorrectCommentValue_ReturnsBadRequest(string comment)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue, 
                FakeData.GetStudentInLesson(comment));

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.GetComment(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task GetCommentAsync_GetCommentAsync_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue,
                FakeData.GetStudentInLesson(FakeData.Values.NotEmptyString));

            _SetupMapping<StudentInLesson, CommentReadDto>(
               FakeData.GetCommentReadDto(
                   FakeData.GetStudentInLesson(FakeData.Values.NotEmptyString)));

            var result = await _parentsController.GetComment(
               It.IsAny<int>(),
               It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrade_StudentExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

           var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
           await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetGrade_StudentExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrade_LessonExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetGrade_LessonExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrade_StudentInLessonExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetGrade_StudentInLessonExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrade_GetCompletedHomeworkThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetCompletedHomeworkAsync(SetupKey.InternalServerError, null);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _parentsController.GetGrade(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void GetGrade_GetCompletedHomeworkNoGrade_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetCompletedHomeworkAsync(
                SetupKey.ReturnsValue, FakeData.GetCompletedHomework(null));

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(
               It.IsAny<int>(),
               It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(
                    _options.Object.Value[MessageKey.NoRating.ToString()], 
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.NotValidGrades))]
        public void GetGrade_GetCompletedHomeworkNotValidGrade_ReturnsBadRequest(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetCompletedHomeworkAsync(
                SetupKey.ReturnsValue, FakeData.GetCompletedHomework(grade));

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrade(
               It.IsAny<int>(),
               It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(
                    _options.Object.Value[MessageKey.GradeNotValid.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

       
        [Test, TestCaseSource(typeof(FakeData), nameof(FakeData.ValidGrades))]
        public async Task GetGrade_GetCompletedHomeworkValidGrade_ReturnsOk(int grade)
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupStudentInLessonExistsAsync(SetupKey.ReturnsValue);
            _SetupGetCompletedHomeworkAsync(
                SetupKey.ReturnsValue, FakeData.GetCompletedHomework(grade));

            var result = await _parentsController.GetGrade(
                 It.IsAny<int>(),
                 It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrades_StudentExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetGrades_StudentExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrades_SubjectExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void GetGrades_SubjectExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void GetGrades_GetGradesAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupGetGrades(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public void GetGrades_GetGradesEmptyCollection_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupGetGrades(SetupKey.ReturnsValue, new List<int>());

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.GetGrades(It.IsAny<int>(), It.IsAny<int>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(exception.Message,Is.EqualTo(ResultHandler.EMPTY_COLLECTION));
            });
        }

        [Test]
        public async Task GetGrades_GetGradesNotEmptyCollection_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupSubjectExistsAsync(SetupKey.ReturnsValue);
            _SetupGetGrades(SetupKey.ReturnsValue, FakeData.ValidGrades.ToList());

            var result = await _parentsController.GetGrades(
                It.IsAny<int>(),
                It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void RemoveParentTeacherMeeting_TeacherParentMeetingExistsThrowsException_ReturnsInternalServerError()
        {
            _SetupTeacherParentMeetingExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.RemoveParentTeacherMeeting(
                 It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void RemoveParentTeacherMeeting_RemoveParentTeacherMeetingExistsThrowsException_ReturnsNotFound()
        {
            _SetupTeacherParentMeetingExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.RemoveParentTeacherMeeting(
                 It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void RemoveParentTeacherMeeting_RemoveParentTeacherMeetingThrowsException_ReturnsInternalServerError()
        {
            _SetupTeacherParentMeetingExistsAsync(SetupKey.ReturnsValue);
            _SetupRemoveParentTeacherMeetingAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.RemoveParentTeacherMeeting(
                 It.IsAny<int>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }


        [Test]
        public async Task RemoveParentTeacherMeeting_RemoveTeacherParentMeeting_ReturnsOk()
        {
            _SetupTeacherParentMeetingExistsAsync(SetupKey.ReturnsValue);
            _SetupRemoveParentTeacherMeetingAsync(SetupKey.ReturnsValue);

            var result = await _parentsController.RemoveParentTeacherMeeting(
                              It.IsAny<int>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateParentTeacherMeeting_StudentExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(), 
                 It.IsAny<int>(), 
                 It.IsAny<int>(), 
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateParentTeacherMeeting_StudentExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateParentTeacherMeeting_TeacherExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateParentTeacherMeeting_TeacherExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateParentTeacherMeeting_ParentExistsAsyncThrowsException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.NOT_FOUND)]
        [Test]
        public void CreateParentTeacherMeeting_ParentExistsAsyncThrowsException_ReturnsNotFound()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.NotFound);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
             await _parentsController.CreateParentTeacherMeeting(
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<int>(),
                 It.IsAny<DateTime>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void CreateParentTeacherMeeting_IncorrectMeetingDateTime_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.CreateParentTeacherMeeting(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                FakeData.Values.DateTimeMinusOneDay));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo(
                    _options.Object.Value[MessageKey.DateTimeNotValid.ToString()]));
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateParentTeacherMeeting_TeacherParentMeetingExistsThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherParentMeetingExistsAsyncWithFourArguments(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.CreateParentTeacherMeeting(
              It.IsAny<int>(),
              It.IsAny<int>(),
              It.IsAny<int>(),
              FakeData.Values.DateTimePlusOneDay));

              Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void CreateParentTeacherMeeting_TeacherParentMeetingExistsReturnsValue_ReturnsBadRequest()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherParentMeetingExistsAsyncWithFourArguments(SetupKey.ReturnsValue);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.CreateParentTeacherMeeting(
              It.IsAny<int>(),
              It.IsAny<int>(),
              It.IsAny<int>(),
              FakeData.Values.DateTimePlusOneDay));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo(
                    _options.Object.Value[MessageKey.DuplicateItem.ToString()]));
            });
        }


        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void CreateParentTeacherMeeting_AddTeacherParentMeetingAsyncThrowException_ReturnsInternalServerError()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.ReturnsValue);
            _SetupAddTeacherParentMeetingAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
            await _parentsController.CreateParentTeacherMeeting(
             It.IsAny<int>(),
             It.IsAny<int>(),
             It.IsAny<int>(),
             FakeData.Values.DateTimePlusOneDay));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task CreateParentTeacherMeeting_AddTeacherParentMeetingAsync_ReturnsOk()
        {
            _SetupStudentExistsAsync(SetupKey.ReturnsValue);
            _SetupTeacherExistsAsync(SetupKey.ReturnsValue);
            _SetupParentExistsAsync(SetupKey.ReturnsValue);

            var result = await _parentsController.CreateParentTeacherMeeting(
                               It.IsAny<int>(),
                               It.IsAny<int>(),
                               It.IsAny<int>(),
                               FakeData.Values.DateTimePlusOneDay);      

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        #endregion Tests
    }
}

using ParentApi.Data.Dtos;
using ParentApi.Models;

namespace ParentApi.Tests.Utilities
{
    internal static class FakeData
    {
        #region Nested

        public record TestData(
            DateTime DateTimePlusOneDay,
            DateTime DateTimeMinusOneDay,
            string NotEmptyString);

        #endregion Nested

        #region Constructors

        static FakeData()
        {
            Values = new(
               DateTime.Now.AddDays(+1),
               DateTime.Now.AddDays(-1),
               "NotEmptyString");
        }

        #endregion Constructors

        #region Properties

       public static TestData Values { get; set; }

       public static int[] NotValidGrades = { 0, 1, 6};

       public static int[] ValidGrades = { 2, 3, 4, 5 };

        #endregion Properties

        #region Methods

        public static List<StudentInLesson> GetStudentInLessonsWithoutComments()
        {
            return new List<StudentInLesson>()
            {
                new StudentInLesson()
                {
                      Id = 1,
                      Comment = null,
                      Lesson = new Lesson(),
                      Student = new Student()
                },

                new StudentInLesson()
                {
                      Id = 2,
                      Comment = null,
                      Lesson = new Lesson(),
                      Student = new Student()
                },
            };
        }

        public static List<StudentInLesson> GetStudentInLessonsWithComments()
        {
            return new List<StudentInLesson>()
            {
                new StudentInLesson()
                {
                      Id = 1,
                      Comment = "Замечание 1",
                      Lesson = new Lesson()
                      {
                          Id = 1,
                          LessonDT = DateTime.Now,
                          TeacherClassSubject = new TeacherClassSubject(),
                          Theme = "Тема урока 1"
                      },
                      Student = new Student()
                },

                new StudentInLesson()
                {
                      Id = 2,
                      Comment = "Замечание 2",
                      Lesson = new Lesson()
                      {
                          Id = 2,
                          LessonDT = DateTime.Now,
                          TeacherClassSubject = new TeacherClassSubject(),
                          Theme = "Тема урока 2"
                      },
                      Student = new Student()
                },
            };
        }

        public static List<CommentReadDto> GetCommentsReadDto(
            List<StudentInLesson> studentInLessons)
        {
            List<CommentReadDto> commentReadDtos = new List<CommentReadDto>();  

            foreach(var item in studentInLessons)
            {
                commentReadDtos.Add(new CommentReadDto()
                {
                    Comment = item.Comment,
                    Lesson = item.Lesson.Theme
                });
            }

            return commentReadDtos;
        }

        public static CommentReadDto GetCommentReadDto(StudentInLesson studentInLesson)
        {
            return new CommentReadDto()
            {
                Comment = studentInLesson.Comment,
                Lesson = studentInLesson.Lesson.Theme
            };
        }

        public static StudentInLesson GetStudentInLesson(string comment)
        {
            return new StudentInLesson()
            {
                Comment = comment,
                Lesson = new Lesson()
                {
                    Theme = "Тема 1"
                }
            };
        }

        public static CompletedHomework GetCompletedHomework(int? grade)
        {
            return new CompletedHomework()
            {
                Work = Values.NotEmptyString,
                Grade = grade
            };
        }

        #endregion Methods

    }
}

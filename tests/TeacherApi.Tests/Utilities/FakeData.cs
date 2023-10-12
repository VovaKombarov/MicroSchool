using System;
using System.Collections.Generic;
using TeacherApi.Data.Dtos;
using TeacherApi.Models;

namespace TeacherApi.Tests.Utilities
{
    internal static class FakeData
    {
        #region Nested
        public record TestData(
            DateTime DateTimePlusOneDay,
            DateTime DateTimeMinusOneDay,
            string NotEmptyString);

        #endregion Nested

        #region Static Constructors

        static FakeData()
        {
            Values = new(
               DateTime.Now.AddDays(+1),
               DateTime.Now.AddDays(-1),
               "NotEmptyString");
        }

        #endregion Static Constructors

        #region Static Fields 

        public static int[] notValidGrades = { 0, 1, 6, 10 };

        public static int[] validGrades = { 2, 3, 4, 5 };

        #endregion Static Fields

        #region Properties

        public static TestData Values { get; set; }

        #endregion Properties

        #region Static Methods

        public static List<StudentReadDto> GetStudentsReadDto(
           List<Student> students)
        {
            List<StudentReadDto> studentsReadDtos = new List<StudentReadDto>();

            foreach (Student student in students)
            {
                StudentReadDto studentReadDto = new StudentReadDto()
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    BirthDate = student.BirthDate.ToString(),
                    Patronymic = student.Patronymic
                };

                studentsReadDtos.Add(studentReadDto);
            }

            return studentsReadDtos;
        }

        public static List<Student> GetStudents()
        {
            return new List<Student>()
            {
                new Student()
                {
                    Name = "Аглая",
                    Patronymic = "Ивановна",
                    Surname = "Епанчина",
                    BirthDate = new DateTime(2008, 7, 2)
                }
            };
        }

        public static List<Parent> GetParents()
        {
            return new List<Parent>()
            {
                new Parent()
                {
                    Id = 1,
                    Patronymic = "Прокофьевна",
                    Name = "Лизавета",
                    Surname = "Епанчина"
                }
            };
        }


        public static List<ParentReadDto> GetParentsReadDto(
            List<Parent> parents)
        {
            List<ParentReadDto> parentsReadDtos = new List<ParentReadDto>();

            foreach (Parent item in parents)
            {
                ParentReadDto parentReadDto = new ParentReadDto()
                {
                    Name = item.Name,
                    Patronymic = item.Patronymic,
                    Surname = item.Surname
                };

                parentsReadDtos.Add(parentReadDto);
            }

            return parentsReadDtos;
        }

        public static CompletedHomework GetCompletedHomework()
        {
            return new CompletedHomework()
            {
                Work = "Домашняя работа"
            };
        }

        public static CompletedHomeworkReadDto GetCompletedHomeworkReadDto(
            CompletedHomework completedHomework)  
        {
            return new CompletedHomeworkReadDto()
            {
                Work = completedHomework.Work
            };
        }
    }

    #endregion Static Methods
}

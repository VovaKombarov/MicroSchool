using AutoMapper;
using TeacherApi.Data.Dtos;
using TeacherApi.Models;

namespace TeachersApi.Profiles
{
    /// <summary>
    /// Маппинг сущностей из БД в сущности для чтения.
    /// </summary>
    public class TeacherProfile : Profile
    {
        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TeacherProfile()
        {
            CreateMap<CompletedHomework, CompletedHomeworkReadDto>();
            CreateMap<Student, StudentReadDto>();
            CreateMap<Parent, ParentReadDto>();
        }

        #endregion Constructors
    }
}

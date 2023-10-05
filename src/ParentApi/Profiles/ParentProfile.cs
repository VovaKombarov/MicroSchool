using AutoMapper;
using ParentApi.Data.Dtos;
using ParentApi.Models;

namespace ParentApi.Profiles
{
    /// <summary>
    /// Маппинг сущностей бд на сущности для чтения.
    /// </summary>
    public class ParentProfile : Profile
    {
        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParentProfile() {
            CreateMap<StudentInLesson, CommentReadDto>()
                .ForMember(dest => dest.Lesson,
                            opt => opt.MapFrom(src => 
                                src.Lesson.Theme));

        }

        #endregion Constructors
    }
}

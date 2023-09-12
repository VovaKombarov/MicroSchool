using System;
using AutoMapper;
using ParentApi.Data.Dtos;
using ParentApi.Models;

namespace ParentApi.Profiles
{
    public class ParentProfile : Profile
    {
        public ParentProfile() {
            CreateMap<StudentInLesson, CommentReadDto>()
                .ForMember(dest => dest.Lesson,
                            opt => opt.MapFrom(src => 
                                src.Lesson.Theme));

        }  
    }
}

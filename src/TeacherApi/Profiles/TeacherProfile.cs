using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data.Dtos;
using TeacherApi.Models;

namespace TeachersApi.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<CompletedHomework, CompletedHomeworkReadDto>();
            CreateMap<Student, StudentReadDto>();
            CreateMap<Parent, ParentReadDto>();
        }
    }
}

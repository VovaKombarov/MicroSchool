using System;

namespace TeacherApi.Data.Dtos
{
    public record StudentReadDto
    {

        private string _birthDate;

        public string Name { get; init; }

        public string Patronymic { get; init; }

        public string Surname { get; init; }

        public string BirthDate
        {
            get { return  Convert.ToDateTime(
                _birthDate).ToString("dd.MM.yyyy");}
            init    
            {
                _birthDate = value;
            }
        }
       
    }
}

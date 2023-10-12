using System;

namespace TeacherApi.Data.Dtos
{
    /// <summary>
    /// Студент.
    /// </summary>
    public record StudentReadDto
    {
        #region Fields 

        /// <summary>
        /// Дата рождения.
        /// </summary>
        private string _birthDate;

        #endregion Fields

        #region Properties 

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; init; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; init; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public string BirthDate
        {
            get
            {
                return Convert.ToDateTime(
                _birthDate).ToString("dd.MM.yyyy");
            }
            init
            {
                _birthDate = value;
            }
        }

        #endregion Properties
    }
}

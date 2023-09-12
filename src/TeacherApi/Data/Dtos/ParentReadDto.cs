namespace TeacherApi.Data.Dtos
{
    public record ParentReadDto
    {
        public string Name { get; init; }

        public string Patronymic { get; init; }

        public string Surname { get; init; }
    }
}

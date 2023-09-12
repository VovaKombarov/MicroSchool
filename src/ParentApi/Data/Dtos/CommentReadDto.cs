namespace ParentApi.Data.Dtos
{
    public record CommentReadDto
    {
        public string Lesson { get; init; } 

        public string Comment { get; init; }
    }
}

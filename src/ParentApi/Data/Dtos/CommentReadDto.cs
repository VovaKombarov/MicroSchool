namespace ParentApi.Data.Dtos
{
    /// <summary>
    /// Замечание. 
    /// </summary>
    public record CommentReadDto
    {
        /// <summary>
        /// Урок.
        /// </summary>
        public string Lesson { get; init; } 

        /// <summary>
        /// Замечание.
        /// </summary>
        public string Comment { get; init; }
    }
}

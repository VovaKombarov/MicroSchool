using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class HomeworkSpecification : BaseSpecification<Homework>
    {
        public HomeworkSpecification(int homeworkId) : base(w => w.Id == homeworkId) 
        { 

        }
    }

    public class HomeworkSpecificationByLesson : BaseSpecification<Homework>
    {
        public HomeworkSpecificationByLesson(int lessonId) : base(w => w.Id ==  lessonId)
        {

        }
    }
}

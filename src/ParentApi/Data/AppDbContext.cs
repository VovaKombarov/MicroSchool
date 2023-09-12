using Microsoft.EntityFrameworkCore;
using ParentApi.Models;

namespace ParentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=microschool;Username=postgres;Password=Lokomotiv1922")
                .UseLowerCaseNamingConvention(); 
        }

        public DbSet<Class> Classes { get; set; }

        public DbSet<CompletedHomework> CompletedHomeworks { get; set; }

        public DbSet<HomeworkProgressStatus> HomeworkProgressStatuses { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<HomeworkStatus> HomeworkStatuses { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Parent> Parents { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentInLesson> StudenstInLessons { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<TeacherClassSubject> TeachersClassesSubjects { get; set; }

        public DbSet<TeacherParentMeeting> TeachersParentsMeetings { get; set; }
    }
}

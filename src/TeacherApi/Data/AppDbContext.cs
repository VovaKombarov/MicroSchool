using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Models;

namespace TeacherApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
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

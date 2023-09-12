using System.Linq.Expressions;
using System;
using ParentApi.Models;
using System.Collections.Generic;
using System.Linq;


namespace ParentApi.Data.Utilities
{
    public static class Expressions
    {
        public static Expression<Func<StudentInLesson, bool>> FilterStudentInLesson(
           int studentId = 0,
           int lessonId = 0)
        {
            if (studentId != 0 && lessonId != 0)
                return x => x.Student.Id == studentId && x.Lesson.Id == lessonId;
            else if (studentId != 0 && lessonId == 0)
                return x => x.Student.Id == studentId;
            else if (studentId == 0 && lessonId != 0)
                return x => x.Lesson.Id == lessonId;
            else
                return x => false;
        }

        public static Expression<Func<CompletedHomework, bool>> FilterCompletedHomework(
            int studentInLessonId)
        {
            return x => x.StudentInLesson.Id == studentInLessonId;
        }

        public static Expression<Func<TeacherParentMeeting, bool>> FilterTeacherParentMeeting (
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT)
        {
            return x => 
                x.Student.Id == studentId && 
                x.Teacher.Id == teacherId && 
                x.Parent.Id == parentId && 
                x.MeetingDT == meetingDT;
        }


        public static Expression<Func<StudentInLesson, bool>> FilterStudentInLessonByStudentIdAndSubjectd(
          int studentId, 
          int subjectId)
        {
            return x =>
                 x.Student.Id == studentId &&
                 x.Lesson.TeacherClassSubject.Subject.Id == subjectId;    
        }


        public static Expression<Func<StudentInLesson, object>>
            StudentInLessonIncludeSubject()
        {
            return x => x.Lesson.TeacherClassSubject.Subject;
        }

        public static Expression<Func<StudentInLesson, object>>
           StudentInLessonIncludeStudent()
        {
            return x => x.Student;
        }

        public static Expression<Func<StudentInLesson, bool>> FilterStudentInLessonByStudentId(int studentId)
        {
            return x => x.Student.Id == studentId;
        }
            

    }
}

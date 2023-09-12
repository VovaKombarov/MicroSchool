using Common.Api.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TeacherApi.Data;
using TeacherApi.Data.Specifications;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.Models;

namespace TeacherApi.IntegrationEvents.Services
{
    public class TeacherIntegrationEventService : ITeacherIntegrationEventService
    {
        #region Fields

        private readonly ILogger<TeacherIntegrationEventService> _logger;
        private IRepository<Parent> _parentRepo;
        private IRepository<Teacher> _teacherRepo;
        private IRepository<Student> _studentRepo;
        private IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        #endregion Fields

        #region Constructors

        public TeacherIntegrationEventService(
            ILogger<TeacherIntegrationEventService> logger,
            IRepository<Parent> parentRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<Student> studentRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo)
        {
            _parentRepo = parentRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public async Task CreateParentTeacherMeetingAsync(CreateParentTeacherMeetingEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                Parent parent = await _parentRepo.GetItemAsync(
                    new ParentSpecification(@event.ParentId));

                Student student = await _studentRepo.GetItemAsync(
                    new StudentSpecification(@event.StudentId));

                Teacher teacher = await _teacherRepo.GetItemAsync(
                    new TeacherSpecification(student.Id));

                TeacherParentMeeting teacherParentMeeting = new TeacherParentMeeting()
                {
                    Parent = parent,
                    Student = student,
                    Teacher = teacher,
                    MeetingDT = @event.MeetingDT,
                    TeacherInitiative = false
                };

                await _teacherParentMeetingRepo.AddAsync(teacherParentMeeting);
                await _teacherParentMeetingRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);

            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        public async Task RemoveParentTeacherMeetingAsync(RemoveParentTeacherMeetingEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                TeacherParentMeeting teacherParentMeeting =
                    await _teacherParentMeetingRepo.GetItemAsync(
                        new TeacherParentMeetingSpecification(@event.TeacherParentMeetingId));

                _teacherParentMeetingRepo.Remove(teacherParentMeeting);

                await _teacherParentMeetingRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        #endregion Methods
    }
}

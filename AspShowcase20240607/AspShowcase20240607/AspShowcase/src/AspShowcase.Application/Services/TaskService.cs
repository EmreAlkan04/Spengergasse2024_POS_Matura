using AspShowcase.Application.Commands;
using AspShowcase.Application.Infrastructure;
using AspShowcase.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspShowcase.Application.Services
{
    public class TaskService
    {
        private readonly AspShowcaseContext _db;
        private readonly IClockService _clock;
        private readonly IMapper _mapper;

        public TaskService(AspShowcaseContext db, IClockService clock, IMapper mapper)
        {
            _db = db;
            _clock = clock;
            _mapper = mapper;
        }

        public IQueryable<Task> Tasks => _db.Tasks.AsQueryable();
        public Guid AddTask(NewTaskCmd cmd)
        {
            // Wir prüfen, ob es die Fremdschlüsselwerte TeamGUID und TaskGUID überhaupt gibt.
            var team = _db.Teams.FirstOrDefault(t => t.Guid == cmd.TeamGuid);
            var teacher = _db.Teachers.FirstOrDefault(t => t.Guid == cmd.TeacherGuid);
            if (team is null) { throw new ServiceException("Invalid Team GUID"); }
            if (teacher is null) { throw new ServiceException("Invalid Teacher GUID"); }

            // Der Task darf nicht abgelaufen sein
            if (cmd.ExpirationDate < _clock.Now)
                throw new ServiceException("Expiration date must be in the future.");

            // Der Name des Tasks muss pro Team eindeutig sein.
            if (_db.Tasks.Any(t => t.Title == cmd.Title && t.Team.Id == team.Id))
                throw new ServiceException("Task title must be unique per team");

            var task = new Models.Task(
                subject: cmd.Subject, title: cmd.Title, team: team, teacher: teacher,
                expirationDate: cmd.ExpirationDate, maxPoints: cmd.MaxPoints);
            _db.Tasks.Add(task);
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                throw new ServiceException(e.InnerException?.Message ?? e.Message);
            }
            return task.Guid;
        }

        public void EditTask(EditTaskCmd cmd)
        {
            // Suche den alten Task in der Datenbank
            var task = _db.Tasks.FirstOrDefault(t => t.Guid == cmd.Guid);
            if (task is null)
                throw new ServiceException("Task not found.")
                { NotFound = true };

            // Ist das ExpirationDate schon abgelaufen, darf nichts mehr bearbeitet werden.
            if (task.ExpirationDate < _clock.Now)
                throw new ServiceException("Task is already expired.");

            // Gibt es schon Handins, darf auch nichts mehr bearbeitet werden.
            if (_db.Handins.Any(h => h.Task.Id == task.Id))
                throw new ServiceException("Task has already handins.");

            task.Subject = cmd.Subject;
            task.Title = cmd.Title;
            task.MaxPoints = cmd.MaxPoints;
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                throw new ServiceException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void DeleteTask(Guid guid, bool force)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Guid == guid);
            if (task is null)
                throw new ServiceException("Task not found.")
                { NotFound = true };

            // Ein Task darf nur gelöscht werden, wenn keine Handins zugeordnet sind oder
            // der Parameter force auf true gesetzt wurde.
            if (_db.Handins.Any(h => h.Task.Id == task.Id))
            {
                if (force)
                {
                    var handins = _db.Handins.Where(h => h.Task.Id == task.Id).ToList();
                    _db.Handins.RemoveRange(handins);
                    try { _db.SaveChanges(); }
                    catch (DbUpdateException e)
                    {
                        throw new ServiceException(e.InnerException?.Message ?? e.Message);
                    }
                }
                else
                    throw new ServiceException("Task has handins. Set force to true to delete anyway.");
            }

            _db.Tasks.Remove(task);
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                throw new ServiceException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}

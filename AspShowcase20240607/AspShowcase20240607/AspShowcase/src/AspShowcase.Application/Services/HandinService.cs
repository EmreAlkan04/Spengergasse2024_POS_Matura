using AspShowcase.Application.Commands;
using AspShowcase.Application.Infrastructure;
using AspShowcase.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspShowcase.Application.Services
{
    public class HandinService
    {
        private readonly AspShowcaseContext _db;
        private readonly IMapper _mapper;

        public HandinService(AspShowcaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IQueryable<Handin> Handins => _db.Set<Handin>().AsQueryable();
        public Guid AddHandin(NewHandinCmd handinCmd)
        {
            var student = _db.Students.FirstOrDefault(s => s.Guid == handinCmd.StudentGuid);
            if (student is null) { throw new ServiceException("Invalid Student GUID"); }
            var task = _db.Tasks.FirstOrDefault(t => t.Guid == handinCmd.TaskGuid);
            if (task is null) { throw new ServiceException("Invalid Task GUID"); }

            if (DateTime.Now > task.ExpirationDate)
                throw new ServiceException("Task is expired.");

            var handin = _mapper.Map<Handin>(handinCmd, opt =>
            {
                opt.AfterMap((src, dest) =>
                {
                    dest.Student = student;
                    dest.Task = task;
                    dest.Created = DateTime.UtcNow;
                });
            });
            _db.Handins.Add(handin);
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                throw new ServiceException(e.InnerException?.Message ?? e.Message, e);
            }
            return handin.Guid;
        }

        public void EditHandin(EditHandinCmd handinCmd)
        {
            var handin = _db.Handins
                .Include(h => h.Task)
                .FirstOrDefault(h => h.Guid == handinCmd.Guid);
            if (handin is null) { throw new ServiceException("Handin not found."); }
            if (DateTime.Now > handin.Task.ExpirationDate)
                throw new ServiceException("Task is expired");

            _mapper.Map(handinCmd, handin);
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                throw new ServiceException(e.InnerException?.Message ?? e.Message, e);
            }
        }

        public (bool success, bool found, string message) DeleteHandin(Guid guid)
        {
            var handin = _db.Handins.Include(h => h.Task)
                .FirstOrDefault(h => h.Guid == guid);
            if (handin is null) { return (false, false, "Handin not found."); }
            if (DateTime.Now > handin.Task.ExpirationDate)
                return (false, true, "Task is expired");

            _db.Handins.Remove(handin);
            try { _db.SaveChanges(); }
            catch (DbUpdateException e)
            {
                return (false, true, e.InnerException?.Message ?? e.Message);
            }
            return (true, true, string.Empty);
        }
    }
}

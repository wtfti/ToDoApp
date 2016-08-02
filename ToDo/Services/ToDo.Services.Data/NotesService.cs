namespace ToDo.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Server.Common.Constants;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;

    public class NotesService : INotesService
    {
        private readonly IRepository<PrivateNote> privateNotesData;
        private readonly IRepository<SharedNote> sharedNotesData;
        private readonly IAccountService accountService;

        public NotesService(
            IRepository<PrivateNote> privateNotesRepository,
            IRepository<SharedNote> sharedNotesRepository,
            IAccountService accountService)
        {
            this.privateNotesData = privateNotesRepository;
            this.sharedNotesData = sharedNotesRepository;
            this.accountService = accountService;
        }

        public void ChangeNote(
            PrivateNote dbNote,
            string newTitle,
            string newContent,
            DateTime? newExpiredOn)
        {
            bool hasChange = false;

            if (dbNote.Title != newTitle)
            {
                dbNote.Title = newTitle;
                hasChange = true;
            }

            if (dbNote.Content != newContent)
            {
                dbNote.Content = newContent;
                hasChange = true;
            }

            if (dbNote.ExpiredOn != newExpiredOn)
            {
                dbNote.ExpiredOn = newExpiredOn;
                hasChange = true;
            }

            if (hasChange)
            {
                this.privateNotesData.SaveChanges();
            }
        }

        public IQueryable<PrivateNote> All()
        {
            var notes = this.privateNotesData.All();

            return notes;
        }

        public void RemoveNoteById(PrivateNote note)
        {
            this.privateNotesData.Delete(note);
            this.privateNotesData.SaveChanges();
        }

        public void SetComplete(PrivateNote note)
        {
            note.IsComplete = true;
            this.privateNotesData.SaveChanges();
        }

        public void SetExpired(PrivateNote note)
        {
            note.IsExpired = true;
            this.privateNotesData.SaveChanges();
        }

        public PrivateNote GetNoteById(int id)
        {
            return this.privateNotesData.GetById(id);
        }

        public IQueryable<PrivateNote> GetNotes(
            string user, 
            int page, 
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var privateNotes = this.privateNotesData
                .All()
                .Where(a => a.UserId == user && !a.IsComplete && !a.IsExpired)
                .OrderBy(q => q.Content)
                .Skip((page*pageSize) - pageSize)
                .Take(pageSize);

            return privateNotes;
        }

        public IQueryable<SharedNote> GetSharedNotes(
            string user, 
            int page, 
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.sharedNotesData
                .All()
                .Where(note => note.Users.Any(u => u.Id == user))
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<PrivateNote> GetNotesFromToday(
            string user,
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var today = DateTime.Now;

            var notes = this.privateNotesData.All()
                .Where(a =>
                    a.UserId == user &&
                    !a.IsComplete &&
                    !a.IsExpired &&
                    a.CreatedOn != null);

            var todayNotes = notes.ToList()
                .Where(a => a.CreatedOn.Date == today.Date)
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize)
                .AsQueryable();

            return todayNotes;
        }

        public IQueryable<PrivateNote> GetCompletedNotes(
            string user,
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.privateNotesData.All()
                .Where(a => a.UserId == user && a.IsComplete)
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<PrivateNote> GetNotesWithExpirationDate(
            string user, 
            int page, 
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.privateNotesData.All()
                .Where(a => a.IsExpired && a.UserId == user)
                .OrderBy(w => w.Id)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public void AddPrivateNote(
            string user, 
            string title, 
            string content, 
            DateTime? expireDate = null)
        {
            var privateNoteDb = new PrivateNote()
            {
                UserId = user,
                Title = title,
                Content = content,
                CreatedOn = DateTime.Now,
                ExpiredOn = expireDate
            };

            this.privateNotesData.Add(privateNoteDb);
            this.privateNotesData.SaveChanges();
        }

        public void AddSharedNote(
            string[] users, 
            string currentUser, 
            string title, 
            string content,
            DateTime? expireDate = null)
        {
            var usersDb = this.accountService.GetUsersByUsername(users);
            var currentUserDb = this.accountService.GetUserByUsername(currentUser);
            usersDb.Add(currentUserDb);

            var sharedNoteDb = new SharedNote()
            {
                Title = title,
                Content = content,
                CreatedOn = DateTime.Now,
                ExpiredOn = expireDate,
                Users = usersDb
            };

            this.sharedNotesData.Add(sharedNoteDb);
            this.sharedNotesData.SaveChanges();
        }
    }
}

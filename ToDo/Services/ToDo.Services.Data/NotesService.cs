namespace ToDo.Services.Data
{
    using System;
    using System.Linq;
    using Contracts;
    using Server.Common.Constants;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;

    public class NotesService : INotesService
    {
        private readonly IRepository<Note> data;

        public NotesService(IRepository<Note> noteRepository)
        {
            this.data = noteRepository;
        }

        public void EditNote(
            Note dbNote,
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
                this.data.SaveChanges();
            }
        }

        public IQueryable<Note> All()
        {
            var notes = this.data.All();

            return notes;
        }

        public void RemoveNoteById(Note note)
        {
            this.data.Delete(note);
            this.data.SaveChanges();
        }

        public void SetComplete(Note note)
        {
            note.IsComplete = true;
            this.data.SaveChanges();
        }

        public void SetExpired(Note note)
        {
            note.IsExpired = true;
            this.data.SaveChanges();
        }

        public Note GetNoteById(int id)
        {
            return this.data.GetById(id);
        }

        public IQueryable<Note> GetNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.data.All()
                .Where(a => a.UserId == user && !a.IsComplete && !a.IsExpired)
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<Note> GetNotesFromToday(
            string user, 
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var today = DateTime.Now;

            var notes = this.data.All()
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

        public IQueryable<Note> GetCompletedNotes(
            string user,
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.data.All()
                .Where(a => a.UserId == user && a.IsComplete)
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<Note> GetNotesWithExpirationDate(string user, int page, int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.data.All()
                .Where(a => a.IsExpired && a.UserId == user)
                .OrderBy(w => w.Id)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public void AddNote(string user, string title, string content, DateTime? expireDate = null)
        {
            var noteDb = new Note()
            {
                UserId = user,
                Title = title,
                Content = content,
                CreatedOn = DateTime.Now,
                ExpiredOn = expireDate
            };

            this.data.Add(noteDb);
            this.data.SaveChanges();
        }
    }
}

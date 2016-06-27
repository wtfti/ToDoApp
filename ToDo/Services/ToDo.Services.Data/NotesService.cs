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

        public void ChangeNoteTitle(Note note, string newValue)
        {
            note.Title = newValue;
            this.data.SaveChanges();
        }

        public void ChangeNoteContent(Note note, string newValue)
        {
            note.Content = newValue;
            this.data.SaveChanges();
        }

        public void ChangeNoteExpireDate(Note note, DateTime date)
        {
            note.ExpiredOn = date;
            this.data.SaveChanges();
        }

        public IQueryable<Note> GetNotes(string currentUser, int page, int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.data.All()
                .Where(a => a.UserId == currentUser)
                .OrderBy(w => w.Id)
                .Skip((page*pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<Note> GetNotesWithExpiredDate(string currentUser, int page, int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.data.All()
                .Where(a => a.UserId == currentUser && a.ExpiredOn != null)
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

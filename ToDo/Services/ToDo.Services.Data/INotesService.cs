namespace ToDo.Services.Data
{
    using System;
    using System.Linq;
    using Server.Common;
    using ToDo.Data.Models;

    public interface INotesService
    {
        IQueryable<Note> GetNotes(string currentUser, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<Note> GetNotesWithExpiredDate(string currentUser, int page, int pageSize = ValidationConstants.DefaultPageSize);

        void AddNote(string user, string title, string content, DateTime? expireDate = null);

        IQueryable<Note> All();

        void RemoveNoteById(Note note);

        void ChangeNoteTitle(Note note, string newValue);

        void ChangeNoteContent(Note note, string newValue);

        void ChangeNoteExpireDate(Note note, DateTime date);
    }
}

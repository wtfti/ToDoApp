namespace ToDo.Services.Data.Contracts
{
    using System;
    using System.Linq;
    using Server.Common.Constants;
    using ToDo.Data.Models;

    public interface INotesService
    {
        Note GetNoteById(int id);

        IQueryable<Note> GetNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<Note> GetNotesWithExpiredDate(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        void AddNote(string user, string title, string content, DateTime? expireDate = null);

        IQueryable<Note> All();

        void RemoveNoteById(Note note);

        void ChangeNoteTitle(Note note, string newValue);

        void SetExpired(Note note);

        void SetComplete(Note note);

        void ChangeNoteContent(Note note, string newValue);

        void ChangeNoteExpireDate(Note note, DateTime date);
    }
}

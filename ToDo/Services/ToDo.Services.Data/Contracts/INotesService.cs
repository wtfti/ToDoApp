namespace ToDo.Services.Data.Contracts
{
    using System;
    using System.Linq;
    using Server.Common.Constants;
    using ToDo.Data.Models;

    public interface INotesService
    {
        PrivateNote GetPrivateNoteById(string id);

        SharedNote GetSharedNoteById(string id);

        IQueryable<PrivateNote> GetCompletedNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<PrivateNote> GetNotesFromToday(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<PrivateNote> GetNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<SharedNote> GetSharedNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<PrivateNote> GetNotesWithExpirationDate(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        void AddPrivateNote(string user, string title, string content, DateTime? expireDate = null);

        void AddSharedNote(string[] users, string currentUser, string title, string content, DateTime? expireDate = null);

        void ChangePrivateNote(PrivateNote dbNote, string newTitle, string newContent, DateTime? newExpiredOn);

        void ChangeSharedNote(SharedNote dbNote, string newTitle, string newContent, DateTime? newExpiredOn);

        IQueryable<PrivateNote> All();

        void RemovePrivateNoteById(PrivateNote note);

        void RemoveSharedNoteById(SharedNote note);

        void SetExpired(PrivateNote note);

        void SetComplete(PrivateNote note);
    }
}

namespace ToDo.Services.Data.Services
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
        private readonly IFriendsService friendService;

        public NotesService(
            IRepository<PrivateNote> privateNotesRepository,
            IRepository<SharedNote> sharedNotesRepository,
            IAccountService accountService,
            IFriendsService friendsService)
        {
            this.privateNotesData = privateNotesRepository;
            this.sharedNotesData = sharedNotesRepository;
            this.accountService = accountService;
            this.friendService = friendsService;
        }

        public void ChangePrivateNote(
            PrivateNote dbNote,
            string newTitle,
            string newContent,
            DateTime? newExpiredOn)
        {
            dbNote.Title = newTitle;
            dbNote.Content = newContent;
            dbNote.ExpiredOn = newExpiredOn;

            this.privateNotesData.SaveChanges();
        }

        public void ChangeSharedNote(
            SharedNote dbNote,
            string newTitle,
            string newContent,
            DateTime? newExpiredOn)
        {
            dbNote.Title = newTitle;
            dbNote.Content = newContent;
            dbNote.ExpiredOn = newExpiredOn;

            this.sharedNotesData.SaveChanges();
        }

        public IQueryable<PrivateNote> All()
        {
            var notes = this.privateNotesData.All();

            return notes;
        }

        public void RemovePrivateNoteById(PrivateNote note)
        {
            this.privateNotesData.Delete(note);
            this.privateNotesData.SaveChanges();
        }

        public void RemoveSharedNoteById(SharedNote note)
        {
            this.sharedNotesData.Delete(note);
            this.sharedNotesData.SaveChanges();
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

        public PrivateNote GetPrivateNoteById(string id)
        {
            return this.privateNotesData.GetById(id);
        }

        public SharedNote GetSharedNoteById(string id)
        {
            return this.sharedNotesData.GetById(id);
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
                .Skip((page * pageSize) - pageSize)
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
                .ToList()
                .Where(note => note.Users.Any(u => u.Id == user))
                .OrderBy(q => q.Content)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize)
                .AsQueryable();

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
            User currentUserDb = this.accountService.GetUserByUsername(user);

            var privateNoteDb = new PrivateNote()
            {
                Id = DateTime.UtcNow.Ticks.ToString(),
                UserId = currentUserDb.Id,
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
            User currentUserDb = this.accountService.GetUserByUsername(currentUser);
            var usersDb = this.accountService.GetUsersByFullName(users);
            List<User> sharedWith = new List<User>(usersDb.Count + 1);
            sharedWith.Add(currentUserDb);

            foreach (var user in usersDb)
            {
                var friendship = this.friendService.GetFriendship(currentUserDb.UserName, user.UserName);

                if (friendship != null)
                {
                    sharedWith.Add(user);
                }
            }

            var sharedNoteDb = new SharedNote()
            {
                Id = DateTime.UtcNow.Ticks.ToString(),
                Title = title,
                Content = content,
                CreatedFrom = currentUserDb.ProfileDetails.FullName,
                CreatedOn = DateTime.Now,
                ExpiredOn = expireDate,
                Users = sharedWith
            };

            this.sharedNotesData.Add(sharedNoteDb);
            this.sharedNotesData.SaveChanges();
        }
    }
}

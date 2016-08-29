namespace ToDo.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Server.Common.Constants;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

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

        public void SetCompletePrivateNote(PrivateNote note)
        {
            note.IsComplete = true;
            this.privateNotesData.SaveChanges();
        }

        public void SetCompleteSharedNote(SharedNote note)
        {
            note.IsComplete = true;
            this.sharedNotesData.SaveChanges();
        }

        public void SetExpiredPrivateNote(PrivateNote note)
        {
            note.IsExpired = true;
            this.privateNotesData.SaveChanges();
        }
        public void SetExpiredSharedNote(SharedNote note)
        {
            note.IsExpired = true;
            this.sharedNotesData.SaveChanges();
        }

        public int GetPrivateNotesCount(string id)
        {
            int count = this.privateNotesData.All().Count(a => !a.IsComplete && !a.IsExpired && a.UserId == id);

            return count;
        }

        public int GetTodayNotesCount(string id)
        {
            DateTime today = DateTime.Now;
            int count =
                this.privateNotesData.All()
                    .Count(
                        a =>
                            !a.IsComplete &&
                            !a.IsExpired &&
                            a.UserId == id &&
                            a.CreatedOn.Day == today.Day &&
                            a.CreatedOn.Month == today.Month &&
                            a.CreatedOn.Year == today.Year);

            return count;
        }

        public int GetSharedNotesCount(string id)
        {
            int count = this.sharedNotesData
                .All()
                .Where(note => !note.IsExpired && !note.IsComplete)
                .ToList()
                .Where(note => note.Users.Any(u => u.Id == id))
                .Count();

            return count;
        }

        public int GetExpiredNotesCount(string id)
        {
            int count = this.privateNotesData
                .All()
                .Count(a => a.IsExpired && a.UserId == id);

            return count;
        }

        public int GetCompletedNotesCount(string id)
        {
            int count = this.sharedNotesData
                .All()
                .Where(a => a.IsComplete)
                .ToList()
                .Where(u => u.Users.Any(user => user.Id == id))
                .Count();

            return count;
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
                .OrderBy(q => q.CreatedOn)
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
                .Where(note => !note.IsExpired && !note.IsComplete)
                .ToList()
                .Where(note => note.Users.Any(u => u.Id == user))
                .OrderBy(q => q.CreatedOn)
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

            var todayNotes = notes
                .ToList()
                .Where(a => a.CreatedOn.Date == today.Date)
                .OrderBy(q => q.CreatedOn)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize)
                .AsQueryable();

            return todayNotes;
        }

        public IQueryable<PrivateNote> GetCompletedPrivateNotes(
            string userId,
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.privateNotesData
                .All()
                .Where(a => a.UserId == userId && a.IsComplete)
                .OrderBy(q => q.CreatedOn)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<SharedNote> GetCompletedSharedNotes(
            string userId,
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.sharedNotesData
                .All()
                .Where(a => a.IsComplete)
                .ToList()
                .Where(u => u.Users.Any(user => user.Id == userId))
                .OrderBy(q => q.CreatedOn)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize)
                .AsQueryable();

            return notes;
        }

        public IQueryable<PrivateNote> GetPrivateNotesWithExpirationDate(
            string user, 
            int page, 
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.privateNotesData
                .All()
                .Where(a => a.IsExpired && a.UserId == user)
                .OrderBy(w => w.CreatedOn)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize);

            return notes;
        }

        public IQueryable<SharedNote> GetSharedNotesWithExpirationDate(
            string userId, 
            int page,
            int pageSize = ValidationConstants.DefaultPageSize)
        {
            var notes = this.sharedNotesData
                .All()
                .Where(a => a.IsExpired)
                .ToList()
                .Where(u => u.Users.Any(user => user.Id == userId))
                .OrderBy(w => w.CreatedOn)
                .Skip((page * pageSize) - pageSize)
                .Take(pageSize)
                .AsQueryable();

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

                if (friendship != null && friendship.Status == Status.Accepted)
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

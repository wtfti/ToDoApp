namespace ToDo.Api.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using Data;
    using Data.Models;
    using Models.Note;
    using Server.Common;

    [Authorize]
    public class NoteController : BaseController
    {
        private readonly ToDoDbContext db;

        public NoteController()
        {
            this.db = new ToDoDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddNote(NoteRequestModel note)
        {
            var user = this.User.Identity.Name;

            var noteDb = new Note()
            {
                UserId = user,
                Title = note.Title,
                Content = note.Content,
                CreatedOn = DateTime.Now
            };

            this.db.Notes.Add(noteDb);
            this.db.SaveChanges();

            return this.Ok(MessageConstants.CreateNoteMessage);
        }

        [HttpPost]
        public IHttpActionResult AddNoteWithExpirationDate(NoteRequestModel note)
        {
            var user = this.User.Identity.Name;

            var noteDb = new Note()
            {
                UserId = user,
                Title = note.Title,
                Content = note.Content,
                CreatedOn = DateTime.Now,
                ExpiredOn =  note.ExpiredOn
            };

            this.db.Notes.Add(noteDb);
            this.db.SaveChanges();

            return this.Ok(MessageConstants.CreateNoteMessage);
        }

        [HttpGet]
        public IHttpActionResult GetNotes(int page = 1, int pageSize = 10)
        {
            var notes = this.db.Notes
                .Where(a => a.UserId == this.User.Identity.Name)
                .OrderBy(w => w.Id)
                .Skip((page * pageSize) - pageSize)
                .Take(10)
                .Select(NoteResponseModel.FromModel);

            return this.Ok(notes);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExpirateDate(int page = 1, int pageSize = 10)
        {
            var notes = this.db.Notes
                .Where(a => a.UserId == this.User.Identity.Name && a.ExpiredOn != null)
                .OrderBy(w => w.Id)
                .Skip((page * pageSize) - pageSize)
                .Take(10)
                .Select(NoteResponseModel.FromModel);

            return this.Ok(notes);
        }

        [HttpDelete]
        public IHttpActionResult RemoveNoteById(int id)
        {
            var note = this.db.Notes.FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            this.db.Notes.Remove(note);
            this.db.SaveChanges();

            return this.Ok(MessageConstants.RemoveNoteMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteTitle(int id, string newValue)
        {
            var note = this.db.Notes.FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            note.Title = newValue;
            this.db.SaveChanges();

            return this.Ok(MessageConstants.TitleChangeMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteContent(int id, string newValue)
        {
            var note = this.db.Notes.FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            note.Content = newValue;
            this.db.SaveChanges();

            return this.Ok(MessageConstants.ContextChangeMessage);
        }

        [HttpPut]
        public IHttpActionResult SetNoteExpireDate(int id, string date)
        {
            var note = this.db.Notes.FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(date, ValidationConstants.ExpireDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDateMessage);
            }

            note.ExpiredOn = parsedDate;
            this.db.SaveChanges();

            return this.Ok(MessageConstants.SetDateMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeExpireDate(int id, string date)
        {
            var note = this.db.Notes.FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(date, ValidationConstants.ExpireDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDateMessage);
            }

            note.ExpiredOn = parsedDate;
            this.db.SaveChanges();

            return this.Ok(MessageConstants.ChangeDateMessage);
        }
    }
}
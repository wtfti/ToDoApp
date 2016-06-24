namespace ToDo.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data;
    using Data.Models;
    using Models.Note;

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

            return this.Ok("Created note");
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

            return this.Ok("Created note");
        }

        [HttpGet]
        public IHttpActionResult GetNotes(int page = 1, int pageSize = 10)
        {
            var notes = this.db.Notes
                .Where(a => a.UserId == this.User.Identity.Name)
                .OrderBy(w => w.Id)
                .Skip((page * pageSize) - pageSize)
                .Take(10)
                .Select(b => new NoteResponseModel()
                {
                    Title = b.Title,
                    Content = b.Content,
                    CreatedOn = b.CreatedOn.Value,
                    ExpiredOn = b.ExpiredOn
                });

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
                .Select(b => new NoteResponseModel()
                {
                    Title = b.Title,
                    Content = b.Content,
                    CreatedOn = b.CreatedOn.Value,
                    ExpiredOn = b.ExpiredOn
                });

            return this.Ok(notes);
        }
    }
}
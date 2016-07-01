namespace ToDo.Api.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using AutoMapper.QueryableExtensions;
    using Infrastructure.Validation;
    using Models.Note;
    using Server.Common.Constants;
    using Services.Data.Contracts;

    [Authorize]
    public class NoteController : BaseController
    {
        private readonly INotesService notesService;

        public NoteController(INotesService service)
        {
            this.notesService = service;
        }

        [HttpPost]
        [ValidateModel]
        public IHttpActionResult AddNote(NoteRequestModel note)
        {
            var user = this.User.Identity.Name;

            this.notesService.AddNote(user, note.Title, note.Content);

            return this.Ok(MessageConstants.CreateNoteMessage);
        }

        [HttpPost]
        [ValidateModel]
        public IHttpActionResult AddNoteWithExpirationDate(NoteRequestModel note)
        {
            var user = this.User.Identity.Name;

            this.notesService.AddNote(user, note.Title, note.Content, note.ExpiredOn);

            return this.Ok(MessageConstants.CreateNoteMessage);
        }

        [HttpGet]
        public IHttpActionResult GetNotes(int page = 1)
        {
            var dbNotes = this.notesService
                .GetNotes(this.User.Identity.Name, page)
                .ProjectTo<NoteResponseModel>();

            return this.Ok(dbNotes);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExpirateDate(int page = 1)
        {
            var dbNotes = this.notesService
                .GetNotesWithExpiredDate(this.User.Identity.Name, page)
                .ProjectTo<NoteResponseModel>();

            return this.Ok(dbNotes);
        }

        [HttpDelete]
        public IHttpActionResult RemoveNoteById(int id)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            this.notesService.RemoveNoteById(note);

            return this.Ok(MessageConstants.RemoveNoteMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteTitle(int id, string newValue)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            this.notesService.ChangeNoteTitle(note, newValue);

            return this.Ok(MessageConstants.TitleChangeMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteContent(int id, string newValue)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            this.notesService.ChangeNoteContent(note, newValue);

            return this.Ok(MessageConstants.ContextChangeMessage);
        }

        [HttpPut]
        public IHttpActionResult SetNoteExpireDate(int id, string date)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(
                date, 
                ValidationConstants.ExpireDateFormat, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDateMessage);
            }

            this.notesService.ChangeNoteExpireDate(note, parsedDate);

            return this.Ok(MessageConstants.SetDateMessage);
        }

        [HttpPut]
        public IHttpActionResult ChangeExpireDate(int id, string date)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.User.Identity.Name);
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExistsMessage);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(
                date, 
                ValidationConstants.ExpireDateFormat, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDateMessage);
            }

            this.notesService.ChangeNoteExpireDate(note, parsedDate);

            return this.Ok(MessageConstants.ChangeDateMessage);
        }
    }
}
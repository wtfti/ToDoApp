namespace ToDo.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Infrastructure.Validation;
    using ModelBinders;
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
        public IHttpActionResult AddNote([ModelBinder(typeof(NoteRequestModelBinder))]NoteRequestModel note)
        {
            if (note.ExpiredOn != null && note.ExpiredOn <= DateTime.Now)
            {
                return this.BadRequest(MessageConstants.InvalidDate);
            }

            if (note.SharedWith.Length > 0)
            {
                this.notesService.AddSharedNote(note.SharedWith, this.CurrentUsername(), note.Title, note.Content, note.ExpiredOn);
            }
            else
            {
                this.notesService.AddPrivateNote(this.CurrentUsername(), note.Title, note.Content, note.ExpiredOn);
            }

            return this.Ok(MessageConstants.CreateNote);
        }

        [HttpPut]
        [ValidateModel]
        public IHttpActionResult ChangeNote([ModelBinder(typeof(NoteRequestModelBinder))]NoteRequestModel note)
        {
            if (note.ExpiredOn != null && note.ExpiredOn <= DateTime.Now)
            {
                return this.BadRequest(MessageConstants.InvalidDate);
            }

            int? id = note.Id;

            if (id == null || id.Value <= 0)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var dbNote = this.notesService.GetNoteById(id.Value);

            if (dbNote.UserId != this.CurrentUserId())
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.ChangeNote(dbNote, note.Title, note.Content, note.ExpiredOn);

            return this.Ok();
        }

        [HttpGet]
        public IHttpActionResult GetNotes(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var dbNotes = this.notesService
                .GetNotes(this.CurrentUserId(), page);

            var dbNotesList = dbNotes.ToList();

            bool hasChange = this.CheckIfNotesAreExpired(dbNotesList);

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotes(this.CurrentUserId(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSharedNotes(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var notes = this.notesService
                .GetSharedNotes(this.CurrentUserId(), page)
                .ProjectTo<SharedNoteResponseModel>();

            return this.Ok(notes);
        }

        [HttpGet]
        public IHttpActionResult GetNotesFromToday(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var dbNotes = this.notesService
                .GetNotesFromToday(this.CurrentUserId(), page);

            var dbNotesList = dbNotes.ToList();

            bool hasChange = this.CheckIfNotesAreExpired(dbNotesList);

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotesFromToday(this.CurrentUserId(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetCompletedNotes(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var dbNotes = this.notesService
                .GetCompletedNotes(this.CurrentUserId(), page);

            var dbNotesList = dbNotes.ToList();

            bool hasChange = this.CheckIfNotesAreExpired(dbNotesList);

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotes(this.CurrentUserId(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExpirationDate(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var dbNotes = this.notesService
                .GetNotesWithExpirationDate(this.CurrentUserId(), page);

            var dbNotesList = dbNotes.ToList();

            bool hasChange = this.CheckIfNotesAreExpired(dbNotesList);

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotesWithExpirationDate(this.CurrentUserId(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult RemoveNoteById(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var note = this.notesService
                .GetNoteById(id);
            if (note == null || note.UserId != this.CurrentUserId())
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.RemoveNoteById(note);

            return this.Ok(MessageConstants.RemoveNote);
        }

        [HttpPut]
        public IHttpActionResult SetComplete(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var note = this.notesService.GetNoteById(id);

            if (note == null || note.UserId != this.CurrentUserId())
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.SetComplete(note);

            return this.Ok();
        }

        [NonAction]
        private bool CheckIfNotesAreExpired(IList<PrivateNote> dbNotes)
        {
            bool hasChange = false;
            DateTime dateNow = DateTime.Now;

            foreach (var note in dbNotes)
            {
                if (note.ExpiredOn != null && note.ExpiredOn <= dateNow && !note.IsComplete && !note.IsExpired)
                {
                    var dbNote = this.notesService.GetNoteById(note.Id);
                    this.notesService.SetExpired(dbNote);
                    hasChange = true;
                }
            }

            return hasChange;
        }
    }
}
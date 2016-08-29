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
            if (note.ExpiredOn != null && note.ExpiredOn <= DateTime.Now.AddMinutes(ValidationConstants.MinutesToAdd))
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
            if (note.ExpiredOn != null && note.ExpiredOn < DateTime.Now.AddMinutes(ValidationConstants.MinutesToAdd))
            {
                return this.BadRequest(MessageConstants.InvalidDate);
            }

            if (note.Id == string.Empty)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var privateNote = this.notesService.GetPrivateNoteById(note.Id);

            if (privateNote != null && privateNote.UserId == this.CurrentUserId())
            {
                if (privateNote.IsComplete)
                {
                    return this.BadRequest(MessageConstants.NoteIsCompleted);
                }

                if (privateNote.IsExpired)
                {
                    return this.BadRequest(MessageConstants.NoteIsExpired);
                }

                this.notesService.ChangePrivateNote(privateNote, note.Title, note.Content, note.ExpiredOn);
                return this.Ok(MessageConstants.ChangedNoteParamsSuccessful);
            }

            var sharedNote = this.notesService.GetSharedNoteById(note.Id);

            if (sharedNote == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            User currentUser = sharedNote.Users.FirstOrDefault(u => u.Id == this.CurrentUserId());

            if (currentUser == null)
            {
                return this.BadRequest(MessageConstants.CurrentUserCannotEditThisNote);
            }

            if (sharedNote.CreatedFrom != currentUser.ProfileDetails.FullName)
            {
                return this.BadRequest(MessageConstants.CurrentUserCannotEditThisNote);
            }

            if (sharedNote.IsComplete)
            {
                return this.BadRequest(MessageConstants.NoteIsCompleted);
            }

            if (sharedNote.IsExpired)
            {
                return this.BadRequest(MessageConstants.NoteIsExpired);
            }

            this.notesService.ChangeSharedNote(sharedNote, note.Title, note.Content, note.ExpiredOn);
            return this.Ok(MessageConstants.ChangedNoteParamsSuccessful);
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

            var dbNotesList = dbNotes.ProjectTo<NoteDataModel>().ToList();

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
        public IHttpActionResult GetNotesCount()
        {
            int count = this.notesService.GetPrivateNotesCount(this.CurrentUserId());

            return this.Ok(count);
        }

        [HttpGet]
        public IHttpActionResult GetSharedNotes(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var notes = this.notesService
                .GetSharedNotes(this.CurrentUserId(), page);

            List<NoteDataModel> notesAsList = notes.ProjectTo<NoteDataModel>().ToList();

            bool hasChange = this.CheckIfNotesAreExpired(notesAsList);

            if (hasChange)
            {
                notes = this.notesService
                .GetSharedNotes(this.CurrentUserId(), page);
            }

            var response = notes.ProjectTo<NoteResponseModel>();

            return this.Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetSharedNotesCount()
        {
            int count = this.notesService.GetSharedNotesCount(this.CurrentUserId());

            return this.Ok(count);
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

            List<NoteDataModel> dbNotesList = dbNotes.ProjectTo<NoteDataModel>().ToList();

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
        public IHttpActionResult GetNotesFromTodayCount()
        {
            int count = this.notesService.GetTodayNotesCount(this.CurrentUserId());

            return this.Ok(count);
        }

        [HttpGet]
        public IHttpActionResult GetCompletedNotes(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var privateNotes = this.notesService
                .GetCompletedPrivateNotes(this.CurrentUserId(), page)
                .ProjectTo<NoteResponseModel>()
                .ToList();

            var sharedNotes = this.notesService
                .GetCompletedSharedNotes(this.CurrentUserId(), page)
                .ProjectTo<NoteResponseModel>()
                .ToList();

            List<NoteResponseModel> result = new List<NoteResponseModel>(privateNotes.Count + sharedNotes.Count);
            result.AddRange(privateNotes);
            result.AddRange(sharedNotes);

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetCompletedNotesCount()
        {
            int count = this.notesService.GetCompletedNotesCount(this.CurrentUserId());

            return this.Ok(count);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExpirationDate(int page = 1)
        {
            if (page <= 0)
            {
                return this.BadRequest(MessageConstants.InvalidPage);
            }

            var privateNotes = this.notesService
                .GetPrivateNotesWithExpirationDate(this.CurrentUserId(), page)
                .ProjectTo<NoteResponseModel>()
                .ToList();

            var sharedNotes = this.notesService
                .GetSharedNotesWithExpirationDate(this.CurrentUserId(), page)
                .ProjectTo<NoteResponseModel>()
                .ToList();

            List<NoteResponseModel> result = new List<NoteResponseModel>(privateNotes.Count + sharedNotes.Count);
            result.AddRange(privateNotes);
            result.AddRange(sharedNotes);

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExparationDateCount()
        {
            int count = this.notesService.GetExpiredNotesCount(this.CurrentUserId());

            return this.Ok(count);
        }

        [HttpDelete]
        public IHttpActionResult RemoveNoteById(string id)
        {
            if (id == string.Empty)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var privateNote = this.notesService
                .GetPrivateNoteById(id);
            if (privateNote != null && privateNote.UserId == this.CurrentUserId())
            {
                this.notesService.RemovePrivateNoteById(privateNote);

                return this.Ok(MessageConstants.RemoveNote);
            }

            var sharedNote = this.notesService.GetSharedNoteById(id);

            if (sharedNote != null)
            {
                User currentUserDb = sharedNote.Users.FirstOrDefault(u => u.Id == this.CurrentUserId());
                if (currentUserDb == null || currentUserDb.ProfileDetails.FullName != sharedNote.CreatedFrom)
                {
                    return this.BadRequest(MessageConstants.CurrentUserCannotRemoveThisNote);
                }

                this.notesService.RemoveSharedNoteById(sharedNote);

                return this.Ok(MessageConstants.RemoveNote);
            }

            return this.BadRequest(MessageConstants.NoteDoesNotExist);
        }

        [HttpPut]
        public IHttpActionResult SetComplete(string id)
        {
            if (id == string.Empty)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            var note = this.notesService.GetPrivateNoteById(id);

            if (note != null && note.UserId == this.CurrentUserId())
            {
                this.notesService.SetCompletePrivateNote(note);
                return this.Ok(MessageConstants.CompletedNoteSucessful);
            }
            
            var sharedNote = this.notesService.GetSharedNoteById(id);

            if (sharedNote == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            User currentUser = sharedNote.Users.FirstOrDefault(u => u.Id == this.CurrentUserId());
            if (currentUser == null)
            {
                return this.BadRequest(MessageConstants.CurrentUserCannotCompleteThisNote);
            }

            if (sharedNote.CreatedFrom != currentUser.ProfileDetails.FullName)
            {
                return this.BadRequest(MessageConstants.CurrentUserCannotCompleteThisNote);
            }

            this.notesService.SetCompleteSharedNote(sharedNote);
            return this.Ok(MessageConstants.CompletedNoteSucessful);
        }

        [NonAction]
        private bool CheckIfNotesAreExpired(IList<NoteDataModel> dbNotes)
        {
            bool hasChange = false;
            DateTime dateNow = DateTime.Now;

            foreach (var note in dbNotes)
            {
                if (note.ExpiredOn != null && note.ExpiredOn <= dateNow && !note.IsComplete && !note.IsExpired)
                {
                    var privateNote = this.notesService.GetPrivateNoteById(note.Id);
                    if (privateNote != null)
                    {
                        this.notesService.SetExpiredPrivateNote(privateNote);
                        hasChange = true;
                    }
                    else
                    {
                        var sharedNote = this.notesService.GetSharedNoteById(note.Id);
                        if (sharedNote != null)
                        {
                            this.notesService.SetExpiredSharedNote(sharedNote);
                            hasChange = true;
                        }
                    }
                }
            }

            return hasChange;
        }
    }
}
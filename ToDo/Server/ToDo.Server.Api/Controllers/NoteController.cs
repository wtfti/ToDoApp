﻿namespace ToDo.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Infrastructure.Validation;
    using Microsoft.AspNet.Identity;
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

            this.notesService.AddNote(this.CurrentUser(), note.Title, note.Content, note.ExpiredOn);

            return this.Ok(MessageConstants.CreateNote);
        }

        [HttpGet]
        public IHttpActionResult GetNotes(int page = 1)
        {
            var dbNotes = this.notesService
                .GetNotes(this.CurrentUser(), page);

            bool hasChange = this.CheckIfNotesAreExpired(dbNotes.ToList());

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotes(this.CurrentUser(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNotesFromToday(int page = 1)
        {
            var dbNotes = this.notesService
                .GetNotesFromToday(this.CurrentUser(), page);

            bool hasChange = this.CheckIfNotesAreExpired(dbNotes.ToList());

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotesFromToday(this.CurrentUser(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetCompletedNotes(int page = 1)
        {
            var dbNotes = this.notesService
                .GetCompletedNotes(this.CurrentUser(), page);

            bool hasChange = this.CheckIfNotesAreExpired(dbNotes.ToList());

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotes(this.CurrentUser(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNotesWithExpirateDate(int page = 1)
        {
            var dbNotes = this.notesService
                .GetNotesWithExpiredDate(this.CurrentUser(), page);

            bool hasChange = this.CheckIfNotesAreExpired(dbNotes.ToList());

            if (hasChange)
            {
                dbNotes = this.notesService
                    .GetNotesWithExpiredDate(this.CurrentUser(), page);
            }

            var result = dbNotes.ProjectTo<NoteResponseModel>();

            return this.Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult RemoveNoteById(int id)
        {
            string user = this.CurrentUser();

            var note = this.notesService
                .All()
                .Where(a => a.Id == id && a.UserId == user)
                .FirstOrDefault();
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.RemoveNoteById(note);

            return this.Ok(MessageConstants.RemoveNote);
        }

        [HttpPut]
        public IHttpActionResult SetComplete(int id)
        {
            var note = this.notesService.GetNoteById(id);

            if (note == null || note.UserId != this.CurrentUser())
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.SetComplete(note);

            return this.Ok();
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteTitle(int id, string newValue)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.CurrentUser());
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.ChangeNoteTitle(note, newValue);

            return this.Ok(MessageConstants.TitleChange);
        }

        [HttpPut]
        public IHttpActionResult ChangeNoteContent(int id, string newValue)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.CurrentUser());
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            this.notesService.ChangeNoteContent(note, newValue);

            return this.Ok(MessageConstants.ContextChange);
        }

        [HttpPut]
        public IHttpActionResult SetNoteExpireDate(int id, string date)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.CurrentUser());
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(
                date, 
                ValidationConstants.ExpireDateFormat, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDate);
            }

            this.notesService.ChangeNoteExpireDate(note, parsedDate);

            return this.Ok(MessageConstants.SetDate);
        }

        [HttpPut]
        public IHttpActionResult ChangeExpireDate(int id, string date)
        {
            var note = this.notesService.All().FirstOrDefault(a => a.Id == id && a.UserId == this.CurrentUser());
            if (note == null)
            {
                return this.BadRequest(MessageConstants.NoteDoesNotExist);
            }

            DateTime parsedDate;

            if (!DateTime.TryParseExact(
                date, 
                ValidationConstants.ExpireDateFormat, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None,
                out parsedDate))
            {
                return this.BadRequest(MessageConstants.InvalidDate);
            }

            this.notesService.ChangeNoteExpireDate(note, parsedDate);

            return this.Ok(MessageConstants.ChangeDate);
        }

        [NonAction]
        private string CurrentUser()
        {
            string result = this.User.Identity.GetUserId();

            return result;
        }

        [NonAction]
        private bool CheckIfNotesAreExpired(List<Note> dbNotes)
        {
            bool hasChange = false;
            DateTime dateNow = DateTime.Now;

            foreach (var note in dbNotes)
            {
                if (note.ExpiredOn != null && note.ExpiredOn <= dateNow && !note.IsComplete)
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
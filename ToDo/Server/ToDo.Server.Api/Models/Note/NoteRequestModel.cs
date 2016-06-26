﻿namespace ToDo.Api.Models.Note
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Server.Common;

    public class NoteRequestModel
    {
        [Required]
        [MinLength(ValidationConstants.TitleMinLenght)]
        [MaxLength(ValidationConstants.TitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.ContentMinLenght)]
        [MaxLength(ValidationConstants.ContentMaxLenght)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        [DisplayFormat(DataFormatString = ValidationConstants.ExpireDateFormat, ApplyFormatInEditMode = true)]
        public DateTime ExpiredOn { get; set; }
    }
}
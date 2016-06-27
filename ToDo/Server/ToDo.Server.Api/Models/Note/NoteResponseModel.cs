namespace ToDo.Api.Models.Note
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Infrastructure.Mapping;
    using Server.Common.Constants;

    public class NoteResponseModel : IMapFrom<Note>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.TitleMinLenght)]
        [MaxLength(ValidationConstants.TitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.ContentMinLenght)]
        [MaxLength(ValidationConstants.ContentMaxLenght)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public bool IsExpired { get; set; }
    }
}
namespace ToDo.Api.Models.Note
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Infrastructure.Ninject.Mapping;
    using Server.Common.Constants;

    public class NoteResponseModel : IMapFrom<PrivateNote>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.TitleMinLenght)]
        [MaxLength(ValidationConstants.TitleMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.ContentMinLenght)]
        [MaxLength(ValidationConstants.ContentMaxLenght)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiredOn { get; set; }

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        public bool IsComplete { get; set; }
    }
}
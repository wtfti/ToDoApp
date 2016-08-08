namespace ToDo.Api.Models.Note
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Infrastructure.Mapping;
    using Server.Common.Constants;

    public class SharedNoteResponseModel : IMapFrom<SharedNote>
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

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiredOn { get; set; }

        [Required]
        public string CreatedFrom { get; set; }

        [Required]
        public ICollection<User> Users { get; set; }

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        public bool IsComplete { get; set; }
    }
}
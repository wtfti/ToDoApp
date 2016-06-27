namespace ToDo.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Server.Common.Constants;

    public class Note
    {
        public int Id { get; set; }

        public string UserId { get; set; }

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

        public bool IsExpired { get; set; }
    }
}

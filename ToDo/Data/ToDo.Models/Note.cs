namespace ToDo.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Server.Common.Constants;

    public class Note : IEquatable<Note>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public virtual User User { get; set; }

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

        public bool Equals(Note other)
        {
            return this.Id == other.Id;
        }
    }
}

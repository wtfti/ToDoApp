namespace ToDo.Api.Models.Note
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NoteRequestModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title lenght must be between 3 and 30 long")]
        [MaxLength(30, ErrorMessage = "Title lenght must be between 3 and 30 long")]
        public string Title { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Content of note must be between 1 and 100 long")]
        [MaxLength(100, ErrorMessage = "Content of note must be between 1 and 100 long")]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime ExpiredOn { get; set; }
    }
}
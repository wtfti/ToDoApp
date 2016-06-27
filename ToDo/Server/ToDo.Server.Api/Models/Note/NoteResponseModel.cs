namespace ToDo.Api.Models.Note
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using Data.Models;
    using Server.Common.Constants;

    public class NoteResponseModel
    {
        public static Expression<Func<Note, NoteResponseModel>> FromModel
        {
            get
            {
                return x => new NoteResponseModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn.Value,
                    ExpiredOn = x.ExpiredOn
                };
            }
        }

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
    }
}
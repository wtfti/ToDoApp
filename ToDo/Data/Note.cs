namespace ToDo.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}

namespace ToDo.Data.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class Friend
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FirstUserId { get; set; }

        [Required]
        public string SecondUserId { get; set; }
    }
}

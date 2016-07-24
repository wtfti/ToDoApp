namespace ToDo.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Account;

    public class UserProfileRequestModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        public string Image { get; set; }
    }
}
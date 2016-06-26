namespace ToDo.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Server.Common;

    public class RegisterModel
    {
        [Required]
        [MinLength(ValidationConstants.UsernameMinLenght)]
        [MaxLength(ValidationConstants.UsernameMaxLenght)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(ValidationConstants.PasswordProperty)]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.EmailRegexPattern)]
        public string Email { get; set; }
    }
}
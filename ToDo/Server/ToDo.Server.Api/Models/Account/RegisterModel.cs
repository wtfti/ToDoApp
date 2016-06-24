namespace ToDo.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
        [Required]
        [MaxLength(15, ErrorMessage = "Username lenght must be between 1 and 15")]
        [MinLength(1, ErrorMessage = "Username lenght must be between 1 and 15")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation password are not same")]
        public string ConfirmPassword { get; set; }
        //Todo fix password to create hash

        [Required]
        [MaxLength(25, ErrorMessage = "Email lenght must be between 7 and 25")]
        [MinLength(7, ErrorMessage = "Email lenght must be between 7 and 25")]
        public string Email { get; set; }
    }
}
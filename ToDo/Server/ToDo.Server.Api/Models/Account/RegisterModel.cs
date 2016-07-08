﻿namespace ToDo.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Server.Common.Constants;

    public class RegisterModel
    {
        [Required]
        [RegularExpression(ValidationConstants.EmailRegexPattern, ErrorMessage = MessageConstants.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PasswordMaxLenght)]
        [MinLength(ValidationConstants.PasswordMinLenght)]
        public string Password { get; set; }

        [Required]
        [Compare(ValidationConstants.PasswordProperty)]
        public string ConfirmPassword { get; set; }
    }
}
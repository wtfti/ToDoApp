namespace ToDo.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Account;
    using Server.Common.Constants;

    public class UserProfileRequestModel
    {
        [MinLength(ValidationConstants.FullNameMinLenght)]
        [MaxLength(ValidationConstants.FullNameMaxLenght)]
        public string FullName { get; set; }

        [Range(ValidationConstants.MinAge, ValidationConstants.MaxAge)]
        public int? Age { get; set; }

        [Range(ValidationConstants.MinGender, ValidationConstants.MaxGender)]
        public GenderType Gender { get; set; }

        public string Image { get; set; }
    }
}
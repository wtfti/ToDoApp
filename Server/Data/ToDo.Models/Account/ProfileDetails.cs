namespace ToDo.Data.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using Server.Common.Constants;

    public class ProfileDetails
    {
        public string Id { get; set; }

        public virtual User User { get; set; }

        [Required]
        [MinLength(ValidationConstants.FullNameMinLenght)]
        [MaxLength(ValidationConstants.FullNameMaxLenght)]
        public string FullName { get; set; }

        [Range(ValidationConstants.MinAge, ValidationConstants.MaxAge)]
        public int? Age { get; set; }

        public GenderType Gender { get; set; }

        public virtual Background Background { get; set; }
    }
}

﻿namespace ToDo.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Server.Common.Constants;

    public class ProfileDetails
    {
        public string Id { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string FullName { get; set; }

        [Range(ValidationConstants.MinAge, ValidationConstants.MaxAge)]
        public int? Age { get; set; }

        public GenderType Gender { get; set; }
    }
}

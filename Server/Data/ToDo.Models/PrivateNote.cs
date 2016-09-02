﻿namespace ToDo.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PrivateNote : Note, IEquatable<PrivateNote>
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public virtual User User { get; set; }

        public bool Equals(PrivateNote other)
        {
            return this.Id == other.Id;
        }
    }
}

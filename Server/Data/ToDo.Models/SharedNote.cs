namespace ToDo.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class SharedNote : Note, IEquatable<SharedNote>
    {
        private ICollection<User> users;

        public SharedNote()
        {
            this.users = new HashSet<User>();
        }

        public string CreatedFrom { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public bool Equals(SharedNote other)
        {
            return this.Id == other.Id;
        }
    }
}

namespace ToDo.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Account;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Note> notes;
        private ICollection<User> friends;

        public User()
        {
            this.notes = new HashSet<Note>();
            this.friends = new HashSet<User>();
        }

        [Key]
        public override string Id { get; set; }

        [Index(IsUnique = true)]
        public override string Email { get; set; }

        public virtual ProfileDetails ProfileDetails { get; set; }

        public virtual ICollection<Note> Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        public virtual ICollection<User> Friends
        {
            get { return this.friends; }
            set { this.friends = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user == null)
            {
                return false;
            }

            return user.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Email.GetHashCode();
        }
    }
}

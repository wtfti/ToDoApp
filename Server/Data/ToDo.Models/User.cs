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
        private ICollection<PrivateNote> privateNotes;
        private ICollection<SharedNote> sharedNotes;
        private ICollection<Friend> friends;

        public User()
        {
            this.privateNotes = new HashSet<PrivateNote>();
            this.sharedNotes = new HashSet<SharedNote>();
            this.friends = new HashSet<Friend>();
        }

        [Key]
        public override string Id { get; set; }

        [Index(IsUnique = true)]
        public override string Email { get; set; }

        public virtual ProfileDetails ProfileDetails { get; set; }

        public virtual ICollection<PrivateNote> PrivateNotes
        {
            get { return this.privateNotes; }
            set { this.privateNotes = value; }
        }

        public virtual ICollection<SharedNote> SharedNotes
        {
            get { return this.sharedNotes; }
            set { this.sharedNotes = value; }
        }

        public virtual ICollection<Friend> Friends
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

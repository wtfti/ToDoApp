namespace ToDo.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Note> notes;

        public User()
        {
            this.notes = new HashSet<Note>();
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}

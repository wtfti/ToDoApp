namespace ToDo.Data
{
    using System.Data.Entity;
    using Common.Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Account;

    public class ToDoDbContext : IdentityDbContext<User>, IToDoDbContext
    {
        public ToDoDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Note> Notes { get; set; }

        public static ToDoDbContext Create()
        {
            return new ToDoDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileDetails>().HasRequired(x => x.User);
            modelBuilder.Entity<Background>().HasRequired(x => x.ProfileDetails);
            base.OnModelCreating(modelBuilder);
        }
    }
}

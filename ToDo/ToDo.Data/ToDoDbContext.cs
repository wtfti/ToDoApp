namespace ToDo.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class ToDoDbContext : IdentityDbContext<User>
    {
        public ToDoDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public static ToDoDbContext Create()
        {
            return new ToDoDbContext();
        }

        public virtual IDbSet<Note> Notes { get; set; }
    }
}

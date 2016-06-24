namespace ToDo.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<ToDoDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }
    }
}

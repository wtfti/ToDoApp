namespace ToDo.Data.Models.Account
{
    public class Background
    {
        public string Id { get; set; }

        public virtual ProfileDetails ProfileDetails { get; set; }

        public string Value { get; set; }
    }
}

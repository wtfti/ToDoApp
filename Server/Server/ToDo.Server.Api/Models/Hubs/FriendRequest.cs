namespace ToDo.Api.Models.Hubs
{
    using System;

    public class FriendRequest : IEquatable<FriendRequest>
    {
        public string Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool Equals(FriendRequest other)
        {
            return (this.From == other.From && this.To == other.To) ||
                (this.From == other.To && this.To == other.From);
        }
    }
}   
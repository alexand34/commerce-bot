using Bot.Enums;
using Bot.Models.BaseEnity;

namespace Bot.Models
{
    public class Order : BaseEntity
    {
        public virtual User UserId { get; set; }
        public Categories Category { get; set; }
        public string OrderJson { get; set; }
        public double Price { get; set; }

        public string DestinationAddress { get; set; }
    }
}
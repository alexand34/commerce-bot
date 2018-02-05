using Bot.Models.BaseEnity;

namespace Bot.Models
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
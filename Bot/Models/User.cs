using Bot.Enums;
using Bot.Models.BaseEnity;

namespace Bot.Models
{
    public class User : BaseEntity
    {
        public string ServiceUserId { get; set; }
        public string Name { get; set; }
        public Languages Language { get; set; }
    }
}
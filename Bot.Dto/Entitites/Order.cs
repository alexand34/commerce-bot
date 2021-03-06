using System;
using System.Collections.Generic;
using Bot.Dto.Enums;

namespace Bot.Dto.Entitites
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public virtual ICollection<OrderItem> OrderData { get; set; }
        public string DestinationAddress { get; set; }
        public double? DeliveryPrice { get; set; }
        public double? Price { get; set; }
        public int? RestaurantId { get; set; }
        public OrderState OrderState { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual BotUser User { get; set; }
    }
}

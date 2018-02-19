using System;
using System.Collections.Generic;

namespace Bot.Dto.Entitites
{
    public partial class Restaurant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Restaurant()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public int? CategoryId { get; set; }
        public double? AverageReceipt { get; set; }
        public double? Rating { get; set; }
        public string AppUserId { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Bot.Dto.Entitites
{
    public partial class BotUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BotUser()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public string MessengerId { get; set; }
        public string UserName { get; set; }
        public int? Language { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

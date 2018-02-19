using System.Collections.Generic;

namespace Bot.Dto.Entitites
{
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }
    
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ImgName { get; set; }
        public string FrenchCategoryName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}

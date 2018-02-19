using System.Collections.Generic;

namespace Bot.Dto.Entitites
{
    public partial class FoodCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FoodCategory()
        {
            this.Foods = new HashSet<Food>();
        }
    
        public int Id { get; set; }
        public string FoodCategoryName { get; set; }
        public string FoodCategoryFrenchName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Food> Foods { get; set; }
    }
}

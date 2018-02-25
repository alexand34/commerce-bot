using System;

namespace Bot.Dto.Entitites
{
    public partial class Food
    {
        public int Id { get; set; }
        public int FoodCategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string DishName { get; set; }
        public double Price { get; set; }
        public string Portion { get; set; }
        public string DishDescription { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual FoodCategory FoodCategory { get; set; }
    }
}

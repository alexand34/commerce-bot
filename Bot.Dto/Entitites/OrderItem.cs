namespace Bot.Dto.Entitites
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int Count { get; set; }

        public virtual Food Food { get; set; }
    }
}

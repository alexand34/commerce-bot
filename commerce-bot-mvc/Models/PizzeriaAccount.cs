namespace commerce_bot_mvc.Models
{
    public partial class PizzeriaAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public double? Rating { get; set; }
        public double? AverageReciept { get; set; }
        public double? Distance { get; set; }
        public virtual string AppUserId { get; set; }
    }
}
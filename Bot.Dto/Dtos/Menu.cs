using System.Collections.Generic;
using Bot.Dto.Entitites;

namespace Bot.Dto.Dtos
{
    public class Menu
    {
        public string Category { get; set; }
        public List<Food> Foods { get; set; }
    }
}

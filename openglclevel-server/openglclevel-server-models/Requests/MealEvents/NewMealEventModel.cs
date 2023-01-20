using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Requests.MealEvents
{
    public class NewMealEventModel
    {
        public NewMealEventModel()
        {
            NewMeals = new List<NewMealItemPair>();
            ItemMeals = new List<ExistingMealItemPair>();
        }
        public DateTime EventDate { get; set; }
        public int GlcLevel { get; set; }
        public List<NewMealItemPair> NewMeals { get; set; }
        public List<ExistingMealItemPair> ItemMeals { get; set; }
        public bool Postprandial { get; set; }
        public int MealType { get; set; }
    }

    public class NewMealItemPair
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class ExistingMealItemPair
    {
        public Guid ID { get; set; }
        public int Quantity { get; set; }
    }
}

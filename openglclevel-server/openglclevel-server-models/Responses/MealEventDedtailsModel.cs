using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Responses
{
    public class MealEventDedtailsModel
    {
        public MealEventDedtailsModel()
        {
            MealList = new List<EventMealItemsModel>();
        }
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public int GlcLevel { get; set; }
        public bool Pospandrial { get; set; }
        public List<EventMealItemsModel> MealList { get; set; }
        public string MealTypeText { get; set; }
    }

    public class EventMealItemsModel
    {
        public string MealName { get; set; }
        public Guid MealID { get; set; }
        public int Quantity { get; set; }
    }
}

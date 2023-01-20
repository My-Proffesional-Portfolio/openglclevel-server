using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.Requests.MealEventItems
{
    public class NewMealItemModel
    {
        public string Name { get; set; }
    }

    public class NewMealItemModelDB
    {
        public string Name { get; set; }
        public Guid  ID { get; set; }
    }
}

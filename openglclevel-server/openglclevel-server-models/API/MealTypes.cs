using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_models.API
{
    public class MealTypes
    {
        public MealTypes()
        {
            Meals = new List<MealNameType>();
        }
        public static List<MealNameType> Meals;

        public static List<MealNameType> GetMealTypesDefinition()
        {
            Meals = new List<MealNameType>();
            Meals.Add(new MealNameType(0, "Desayuno"));
            Meals.Add(new MealNameType(1, "Almuerzo"));
            Meals.Add(new MealNameType(2, "Comida"));
            Meals.Add(new MealNameType(3, "Merienda"));
            Meals.Add(new MealNameType(4, "Cena"));

            return Meals;
        }
    }

    public class MealNameType
    {
        public MealNameType(int type, string name)
        {
            Type = type;
            Name = name;
        }
        public int Type { get; set; }
        public string Name { get; set; }
    }
}

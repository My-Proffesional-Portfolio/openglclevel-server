using System;
using System.Collections.Generic;

namespace openglclevel_server_data.DataAccess;

public partial class MealEventItem
{
    public Guid Id { get; set; }

    public Guid MealEventId { get; set; }

    public string Description { get; set; }

    public virtual MealEvent MealEvent { get; set; }
}

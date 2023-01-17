using System;
using System.Collections.Generic;

namespace openglclevel_server_data.DataAccess;

public partial class MealItem
{
    public Guid Id { get; set; }

    public string MealName { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<MealEventItem> MealEventItems { get; } = new List<MealEventItem>();

    public virtual User User { get; set; }
}

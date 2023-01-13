using System;
using System.Collections.Generic;

namespace openglclevel_server_data.DataAccess;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string HashedPassword { get; set; }

    public string Email { get; set; }

    public string Salt { get; set; }

    public long UserNumber { get; set; }

    public string Name { get; set; }

    public string FirstName { get; set; }

    public virtual ICollection<MealEvent> MealEvents { get; } = new List<MealEvent>();
}

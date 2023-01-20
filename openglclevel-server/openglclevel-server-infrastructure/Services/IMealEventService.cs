﻿using openglclevel_server_models.Requests.MealEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Services
{
    public interface IMealEventService
    {
        public Task<Guid> AddMealEvent(Guid userID, NewMealEventModel mealEvent);
    }
}

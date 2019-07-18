using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficlightAPI.Models
{
    public enum State
    {
        APICallIsMade = 0,
        APICallIsNotMadeFirstTry = 1,
        APICallIsNotMadeSecondTry = 2
    }

}

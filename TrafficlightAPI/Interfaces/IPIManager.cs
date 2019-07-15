using Iot.Device.GrovePiDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficlightAPI.Interfaces
{
    public interface IPIManager
    {
        GrovePi grovePi {get;}
        int pulse { get; set; }

        List<string> GetList();
    }
}

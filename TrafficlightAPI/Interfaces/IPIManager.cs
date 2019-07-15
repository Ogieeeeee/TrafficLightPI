using Iot.Device.GrovePiDevice;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficlightAPI.Models;

namespace TrafficlightAPI.Interfaces
{
    public interface IPIManager
    {
        GrovePi grovePi {get;}
        int pulse { get; set; }

        List<string> GetList();
        string TurnLightOn(Colors color);
        string TurnLightOff(Colors color);
        int GetPulse();
        int IncrementPulse();




    }
}

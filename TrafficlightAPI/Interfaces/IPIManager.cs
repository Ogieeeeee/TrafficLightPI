using Iot.Device.CharacterLcd;
using Iot.Device.GrovePiDevice;
using Iot.Device.GrovePiDevice.Sensors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;
using TrafficlightAPI.Models;

namespace TrafficlightAPI.Interfaces
{
    public interface IPIManager
    {
        GrovePi grovePi { get; }
        int greenPulse { get; set; }
        int redPulse { get; set; }
        int orangePulse { get; set; }

        I2cDevice i2cLcdDevice { get; }
        I2cDevice i2cRgbDevice { get; }
        LcdRgb1602 lcd { get; }

        List<string> GetList();
        string TurnLightOn(Colors color);
        string TurnLightOff(Colors color);
        int GetPulse();
        int GetRedPulse();
        int GetOrangePulse();
        int IncrementPulse();
        void UpdateLCD();

    }
}

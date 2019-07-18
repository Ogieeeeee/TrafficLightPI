using Iot.Device.GrovePiDevice;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;
using TrafficlightAPI.Interfaces;
using Iot.Device.GrovePiDevice.Models;
using TrafficlightAPI.Models;
using System.Device.Gpio;
using Microsoft.Extensions.Hosting;
using Iot.Device.GrovePiDevice.Sensors;
using Iot.Device.CharacterLcd;
using System.Drawing;

namespace TrafficlightAPI.Managers
{
    public class PIManager : IPIManager
    {

        public List<string> Info;
        public int pulse { get; set; }
        I2cConnectionSettings i2CConnectionSettings;

        private GrovePi _grovePi;
        public GrovePi grovePi
        {
            get
            {
                return _grovePi;
            }
            private set
            {
                _grovePi = value;
            }
        }

        public PIManager()
        {
            i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));
            Info = new List<string>();
            pulse = 0;

            grovePi.PinMode(GrovePort.DigitalPin4, PinMode.Output);
            grovePi.PinMode(GrovePort.DigitalPin3, PinMode.Output);
            grovePi.PinMode(GrovePort.DigitalPin2, PinMode.Output);


        }

        public List<string> GetList()
        {
            return Info;
        }

        public string TurnLightOn(Colors color)
        {

            if (color == Colors.green)
            {
                //Low = Turn off
                //High = Turn on
                grovePi.DigitalWrite(GrovePort.DigitalPin4, PinValue.High);
                grovePi.DigitalWrite(GrovePort.DigitalPin3, PinValue.Low);

                //Api call is made increments pulse by 1
                IncrementPulse();
            }
            else if (color == Colors.red)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin3, PinValue.High);
            }
            else if (color == Colors.yellow)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin2, PinValue.High);
            }


            return $"Succesfully turned {color} on, pulse = {pulse}";
        }

        public string TurnLightOff(Colors color)
        {
            if (color == Colors.green)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin4, PinValue.Low);
            }
            else if (color == Colors.red)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin3, PinValue.Low);
            }
            else if (color == Colors.yellow)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin2, PinValue.Low);
            }


            return $"Succesfully turned {color} off, pulse = {pulse}";
        }

        public int GetPulse()
        {
            return pulse;
        }

        public int IncrementPulse()
        {
            pulse++;
            return pulse;
        }

    }
}

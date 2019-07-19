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
        public int redPulse { get; set; }
        public int orangePulse { get; set; }
        public int greenPulse { get; set; }
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

        private I2cDevice _i2cLcdDevice;
        public I2cDevice i2cLcdDevice
        {
            get
            {
                return _i2cLcdDevice;
            }
            private set
            {
                _i2cLcdDevice = value;
            }
        }

        private I2cDevice _i2cRgbDevice;
        public I2cDevice i2cRgbDevice
        {
            get
            {
                return _i2cRgbDevice;
            }
            private set
            {
                _i2cRgbDevice = value;
            }
        }

        LcdRgb1602 _lcd;
        public LcdRgb1602 lcd
        {
            get
            {
                return _lcd;
            }
            private set
            {
                _lcd = value;
            }
        }
        

        public PIManager()
        {
            i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));

            try
            {
                i2cLcdDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x3E));
                i2cRgbDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x62));
                lcd = new LcdRgb1602(i2cLcdDevice, i2cRgbDevice);

            }
            catch (Exception e)
            {
                Console.WriteLine("Lcd device not found.");
            }


            Info = new List<string>();
            greenPulse = 0;
            orangePulse = 0;
            redPulse = 0;

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
                IncrementRedPulse();
            }
            else if (color == Colors.yellow)
            {
                grovePi.DigitalWrite(GrovePort.DigitalPin2, PinValue.High);
                IncrementOrangePulse();
            }

            UpdateLCD();
            return $"Succesfully turned {color} on, pulse = {greenPulse}";
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

            UpdateLCD();
            return $"Succesfully turned {color} off, pulse = {greenPulse}";
        }

        public int GetPulse()
        {
            return greenPulse;
        }

        public int IncrementPulse()
        {
            greenPulse++;
            return greenPulse;
        }

        public int GetRedPulse()
        {
            return redPulse;
        }


        public int IncrementRedPulse()
        {
            redPulse++;
            return redPulse;
        }


        public int GetOrangePulse()
        {
            return orangePulse;
        }

        public int IncrementOrangePulse()
        {
            orangePulse++;
            return orangePulse;
        }

        public void UpdateLCD()
        {
            if (lcd != null)
            {
                lcd.Clear();
                lcd.Write($"Red: {GetRedPulse()} Green: {GetPulse()}");

                lcd.SetCursorPosition(0, 1);
                lcd.Write($"Orange:{GetOrangePulse()}");

            }
            else
            {
                Console.WriteLine("No led device detected");
            }

        }


    }
}

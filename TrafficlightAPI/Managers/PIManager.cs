using Iot.Device.GrovePiDevice;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;
using TrafficlightAPI.Interfaces;

namespace TrafficlightAPI.Managers
{
    public class PIManager : IPIManager
    {
        public List<string> Info;
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

        public int pulse { get; set; }



        public PIManager()
        {
            Info = new List<string>();
            i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));
            pulse = 0;
        }


        public List<string> GetList()
        {
            return Info;
        }
    }
}

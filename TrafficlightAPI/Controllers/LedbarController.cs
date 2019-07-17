using Iot.Device.CharacterLcd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TrafficlightAPI.Interfaces;

namespace TrafficlightAPI.Controllers
{
    [Route("api/Ledbar")]
    [ApiController]
    public class LedbarController: ControllerBase
    {
        private static IPIManager _piManager;
        public LedbarController(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

        [HttpGet("ShowText/{text}/{color}")]
        public ActionResult<string> ShowText(string text)
        {
            var i2cLcdDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x3E));
            var i2cRgbDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x62));
            var lcd = new LcdRgb1602(i2cLcdDevice, i2cRgbDevice);
            
            lcd.Write(text);
            lcd.SetBacklightColor(Color.Azure);
            
            return $"Succesfully printed text: {text}";
        }

    }
}

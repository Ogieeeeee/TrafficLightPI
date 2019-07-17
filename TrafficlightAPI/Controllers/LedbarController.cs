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
        private IHostedService _timerHostedService;

        public LedbarController(IPIManager pIManager, IHostedService timerHostedService)
        {
            _piManager = pIManager;
            _timerHostedService = timerHostedService;
        }

        [HttpGet("ShowText/{text}")]
        public ActionResult<string> ShowText(string text)
        {
            var i2cLcdDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x3E));
            var i2cRgbDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x62));
            var lcd = new LcdRgb1602(i2cLcdDevice, i2cRgbDevice);
            
            lcd.Write(text);
            lcd.SetBacklightColor(Color.SpringGreen);
            
            return $"Succesfully printed text: {text}";
        }

    }
}

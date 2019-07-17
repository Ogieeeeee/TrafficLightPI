using System.Collections.Generic;
using System.Device.I2c;
using Iot.Device.GrovePiDevice;
using Iot.Device.GrovePiDevice.Sensors;
using Iot.Device.GrovePiDevice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Device.Gpio;
using TrafficlightAPI.Models;
using TrafficlightAPI.Interfaces;
using System.Threading;
using Microsoft.Extensions.Hosting;
using TrafficlightAPI.Service;

namespace TrafficlightAPI.Controllers
{
    [Route("api/Lights")]
    [ApiController]
    public class LightsController : ControllerBase
    {

        //private static GrovePi grovePi;
        //I2cConnectionSettings i2CConnectionSettings;
        //int pulse = 0;

        private static IPIManager _piManager;
        private IHostedService _greenTimerHostedService;
        private OrangeTimerHostedService _orangeTimerHostedService;

        public LightsController(IPIManager pIManager, IHostedService timerHostedService, OrangeTimerHostedService orangeTimerhostedService)
        {
            _piManager = pIManager;
            _greenTimerHostedService = timerHostedService;
            _orangeTimerHostedService = orangeTimerhostedService;

            //i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            //grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));
        }

        // GET api/Lights
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return _piManager.GetList();
            //return new string[] { "value1", "value2" };
        }

        ////// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // Example: api/Lights/red/on
        [HttpGet("{color}/on")]
        public ActionResult<string> TurnLightOn(Colors color)
        {
            string result = "";
            if (color == Colors.green)
            {
                _greenTimerHostedService.StopAsync(new System.Threading.CancellationToken());
                result = _piManager.TurnLightOn(color);
                _greenTimerHostedService.StartAsync(new System.Threading.CancellationToken());

            }

            else if (color == Colors.yellow)
            {
                _orangeTimerHostedService.StartAsync(new System.Threading.CancellationToken());
                result = _piManager.TurnLightOn(color);
                _orangeTimerHostedService.StopAsync(new System.Threading.CancellationToken());

            }

            return result;
        }

        // Example: api/Lights/red/off
        [HttpGet("{color}/off")]
        public ActionResult<string> TurnLightOff(Colors color)
        {
            return _piManager.TurnLightOff(color);
        }

        // Example: api/Lights/GetPulse
        [HttpGet("GetPulse")]
        public ActionResult<int> GetPulse()
        {
            return _piManager.GetPulse();
        }

        // Example: api/Lights/IncrementPulse
        [HttpGet("IncrementPulse")]
        public ActionResult<int> IncrementPulse()
        {
            return _piManager.IncrementPulse();
        }




        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{

        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}



    }
}

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
        private static IPIManager _piManager;
        private IGreenHostedService _greenTimerHostedService;
        private IOrangeTimerHostedService _orangeTimerHostedService;

        public LightsController(IPIManager pIManager, IGreenHostedService greenTimerHostedService, IOrangeTimerHostedService orangeTimerhostedService)
        {
            _piManager = pIManager;
            _greenTimerHostedService = greenTimerHostedService;
            _orangeTimerHostedService = orangeTimerhostedService;
        }

        // GET api/Lights
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return _piManager.GetList();
        //}

        // Example: api/Lights/red/on
        [HttpGet("{color}/on")]
        public ActionResult<string> TurnLightOn(Colors color)
        {
            string result = "";
            if (color == Colors.green)
            {
                // This piece of code is triggered when a green api call is made.
                // It basically resets the timer by turning the service off and on.
                  
                _greenTimerHostedService.StopAsync(new System.Threading.CancellationToken());
                result = _piManager.TurnLightOn(color);
                _greenTimerHostedService.StartAsync(new System.Threading.CancellationToken());
            }

            else if (color == Colors.yellow)
            {
                // Basically the same as green. 
                _orangeTimerHostedService.StopAsync(new System.Threading.CancellationToken());
                result = _piManager.TurnLightOn(color);
                _orangeTimerHostedService.StartAsync(new System.Threading.CancellationToken());
            }

            else if (color == Colors.red)
            {
                result = _piManager.TurnLightOn(color);
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

    }
}

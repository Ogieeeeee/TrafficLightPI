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

namespace TrafficlightAPI.Controllers
{
    [Route("api/Lights")]
    [ApiController]
    public class LightsController : ControllerBase
    {

        //http://192.168.17.104:5000/api/red/on


        //private static GrovePi grovePi;
        //I2cConnectionSettings i2CConnectionSettings;
        //int pulse = 0;

        public static IPIManager _piManager; 

        public LightsController(IPIManager pIManager)
        {
            _piManager = pIManager;
            //i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            //grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));
        }

        // GET api/values
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

        [HttpGet("{color}/on")]
        public ActionResult<string> TurnLightOn(Colors color)
        {
            return _piManager.TurnLightOn(color);
        }

        [HttpGet("{color}/off")]
        public ActionResult<string> TurnLightOff(Colors color)
        {
            return _piManager.TurnLightOff(color);
        }

        [HttpGet("GetPulse")]
        public ActionResult<int> GetPulse()
        {
            return _piManager.GetPulse();
        }

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

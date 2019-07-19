using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrafficlightAPI.Interfaces;

namespace TrafficlightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ButtonController : ControllerBase
    {
        private static IPIManager _piManager;

        public ButtonController(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

    }
}
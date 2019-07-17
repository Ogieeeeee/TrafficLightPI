using Iot.Device.GrovePiDevice.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficlightAPI.Interfaces;

namespace TrafficlightAPI.Service
{
    public class OrangeTimerHostedService : IHostedService
    {
        private Timer _timerOrange;
        private IPIManager _piManager;

        public OrangeTimerHostedService(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STARTING");
            _timerOrange = new Timer(TurnOffTimerEvery5sec, null, 5000, 5000);

            return Task.CompletedTask;
        }


        void TurnOffTimerEvery5sec(object state)
        {
            _piManager.TurnLightOff(Models.Colors.yellow);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timerOrange?.Change(Timeout.Infinite, 0);
            Console.WriteLine("Stopped orange Timer");
            return Task.CompletedTask;
        }
    }
}

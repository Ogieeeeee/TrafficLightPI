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

    public interface IOrangeTimerHostedService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);

    }

    public class OrangeTimerHostedService : IOrangeTimerHostedService, IHostedService
    {
        private Timer _timerOrange;
        private IPIManager _piManager;
        private CancellationToken _cancellationToken;

        public OrangeTimerHostedService(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STARTING ORANGE TIMER");
            _cancellationToken = cancellationToken;
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
            Console.WriteLine("STOPPED ORANGE TIMER");
            return Task.CompletedTask;
        }
    }
}

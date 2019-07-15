using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficlightAPI.Controllers;
using TrafficlightAPI.Interfaces;
using TrafficlightAPI.Managers;
using TrafficlightAPI.Models;

namespace TrafficlightAPI
{

    public enum State
    {
        APICallIsMade = 0,
        APICallIsNotMadeFirstTry = 1,
        APICallIsNotMadeSecondTry = 2
    }

    public class HelloWorldHostedService : IHostedService
    {
        private Timer _timer;
        private IPIManager _piManager;
        State apiStatus;
        int PulseCheckBegin, PulseCheckAfter;

        public HelloWorldHostedService(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STARTING");
            //piManager = new PIManager();
            PulseCheckBegin = _piManager.GetPulse();
            apiStatus = State.APICallIsMade;

            _timer = new Timer(CheckForAPICallEvery60Seconds, null, 0, 10_000) ;

            return Task.CompletedTask;
        }
        void CheckForAPICallEvery60Seconds(object state)
        {
            PulseCheckAfter = _piManager.GetPulse();

            if(PulseCheckAfter > PulseCheckBegin)
            {
                apiStatus = State.APICallIsMade;

                _piManager.TurnLightOn(Colors.green);
                _piManager.TurnLightOff(Colors.red);
                Console.WriteLine("Turned green light on");
                Console.WriteLine("Changed timer");
                _timer.Change(0, 10_000);
                
            }
            else
            {
                if(apiStatus == State.APICallIsMade)
                {
                    apiStatus = State.APICallIsNotMadeFirstTry;
                }
                else if ( apiStatus == State.APICallIsNotMadeFirstTry)
                {
                    apiStatus = State.APICallIsNotMadeSecondTry;
                }
                else if ( apiStatus == State.APICallIsNotMadeSecondTry)
                {
                    Console.WriteLine("tunred red light on");
                    _piManager.TurnLightOn(Colors.red);
                    _piManager.TurnLightOff(Colors.green);
                }
            }

            PulseCheckBegin = _piManager.GetPulse();
            Console.WriteLine("Pulse begin: " + PulseCheckBegin + " " + " Pulse afteR: " + PulseCheckAfter + " State: " + apiStatus);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}

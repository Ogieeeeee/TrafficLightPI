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

namespace TrafficlightAPI.Service
{

    public class GreenTimerHostedService : IGreenHostedService, IHostedService
    {
        private Timer _timerGreen;
        private IPIManager _piManager;
        private ApiState apiStatus;

        //These 2 integers are used to check if an api call is made
        int PulseCheckBegin, PulseCheckAfter;

        public GreenTimerHostedService(IPIManager pIManager)
        {
            _piManager = pIManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STARTING GREEN TIMER");

            //Getting a integer back. This is a number of how many  api calls are made ... If api call is made it increments by 1
            PulseCheckBegin = _piManager.GetPulse();
            Console.WriteLine($"PulseCheckerBegin: {PulseCheckBegin}");
            apiStatus = ApiState.APICallIsNotMadeFirstTry;

            // change 10_000  = 10 sec,  60_000 = 1 min
            // first 10_000 = delay when it should start executing the CheckForAPICallEvert120seconds method
            // the second 10_000 is when it should execute over and over 
            // press ctrl + shift to check method explanation
            _timerGreen = new Timer(CheckForAPICallEvery120Seconds, null, 60_000, 60_000);

            return Task.CompletedTask;
        }

        void CheckForAPICallEvery120Seconds(object state)
        {
            // getting after x seconds how many appi calls are made
            PulseCheckAfter = _piManager.GetPulse();

            // checks if there are api calls made. If they have the same number there is no API call made.
            if (PulseCheckAfter > PulseCheckBegin)
            {
                Console.WriteLine($"PulseCheckerAfter: {PulseCheckAfter}");
                apiStatus = ApiState.APICallIsMade;
                Console.WriteLine("GreenLight Should be on");
                Console.WriteLine("Red light should be off");
            }

            //if there is no api call made
            else
            {
                //changing the state and/or turn on red light
                if (apiStatus == ApiState.APICallIsMade)
                {
                    apiStatus = ApiState.APICallIsNotMadeFirstTry;
                }
                else if (apiStatus == ApiState.APICallIsNotMadeFirstTry)
                {
                    apiStatus = ApiState.APICallIsNotMadeSecondTry;
                    Console.WriteLine("1 min should be passed");
                }
                else if (apiStatus == ApiState.APICallIsNotMadeSecondTry)
                {
                    Console.WriteLine("2 mins should be passed");
                    Console.WriteLine("turned red light on");
                    _piManager.TurnLightOn(Colors.red);
                    _piManager.TurnLightOff(Colors.green);

                    //Changes timer to 2 minutes so it keeps..
                    //.. sending a turn red light on message every 2 minutes instead of 1 minute
                    // 20_000 = 20 seconds.  // 120_0000 = 120 seconds
                    _timerGreen.Change(120_000, 120_000);
                }
            }

            PulseCheckBegin = _piManager.GetPulse();
            Console.WriteLine("Pulse begin: " + PulseCheckBegin + " " + " Pulse afteR: " + PulseCheckAfter + " State: " + apiStatus);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timerGreen?.Change(Timeout.Infinite, 0);
            Console.WriteLine("STOPPED GREEN TIMER");
            return Task.CompletedTask;
        }
    }
}

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

    public enum State
    {
        APICallIsMade = 0,
        APICallIsNotMadeFirstTry = 1,
        APICallIsNotMadeSecondTry = 2
    }

    public class GreenTimerHostedService : IHostedService
    {
        private Timer _timerGreen;
        private IPIManager _piManager;
        State apiStatus;
        int PulseCheckBegin, PulseCheckAfter;

        public GreenTimerHostedService(IPIManager pIManager)
        {
            _piManager = pIManager;
        }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STARTING");

            //get an integer if a api call is changed... If api call is made it increments by 1
            PulseCheckBegin = _piManager.GetPulse();
            Console.WriteLine($"PulseCheckerBegin: {PulseCheckBegin}");
            apiStatus = State.APICallIsNotMadeFirstTry;


            // change 10_000  = 10 sec,  60_000 = 1 min
            // first 10_000 = delay when it should start executing the CheckForAPICallEvert120seconds method
            // the second 10_000 is when it should execute over and over 
            // press ctrl + shift to check method explenation

            //_timerOrange = new Timer();
            _timerGreen = new Timer(CheckForAPICallEvery120Seconds, null, 10_000 ,10_000) ;

            return Task.CompletedTask;
        }



        void CheckForAPICallEvery120Seconds(object state)
        {
            PulseCheckAfter = _piManager.GetPulse();

            if(PulseCheckAfter > PulseCheckBegin)
            {
                Console.WriteLine($"PulseCheckerAfter: {PulseCheckAfter}");


                apiStatus = State.APICallIsMade;

                Console.WriteLine("GreenLight Should be on");
                Console.WriteLine("Red light hsould be off");

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
                    Console.WriteLine("1 min should be passed");
                }
                else if ( apiStatus == State.APICallIsNotMadeSecondTry)
                {
                    Console.WriteLine("2 mins should be passed");
                    Console.WriteLine("turned red light on");
                    _piManager.TurnLightOn(Colors.red);
                    _piManager.TurnLightOff(Colors.green);
                    _timerGreen.Change(20_000, 20_000);
                }
            }

            PulseCheckBegin = _piManager.GetPulse();
            Console.WriteLine("Pulse begin: " + PulseCheckBegin + " " + " Pulse afteR: " + PulseCheckAfter + " State: " + apiStatus);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timerGreen?.Change(Timeout.Infinite, 0);
            Console.WriteLine("Stopped Timer");
            return Task.CompletedTask;
        }
    }
}

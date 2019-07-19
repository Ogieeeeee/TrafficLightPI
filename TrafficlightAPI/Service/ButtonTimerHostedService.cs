using Iot.Device.GrovePiDevice.Sensors;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficlightAPI.Interfaces;
using Iot.Device.GrovePiDevice.Models;
using System.Device.Gpio;
using TrafficlightAPI.Models;

namespace TrafficlightAPI.Service
{
    public class ButtonTimerHostedService : IHostedService
    {
        private Timer _timer;
        private Button button;
        private Buzzer buzzer;
        private LightSensor lightSensor;
        private SoundSensor soundSesnor;
        private UltrasonicSensor ultrasonicSensor;
        IPIManager _piManager;
        

        public ButtonTimerHostedService(IPIManager Pimanager)
        {
            _piManager = Pimanager;
            button = new Button(_piManager.grovePi, GrovePort.DigitalPin7);
            buzzer = new Buzzer(_piManager.grovePi, GrovePort.DigitalPin6);
            buzzer.Value = 10;

            lightSensor = new LightSensor(_piManager.grovePi, GrovePort.AnalogPin0);
            soundSesnor = new SoundSensor(_piManager.grovePi, GrovePort.AnalogPin1);
            ultrasonicSensor = new UltrasonicSensor(_piManager.grovePi, GrovePort.DigitalPin8);


        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start Button service");
            _timer = new Timer(DoWork, null, 0, 100);
            return Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            Console.WriteLine("InsideDoWork method");
            Console.WriteLine(button.Value);
            Console.WriteLine(button.ToString());
            if(button.Value == PinValue.High)
            {
                _piManager.TurnLightOn(Models.Colors.yellow);
                buzzer.Start();
            }
            if(button.Value == PinValue.Low)
            {
                buzzer.Stop();
            }
            Console.WriteLine("LightSensor ADC");
            Console.WriteLine($" ADC = {lightSensor.Value}, Percentage : {lightSensor.ValueAsPercent}");


            Console.WriteLine("Sound DB");
            Console.WriteLine($"DB DC: {soundSesnor.Value}, Percentage: {soundSesnor.ValueAsPercent} ");

            Console.WriteLine(ultrasonicSensor.SensorName);
            Console.WriteLine("Distance: " + ultrasonicSensor.Value);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            Console.WriteLine("Stopped Button Service");
            return Task.CompletedTask;
        }
    }
}

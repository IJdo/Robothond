using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LattePanda.Firmata; // includes arduino.cs needed to send UART messages to Arduino from Latte



namespace RobotHond.Components
{
 
    class RobotArduino
    {
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public Arduino arduino;
        public RobotArduino()
        {
            arduino = new Arduino();
        }
        
        public void Sleep(int delayMicroseconds)
        {
            manualResetEvent.WaitOne(TimeSpan.FromMilliseconds((double)delayMicroseconds / 1000d));
        }

    }


    class Motor
    {
        RobotArduino RobotBrain;
        public int Motor_IN1;
        public int Motor_IN2;
        public int Motor_PWM;

        public Motor(RobotArduino Brain, int IN1, int IN2, int PWM)
        {
            RobotBrain = Brain;
            Motor_IN1 = IN1;
            Motor_IN2 = IN2;
            Motor_PWM = PWM;
            this.Init();
        }
        public void Init() 
        {
            RobotBrain.arduino.pinMode(Motor_IN1, Arduino.OUTPUT);
            RobotBrain.arduino.pinMode(Motor_IN2, Arduino.OUTPUT);
            RobotBrain.arduino.pinMode(Motor_PWM, Arduino.PWM);
        }
    }
    class Ultrasonic_Sensor
    {
        RobotArduino RobotBrain;
        public int Ultra_TRG;
        public int Ultra_ECH;
        public double Distance;
        private double SPEED_OF_SOUND = 343;
        private static Stopwatch stopWatch = new Stopwatch();
        public Ultrasonic_Sensor(RobotArduino Brain, int TRG, int ECH)
        {
            RobotBrain = Brain;
            Ultra_TRG = TRG;
            Ultra_ECH = ECH;
            this.Init();
        }
        public void Init()
        {
            RobotBrain.arduino.pinMode(Ultra_TRG, Arduino.OUTPUT);
            RobotBrain.arduino.pinMode(Ultra_ECH, Arduino.INPUT);
            RobotBrain.arduino.digitalRead(Ultra_TRG);
            Distance = 3; // initialize with 3 because 3 is maximum distance in a 3x3 room
        }


        public double GetTimeUntilNextEdge(int maximumTimeToWaitInMilliseconds)
        {
            var t = Task.Run(() =>
            {
                stopWatch.Reset();

                while (RobotBrain.arduino.digitalRead(Ultra_TRG) != Arduino.HIGH)
                { 
                };
                stopWatch.Start();

                while (RobotBrain.arduino.digitalRead(Ultra_TRG) != Arduino.HIGH)
                { 
                };
                stopWatch.Stop();

                return stopWatch.Elapsed.TotalSeconds;
            });

            bool isCompleted = t.Wait(TimeSpan.FromMilliseconds(maximumTimeToWaitInMilliseconds));

            if (isCompleted)
            {
                return t.Result;
            }
            else
            {
                return -1d;
            }
        }

        private double PulseWidth()
        {
            //reset the pin.
            RobotBrain.arduino.digitalWrite(Ultra_TRG, Arduino.LOW);
            RobotBrain.Sleep(5);
            //Set triggerpin high
            RobotBrain.arduino.digitalWrite(Ultra_TRG, Arduino.HIGH);
            RobotBrain.Sleep(10);
            //set trigger pin low after 10 microseconds
            RobotBrain.arduino.digitalWrite(Ultra_TRG, Arduino.LOW);
            // Read the signal from the sensor: a HIGH pulse whose
            // duration is the time (in microseconds) from the sending
            // of the ping to the reception of its echo off of an object.
            return this.GetTimeUntilNextEdge(100);
        }


        public void Calculate_distance()
        {
            //To calculate the distance of a ultrasonic sensor we need to calculate the time it takes for the echo pin to go to HIGH after the Trigger pin sends out a pulse.
            //delta t = Timedelay, c = speed of sound, D = distance measured. Speed of Sound = 343 m/s in dry air at 20 degree celsius. Since we measure the returning pulse we need to divide by 2.
            //the way the ultrasonic sensor works is it:
            //1. Trigger pin goes high and sends a 5 volt 10uS pulse.
            //2. Ultrasonic sensor sends out 8 pulses(40Khz).
            //3.The Echo pin outputs a pulse of 150uS to 25mS.
            //The pulse width of the echopulse is used to calculate the distance.

            //TO DO: Set trigger pin high. start a timer when echo pin goes high. End timer when echo pin goes low.
            //how to wait 10 microseconds???
            //the firmata libary uses a uart connection from the LattePanda processor to the Arduino co-processor. This uart connection has a baudrate of 56700. This means that it takes 17.361 microseconds.
            //this means that to wait 10 microseconds after the pin is set to high we have to wait 27.361 microseconds. The issue here is that to set the pin low after exactly 10 microseconds it would take 17.361 micro seconds to send that signal.
            // this means the pin stays high for 17.361 microseconds to long and it is impossiable to keep it high exactly 10 micro seconds. To tackle this problem we should adapt the firmata libary running on the arduino to either:
            // a. accept a higher baudrate (atleast 115200 https://lucidar.me/en/serialib/most-used-baud-rates-table/).
            // b. add my own get pulsewidth function to the firmata lib on the arduino.
            // Option b seems the most logical to me so this is what I am gonna do.


            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //RobotBrain.arduino.digitalWrite(Ultra_TRG, Arduino.HIGH);
            //sw.Start();
            //if(sw.ElapsedTicks == )
            Distance = (SPEED_OF_SOUND / 2 * PulseWidth()); //Distance in meters
        }
    }
}

using LattePanda.Firmata; // includes arduino.cs needed to send UART messages to Arduino from Latte
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotHond.Components;

/*digital pins */
//Motor_A_IN1 = 8;
//Motor_A_IN2 = 9;
//Motor_B_IN1 = 10;
//Motor_B_IN2 = 11;
//Ultra_FR_TRG = 18;
//Ultra_FR_ECH = 19;
//Ultra_RGHT_TRG = 20;
//Ultra_RGHT_ECH = 21;
//Ultra_LFT_TRG = 22;
//Ultra_LFT_ECH = 23;
/*for speed control we must connect a PWN output pin of the arduino to the enable pins of the motor. D5 and D6 are still un used and able to provide pwm signal */


namespace RobotHond
{
    class Robot
    {
        public RobotArduino RobotBrain;
        Motor Motor_RGHT;
        Motor Motor_LFT;
        public Ultrasonic_Sensor Ultra_FRNT;
        public Ultrasonic_Sensor Ultra_RGHT;
        public Ultrasonic_Sensor Ultra_LFT;
        public Robot()
        {
            RobotBrain = new RobotArduino();
            Motor_RGHT = new Motor(RobotBrain, 8, 9, 5);
            Motor_LFT = new Motor(RobotBrain, 10, 11, 6);
            Ultra_FRNT = new Ultrasonic_Sensor(RobotBrain, 18, 19); //d18 and d19 are pins A0 and A1
            Ultra_RGHT = new Ultrasonic_Sensor(RobotBrain, 20, 21); // d20 and d21 are pins A2 and A3
            Ultra_LFT = new Ultrasonic_Sensor(RobotBrain, 22, 23); // d22 and d23 are pins A4 and A5
        }

        public void DriveForward(byte Speed)
        {
            //Motor control Right
            RobotBrain.arduino.analogWrite(Motor_RGHT.Motor_PWM, Speed);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN1, Arduino.HIGH);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN2, Arduino.LOW);
            //Motor Control Left
            RobotBrain.arduino.analogWrite(Motor_LFT.Motor_PWM, Speed);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN1, Arduino.HIGH);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN2, Arduino.LOW);
        }
        public void StopMotor()
        {
            //stop the motor
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN1, Arduino.LOW);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN2, Arduino.LOW);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN1, Arduino.LOW);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN2, Arduino.LOW);
        }
        public void TurnLeft(byte Speed, int Duration) //to skid turn to the left the right wheel has to be powered while the left wheel is off. To increase smoothness diffrent pwm speeds can be used. Duration is the duration of turning in milisecs
        {
            RobotBrain.arduino.analogWrite(Motor_RGHT.Motor_PWM, Speed);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN1, Arduino.HIGH);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN2, Arduino.LOW);
            Thread.Sleep(Duration); 
            //stop the motor
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN1, Arduino.LOW);
            RobotBrain.arduino.digitalWrite(Motor_RGHT.Motor_IN2, Arduino.LOW);
        }
        public void TurnRight(byte Speed, int Duration) // to skid turn to the right the left wheel has to be powered and the right wheel has to be turned off. To increase smoothness diffrent pwm speeds can be used.
        {
            RobotBrain.arduino.analogWrite(Motor_LFT.Motor_PWM, Speed);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN1, Arduino.HIGH);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN2, Arduino.LOW);
            Thread.Sleep(Duration);
            //stop the motor
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN1, Arduino.LOW);
            RobotBrain.arduino.digitalWrite(Motor_LFT.Motor_IN2, Arduino.LOW);
        }
        public void Collect_Data() 
        {

        }

    }
  
}

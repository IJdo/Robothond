

namespace RobotHond
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Robot Hond = new Robot();
            while (true)
            {

                if (Hond.Ultra_FRNT.Distance <= 0.20)
                {
                    Hond.StopMotor();
                    if (Hond.Ultra_RGHT.Distance >= Hond.Ultra_LFT.Distance)
                    {
                        Hond.TurnRight(100, 100);
                    }
                    else
                    {
                        Hond.TurnLeft(100, 100);
                    }
                }
                else
                {
                    Hond.DriveForward(100);
                }
                Hond.Ultra_FRNT.Calculate_distance();
                Hond.Ultra_RGHT.Calculate_distance();
                Hond.Ultra_LFT.Calculate_distance();

                //Hond.Ultra_FRNT.Calculate_distance();
                //Console.WriteLine("afstand = " + Hond.Ultra_FRNT.Distance);                
            }
        }
    }
}

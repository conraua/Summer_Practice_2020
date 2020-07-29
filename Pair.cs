using System;

namespace SummerPractice2020
{
    public class Pair
    {
        public double x { get; set; }
        public double y { get; set; }

        public Pair(double x1 = 0.0, double y1 = 0.0)
        {
            x = x1;
            y = y1;
        }
        
        public double Length()
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
    }
}
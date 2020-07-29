using System;

namespace SummerPractice2020
{
    public class Pair
    {
        public double x { get; set; }
        public double y { get; set; }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
    }
}
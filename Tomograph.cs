using System;
using System.Collections.Generic;

namespace SummerPractice2020
{
    public class Tomograph
    {
        public readonly int Nphi, Nr;
        private double phi = 0.0;
        private double rho = 0.0;
        private double parameter1 = 0.0;
        private double parameter2 = 0.0;
        private double discriminant = 0.0; 
        private double incomingRadiationDensity = 0.0;
        public Pair omega = new Pair();
        public Pair eta = new Pair();
        public Pair distance = new Pair();
        public List<List<double>> H = new List<List<double>>();
        public Figure figure = new Figure();
        public Tomograph(Figure fig, int Nphi1 = 100, int Nr1 = 100)
        {
            figure = fig;
            Nphi = Nphi1;
            Nr = Nr1;
        }
        
        public Tomograph(int Nphi1 = 100, int Nr1 = 100)
        {
            Nphi = Nphi1;
            Nr = Nr1;
        }
        
        public void CalculateRadiationDensity()
        {
            for (int i = 0; i < Nphi; i++)
            {
                List<double> tmp = new List<double>();
                phi = 2 * Math.PI * i / Nphi;
                omega.x = Math.Sin(phi);
                omega.y = -Math.Cos(phi);
                for (int j = -Nr; j <= Nr; j++)
                {
                    rho = j / (double)Nr;
                    eta.x = rho * Math.Cos(phi) + Math.Sqrt(1 - rho * rho) * Math.Sin(phi);
                    eta.y = rho * Math.Sin(phi) - Math.Sqrt(1 - rho * rho) * Math.Cos(phi);
                    discriminant = 4 * (Math.Pow((eta.x * omega.x + eta.y * omega.y), 2) - (omega.x * omega.x + 
                                             omega.y * omega.y) * (eta.x * eta.x + eta.y * eta.y - 1));
                    parameter1 = (-2 * (eta.x * omega.x + eta.y + omega.y) + Math.Sqrt(discriminant)) / 
                                            (2 * (omega.x * omega.x + omega.y * omega.y));
                    parameter2 = (-2 * (eta.x * omega.x + eta.y + omega.y) - Math.Sqrt(discriminant)) / 
                                            (2 * (omega.x * omega.x + omega.y * omega.y));
                    distance.x = eta.x - omega.x * Math.Max(parameter1, parameter2);
                    distance.y = eta.y - omega.y * Math.Max(parameter1, parameter2);
                    for (double k = 0; k < distance.Length(); k += distance.Length() / 100)
                    {
                        Pair point = new Pair();
                        point.x = eta.x - omega.x * k;
                        point.y = eta.y - omega.y * k;
                        incomingRadiationDensity += Mu(point) * distance.Length() / 100;
                    }
                    tmp.Add(incomingRadiationDensity);
                    incomingRadiationDensity = 0;
                }
                H.Add(tmp);
            }
        }

        public double Mu(Pair r)
        {
            if (figure.name == "Rectangle")
            {
                if (r.x > figure.x - figure.a / 2 && r.x < figure.x + figure.a / 2 && 
                    r.y > figure.y - figure.b / 2 && r.y < figure.y + figure.b / 2)
                {
                    return 1.536;
                }

                return 0.0351;
            }
            else
            {
                if (Math.Pow((r.x - figure.x), 2) / (figure.a * figure.a) + 
                        Math.Pow((r.y - figure.y), 2) / (figure.b * figure.b) - 
                            figure.r * figure.r <= 0)
                {
                    return 1.536;
                }

                return 0.0351;
            }
        }
    }
}
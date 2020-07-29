using System;
using System.Collections.Generic;

namespace SummerPractice2020
{
    public class Tomograph
    {
        public int Nphi = 100;
        public int Nr = 100;
        public double phi = 0.0;
        public Pair omega;
        public double rho = 0.0;
        public Pair eta;
        public double parameter1 = 0.0, parameter2 = 0.0;
        public double discriminant = 0.0;
        public Pair distance;
        public double incomingRadiationDensity = 0.0;
        public List<List<double>> H = new List<List<double>>();

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
                    rho = j / Nr;
                    eta.x = rho * Math.Cos(phi) + Math.Sqrt(1 - rho * rho) * Math.Sin(phi);
                    eta.y = rho * Math.Sin(phi) - Math.Sqrt(1 - rho * rho) * Math.Cos(phi);
                    discriminant = 4 * (Math.Pow((eta.x * omega.x + eta.y * omega.y), 2) - (Math.Pow(omega.x, 2) + 
                                            Math.Pow(omega.y, 2)) * (Math.Pow(eta.x, 2) + Math.Pow(eta.x, 2) - 1));
                    parameter1 = (2 * (eta.x * omega.x + eta.y + omega.y) + Math.Sqrt(discriminant)) / 
                                            (2 * (Math.Pow(omega.x,2) + Math.Pow(omega.y, 2)));
                    parameter2 = (2 * (eta.x * omega.x + eta.y + omega.y) - Math.Sqrt(discriminant)) / 
                                 (2 * (Math.Pow(omega.x,2) + Math.Pow(omega.y, 2)));
                    distance.x = eta.x - omega.x * Math.Max(parameter1, parameter2);
                    distance.x = eta.y - omega.y * Math.Max(parameter1, parameter2);
                    for (double k = 0; k < distance.Length(); k += distance.Length() / 100)
                    {
                        incomingRadiationDensity += Mu(eta) * distance.Length() / 100;
                    }
                    tmp.Add(incomingRadiationDensity);
                }
                H.Add(tmp);
            }
        }

        public double Mu(Pair r)
        {
            if (Math.Pow((r.x - 50), 2) + Math.Pow((r.y - 50), 2) - 400 <= 0)
            {
                return 2;
            }
            return 0.1;
        }
    }
}
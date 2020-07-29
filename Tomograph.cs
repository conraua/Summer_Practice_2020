using System;
using System.Collections.Generic;

namespace SummerPractice2020
{
    public class Tomograph
    {
        public readonly int Nphi, Nr;
        private double phi, rho, parameter1, parameter2, discriminant, incomingRadiationDensity;
        public Pair omega = new Pair();
        public Pair eta = new Pair();
        public Pair distance = new Pair();
        public List<List<double>> H = new List<List<double>>();
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
                    incomingRadiationDensity = 0;
                }
                H.Add(tmp);
            }
        }

        public double Mu(Pair r)
        {
            //if (Math.Pow((r.x - 0.5), 2) + Math.Pow((r.y - 0.5), 2) - 0.04 <= 0)
            if (r.x > -0.2 && r.x < 0.2 && r.y > -0.2 && r.y < 0.2)
            {
                return 1.53;
            }
            return 0.0361;
        }
    }
}
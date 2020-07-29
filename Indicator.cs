using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml.Schema;

namespace SummerPractice2020
{
    public class Indicator
    {
        public readonly int Nphi, Nr, Nx, Ny;
        public List<List<double>> IndicatorValues = new List<List<double>>();
        private List<List<double>> Hgrad = new List<List<double>>();

        private void calculateGradient(List<List<double>> H) 
        {
            for (int i = 0; i < H.Count; i++)
            {
                List<double> tmp = new List<double>();
                tmp.Add(0);
                for (int j = 1; j < H[i].Count; j++)
                {
                    tmp.Add(H[i][j - 1] - H[i][j]);
                }
                Hgrad.Add(tmp);
            }
        }

        public Indicator(Tomograph tm, int Nx1 = 100, int Ny1 = 100)
        {
            Nphi = tm.Nphi;
            Nr = tm.Nr;
            Nx = Nx1;
            Ny = Ny1;
            calculateGradient(tm.H);
        }
        private double Ind(Pair r)
        {
            double sum = 0.0;
            int index;
            double S;
            for (int j = 0; j < Nphi; j++)
            {
                S = r.x * Math.Cos(2 * Math.PI * j / Nphi) + r.y * Math.Sin(2 * Math.PI * j / Nphi);
                index = (int) (Math.Round(S * Nr)) + Nr;
                sum += Hgrad[j][index];
            }
            return sum;
        }

        public void CalculateHeterogenityIndicator()
        {
            for (int i = -Nx / 2; i < Nx / 2; i++)
            {
                List<double> tmp = new List<double>();
                for (int j = -Ny / 2; j < Ny / 2; j++)
                {
                    double x = i / (Nx / 2.0);
                    double y = j / (Ny / 2.0);
                    Pair r = new Pair(x, y);
                    if ((x * x) + (y * y) < 1)
                    {
                        tmp.Add(Ind(r));
                    }
                }
                IndicatorValues.Add(tmp);
            }
        }
    }
}
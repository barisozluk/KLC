using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AYP.Calculations
{
    public class Calculation
    {
        public double slice;

        /// <summary>
        /// Gerilim Düşümü Hesabı
        /// 
        /// L = Hat uzunluğu (m)
        /// to connector inputpower = Node'un girdi güç değeri (w) cinsinden
        /// kablotipi = Bakır ise K=56 aliminyum ise K=35
        /// Kablokesit= iletken kesiti (mm^2)
        /// gerilim = from connectorun gerilim değeri. 3 faz tek faz için gerilim (220/380) 
        /// 
        /// </summary>
        /// <returns></returns>
        public double VoltageDropCalculation(double length, double InputPower, int kablotipi, double gerilim)// double KabloKesit, int gerilim)
        {
            double VoltageDrop = 0;
            slice = CableSuggestionCalculation(InputPower, gerilim);
            int K = 0;
            if (kablotipi == 0)
                K = 56;
            else
                K = 35;
            
            VoltageDrop = (100 * length * InputPower) / (K * slice * Math.Pow(gerilim, 2));
            //VoltageDropYuzde = (VoltageDrop * 100) / gerilim;

            return Math.Round(VoltageDrop, 3);

        }

        public double VoltageDropCalculationMaxCheck(double length, double InputPower, int kablotipi, double gerilim, double maksVoltageDrop)// double KabloKesit, int gerilim)
        {
            //double Slice = 0;
            //double slice = CableSuggestionCalculation(InputPower, Voltage);
            int K = 0;
            if (kablotipi == 0)
                K = 56;
            else
                K = 35;

            slice = (100 * length * InputPower) / (K * maksVoltageDrop * Math.Pow(gerilim, 2));

            if (slice > 0 && slice < 0.35)
                slice = 0.35;
            else if (slice > 0.35 && slice < 0.5)
                slice = 0.5;
            else if (slice > 0.5 && slice < 0.75)
                slice = 0.75;
            else if (slice > 0.75 && slice < 1)
                slice = 1;
            else if (slice > 1 && slice < 1.5)
                slice = 1.5;
            else if (slice > 1.5 && slice < 2.5)
                slice = 2.5;
            else if (slice > 2.5 && slice < 4)
                slice = 4;
            else if (slice > 4 && slice < 6)
                slice = 6;
            else if (slice > 6 && slice < 10)
                slice = 10;
            else if (slice > 10 && slice < 16)
                slice = 16;
            else if (slice > 16 && slice < 25)
                slice = 25;
            else if (slice > 25 && slice < 35)
                slice = 35;
            else if (slice > 35 && slice < 50)
                slice = 50;
            else if (slice > 50 && slice < 70)
                slice = 70;
            else if (slice > 70 && slice < 95)
                slice = 95;
            else if (slice > 95 && slice < 120)
                slice = 120;
            else if (slice > 120 && slice < 150)
                slice = 150;
            else if (slice > 150 && slice < 185)
                slice = 185;
            else if (slice > 185 && slice < 240)
                slice = 240;
            else if (slice > 240 && slice < 300)
                slice = 300;

            return Math.Round(slice, 3);
        }


        /// <summary>
        /// Isı Kaybı hesabı
        /// </summary>
        /// <param name="VoltageDrop"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public double HeatLossCalculation(double InputPower, double Voltage, int kablotipi, double length)
        {
            double Current = CurrentCalculation(InputPower, Voltage);
            double r = 0;
            //slice = CableSuggestionCalculation(InputPower, Voltage);
            double HeatLoss = 0;
            if (slice == 0.35)
                r = 50.3;
            else if (slice == 0.5)
                r = 37.6;
            else if (slice == 0.75)
                r = 24.8;
            else if (slice == 1)
                r = 18.2;
            else if (slice == 1.5)
                r = 11.9;
            else if (slice == 2.5)
                r = 7.14;
            else if (slice == 4)
                r = 4.5;
            else if (slice == 6)
                r = 3.71;
            else if (slice == 10)
                r = 2.24;
            else if (slice == 16)
                r = 1.41;
            else if (slice == 25)
                r = 0.9;
            else if (slice == 35)
                r = 0.64;
            else if (slice == 50)
                r = 0.47;
            else if (slice == 70)
                r = 0.32;
            else if (slice == 95)
                r = 0.23;
            else if (slice == 120)
                r = 0.18;
            else if (slice == 150)
                r = 0.15;
            else if (slice == 185)
                r = 0.12;
            else if (slice == 240)
                r = 0.09;
            else if (slice == 300)
                r = 0.07;

            if (kablotipi == 0)
                HeatLoss = (2 * Math.Pow(Current, 2) * r * length) / 1000;
            else
                HeatLoss = (2 * Math.Pow(Current, 2) * r * 1.64 * length) / 1000;

            return HeatLoss;
        }

       public double CurrentCalculation(double InputPower, double Voltage)
        {
            double Current = 0;
            Current = InputPower / (1.732 * Voltage * 0.8);

            return Current;
        }

        /// <summary>
        /// Sadece XLPE izolasyonlu kablo için kesit önerilir. 
        /// </summary>
        /// <param name="InputPower"> Watt cinsinden to connector input power </param>
        /// <param name="VoltageDrop"></param>
        /// <param name="Voltage">V cinsinden</param>
        /// Akım A cinsinden
        /// cos phi sabit 0.8 alındı
        /// <returns></returns>
        public double CableSuggestionCalculation(double InputPower, double Voltage)
        {
            slice = 0;
            double Current = CurrentCalculation(InputPower, Voltage);
                   
            if (Current > 0 && Current < 6)
                slice = 0.35;
            else if (Current > 6 && Current < 10)
                slice = 0.5;
            else if (Current > 10 && Current < 15)
                slice = 0.75;
            else if (Current > 15 && Current < 20)
                slice = 1;
            else if (Current > 20 && Current < 25)
                slice = 1.5;
            else if (Current > 25 && Current < 35)
                slice = 2.5;
            else if (Current > 35 && Current < 45)
                slice = 4;
            else if (Current > 45 && Current < 60)
                slice = 6;
            else if (Current > 60 && Current < 80)
                slice = 10;
            else if (Current > 80 && Current < 125)
                slice = 16;
            else if (Current > 125 && Current < 145)
                slice = 25;
            else if (Current > 145 && Current < 174)
                slice = 35;
            else if (Current > 174 && Current < 206)
                slice = 50;
            else if (Current > 206 && Current < 254)
                slice = 70;
            else if (Current > 254 && Current < 305)
                slice = 95;
            else if (Current > 305 && Current < 365)
                slice = 120;
            else if (Current > 365 && Current < 415)
                slice = 150;
            else if (Current > 415 && Current < 475)
                slice = 185;
            else if (Current > 475 && Current < 560)
                slice = 240;
            else
                slice = 300;

            return slice;
        }
        /// <summary>
        /// Besleme Süresi hesabı
        /// </summary>
        /// <param name="powers"></param>
        /// <param name="Voltage"></param>
        /// <param name="batterycapacity">Ah cinsinden</param>
        /// Akım A cinsinden
        /// <returns></returns>
        public double FeedingTimeCalculation(double powers, double Voltage, double batterycapacity) //List<double> 
        {
            double current = 0;
            //double totalpowers = 0;
            //foreach (var power in powers)
            //{
            //    totalpowers += power;
            //}
            current = (powers / Voltage);

            return (batterycapacity / current * 0.707);
        }
    }
}

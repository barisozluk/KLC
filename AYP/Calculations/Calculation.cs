using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AYP.Calculations
{
    public class Calculation
    {
        /// <summary>
        /// Gerilim Düşümü Hesabı
        /// e = 2.P.L.k/χ.S.Un^2 -> Bir fazlı yüklerde
        /// e = P.L.k/χ.S.Un^2 -> Üç fazlı yüklerde
        /// P = Yükün aktif gücü (W)
        /// L = Hat uzunluğu (m)
        /// k = 1 + (x/r).tan(phi) -> 16mm^2 kesite kadar k 1 alınır. 16mm^2 den büyük için formül hesaplanır
        /// χ = Malzeme iletkenlik katsayısı (m/Ω.mm^2)
        /// S = kesit (mm^2)
        /// Un = Şebeke nominal gerilimi
        /// </summary>
        /// <returns></returns>
        public double VoltageDropCalculation(double InputPower, int EfficiencyRatio = 0, double InternalPowerConsumption = 0)
        {
            double VoltageDrop = 0;
            if (EfficiencyRatio != 0)
                VoltageDrop = InputPower - ((InputPower * EfficiencyRatio) / 100);
            else if (InternalPowerConsumption != 0)
                VoltageDrop = InternalPowerConsumption;
            else VoltageDrop = 0;

            return VoltageDrop;
        }
        /// <summary>
        /// Isı Kaybı hesabı
        /// </summary>
        /// <param name="VoltageDrop"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public double HeatLossCalculation(double VoltageDrop, double length)
        {
            double HeatLoss = (2 * VoltageDrop * length) / 1000;

            return HeatLoss;
        }

        /// <summary>
        /// Sadce XLPE izolasyonlu kablo için kesit önerilir. 
        /// </summary>
        /// <param name="InputPower"></param>
        /// <param name="VoltageDrop"></param>
        /// <param name="Voltage"></param>
        /// <returns></returns>
        public double CableSuggestionCalculation(double InputPower, double VoltageDrop, double Voltage)
        {
            double Current = 0;
            int slice = 0;

            double OutputPower = InputPower - VoltageDrop;

            Current = OutputPower / (1.732 * Voltage * 0.8);
            if (Current > 0 && Current < 145)
                slice = 25;
            else if (Current > 145 && Current < 174)
                slice = 35;
            else if (Current > 174 && Current < 206)
                slice = 50;
            else if (Current > 206 && Current < 254)
                slice = 70;
            else if (Current > 254 && Current < 305)
                slice = 35;
            else
                slice = 0;

            return slice;
        }
        /// <summary>
        /// Besleme Süresi hesabı
        /// </summary>
        /// <param name="powers"></param>
        /// <param name="Voltage"></param>
        /// <param name="batterycapacity"></param>
        /// <returns></returns>
        public double FeedingTimeCalculation(List<double> powers, double Voltage, double batterycapacity)
        {
            double current = 0;
            double totalpowers = 0;
            foreach (var power in powers)
            {
                totalpowers += power;
            }
            current = totalpowers / Voltage;

            return batterycapacity / current;
        }
    }
}

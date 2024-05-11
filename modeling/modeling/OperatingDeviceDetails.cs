using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modeling
{

    public class OperatingDeviceDetails
    {
        public UInt32 AM { get; }
        public UInt32 BM { get; }
        public UInt32 DM { get; }
        public UInt32 C { get; }
        public bool[] Y { get; }
        public bool[] D { get; }
        public bool[] Q { get; }
        public bool[] XM { get; }
        public bool[] X { get; }
        public bool[] a { get; }
        public Byte count { get; }
        public Boolean overflow { get; }
        public bool[] T { get; }

        public OperatingDeviceDetails(UInt32 AM,
            UInt32 BM,
            UInt32 DM,
            UInt32 C,
            bool[] Y,
            bool[] D,
            bool[] T,
            bool[] Q,
            bool[] XM,
            bool[] X,
            bool[] a,
            Byte count,
            Boolean overflow)
        {
            this.AM = AM;
            this.BM = BM;
            this.DM = DM;
            this.C = C;
            this.Y = Y;
            this.D = D;
            this.T = T;
            this.Q = Q;
            this.XM = XM;
            this.X = X;
            this.a = a;
            this.count = count;
            this.overflow = overflow;
        }

        public override string ToString()
        {
            return $"AM = {toBinaryString(AM, 32)}, {calcDecimal(AM, 31,31)}\n" +
                $"BM = {toBinaryString(BM, 32)}, {calcDecimal(BM,31,31)}\n" +
                $"DM = {toBinaryString(DM, 32)}, {calcDecimal(DM, 31,31)}\n" +
                $"C = {toBinaryString(C,17)}, {calcDecimal(C,16,16)}\n" +
                $"Y  = {toBinaryString(Y)}\n" +
                $"D = {toBinaryString(D)}\n" +
                $"Q = {toBinaryString(Q)}\n" +
                $"XM = {toBinaryString(XM)}\n" +
                $"X = {toBinaryString(X)}\n" +
                $"a = {asNumber(a)}\n" +
                $"count = {toBinaryString(count, 4)}\n" +
                $"overflow = {overflow}";
        }

        public static string asNumber(bool[] num)
        {
            for(int i = 0; i < num.Length; i++)
            {
                if (num[i])
                    return "" + i;
            }
            return "0";
        }

        public static string toBinaryString(bool[] num)
        {
            StringBuilder numStr = new StringBuilder();
            for(int i = 0; i < num.Length; i++)
            {
                numStr.Append(num[i] ? "1" : "0");
            }
            return reverse(numStr.ToString());
        }

        public static string toBinaryString(UInt32 num, int digitCount)
        {
            StringBuilder numStr = new StringBuilder();
            while (num != 0)
            {
                if ((num & 0b1) == 0)
                    numStr.Append("0");
                else
                    numStr.Append("1");
                num = num >> 1;
                digitCount--;
            }
            for(int i = 0; i < digitCount; i++)
            {
                numStr.Append("0");
            }
            return reverse(numStr.ToString());
        }

        public static string toBinaryString(Byte num, int digitCount)
        {
            StringBuilder numStr = new StringBuilder();
            while (num != 0)
            {
                if ((num & 0b1) == 0)
                    numStr.Append("0");
                else
                    numStr.Append("1");
                num = (byte)(num >> 1);
                digitCount--;
            }
            for (int i = 0; i < digitCount; i++)
            {
                numStr.Append("0");
            }
            return reverse(numStr.ToString());
        }
 
        private static string reverse(string str)
        {
            string rev = "";
            for(int i = str.Length - 1; i >= 0; i--)
            {
                rev += str[i];
            }
            return rev;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">Число которое нужно преобразовать к десятичному виду</param>
        /// <param name="signBit">Номер знакового бита</param>
        /// <param name="posBit">Номер бита после которого идёт позиционная точка</param>
        /// <returns>Строковое представление числа с позиционной точкой</returns>
        public static string calcDecimal(UInt32 num, int signBit, int posBit)
        {
            double value = Math.Abs(num);
            value /= Math.Pow(2, posBit);
            if ((num & (1 << signBit)) != 0)
            { 
                value *= -1;
                value += 1;
            }
            return Math.Round(value, 5).ToString("0.#####"); // Возвращаем строковое представление числа в десятичной системе
        }

    }

}

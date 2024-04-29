using System;
using modeling;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OperatingDevice od = new OperatingDevice();
            UInt32 A = 0xC0_00;
            UInt32 B = 0xC0_00;
            UInt32 C = 0x01_80_00;
            Console.WriteLine(OperatingDeviceDetails.calcDecimal(C,16,16));
            od.setA(A);
            od.setB(B);
            od.setRun(true);
            /*Console.WriteLine($"A_10 = {OperatingDeviceDetails.calcDecimal(A, 15)},A_2 = {OperatingDeviceDetails.toBinaryString(A,16)}");
            Console.WriteLine($"B_10 = {OperatingDeviceDetails.calcDecimal(B, 15)},A_2 = {OperatingDeviceDetails.toBinaryString(B, 16)}");*/
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine($"\nтакт {i}");
                Console.WriteLine(od.tact());
            }
        }
    }
}

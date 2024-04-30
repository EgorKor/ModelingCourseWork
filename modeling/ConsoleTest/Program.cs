using System;
using modeling;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OperatingDevice device = new OperatingDevice();
            UInt16 A = 0x0800;
            UInt16 B = 0x4000;
            try
            {
                Console.WriteLine(OperatingDeviceDetails.calcDecimal(device.runProgram(A, B), 16, 16));
            }
            catch (ExecutionOverflowException)
            {
                Console.WriteLine("Overflow exception");
            }
            /*device.setA(A);
            device.setB(B);
            device.setRun(true);

            Console.WriteLine($"{OperatingDeviceDetails.toBinaryString(A, 16)} {OperatingDeviceDetails.calcDecimal(A, 15, 15)}");
            Console.WriteLine($"{OperatingDeviceDetails.toBinaryString(B, 16)} {OperatingDeviceDetails.calcDecimal(B, 15, 15)}");

            for (int i = 0; i < 55; i++)
            {
                Console.WriteLine($"\nтакт - {i}");
                Console.WriteLine(device.tact());
            }*/



            /*UInt32 num = 0x80_00_00_00;
            Console.WriteLine(OperatingDeviceDetails.toBinaryString(num, 32));
            Console.WriteLine(OperatingDeviceDetails.toBinaryString(num >> 31, 32));*/
        }
    }
}

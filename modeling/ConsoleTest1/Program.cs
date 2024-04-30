using modeling;

OperatingDevice device = new OperatingDevice();
UInt32 A = 0x3FFF;
UInt32 B = 0xFFFF;
device.setA(A);
device.setB(B);
device.setRun(true);

Console.WriteLine($"{OperatingDeviceDetails.toBinaryString(A,16)} {OperatingDeviceDetails.calcDecimal(A,15,15)}");
Console.WriteLine($"{OperatingDeviceDetails.toBinaryString(B,16)} {OperatingDeviceDetails.calcDecimal(B,15,15)}");

for (int i = 0; i < 25; i++)
{
    Console.WriteLine($"\nтакт - {i}");
    Console.WriteLine(device.tact());
}
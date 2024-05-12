using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modeling
{
    public class OperatingMachine
    {


        public bool[] x { get; set; }
        public List<ConditionX> conditions { get; set; }
        public UInt32 AM { get; set; }
        public UInt32 BM { get; set; }
        public UInt32 DM { get; set; }
        public UInt32 A { get; set; }
        public UInt32 B { get; set; }
        public UInt32 C { get; set; }
        public Boolean overflow { get; set; }
        public Byte count { get; set; }
        public Boolean run { get; set; }


        public OperatingMachine()
        {
            x = new bool[7];
            conditions = new List<ConditionX>()
            {
                () => false,                           //X0  
                () => BM == 0,                         //X1
                () => AM == 0,                         //X2
                () => (AM & 0x80000000) == 0x80000000, //X3
                () => count == 0,                      //X4
                () => (C & 0b1) == 1,                  //X5
                () => (((A >> 15) & 0b1) == 1 && ((B >> 15) & 0b1) == 0) 
                || (((A >> 15) & 0b1 ) == 0 && (((B >> 15) & 0b1) == 1))   //X6
            };
        }

        public bool[] generateX()
        {
            for (int i = 0; i < conditions.Count; i++)
                x[i] = conditions[i]();
            return x;
        }

        public void doOperations(bool[] y)
        {
            UInt32 AM = this.AM;
            UInt32 BM = this.BM;
            UInt32 DM = this.DM;
            UInt32 C = this.C;
            UInt32 D = this.DM;
            Byte count = this.count;

            if (y[0])
                y0(AM);
            if (y[1])
                y1(AM);
            if (y[2])
                y2(BM);
            if (y[3])
                y3(BM);
            if (y[4])
                y4();
            if (y[5])
                y5();
            if (y[6])
                y6();
            if (y[7])
                y7(AM, BM);
            if (y[8])
                y8(AM, BM);
            if (y[9])
                y9(AM);
            if (y[10])
                y10(BM);
            if (y[11])
                y11();
            if (y[12])
                y12(C);
            if (y[13])
                y13(C);
            if (y[14])
                y14(DM);
            if (y[15])
                y15(count);
            if (y[16])
                y16(C);
            if (y[17])
                y17(C);
        }

        public void y0(UInt32 AM)
        {
            this.AM = (AM & 0x0000FFFF) | ((A & 0x7FFF) << 15);
        }
        public void y1(UInt32 AM)
        {
            this.AM = this.AM & 0xFFFF8000;
        }
        public void y2(UInt32 BM)
        {
            this.BM = (BM & 0x0000FFFF) | (((UInt32)B & 0x7FFF) << 15);
        }
        public void y3(UInt32 BM)
        {
            this.BM = this.BM & 0xFFFF8000;
        }
        public void y4()
        {
            overflow = false;
        }
        public void y5()
        {
            C = 0;
        }
        public void y6()
        {
            overflow = true;
        }
        public void y7(UInt32 AM, UInt32 BM)
        {
            this.AM = AM + (0xC0_00_00_00 | ((~BM) & 0x3F_FF_FF_FF) + 1);
        }
        public void y8(UInt32 AM, UInt32 BM)
        {

            this.AM = AM + (BM & 0x3FFFFFFF);
        }
        public void y9(UInt32 AM)
        {
            DM = AM;
        }
        public void y10(UInt32 BM)
        {
            this.BM = BM >> 1;
        }
        public void y11()
        {
            count = 0;
        }

        public void y12(UInt32 C)
        {
            this.C = (C << 1) | 0b1;
        }

        public void y13(UInt32 C)
        {
            this.C = C << 1;
        }

        public void y14(UInt32 DM)
        {
            AM = DM;
        }
        public void y15(byte count)
        {
            this.count = (byte)(this.count == 0 ? 0xF : count - 1);
        }
        public void y16(UInt32 C)
        {
            this.C = C + 2;
        }

        public void y17(UInt32 C)
        {
            this.C = C | 0x10000;
        }

    }
}

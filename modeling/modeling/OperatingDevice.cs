using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace modeling
{
    public class OperatingDevice
    {
        private UInt32 DM { get; set; }//регистр DM
        private UInt32 AM { get; set; }//регистр AM
        private UInt32 BM { get; set; }//регистр BM
        private UInt16 XM { get; set; }//регистр памяти ЛУ(логических условий)
        private UInt32 C { get; set; } //частное
        private UInt16 a { get; }//регистр памяти кодов состояний
        private UInt16 q;
        private UInt16 D { get; }//вектор сигналов D - управление регистрами кодов состояний
        private UInt32 Y { get;  }//вектор сигналов Y - управление операционным автоматом
        private UInt16 X { get; set; }//вектор сигналов X - логические условия
        private UInt16 A { get; set; } //делимое
        private UInt16 B { get; set; } //делитель

        private Byte count;//счётчик
        private bool run { get; set; }
        private bool overflow { get; set; }
        
        private OperatingMachine operatingMachine;//операционный автомат

        private List<Term> termList;              //список терм

        private List<YFunction> YfunctionsList;  //список функций КС - Y  
        private List<DFunction> DFunctionsList;  //список функций КС - D

        private bool x0;
        private bool x1;
        
        

        public OperatingDevice()
        {
            operatingMachine = new OperatingMachine(this);
            
            termList = new List<Term>()
            {
                () => a == 0 && (X & 0b1) == 0,                                          //t0
                () => a == 0 && (X & 0b1) == 1,                                           //t1
                () => a == 1 && (X & 0b10 >> 1) == 1,                                           //t2
                () => a == 1 && (X & 0b10 >> 1) == 1 && (X & 0b100 >> 2) == 1,  //t3
                () => a == 1 && (X & 0b10 >> 1) == 1 && (X & 0b100 >> 2) == 1,  //t4
                () => a == 2 && (X & 0b1000 >> 3) == 0,                                          //t5
                () => a == 2 && (X & 0b1000 >> 3) == 1,                                           //t6
                () => a == 3,                                                                                    //t7
                () => a == 4 && (X & 0b1000 >> 3) == 0,                                          //t8
                () => a == 4 && (X & 0b1000 >> 3) == 1,                                           //t9
                () => a == 5,                                                                                    //t10
                () => a == 6 && (X & 0b10000 >> 4) == 0 && (X & 0b1000 >> 3) == 1, //t11
                () => a == 6 && (X & 0b10000 >> 4) == 0 && (X & 0b1000 >> 3) == 0,  //t12
                () => a == 6 && (X & 0b10000 >> 4) == 1 && (X & 0b100000 >> 5) == 1,  //t13
                () => a == 6 && (X & 0b10000 >> 4) == 1 && (X & 0b100000 >> 5) == 0, //t14
                () => a == 7 && (X & 0b1000000 >> 6) == 1,                                       //t15
                () => a == 7 && (X & 0b1000000 >> 6) == 0                                           //t16
            };
            
            YfunctionsList = new List<YFunction>()
            {
                () => termList[1](),                                                      //y0
                () => termList[1](),                                                      //y1
                () => termList[1](),                                                      //y2
                () => termList[1](),                                                      //y3
                () => termList[1](),                                                      //y4
                () => termList[3]() || termList[7](),                                     //y5
                () => termList[2](),                                                      //y6
                () => termList[4]() || termList[11]() || termList[12](),                  //y7
                () => termList[6](),                                                      //y8
                () => termList[7]() || termList[10]() || termList[11]() || termList[12](),//y9
                () => termList[7]() || termList[10]() || termList[11]() || termList[12](),//y10
                () => termList[7]() ,                                                     //y11
                () => termList[8]() || termList[12]() ,                                   //y12
                () => termList[9]() || termList[11]() ,                                   //y13
                () => termList[9]() || termList[11](),                                    //y14
                () => termList[10]() || termList[11]() || termList[12](),                 //y15
                () => termList[13](),                                                     //y16
                () => termList[15]()                                                      //y17
            };
            DFunctionsList = new List<DFunction>()
            {
                () => termList[1]() || termList[6]() || termList[9]() || termList[13]() || termList[14](), //D0
                () => termList[4]() || termList[6]() || termList[10]() 
                || termList[11]() || termList[12]() || termList[13]() || termList[14](),                   //D1
                () => termList[7]() || termList[8]() || termList[9]()  
                || termList[10]() || termList[11]() || termList[12]() || termList[13]() || termList[14]()  //D2
            };

        }

        public void tact()
        {
            
        }

        public class OperatingMachine
        {
            private List<ConditionX> conditionList;

            private List<OperationY> operationList;
            
            private OperatingDevice od;

            public List<ConditionX> GetConditions() { return conditionList; }

            public OperatingMachine(OperatingDevice od)
            {
                this.od = od;
                conditionList = new List<ConditionX>()
            {
                () => od.run,                                                                             //X0  
                () => od.BM == 0,                                                                         //X1
                () => od.AM == 0,                                                                         //X2
                () => (od.AM & 0x100000000) == 1,                                                         //X3
                () => od.count == 0,                                                                      //X4
                () => (od.C & 0b1) == 1,                                                                  //X5
                () => ((od.A & 0b1) == 1 && (od.B & 0b1) == 0) || ((od.A & 0b1) == 0 && (od.B & 0b1) == 1)//X6
            };
                operationList = new List<OperationY>()
                {
                    (AM, BM, C,DM, COUNT_COPY) => {od.AM = (AM & 0x0000FFFF) | ((UInt32)od.A & 0xEFFF) << 16; },   //y0
                    (AM, BM, C,DM, COUNT_COPY) => {od.AM  = AM & 0xFFFF1000; },                                    //y1
                    (AM, BM, C,DM, COUNT_COPY) => {od.BM = (od.BM & 0x0000FFFF) | ((UInt32)od.B & 0xEFFF) << 16; },//y2
                    (AM, BM, C,DM, COUNT_COPY) => {od.BM  = BM & 0xFFFF1000; },                                    //y3
                    (AM, BM, C,DM, COUNT_COPY) => {od.overflow = false; },                                         //y4
                    (AM, BM, C,DM, COUNT_COPY) => {od.C = 0; },                                                    //y5
                    (AM, BM, C,DM, COUNT_COPY) => {od.overflow = true;},                                           //y6
                    (AM, BM, C,DM, COUNT_COPY) => {od.AM = AM + 0xC0000000 | ((~BM + 1) & 0x3FFFFFFF) ; },          //y7
                    (AM, BM, C,DM, COUNT_COPY) => {od.AM = AM + (BM & 0x3FFFFFFF); },                              //y8
                    (AM, BM, C,DM, COUNT_COPY) => {od.DM = AM; },                                                  //y9
                    (AM, BM, C,DM, COUNT_COPY) => {od.BM = BM >> 1;},                                              //y10
                    (AM, BM, C,DM, COUNT_COPY) => {od.count = 0; },                                                //y11
                    (AM, BM, C,DM, COUNT_COPY) => {od.C = (C << 1) | 0b1; },                                       //y12
                    (AM, BM, C,DM, COUNT_COPY) => {od.C = C << 1; },                                               //y13
                    (AM, BM, C,DM, COUNT_COPY) => {od.AM = DM; },                                                  //y14
                    (AM, BM, C,DM, COUNT_COPY) => {od.count = (byte)(od.count == 0 ? 0xF:COUNT_COPY - 1) ; },      //y15
                    (AM, BM, C,DM, COUNT_COPY) => {od.C = C & 0xFFFE0000 | (C & 0x1FFFE + 2) & 0x1FFFE; },         //y16
                    (AM, BM, C,DM, COUNT_COPY) => {od.C = C | 0x10000; }                                           //y17
                };
            }

            public UInt16 calculateConditions()
            {
                UInt16 conditions = 0;
                for(int i = 0; i < conditionList.Count; i++)
                {
                    if (conditionList[i]())
                        conditions += (UInt16) Math.Pow(2, i);
                }
                return conditions;
            }

            public void doOperations()
            {
                UInt32 AM_COPY  = od.AM;
                UInt32 BM_COPY  = od.B;
                UInt32 C_COPY   = od.C;
                UInt32 DM_COPY  = od.DM;
                Byte COUNT_COPY = od.count;
                for (int i = 0; i < operationList.Count; i++)
                {
                    if ((od.Y & (UInt32)Math.Pow(2,i)) == 1)
                        operationList[i](AM_COPY, BM_COPY, C_COPY, DM_COPY, COUNT_COPY);
                }
            }

        }

        public delegate bool ConditionX();
        public delegate void OperationY(UInt32 AM, UInt32 BM, UInt32 C, UInt32 DM, Byte COUNT_COPY);
        public delegate bool Term();
        public delegate bool YFunction();
        public delegate bool DFunction();
    }
}

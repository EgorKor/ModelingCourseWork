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
        private OperatingMachine operatingMachine;
        private ControlMachine controlMachine;
        private bool run;


        public OperatingDevice()
        {
            operatingMachine = new OperatingMachine();
            controlMachine = new ControlMachine(operatingMachine);
        }
        
        public void setA(UInt32 A)
        {
            operatingMachine.A = A;
        }

        public void setB(UInt32 B)
        {
            operatingMachine.B = B;
        }

        public void setRun(bool run)
        {
            this.run = run;
        }

        public void reset()
        {
            operatingMachine = new OperatingMachine();
            controlMachine = new ControlMachine(operatingMachine);
        }

        public UInt32 runProgram(UInt16 A, UInt16 B)
        {
            reset();
            setA(A);
            setB(B);
            setRun(true);
            odTact();
            OperatingDeviceDetails tactResult;
            do
            {
                tactResult = odTact();
            } while (!controlMachine.a[0]);
            if (operatingMachine.overflow)
                throw new ExecutionOverflowException();
            return tactResult.C;
        }
        
        public OperatingDeviceDetails odTact()
        {
            doModelingPLU();
            doModelingPS();
            doModelingDSH();
            doModelingKS_T();
            doModelingKS_Y();
            doModelingOA();
            doModelingKS_D();
            OperatingMachine om = operatingMachine;
            ControlMachine cm = controlMachine;
            return new OperatingDeviceDetails(om.AM, om.BM, om.DM, om.C, cm.Y, cm.D, cm.T, cm.Q, cm.xm, om.x, cm.a, om.count,om.overflow);
        }

        public OperatingDeviceDetails mpTact()
        {
            OperatingMachine om = operatingMachine;
            ControlMachine cm = controlMachine;
            doModelingMP(cm.a, om.conditions, om, cm);
            return new OperatingDeviceDetails(om.AM, om.BM, om.DM, om.C, cm.Y, cm.D, cm.T, cm.Q, cm.xm, om.x, cm.a, om.count, om.overflow);
        }

        private void doModelingMP(bool[] a, List<ConditionX> x, OperatingMachine om, ControlMachine cm)
        {
            if (a[0])
            {
                if (run)
                {
                    om.y0(om.AM);
                    om.y1(om.AM);
                    om.y2(om.BM);
                    om.y3(om.BM);
                    om.y4();
                    cm.a[1] = true;
                    cm.a[0] = false;
                    run = false;
                }
            }else if (a[1])
            {
                if (x[1]())
                {
                    om.y6();
                    cm.a[0] = true;

                }else if (!x[1]() && x[2]())
                {
                    om.y5();
                    cm.a[0] = true;
                }else if (!x[1]() && !x[2]())
                {
                    om.y7(om.AM, om.BM);
                    cm.a[2] = true;
                }
                cm.a[1] = false;
            }else if (a[2])
            {
                if (!x[3]())
                {
                    om.y6();
                    cm.a[0] = true;
                }
                else
                {
                    om.y8(om.AM, om.BM);
                    cm.a[3] = true;
                }
                cm.a[2] = false;
            }
            else if (a[3])
            {
                om.y5();
                om.y9(om.AM);
                om.y10(om.BM);
                om.y11();
                cm.a[3] = false;
                cm.a[4] = true;
            }
            else if (a[4])
            {
                om.y7(om.AM, om.BM);
                cm.a[4] = false;
                cm.a[5] = true;
            }
            else if (a[5])
            {
                if (x[3]())
                {
                    om.y13(om.C);
                    om.y14(om.DM);
                }
                else
                {
                    om.y12(om.C);
                }
                cm.a[5] = false;
                cm.a[6] = true;
            }
            else if (a[6])
            {
                om.y9(om.AM);
                om.y10(om.BM);
                om.y15(om.count);
                cm.a[6] = false;
                cm.a[7] = true;
            }
            else if (a[7])
            {
                if (!x[4]())
                {
                    om.y7(om.AM, om.BM);
                    cm.a[5] = true;
                }
                else
                {
                    if (x[5]())
                    {
                        om.y16(om.C);
                        cm.a[8] = true;
                    }
                    else if (x[6]())
                    {
                        om.y17(om.C);
                        cm.a[0] = true;
                    }
                    else
                    {
                        cm.a[0] = true;
                    }
                }
                cm.a[7] = false;
            }
            else if (a[8])
            {
                if (x[6]())
                {
                    om.y17(om.C);
                }
                a[8] = false;
                a[0] = true;
            }
        }

        private void doModelingPLU()
        {
            controlMachine.xm[0] = operatingMachine.x[2];
            controlMachine.xm[1] = operatingMachine.x[3];

        }

        private void doModelingPS()
        {
            controlMachine.Q = controlMachine.D;
        }


        private void doModelingDSH()
        {
            byte a = 0;
            for(int i = 0; i < controlMachine.Q.Length; i++)
            {
                if (controlMachine.Q[i])
                    a += (byte)Math.Pow(2, i);
            }
            controlMachine.a = new bool[9];
            controlMachine.a[a] = true;
        }

        private void doModelingKS_T()
        {
            bool[] x = new bool[8];
            x[0] = run;
            x[1] = operatingMachine.x[1];
            x[2] = controlMachine.xm[0];
            x[3] = controlMachine.xm[1];
            x[4] = operatingMachine.x[4];
            x[5] = operatingMachine.x[5];
            x[6] = operatingMachine.x[6];
            controlMachine.generateT(x);
            run = false;
        }

        private void doModelingKS_Y()
        {
            controlMachine.generateY();
        }

        private void doModelingOA()
        {
            operatingMachine.doOperations(controlMachine.Y);
            operatingMachine.generateX();
        }

        private void doModelingKS_D()
        {
            controlMachine.generateD();
        }

    }
}

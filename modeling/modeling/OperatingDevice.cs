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
            tact();
            OperatingDeviceDetails tactResult;
            do
            {
                tactResult = tact();
            } while (!controlMachine.a[0]);
            if (operatingMachine.overflow)
                throw new ExecutionOverflowException();
            return tactResult.C;
        }
        
        public OperatingDeviceDetails tact()
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

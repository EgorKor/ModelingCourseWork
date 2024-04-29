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


        
        public OperatingDeviceDetails tact()
        {
            doModelingPLU();
            doModelingPS();
            doModelingDSH();
            doModelingKS_Y();
            doModelingOA();
            doModelingKS_D();
            OperatingMachine om = operatingMachine;
            ControlMachine cm = controlMachine;
            return new OperatingDeviceDetails(om.AM, om.BM, om.DM, om.C, cm.Y, cm.D, cm.Q, cm.x, om.x, cm.a, om.count,om.overflow);
        }

        public void doModelingPLU()
        {
            controlMachine.x = operatingMachine.x;
            controlMachine.x[0] = run;
            run = false;
        }

        public void doModelingPS()
        {
            controlMachine.Q = controlMachine.D;
        }

        public void doModelingDSH()
        {
            byte a = 0;
            for(int i = 0; i < controlMachine.Q.Length; i++)
            {
                if (controlMachine.Q[i])
                    a += (byte)Math.Pow(2, i);
            }
            controlMachine.a = a;
        }

        public void doModelingKS_Y()
        {
            controlMachine.generateY();
        }

        public void doModelingOA()
        {
            operatingMachine.doOperations(controlMachine.Y);
            operatingMachine.generateX();
        }

        public void doModelingKS_D()
        {
            controlMachine.generateD();
        }

    }
}

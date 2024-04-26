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
        private OperatingMachine operatingMachine ;
        private ControlMachine controlMachine;


        public OperatingDevice()
        {
            operatingMachine = new OperatingMachine();
            controlMachine = new ControlMachine();
        }

        public void tact()
        {

        }

        




    }
}

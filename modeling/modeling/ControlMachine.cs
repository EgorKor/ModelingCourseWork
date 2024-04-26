using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modeling
{
    class ControlMachine
    {
        public delegate bool Term();
        public delegate bool YFunction();
        public delegate bool DFunction();

        private List<YFunction> yFunctions;
        private List<DFunction> dFunctions;


        private bool[] Y { get; set; }
        private bool[] D { get; set; }
        private bool[] Q { get; set; }
        private Byte a;
        
        public ControlMachine()
        {
            Y = new bool[18];
            yFunctions = new List<YFunction>()
            {
                //TODO
            };
            dFunctions = new List<DFunction>()
            {
                //TODO
            };
        }


        public bool[] generateY()
        {
            bool[] D = new bool[dFunctions.Count];
            for (int i = 0; i < dFunctions.Count; i++)
                D[i] = dFunctions[i]();
            return D;
        }

        public bool[] generateD()
        {
            bool[] Y = new bool[yFunctions.Count];
            for (int i = 0; i < yFunctions.Count; i++)
                Y[i] = yFunctions[i]();
            return Y;
        }

        

    }
}

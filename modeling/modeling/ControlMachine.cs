using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modeling
{
    /// <summary>
    /// Класс реализующий поведение УА
    /// </summary>
    class ControlMachine
    {
        public delegate bool Term();
        public delegate bool YFunction();
        public delegate bool DFunction();

        /// <summary>
        /// Словапь функций КС Y
        /// </summary>
        public Dictionary<String, YFunction> yFunctions;
        /// <summary>
        /// Словарь функций КС D
        /// </summary>
        public Dictionary<String, DFunction> dFunctions;
        /// <summary>
        /// Словарь термов
        /// </summary>
        public Dictionary<String, Term> terms;

        /// <summary>
        /// Выходы КС Y
        /// </summary>
        public bool[] Y { get; set; }
        /// <summary>
        /// Выходы КС D
        /// </summary>
        public bool[] D { get; set; }
        /// <summary>
        /// выходы ПС
        /// </summary>
        public bool[] Q { get; set; }
        /// <summary>
        /// выходы ПЛУ
        /// </summary>
        public bool[] x { get; set; }
        /// <summary>
        /// Выход ДШ
        /// </summary>
        public bool[] a { get; set; }
        private OperatingMachine operatingMachine;
        
        public ControlMachine(OperatingMachine om)
        {
            Y = new bool[18];
            x = new bool[7];
            Q = new bool[3];
            D = new bool[3];
            a = new bool[9];
            
            terms = new Dictionary<string, Term>()
            {
                {"t0",() => a[0] && !x[0]},
                {"t1",() => a[1] && x[1]},
                {"t2",() => a[1] && !x[1] && x[2]},
                {"t3",() => a[2] && !x[3]},
                {"t4",() => a[7] && x[4] && !x[5] && !x[6]},
                {"t5",() => a[8] && !x[5] && x[6]},
                {"t6",() => a[8] && x[6]},
                {"t7",() => a[8] && x[6]},
                {"t8",() => a[0] && x[0]},
                {"t9",() => a[1] && !x[1] && !x[2]},
                {"t10",() => a[2] && x[3]},
                {"t11",() => a[3]},
                {"t12",() => a[4]},
                {"t13",() => a[7] && !x[4]},
                {"t14",() => a[5] && !x[3]},
                {"t15",() => a[5] && x[3]},
                {"t16",() => a[6]},
                {"t17",() => a[7] && x[4] && x[5]}
            };
            Dictionary<string, Term> t = terms;
            dFunctions = new Dictionary<String, DFunction>()
            {
                {"d0" ,() => t["t8"]() || t["t10"]() || t["t12"]() || t["t13"]() || t["t16"]()},
                {"d1" ,() => t["t9"]() || t["t10"]() || t["t11"]() || t["t14"]() || t["t15"]() || t["t16"]()},
                {"d2" ,() => t["t11"]() || t["t12"]() || t["t13"]() || t["t14"]() || t["t15"]() || t["t16"]()},
                {"d3", () => t["t17"]() }
            };
            yFunctions = new Dictionary<String, YFunction>()
            {
                {"y0" ,() => t["t8"]() },
                {"y1" ,() => t["t8"]() },
                {"y2" ,() => t["t8"]() },
                {"y3" ,() => t["t8"]() },
                {"y4" ,() => t["t8"]() },
                {"y5" ,() => t["t2"]() || t["t11"]()},
                {"y6" ,() => t["t1"]() || t["t3"]() },
                {"y7" ,() => t["t9"]() || t["t12"]() || t["t13"]()},
                {"y8" ,() => t["t10"]() },
                {"y9" ,() => t["t11"]() || t["t16"]() },
                {"y10" ,() => t["t11"]() || t["t16"]() },
                {"y11" ,() => t["t11"]()},
                {"y12" ,() => t["t14"]()},
                {"y13" ,() => t["t15"]()},
                {"y14" ,() => t["t15"]()},
                {"y15" ,() => t["t16"]()},
                {"y16" ,() => t["t17"]()},
                {"y17" ,() => t["t5"]() || t["t7"]()},
            };
            
            
        }

        /// <summary>
        /// Генерирует новые значения выходов КС Y
        /// </summary>
        /// <returns></returns>
        public bool[] generateY()
        {
            Y = new bool[yFunctions.Count];
            int i = 0;
            foreach (KeyValuePair<String, YFunction> yFunc in yFunctions)
            {
                Y[i] = yFunc.Value();
                i++;
            }
            return Y;
        }
        /// <summary>
        /// Генерирует новые значения выходов КС D
        /// </summary>
        /// <returns></returns>
        public bool[] generateD()
        {
            D = new bool[dFunctions.Count];
            int i = 0;
            foreach (KeyValuePair<String, DFunction> dFunc in dFunctions)
            {
                D[i] = dFunc.Value();
                i++;
            }
            return D;
        }

       



    }
}

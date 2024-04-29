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
        public Byte a { get; set; }
        private OperatingMachine operatingMachine;
        
        public ControlMachine(OperatingMachine om)
        {
            Y = new bool[18];
            x = new bool[7];
            Q = new bool[3];
            D = new bool[3];
            
            terms = new Dictionary<string, Term>()
            {
                {"t0",() => a == 0 && !x[0]},
                {"t1",() => a == 1 && x[1]},
                {"t2",() => a == 1 && !x[1] && x[2]},
                {"t3",() => a == 2 && !x[3]},
                {"t4",() => a == 7 && x[6]},
                {"t5",() => a == 7 && !x[6]},
                {"t6",() => a == 0 && x[0]},
                {"t7",() => a == 1 && !x[1] && !x[2]},
                {"t8",() => a == 2 && x[3]},
                {"t9",() => a == 3},
                {"t10",() => a == 4 && !x[3]},
                {"t11",() => a == 4 && x[3]},
                {"t12",() => a == 5},
                {"t13",() => a == 6 && !x[4] && x[3]},
                {"t14",() => a == 6 && !x[4] && !x[3]},
                {"t15",() => a == 6 && x[4] && x[5]},
                {"t16",() => a == 6 && x[4] && !x[5]},
            };
            Dictionary<string, Term> t = terms;
            dFunctions = new Dictionary<String, DFunction>()
            {
                {"d0" ,() => t["t6"]() || t["t8"]() || t["t10"]() || t["t11"]() || t["t15"]() || t["t16"]()},
                {"d1" ,() => t["t7"]() || t["t8"]() || t["t12"]() || t["t13"]() || t["t14"]() || t["t15"]() || t["t16"]()},
                {"d2" ,() => t["t9"]() || t["t10"]() || t["t11"]() || t["t12"]() || t["t13"]() || t["t14"]() || t["t15"]() || t["t16"]() }
            };
            yFunctions = new Dictionary<String, YFunction>()
            {
                {"y0" ,() => t["t6"]() },
                {"y1" ,() => t["t6"]() },
                {"y2" ,() => t["t6"]() },
                {"y3" ,() => t["t6"]() },
                {"y4" ,() => t["t6"]() },
                {"y5" ,() => t["t9"]() || t["t2"]()},
                {"y6" ,() => t["t3"]() },
                {"y7" ,() => t["t7"]() || t["t13"]() || t["t14"]() || t["t10"]() || t["t11"]()},
                {"y8" ,() => t["t8"]() },
                {"y9" ,() => t["t9"]() || t["t12"]() || t["t13"]() || t["t14"]()},
                {"y10" ,() => t["t9"]() || t["t12"]() || t["t13"]() || t["t14"]()},
                {"y11" ,() => t["t9"]()},
                {"y12" ,() => t["t10"]() || t["t14"]()},
                {"y13" ,() => t["t11"]() || t["t13"]()},
                {"y14" ,() => t["t11"]() || t["t13"]()},
                {"y15" ,() => t["t12"]() || t["t13"]() || t["t14"]() },
                {"y16" ,() => t["t15"]()},
                {"y17" ,() => t["t4"]()},
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

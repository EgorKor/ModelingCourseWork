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
        public delegate bool Term(bool[] xm);
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
        public bool[] xm { get; set; }
        /// <summary>
        /// Выход ДШ
        /// </summary>
        public bool[] a { get; set; }
        /// <summary>
        /// Выходы КС T
        /// </summary>
        public bool[] T { get; set; }

        private OperatingMachine operatingMachine;
        
        public ControlMachine(OperatingMachine om)
        {
            Y = new bool[18];
            xm = new bool[2];
            Q = new bool[4];
            D = new bool[4];
            a = new bool[9];
            T = new bool[18];
            
            terms = new Dictionary<string, Term>()
            {
                {"t0",(bool[] x) => a[0] && !x[0]},
                {"t1",(bool[] x) => a[1] && x[1]},
                {"t2",(bool[] x) => a[1] && !x[1] && x[2]},
                {"t3",(bool[] x) => a[2] && !x[3]},
                {"t4",(bool[] x) => a[7] && x[4] && !x[5] && !x[6]},
                {"t5",(bool[] x) => a[7] && x[4] && !x[5] && x[6]},
                {"t6",(bool[] x) => a[8] && x[6]},
                {"t7",(bool[] x) => a[8] && x[6]},
                {"t8",(bool[] x) => a[0] && x[0]},
                {"t9",(bool[] x) => a[1] && !x[1] && !x[2]},
                {"t10",(bool[] x) => a[2] && x[3]},
                {"t11",(bool[] x) => a[3]},
                {"t12",(bool[] x) => a[4]},
                {"t13",(bool[] x) => a[7] && !x[4]},
                {"t14",(bool[] x) => a[5] && !x[3]},
                {"t15",(bool[] x) => a[5] && x[3]},
                {"t16",(bool[] x) => a[6]},
                {"t17",(bool[] x) => a[7] && x[4] && x[5]}
            };
            dFunctions = new Dictionary<String, DFunction>()
            {
                {"d0" ,() => T[8] || T[10] || T[12] || T[13] || T[16]},
                {"d1" ,() => T[9] || T[10] || T[14] || T[15] || T[16]},
                {"d2" ,() => T[11] || T[12] || T[13] || T[14] || T[15] || T[16]},
                {"d3", () => T[17] }
            };
            yFunctions = new Dictionary<String, YFunction>()
            {
                {"y0" ,() => T[8] },
                {"y1" ,() => T[8] },
                {"y2" ,() => T[8] },
                {"y3" ,() => T[8] },
                {"y4" ,() => T[8] },
                {"y5" ,() => T[2] || T[11]},
                {"y6" ,() => T[1] || T[3] },
                {"y7" ,() => T[9] || T[12] || T[13]},
                {"y8" ,() => T[10] },
                {"y9" ,() => T[11] || T[16] },
                {"y10" ,() => T[11] || T[16] },
                {"y11" ,() => T[11]},
                {"y12" ,() => T[14]},
                {"y13" ,() => T[15]},
                {"y14" ,() => T[15]},
                {"y15" ,() => T[16]},
                {"y16" ,() => T[17]},
                {"y17" ,() => T[5] || T[7]},
            };
            
            
        }

        /// <summary>
        /// Генерирует новые значения выходов КС Y
        /// </summary>
        /// <returns>Новые значения КС Y</returns>
        public bool[] generateY()
        {
            Y = new bool[Y.Length];
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
        /// <returns>Новые значения выходов D</returns>
        public bool[] generateD()
        {
            D = new bool[D.Length];
            int i = 0;
            foreach (KeyValuePair<String, DFunction> dFunc in dFunctions)
            {
                D[i] = dFunc.Value();
                i++;
            }
            return D;
        }
        /// <summary>
        /// Генерирует новые значения выходов KC T
        /// </summary>
        /// <param name="x">входы КС Т</param>
        /// <returns>Новые значения выходов КС Т</returns>
        public bool[] generateT(bool[] x)
        {
            T = new bool[T.Length];
            int i = 0;
            foreach (KeyValuePair<String, Term> term in terms)
            {
                T[i] = term.Value(x); 
                i++;
            }
            return D;
        }
    }
}

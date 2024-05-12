using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace modeling
{
    public partial class Form1 : Form
    {
        private CheckBox[] box_Q;
        private CheckBox[] box_A;
        private CheckBox[] box_D;
        private CheckBox[] box_XM;
        private CheckBox[] box_X;
        private CheckBox[] box_Y;
        private CheckBox[] box_A_out;
        private CheckBox[] box_T;
        private OperatingDevice operatingDevice;
        private bool isFirstTact = true;

        public Form1()
        {
            InitializeComponent();
            tactButton.Enabled = false;
            aDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            bDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            cDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            amDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            bmDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            cntDataGrid.Rows.Add(0,0,0,0);
            box_Q = new CheckBox[]
            {
                Q0,Q1,Q2,Q3
            };
            box_A = new CheckBox[]
            {
                a0,a1,a2,a3,a4,a5,a6,a7,a8
            };
            box_D = new CheckBox[]
            {
                D0,D1,D2,D3
            };
            box_XM = new CheckBox[]
            {
                xm2,xm3
            };
            box_X = new CheckBox[]
            {
                x0,x1,x2,x3,x4,x5,x6
            };
            box_Y = new CheckBox[]
            {
                y0,y1,y2,y3,y4,y5,y6,y7,y8,y9,y10,y11,y12,y13,y14,y15,y16,y17
            };
            box_A_out = new CheckBox[]
            {
                ga0,ga1,ga2,ga3,ga4,ga5,ga6,ga7,ga8,ga0_0
            };
            box_T = new CheckBox[]
            {
                t0,t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14,t15,t16,t17
            };
            unableBoxes(box_Q);
            unableBoxes(box_A);
            unableBoxes(box_D);
            unableBoxes(box_XM);
            unableBoxes(box_X);
            unableBoxes(box_Y);
            unableBoxes(box_A_out);
            unableBoxes(box_T);
            operatingDevice = new OperatingDevice();
        }

        private void unableBoxes(CheckBox[] boxes)
        {
            for (int i = 0; i < boxes.Length; i++)
                boxes[i].Enabled = false;
        }

        

        private void runButton_Click(object sender, EventArgs e)
        {
            try
            { 
                resultTextBox.Text = $"{OperatingDeviceDetails.calcDecimal(operatingDevice.runProgram(readRegister(aDataGrid), readRegister(bDataGrid)),16,16)}";
            }
            catch (ExecutionOverflowException)
            {
                resultTextBox.Text = "Переполнение";
            }
            runButton.Enabled = false;
        }

        private void tactButton_Click(object sender, EventArgs e)
        {
            if (OUtactRadioButton.Checked)
            {
                ouTact();
            }
            else if (mpTactRadioButton.Checked)
            {
                mpTact();
            }
        }

        private void mpTact()
        {
            OperatingDeviceDetails tactResult;
            if (isFirstTact)
            {
                operatingDevice.setRun(true);
                operatingDevice.setA(readRegister(aDataGrid));
                operatingDevice.setB(readRegister(bDataGrid));
                isFirstTact = false;
                tactResult = operatingDevice.mpTact();
                fillGSA(tactResult.a);
                fillRegisters(tactResult.AM, tactResult.BM, tactResult.DM, tactResult.C, tactResult.count);
                return;
            }
            tactResult = operatingDevice.mpTact();
            fillRegisters(tactResult.AM, tactResult.BM, tactResult.DM, tactResult.C, tactResult.count);
            fillGSA(tactResult.a);
            if (tactResult.a[0])
            {
                clearGSA();
                box_A_out[9].Checked = true;
                if (tactResult.overflow)
                {
                    resultTextBox.Text = "Переполнение";
                }
                else
                {
                    resultTextBox.Text = $"{OperatingDeviceDetails.calcDecimal(tactResult.C, 16, 16)}";
                }
                tactButton.Enabled = false;
            }
        }

        private void ouTact()
        {
            OperatingDeviceDetails tactResult;
            if (isFirstTact)
            {
                operatingDevice.setRun(true);
                operatingDevice.setA(readRegister(aDataGrid));
                operatingDevice.setB(readRegister(bDataGrid));
                isFirstTact = false;
                tactResult = operatingDevice.odTact();
                fillRegisters(tactResult.AM, tactResult.BM, tactResult.DM, tactResult.C, tactResult.count);
                fillScheme(tactResult.Q, tactResult.a, tactResult.D, tactResult.Y, tactResult.XM, tactResult.X, tactResult.T);
                box_X[0].Checked = true;
                return;
            }
            tactResult = operatingDevice.odTact();
            fillRegisters(tactResult.AM, tactResult.BM, tactResult.DM, tactResult.C, tactResult.count);
            fillScheme(tactResult.Q, tactResult.a, tactResult.D, tactResult.Y, tactResult.XM, tactResult.X, tactResult.T);
            fillGSA(tactResult.a);
            if (tactResult.a[0])
            {
                clearGSA();
                box_A_out[9].Checked = true;
                if (tactResult.overflow)
                {
                    resultTextBox.Text = "Переполнение";
                }
                else
                {
                    resultTextBox.Text = $"{OperatingDeviceDetails.calcDecimal(tactResult.C, 16, 16)}";
                }
                tactButton.Enabled = false;
            }
        }

        private UInt16 readRegister(DataGridView register)
        {
            UInt16 value = 0;
            for(int i = 0; i < 16; i++)
            {
                if ((int)register[i, 0].Value == 1)
                    value += (UInt16)Math.Pow(2, 15 - i);
            }
            string value_bin = OperatingDeviceDetails.toBinaryString(value, 16);
            return value;
        }


        private void fillRegisters(UInt32 AM, UInt32 BM, UInt32 DM, UInt32 C, Byte count)
        {
            fillRegister(amDataGrid, OperatingDeviceDetails.toBinaryString(AM,32));
            fillRegister(bmDataGrid, OperatingDeviceDetails.toBinaryString(BM, 32));
            fillRegister(dDataGrid, OperatingDeviceDetails.toBinaryString(DM, 32));
            fillRegister(cDataGrid, OperatingDeviceDetails.toBinaryString(C, 17));
            fillRegister(cntDataGrid, OperatingDeviceDetails.toBinaryString(count, 4));
        }

        private void fillRegister(DataGridView register, string value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                register[i, 0].Value = int.Parse("" + value[i]);
            }

        }

        private void fillScheme(bool[] Q, bool[] a, bool[] D, bool[] Y, bool[] XM, bool[] X, bool[] T)
        {
            fillCheckBoxes(box_Q, Q);
            fillCheckBoxes(box_A, a);
            fillCheckBoxes(box_D, D);
            fillCheckBoxes(box_Y, Y);
            fillCheckBoxes(box_XM, XM);
            fillCheckBoxes(box_X, X);
            fillCheckBoxes(box_T, T);
        }

        private void fillCheckBoxes(CheckBox[] boxes, bool[] values)
        {
            for(int i = 0; i < boxes.Length; i++)
            {
                boxes[i].Checked = values[i];
            }
        }

        private void fillGSA(bool[] a)
        {
            for (int i = 0; i < a.Length; i++)
                box_A_out[i].Checked = a[i];
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void reset()
        {
            if (OUtactRadioButton.Checked || mpTactRadioButton.Checked)
                tactButton.Enabled = true;
            else
                runButton.Enabled = true;
            aDataGrid.Rows.Clear();
            bDataGrid.Rows.Clear();
            cDataGrid.Rows.Clear();
            amDataGrid.Rows.Clear();
            bmDataGrid.Rows.Clear();
            dDataGrid.Rows.Clear();
            cntDataGrid.Rows.Clear();
            operatingDevice.reset();
            isFirstTact = true;
            resultTextBox.Text = "";
            aDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            bDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            cDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            amDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            bmDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dDataGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            cntDataGrid.Rows.Add(0, 0, 0, 0);
            clearScheme();
            aDecimalTextBox.Text = "0";
            bDecimalTextBox.Text = "0";
            clearGSA();
            box_A_out[0].Checked = true;
        }

        private void clearGSA()
        {
            for(int i = 0; i < box_A_out.Length; i++)
            {
                box_A_out[i].Checked = false;
            }
        }

        private void clearScheme()
        {
            clearCheckBoxes(box_A);
            clearCheckBoxes(box_D);
            clearCheckBoxes(box_Q);
            clearCheckBoxes(box_X);
            clearCheckBoxes(box_XM);
            clearCheckBoxes(box_Y);
        }

        private void clearCheckBoxes(CheckBox[] boxes)
        {
            for(int i = 0; i < boxes.Length; i++)
            {
                boxes[i].Checked = false;
            }
        }

        private void autoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            reset();
            if (autoRadioButton.Checked)
                tactButton.Enabled = false;
            else
                tactButton.Enabled = true;
        }

        private void tactRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            reset();
            if (OUtactRadioButton.Checked || mpTactRadioButton.Checked)
                runButton.Enabled = false;
            else
                runButton.Enabled = true;
        }

        private void bDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bDataGrid.Rows[0].Cells[e.ColumnIndex].Value = (int)bDataGrid.Rows[0].Cells[e.ColumnIndex].Value == 0 ? 1 : 0;
            bDecimalTextBox.Text = OperatingDeviceDetails.calcDecimal(readRegister(bDataGrid), 15,15);

        }

        private void aDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            aDataGrid.Rows[0].Cells[e.ColumnIndex].Value = (int)aDataGrid.Rows[0].Cells[e.ColumnIndex].Value == 0 ? 1 : 0;
            aDecimalTextBox.Text = OperatingDeviceDetails.calcDecimal(readRegister(aDataGrid), 15, 15);
        }

        private void mpTactRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            reset();
            if (OUtactRadioButton.Checked || mpTactRadioButton.Checked)
                runButton.Enabled = false;
            else
                runButton.Enabled = true;
        }
    }
}

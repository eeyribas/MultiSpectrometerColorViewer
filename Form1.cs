using MultiSpectrometerColorViewer.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MultiSpectrometerColorViewer
{
    public partial class Form1 : Form
    {
        public Thread thread1;
        public Thread thread2;
        public Thread thread3;
        public Thread thread4;
        public Thread thread5;
        public Thread thread6;
        public Thread thread7;
        public Thread thread8;
        public bool threadState1 = false;
        public bool threadState2 = false;
        public bool threadState3 = false;
        public bool threadState4 = false;
        public bool threadState5 = false;
        public bool threadState6 = false;
        public bool threadState7 = false;
        public bool threadState8 = false;

        public double[] standartPlate1 = new double[Parameters.dataCount];
        public double[] standartPlate2 = new double[Parameters.dataCount];
        public double[] ERsLed1 = new double[Parameters.dataCount];
        public double[] ERsLed2 = new double[Parameters.dataCount];
        public double[] ERsLed3 = new double[Parameters.dataCount];

        public int[] nmCount1 = new int[Parameters.dataCount];
        public int[] nmCount2 = new int[Parameters.dataCount];
        public int[] nmCount3 = new int[Parameters.dataCount];

        public double deltaEResult1 = 0;
        public double deltaEResult2 = 0;
        public double deltaEResult3 = 0;

        public double deltaECMC11Result1 = 0;
        public double deltaECMC11Result2 = 0;
        public double deltaECMC11Result3 = 0;
        public double deltaECMC21Result1 = 0;
        public double deltaECMC21Result2 = 0;
        public double deltaECMC21Result3 = 0;
        public double deltaEH1 = 0;
        public double deltaEH2 = 0;
        public double deltaEH3 = 0;

        public double deltaE12 = 0;
        public double deltaE13 = 0;
        public double deltaE23 = 0;

        public List<double> LStandartSampleList1 = new List<double>();
        public List<double> aStandartSampleList1 = new List<double>();
        public List<double> bStandartSampleList1 = new List<double>();
        public List<double> LStandartSampleList2 = new List<double>();
        public List<double> aStandartSampleList2 = new List<double>();
        public List<double> bStandartSampleList2 = new List<double>();
        public List<double> LStandartSampleList3 = new List<double>();
        public List<double> aStandartSampleList3 = new List<double>();
        public List<double> bStandartSampleList3 = new List<double>();
        public double lStandartSample1 = 0;
        public double aStandartSample1 = 0;
        public double bStandartSample1 = 0;
        public double lStandartSample2 = 0;
        public double aStandartSample2 = 0;
        public double bStandartSample2 = 0;
        public double lStandartSample3 = 0;
        public double aStandartSample3 = 0;
        public double bStandartSample3 = 0;

        public double cSample1 = 0;
        public double hSample1 = 0;
        public double SLSample1 = 0;
        public double SCSample1 = 0;
        public double SHSample1 = 0;
        public double cTest1 = 0;
        public double hTest1 = 0;
        public double cSample2 = 0;
        public double hSample2 = 0;
        public double SLSample2 = 0;
        public double SCSample2 = 0;
        public double SHSample2 = 0;
        public double cTest2 = 0;
        public double hTest2 = 0;
        public double cSample3 = 0;
        public double hSample3 = 0;
        public double SLSample3 = 0;
        public double SCSample3 = 0;
        public double SHSample3 = 0;
        public double cTest3 = 0;
        public double hTest3 = 0;

        public List<double> deltaEList1 = new List<double>();
        public List<double> deltaEList2 = new List<double>();
        public List<double> deltaEList3 = new List<double>();
        public List<double> deltaEList12 = new List<double>();
        public List<double> deltaEList13 = new List<double>();
        public List<double> deltaEList23 = new List<double>();

        public int graphicsMaximumDelta = 0;
        public int firstTime = 0;
        public int deltaEValue = 1;

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GroupBoxEnableFalse();
            CreateStorage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadState1 = false;
            threadState2 = false;
            threadState3 = false;
            threadState4 = false;
            threadState5 = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Default";
            label1.ForeColor = Color.Brown;
            this.Refresh();
            Thread.Sleep(200);

            string sendData = "*MEASure:DARKspectra " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";
            double[] pixelSectionDataValue1 = new double[Parameters.dataCount];
            double[] pixelSectionDataValue2 = new double[Parameters.dataCount];
            double[] pixelSectionDataValue3 = new double[Parameters.dataCount];

            if (serialPort1.IsOpen)
            {
                try
                {
                    if (Functions.DigitalGainSetting(serialPort1, Parameters.digitalGain) == 6 && Functions.AnalogGainSetting(serialPort1, Parameters.analogGain) == 6)
                        label1.Text = "True(1),";
                    else
                        label1.Text = "False(1),";

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort1, sendData, nmCount1, Parameters.filter,
                                                                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    pixelSectionDataValue1 = keyValuePair.Value;
                }
                catch (Exception)
                {
                    label1.Text = "False(1),";
                }
            }
            else
            {
                label1.Text = "Close(1),";
            }

            if (serialPort2.IsOpen)
            {
                try
                {
                    if (Functions.DigitalGainSetting(serialPort2, Parameters.digitalGain) == 6 && Functions.AnalogGainSetting(serialPort2, Parameters.analogGain) == 6)
                        label1.Text += "True(2),";
                    else
                        label1.Text += "False(2),";

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort2, sendData, nmCount2, Parameters.filter,
                                                                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    pixelSectionDataValue2 = keyValuePair.Value;
                }
                catch (Exception)
                {
                    label1.Text += "False(2),";
                }
            }
            else
            {
                label1.Text += "Close(2),";
            }

            if (serialPort3.IsOpen)
            {
                try
                {
                    if (Functions.DigitalGainSetting(serialPort3, Parameters.digitalGain) == 6 && Functions.AnalogGainSetting(serialPort3, Parameters.analogGain) == 6)
                        label1.Text += "True(3)";
                    else
                        label1.Text += "False(3)";

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort3, sendData, nmCount3, Parameters.filter,
                                                                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    pixelSectionDataValue3 = keyValuePair.Value;
                }
                catch (Exception)
                {
                    label1.Text += "False(3)";
                }
            }
            else
            {
                label1.Text += "Close(3)";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "Default";
            label3.ForeColor = Color.Brown;
            this.Refresh();
            Thread.Sleep(200);

            string sendData = "*MEASure:REFERence " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";
            double[] ERefLed1 = new double[Parameters.dataCount];

            if (serialPort1.IsOpen)
            {
                try
                {
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort1, sendData, nmCount1, Parameters.filter,
                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    ERefLed1 = keyValuePair.Value;
                    for (int i = 0; i < ERsLed1.Length; i++)
                    {
                        if (comboBox1.SelectedIndex == 0)
                            ERsLed1[i] = ERefLed1[i] / standartPlate1[i];
                        else if (comboBox1.SelectedIndex == 1)
                            ERsLed1[i] = ERefLed1[i] / standartPlate2[i];
                    }
                    label3.Text = "True(1)";
                }
                catch (Exception)
                {
                    label3.Text = "False(1)";
                }
            }
            else
            {
                label3.Text = "Close(1)";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Default";
            label3.ForeColor = Color.Brown;
            this.Refresh();
            Thread.Sleep(200);

            string sendData = "*MEASure:REFERence " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";
            double[] ERefLed2 = new double[Parameters.dataCount];

            if (serialPort2.IsOpen)
            {
                try
                {
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort2, sendData, nmCount2, Parameters.filter,
                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    ERefLed2 = keyValuePair.Value;
                    for (int i = 0; i < ERsLed2.Length; i++)
                    {
                        if (comboBox1.SelectedIndex == 0)
                            ERsLed2[i] = ERefLed2[i] / standartPlate1[i];
                        else if (comboBox1.SelectedIndex == 1)
                            ERsLed2[i] = ERefLed2[i] / standartPlate2[i];
                    }
                    label3.Text = "True(2),";
                }
                catch (Exception)
                {
                    label3.Text = "False(2),";
                }
            }
            else
            {
                label3.Text = "Close(2),";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = "Default";
            label3.ForeColor = Color.Brown;
            this.Refresh();
            Thread.Sleep(200);

            string sendData = "*MEASure:REFERence " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";
            double[] ERefLed3 = new double[Parameters.dataCount];

            if (serialPort3.IsOpen)
            {
                try
                {
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort3, sendData, nmCount3, Parameters.filter,
                        Convert.ToInt32(Parameters.otherLoopCount), Parameters.dataLenght, Parameters.dataCount);
                    ERefLed3 = keyValuePair.Value;
                    for (int i = 0; i < ERsLed3.Length; i++)
                    {
                        if (comboBox1.SelectedIndex == 0)
                            ERsLed3[i] = ERefLed3[i] / standartPlate1[i];
                        else if (comboBox1.SelectedIndex == 1)
                            ERsLed3[i] = ERefLed3[i] / standartPlate2[i];
                    }
                    label3.Text = "True(3)";
                }
                catch (Exception)
                {
                    label3.Text = "False(3)";
                }
            }
            else
            {
                label3.Text = "Close(3)";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";

            LStandartSampleList1.Clear();
            aStandartSampleList1.Clear();
            bStandartSampleList1.Clear();
            LStandartSampleList2.Clear();
            aStandartSampleList2.Clear();
            bStandartSampleList2.Clear();
            LStandartSampleList3.Clear();
            aStandartSampleList3.Clear();
            bStandartSampleList3.Clear();

            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = false;
            button8.Enabled = false;

            threadState6 = true;
            threadState7 = true;
            threadState8 = true;

            string sendData = "*MEASure:REFERence " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";

            if (serialPort1.IsOpen)
            {
                try
                {
                    label4.Text = "True(1),";
                    if (thread6 != null && thread6.IsAlive == true)
                        return;
                    thread6 = new Thread(() => Process1(serialPort1, sendData, nmCount1, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread6.Start();
                }
                catch (Exception)
                {
                    label4.Text = "False(1),";
                }
            }
            else
            {
                label4.Text += "Close(1),";
            }

            if (serialPort2.IsOpen)
            {
                try
                {
                    label4.Text += "True(2),";
                    if (thread7 != null && thread7.IsAlive == true)
                        return;
                    thread7 = new Thread(() => Process2(serialPort2, sendData, nmCount2, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread7.Start();
                }
                catch (Exception)
                {
                    label4.Text += "False(2),";
                }
            }
            else
            {
                label4.Text += "Close(2),";
            }

            if (serialPort3.IsOpen)
            {
                try
                {
                    label4.Text += "True(3)";
                    if (thread8 != null && thread8.IsAlive == true)
                        return;
                    thread8 = new Thread(() => Process3(serialPort3, sendData, nmCount3, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread8.Start();
                }
                catch (Exception)
                {
                    label4.Text += "False(3),";
                }
            }
            else
            {
                label4.Text += "Close(3)";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;

            threadState6 = false;
            threadState7 = false;
            threadState8 = false;
            label4.Text = "Stop";
            label4.ForeColor = Color.Red;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = true;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox22.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";

            label48.Text = "";
            label44.Text = "";
            label40.Text = "";
            label50.Text = "";
            label28.Text = "";
            label32.Text = "";
            label46.Text = "";
            label42.Text = "";
            label30.Text = "";
            label34.Text = "";
            label38.Text = "";
            label36.Text = "";

            chart3.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].Points.Clear();
            chart2.Series["Series1"].Points.Clear();
            chart6.Series["Series1"].Points.Clear();
            chart5.Series["Series1"].Points.Clear();
            chart4.Series["Series1"].Points.Clear();

            deltaEList1.Clear();
            deltaEList2.Clear();
            deltaEList3.Clear();
            deltaEList12.Clear();
            deltaEList13.Clear();
            deltaEList23.Clear();

            threadState1 = true;
            threadState2 = true;
            threadState3 = true;
            threadState4 = true;
            threadState5 = true;

            checkBox1.Checked = false;

            string sendData = "*MEASure:REFERence " + Parameters.integrationTime + " " + Parameters.averageScan + " format<CR>\r";
            firstTime = (DateTime.Now.Hour * 60 * 60) + (DateTime.Now.Minute * 60) + (DateTime.Now.Second);

            if (serialPort1.IsOpen)
            {
                try
                {
                    label11.Text = "True(1),";
                    if (thread1 != null && thread1.IsAlive == true)
                        return;
                    thread1 = new Thread(() => Process4(serialPort1, sendData, nmCount1, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread1.Start();
                }
                catch (Exception)
                {
                    label11.Text = "False(1),";
                }
            }
            else
            {
                label11.Text += "Close(1),";
            }

            if (serialPort2.IsOpen)
            {
                try
                {
                    label11.Text += "True(2),";
                    if (thread2 != null && thread2.IsAlive == true)
                        return;
                    thread2 = new Thread(() => Process5(serialPort2, sendData, nmCount2, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread2.Start();
                }
                catch (Exception)
                {
                    label11.Text += "False(2),";
                }
            }
            else
            {
                label11.Text += "Close(2),";
            }

            if (serialPort3.IsOpen)
            {
                try
                {
                    label11.Text += "True(3)";
                    if (thread3 != null && thread3.IsAlive == true)
                        return;
                    thread3 = new Thread(() => Process6(serialPort3, sendData, nmCount3, Parameters.filter, Convert.ToInt32(Parameters.testLoopCount), Parameters.dataLenght, Parameters.dataCount));
                    thread3.Start();
                }
                catch (Exception)
                {
                    label11.Text += "False(3),";
                }
            }
            else
            {
                label11.Text += "Close(3)";
            }

            if (thread4 != null && thread4.IsAlive == true)
                return;
            thread4 = new Thread(() => Process7());
            thread4.Start();

            if (thread5 != null && thread5.IsAlive == true)
                return;
            thread5 = new Thread(() => Process8());
            thread5.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = false;

            threadState1 = false;
            threadState2 = false;
            threadState3 = false;
            threadState4 = false;
            threadState5 = false;
            label11.Text = "Stop";
            label11.ForeColor = Color.Red;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            threadState1 = false;
            threadState2 = false;
            threadState3 = false;
            threadState4 = false;
            threadState5 = false;
            Functions.CloseSerialPort(serialPort1);
            Functions.CloseSerialPort(serialPort2);
            Functions.CloseSerialPort(serialPort3);

            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReadStandartPlate1();
            ReadStandartPlate2();
            ReadSettings();
            ReadParameterId();

            if (Parameters.selectDevicePort1 != Parameters.selectDevicePort2 && Parameters.selectDevicePort1 != Parameters.selectDevicePort3 &&
                Parameters.selectDevicePort2 != Parameters.selectDevicePort3)
            {
                if (Parameters.selectDevicePort1 == "A")
                {
                    serialPort1.Close();
                }
                else
                {
                    serialPort1.PortName = Parameters.selectDevicePort1;
                    Functions.OpenSerialPort(serialPort1, Parameters.selectDevicePort1, 3000000);
                }

                if (Parameters.selectDevicePort2 == "B")
                {
                    serialPort2.Close();
                }
                else
                {
                    serialPort2.PortName = Parameters.selectDevicePort2;
                    Functions.OpenSerialPort(serialPort2, Parameters.selectDevicePort2, 3000000);
                }

                if (Parameters.selectDevicePort3 == "C")
                {
                    serialPort3.Close();
                }
                else
                {
                    serialPort3.PortName = Parameters.selectDevicePort3;
                    Functions.OpenSerialPort(serialPort3, Parameters.selectDevicePort3, 3000000);
                }

                if (!serialPort1.IsOpen && !serialPort2.IsOpen && !serialPort3.IsOpen)
                {
                    GroupBoxEnableFalse();
                }
                else
                {
                    ReadNmIndisRelatedPixel();

                    checkBox1.Checked = false;
                    comboBox1.SelectedIndex = 0;
                    groupBox9.BringToFront();

                    groupBox1.Enabled = true;
                    groupBox3.Enabled = true;
                    groupBox10.Enabled = true;
                    groupBox9.Enabled = true;
                    groupBox11.Enabled = true;
                    groupBox7.Enabled = true;
                    groupBox5.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox8.Enabled = true;
                    groupBox4.Enabled = true;
                    groupBox6.Enabled = true;
                }
            }
            else
            {
                GroupBoxEnableFalse();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DisconnectionFunction();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DisconnectionFunction();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            DisconnectionFunction();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DisconnectionFunction();
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            DisconnectionFunction();
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "Wing Difference";
                groupBox11.BringToFront();
            }
            else
            {
                checkBox1.Text = "Color Filter";
                groupBox9.BringToFront();
            }
        }

        private void Process1(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState6 == true)
                {
                    double[] RRefSample = new double[dataCount];
                    double[] ERefSample = new double[dataCount];
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort1, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ERefSample = keyValuePair.Value;
                    for (int i = 0; i < RRefSample.Length; i++)
                        RRefSample[i] = ERefSample[i] / ERsLed1[i];

                    double XRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lRefSample = (116 * Math.Pow(YRefSample, 1.0 / 3.0)) - 16;
                    double aRefSample = 500 * (Math.Pow(XRefSample, 1.0 / 3.0) - Math.Pow(YRefSample, 1.0 / 3.0));
                    double bRefSample = 200 * (Math.Pow(YRefSample, 1.0 / 3.0) - Math.Pow(ZRefSample, 1.0 / 3.0));

                    LStandartSampleList1.Add(lRefSample);
                    aStandartSampleList1.Add(aRefSample);
                    bStandartSampleList1.Add(bRefSample);
                }
                else
                {
                    lStandartSample1 = LStandartSampleList1.Average();
                    aStandartSample1 = aStandartSampleList1.Average();
                    bStandartSample1 = bStandartSampleList1.Average();

                    cSample1 = Math.Pow(aStandartSample1, 2) + Math.Pow(bStandartSample1, 2);
                    cSample1 = Math.Sqrt(cSample1);

                    double bDeviceA1 = 0;
                    if (aStandartSample1 > 0 && bStandartSample1 > 0)
                    {
                        bDeviceA1 = Math.Abs(bStandartSample1) / Math.Abs(aStandartSample1);
                        hSample1 = Math.Atan(bDeviceA1) * (180 / Math.PI);
                    }
                    else if (aStandartSample1 < 0 && bStandartSample1 > 0)
                    {
                        bDeviceA1 = Math.Abs(bStandartSample1) / Math.Abs(aStandartSample1);
                        hSample1 = Math.Atan(bDeviceA1) * (180 / Math.PI);
                        hSample1 = hSample1 + 90;
                    }
                    else if (aStandartSample1 < 0 && bStandartSample1 < 0)
                    {
                        bDeviceA1 = Math.Abs(bStandartSample1) / Math.Abs(aStandartSample1);
                        hSample1 = Math.Atan(bDeviceA1) * (180 / Math.PI);
                        hSample1 = hSample1 + 180;
                    }
                    else if (aStandartSample1 > 0 && bStandartSample1 < 0)
                    {
                        bDeviceA1 = Math.Abs(bStandartSample1) / Math.Abs(aStandartSample1);
                        hSample1 = Math.Atan(bDeviceA1) * (180 / Math.PI);
                        hSample1 = hSample1 + 270;
                    }
                    else
                    {
                        hSample1 = 0;
                    }

                    if (lStandartSample1 < 16)
                        SLSample1 = 0.511;
                    else
                        SLSample1 = (0.04097 * lStandartSample1) / (1 + (0.0165 * lStandartSample1));

                    SCSample1 = ((0.0638 * cSample1) / (1 + (0.0131 * cSample1))) + 0.638;
                    double T1 = 0;
                    double F1 = 0;
                    if (hSample1 > 164 && hSample1 < 345)
                        T1 = 0.56 + Math.Abs((0.2 * (Math.Cos(hSample1 + 168))));
                    else
                        T1 = 0.38 + Math.Abs((0.4 * (Math.Cos(hSample1 + 35))));
                    F1 = Math.Pow(cSample1, 4) / (Math.Pow(cSample1, 4) + 1900);
                    F1 = Math.Sqrt(F1);
                    SHSample1 = SCSample1 * ((T1 * F1) + 1 - F1);

                    SetTextBox(textBox4, Math.Round(lStandartSample1, 4).ToString());
                    SetTextBox(textBox5, Math.Round(aStandartSample1, 4).ToString());
                    SetTextBox(textBox6, Math.Round(bStandartSample1, 4).ToString());

                    break;
                }
            }
        }

        private void Process2(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState7 == true)
                {
                    double[] RRefSample = new double[dataCount];
                    double[] ERefSample = new double[dataCount];
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort2, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ERefSample = keyValuePair.Value;
                    for (int i = 0; i < RRefSample.Length; i++)
                        RRefSample[i] = ERefSample[i] / ERsLed2[i];

                    double XRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lRefSample = (116 * Math.Pow(YRefSample, 1.0 / 3.0)) - 16;
                    double aRefSample = 500 * (Math.Pow(XRefSample, 1.0 / 3.0) - Math.Pow(YRefSample, 1.0 / 3.0));
                    double bRefSample = 200 * (Math.Pow(YRefSample, 1.0 / 3.0) - Math.Pow(ZRefSample, 1.0 / 3.0));

                    LStandartSampleList2.Add(lRefSample);
                    aStandartSampleList2.Add(aRefSample);
                    bStandartSampleList2.Add(bRefSample);
                }
                else
                {
                    lStandartSample2 = LStandartSampleList2.Average();
                    aStandartSample2 = aStandartSampleList2.Average();
                    bStandartSample2 = bStandartSampleList2.Average();

                    cSample2 = Math.Pow(aStandartSample2, 2) + Math.Pow(bStandartSample2, 2);
                    cSample2 = Math.Sqrt(cSample2);

                    double bDeviceA2 = 0;
                    if (aStandartSample2 > 0 && bStandartSample2 > 0)
                    {
                        bDeviceA2 = Math.Abs(bStandartSample2) / Math.Abs(aStandartSample2);
                        hSample2 = Math.Atan(bDeviceA2) * (180 / Math.PI);
                    }
                    else if (aStandartSample2 < 0 && bStandartSample2 > 0)
                    {
                        bDeviceA2 = Math.Abs(bStandartSample2) / Math.Abs(aStandartSample2);
                        hSample2 = Math.Atan(bDeviceA2) * (180 / Math.PI);
                        hSample2 = hSample2 + 90;
                    }
                    else if (aStandartSample2 < 0 && bStandartSample2 < 0)
                    {
                        bDeviceA2 = Math.Abs(bStandartSample2) / Math.Abs(aStandartSample2);
                        hSample2 = Math.Atan(bDeviceA2) * (180 / Math.PI);
                        hSample2 = hSample2 + 180;
                    }
                    else if (aStandartSample2 > 0 && bStandartSample2 < 0)
                    {
                        bDeviceA2 = Math.Abs(bStandartSample2) / Math.Abs(aStandartSample2);
                        hSample2 = Math.Atan(bDeviceA2) * (180 / Math.PI);
                        hSample2 = hSample2 + 270;
                    }
                    else
                    {
                        hSample2 = 0;
                    }

                    if (lStandartSample2 < 16)
                        SLSample2 = 0.511;
                    else
                        SLSample2 = (0.04097 * lStandartSample2) / (1 + (0.0165 * lStandartSample2));

                    SCSample2 = ((0.0638 * cSample2) / (1 + (0.0131 * cSample2))) + 0.638;
                    double T2 = 0;
                    double F2 = 0;
                    if (hSample2 > 164 && hSample2 < 345)
                        T2 = 0.56 + Math.Abs((0.2 * (Math.Cos(hSample2 + 168))));
                    else
                        T2 = 0.38 + Math.Abs((0.4 * (Math.Cos(hSample2 + 35))));
                    F2 = Math.Pow(cSample2, 4) / (Math.Pow(cSample2, 4) + 1900);
                    F2 = Math.Sqrt(F2);
                    SHSample2 = SCSample2 * ((T2 * F2) + 1 - F2);

                    SetTextBox(textBox7, Math.Round(lStandartSample2, 4).ToString());
                    SetTextBox(textBox8, Math.Round(aStandartSample2, 4).ToString());
                    SetTextBox(textBox9, Math.Round(bStandartSample2, 4).ToString());

                    break;
                }
            }
        }

        private void Process3(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState8 == true)
                {
                    double[] RRefSample = new double[dataCount];
                    double[] ERefSample = new double[dataCount];
                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort3, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ERefSample = keyValuePair.Value;
                    for (int i = 0; i < RRefSample.Length; i++)
                        RRefSample[i] = ERefSample[i] / ERsLed3[i];

                    double XRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZRefSample = Functions.XYZCalculation(RRefSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lRefSample = (116 * Math.Pow(YRefSample, 1.0 / 3.0)) - 16;
                    double aRefSample = 500 * (Math.Pow(XRefSample, 1.0 / 3.0) - Math.Pow(YRefSample, 1.0 / 3.0));
                    double bRefSample = 200 * (Math.Pow(YRefSample, 1.0 / 3.0) - Math.Pow(ZRefSample, 1.0 / 3.0));

                    LStandartSampleList3.Add(lRefSample);
                    aStandartSampleList3.Add(aRefSample);
                    bStandartSampleList3.Add(bRefSample);
                }
                else
                {
                    lStandartSample3 = LStandartSampleList3.Average();
                    aStandartSample3 = aStandartSampleList3.Average();
                    bStandartSample3 = bStandartSampleList3.Average();

                    cSample3 = Math.Pow(aStandartSample3, 2) + Math.Pow(bStandartSample3, 2);
                    cSample3 = Math.Sqrt(cSample3);

                    double bDeviceA3 = 0;
                    if (aStandartSample3 > 0 && bStandartSample3 > 0)
                    {
                        bDeviceA3 = Math.Abs(bStandartSample3) / Math.Abs(aStandartSample3);
                        hSample3 = Math.Atan(bDeviceA3) * (180 / Math.PI);
                    }
                    else if (aStandartSample3 < 0 && bStandartSample3 > 0)
                    {
                        bDeviceA3 = Math.Abs(bStandartSample3) / Math.Abs(aStandartSample3);
                        hSample3 = Math.Atan(bDeviceA3) * (180 / Math.PI);
                        hSample3 = hSample3 + 90;
                    }
                    else if (aStandartSample3 < 0 && bStandartSample3 < 0)
                    {
                        bDeviceA3 = Math.Abs(bStandartSample3) / Math.Abs(aStandartSample3);
                        hSample3 = Math.Atan(bDeviceA3) * (180 / Math.PI);
                        hSample3 = hSample3 + 180;
                    }
                    else if (aStandartSample3 > 0 && bStandartSample3 < 0)
                    {
                        bDeviceA3 = Math.Abs(bStandartSample3) / Math.Abs(aStandartSample3);
                        hSample3 = Math.Atan(bDeviceA3) * (180 / Math.PI);
                        hSample3 = hSample3 + 270;
                    }
                    else
                    {
                        hSample3 = 0;
                    }

                    if (lStandartSample3 < 16)
                        SLSample3 = 0.511;
                    else
                        SLSample3 = (0.04097 * lStandartSample3) / (1 + (0.0165 * lStandartSample3));

                    SCSample3 = ((0.0638 * cSample3) / (1 + (0.0131 * cSample3))) + 0.638;
                    double T3 = 0;
                    double F3 = 0;
                    if (hSample3 > 164 && hSample3 < 345)
                        T3 = 0.56 + Math.Abs((0.2 * (Math.Cos(hSample3 + 168))));
                    else
                        T3 = 0.38 + Math.Abs((0.4 * (Math.Cos(hSample3 + 35))));
                    F3 = Math.Pow(cSample3, 4) / (Math.Pow(cSample3, 4) + 1900);
                    F3 = Math.Sqrt(F3);
                    SHSample3 = SCSample3 * ((T3 * F3) + 1 - F3);

                    SetTextBox(textBox10, Math.Round(lStandartSample3, 4).ToString());
                    SetTextBox(textBox11, Math.Round(aStandartSample3, 4).ToString());
                    SetTextBox(textBox12, Math.Round(bStandartSample3, 4).ToString());

                    break;
                }
            }
        }

        private void Process4(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState1 == true)
                {
                    double[] RTestSample = new double[dataCount];
                    double[] ETestSample = new double[dataCount];
                    int lastTime = 0;
                    double deltaEAverage = 0;
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ETestSample = keyValuePair.Value;
                    List<string> times = keyValuePair.Key;

                    stopwatch.Stop();
                    string time = stopwatch.Elapsed.TotalMilliseconds.ToString();
                    times.Add(time);
                    AddListBox(listBox1, times);

                    for (int i = 0; i < RTestSample.Length; i++)
                        RTestSample[i] = ETestSample[i] / ERsLed1[i];

                    double XTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lTestSample = (116 * Math.Pow(YTestSample, 1.0 / 3.0)) - 16;
                    double aTestSample = 500 * (Math.Pow(XTestSample, 1.0 / 3.0) - Math.Pow(YTestSample, 1.0 / 3.0));
                    double bTestSample = 200 * (Math.Pow(YTestSample, 1.0 / 3.0) - Math.Pow(ZTestSample, 1.0 / 3.0));

                    double lDifferenceAbs = lTestSample - lStandartSample1;
                    lDifferenceAbs = Math.Abs(lDifferenceAbs);
                    double lDifference = Math.Pow(lDifferenceAbs, 2);

                    double aDifferenceAbs = aTestSample - aStandartSample1;
                    aDifferenceAbs = Math.Abs(aDifferenceAbs);
                    double aDifference = Math.Pow(aDifferenceAbs, 2);

                    double bDifferenceAbs = bTestSample - bStandartSample1;
                    bDifferenceAbs = Math.Abs(bDifferenceAbs);
                    double bDifference = Math.Pow(bDifferenceAbs, 2);

                    deltaEResult1 = Math.Sqrt(lDifference + aDifference + bDifference);

                    cTest1 = Math.Pow(aTestSample, 2) + Math.Pow(bTestSample, 2);
                    cTest1 = Math.Sqrt(cTest1);
                    deltaEH1 = Math.Pow(deltaEResult1, 2) - lDifference - Math.Pow((cTest1 - cSample1), 2);
                    deltaEH1 = Math.Sqrt(deltaEH1);

                    deltaECMC11Result1 = Math.Pow((lDifference / (1 * SLSample1)), 2) + Math.Pow(((cTest1 - cSample1) / (1 * SCSample1)), 2)
                                       + Math.Pow((deltaEH1 / SHSample1), 2);
                    deltaECMC11Result1 = Math.Sqrt(deltaECMC11Result1);
                    deltaECMC21Result1 = Math.Pow((lDifference / (2 * SLSample1)), 2) + Math.Pow(((cTest1 - cSample1) / (1 * SCSample1)), 2)
                                       + Math.Pow((deltaEH1 / SHSample1), 2);
                    deltaECMC21Result1 = Math.Sqrt(deltaECMC21Result1);

                    SetTextBox(textBox13, Math.Round(lTestSample, 5).ToString());
                    SetTextBox(textBox14, Math.Round(aTestSample, 5).ToString());
                    SetTextBox(textBox15, Math.Round(bTestSample, 5).ToString());

                    lastTime = (DateTime.Now.Hour * 60 * 60) + (DateTime.Now.Minute * 60) + (DateTime.Now.Second);
                    lastTime = lastTime - firstTime;

                    if (Parameters.cmcType == "Normal")
                    {
                        deltaEList1.Add(deltaEResult1);
                        SetTextBox(textBox16, Math.Round(deltaEResult1, 5).ToString());

                        SetLabel(label28, Math.Round(deltaEResult1, 5).ToString());
                        SetChart(chart1, lastTime, Math.Round(deltaEResult1, 5));
                    }
                    else if (Parameters.cmcType == "CMC1:1")
                    {
                        deltaEList1.Add(deltaECMC11Result1);
                        SetTextBox(textBox16, Math.Round(deltaECMC11Result1, 5).ToString());

                        SetLabel(label28, Math.Round(deltaECMC11Result1, 5).ToString());
                        SetChart(chart1, lastTime, Math.Round(deltaECMC11Result1, 5));
                    }
                    else if (Parameters.cmcType == "CMC2:1")
                    {
                        deltaEList1.Add(deltaECMC21Result1);
                        SetTextBox(textBox16, Math.Round(deltaECMC21Result1, 5).ToString());

                        SetLabel(label28, Math.Round(deltaECMC21Result1, 5).ToString());
                        SetChart(chart1, lastTime, Math.Round(deltaECMC21Result1, 5));
                    }

                    for (int j = 1; j < deltaEList1.Count; j++)
                        deltaEAverage = deltaEAverage + deltaEList1[j];
                    deltaEAverage = deltaEAverage / (deltaEList1.Count - 1);
                    SetLabel(label30, Math.Round(deltaEAverage, 5).ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void Process5(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState2 == true)
                {
                    double[] RTestSample = new double[dataCount];
                    double[] ETestSample = new double[dataCount];
                    int lastTime = 0;
                    double deltaEAverage = 0;

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ETestSample = keyValuePair.Value;
                    for (int i = 0; i < RTestSample.Length; i++)
                        RTestSample[i] = ETestSample[i] / ERsLed2[i];

                    double XTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lTestSample = (116 * Math.Pow(YTestSample, 1.0 / 3.0)) - 16;
                    double aTestSample = 500 * (Math.Pow(XTestSample, 1.0 / 3.0) - Math.Pow(YTestSample, 1.0 / 3.0));
                    double bTestSample = 200 * (Math.Pow(YTestSample, 1.0 / 3.0) - Math.Pow(ZTestSample, 1.0 / 3.0));

                    double lDifferenceAbs = lTestSample - lStandartSample2;
                    lDifferenceAbs = Math.Abs(lDifferenceAbs);
                    double lDifference = Math.Pow(lDifferenceAbs, 2);

                    double aDifferenceAbs = aTestSample - aStandartSample2;
                    aDifferenceAbs = Math.Abs(aDifferenceAbs);
                    double aDifference2 = Math.Pow(aDifferenceAbs, 2);

                    double bDifferenceAbs = bTestSample - bStandartSample2;
                    bDifferenceAbs = Math.Abs(bDifferenceAbs);
                    double bDifference2 = Math.Pow(bDifferenceAbs, 2);

                    deltaEResult2 = Math.Sqrt(lDifference + aDifference2 + bDifference2);

                    cTest2 = Math.Pow(aTestSample, 2) + Math.Pow(bTestSample, 2);
                    cTest2 = Math.Sqrt(cTest2);
                    deltaEH2 = Math.Pow(deltaEResult2, 2) - lDifference - Math.Pow((cTest2 - cSample2), 2);
                    deltaEH2 = Math.Sqrt(deltaEH2);

                    deltaECMC11Result2 = Math.Pow((lDifference / (1 * SLSample2)), 2) + Math.Pow(((cTest2 - cSample2) / (1 * SCSample2)), 2)
                                       + Math.Pow((deltaEH2 / SHSample2), 2);
                    deltaECMC11Result2 = Math.Sqrt(deltaECMC11Result2);
                    deltaECMC21Result2 = Math.Pow((lDifference / (2 * SLSample2)), 2) + Math.Pow(((cTest2 - cSample2) / (1 * SCSample2)), 2)
                                       + Math.Pow((deltaEH2 / SHSample2), 2);
                    deltaECMC21Result2 = Math.Sqrt(deltaECMC21Result2);

                    SetTextBox(textBox17, Math.Round(lTestSample, 5).ToString());
                    SetTextBox(textBox18, Math.Round(aTestSample, 5).ToString());
                    SetTextBox(textBox19, Math.Round(bTestSample, 5).ToString());

                    lastTime = (DateTime.Now.Hour * 60 * 60) + (DateTime.Now.Minute * 60) + (DateTime.Now.Second);
                    lastTime = lastTime - firstTime;

                    if (Parameters.cmcType == "Normal")
                    {
                        deltaEList2.Add(deltaEResult2);
                        SetTextBox(textBox20, Math.Round(deltaEResult2, 5).ToString());

                        SetLabel(label32, Math.Round(deltaEResult2, 5).ToString());
                        SetChart(chart2, lastTime, Math.Round(deltaEResult2, 5));
                    }
                    else if (Parameters.cmcType == "CMC1:1")
                    {
                        deltaEList2.Add(deltaECMC11Result2);
                        SetTextBox(textBox20, Math.Round(deltaECMC11Result2, 5).ToString());

                        SetLabel(label32, Math.Round(deltaECMC11Result2, 5).ToString());
                        SetChart(chart2, lastTime, Math.Round(deltaECMC11Result2, 5));
                    }
                    else if (Parameters.cmcType == "CMC2:1")
                    {
                        deltaEList2.Add(deltaECMC21Result2);
                        SetTextBox(textBox20, Math.Round(deltaECMC21Result2, 5).ToString());

                        SetLabel(label32, Math.Round(deltaECMC21Result2, 5).ToString());
                        SetChart(chart2, lastTime, Math.Round(deltaECMC21Result2, 5));
                    }

                    for (int j = 1; j < deltaEList2.Count; j++)
                        deltaEAverage = deltaEAverage + deltaEList2[j];
                    deltaEAverage = deltaEAverage / (deltaEList2.Count - 1);
                    SetLabel(label34, Math.Round(deltaEAverage, 5).ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void Process6(SerialPort serialPort, string sendData, int[] nmCount, string choosingFilter, int loopCount, int dataLenght, int dataCount)
        {
            while (true)
            {
                if (threadState3 == true)
                {
                    double[] RTestSample = new double[dataCount];
                    double[] ETestSample = new double[dataCount];
                    int lastTime = 0;
                    double deltaEAverage = 0;

                    KeyValuePair<List<string>, double[]> keyValuePair = Functions.LoopPixelDataProcess(serialPort, sendData, nmCount, choosingFilter, loopCount, dataLenght, dataCount);
                    ETestSample = keyValuePair.Value;
                    for (int i = 0; i < RTestSample.Length; i++)
                        RTestSample[i] = ETestSample[i] / ERsLed3[i];

                    double XTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.x10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10x;
                    double YTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.y10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10y;
                    double ZTestSample = Functions.XYZCalculation(RTestSample, StandartValues.d65, StandartValues.z10, StandartValues.y10, StandartValues.deltaLamda) / StandartValues.W10z;

                    double lTestSample = (116 * Math.Pow(YTestSample, 1.0 / 3.0)) - 16;
                    double aTestSample = 500 * (Math.Pow(XTestSample, 1.0 / 3.0) - Math.Pow(YTestSample, 1.0 / 3.0));
                    double bTestSample = 200 * (Math.Pow(YTestSample, 1.0 / 3.0) - Math.Pow(ZTestSample, 1.0 / 3.0));

                    double lDifferenceAbs = lTestSample - lStandartSample3;
                    lDifferenceAbs = Math.Abs(lDifferenceAbs);
                    double lDifference = Math.Pow(lDifferenceAbs, 2);

                    double aDifferenceAbs = aTestSample - aStandartSample3;
                    aDifferenceAbs = Math.Abs(aDifferenceAbs);
                    double aDifference = Math.Pow(aDifferenceAbs, 2);

                    double bDifferenceAbs = bTestSample - bStandartSample3;
                    bDifferenceAbs = Math.Abs(bDifferenceAbs);
                    double bDifference = Math.Pow(bDifferenceAbs, 2);

                    deltaEResult3 = Math.Sqrt(lDifference + aDifference + bDifference);

                    cTest3 = Math.Pow(aTestSample, 2) + Math.Pow(bTestSample, 2);
                    cTest3 = Math.Sqrt(cTest3);
                    deltaEH3 = Math.Pow(deltaEResult3, 2) - lDifference - Math.Pow((cTest3 - cSample3), 2);
                    deltaEH3 = Math.Sqrt(deltaEH3);

                    deltaECMC11Result3 = Math.Pow((lDifference / (1 * SLSample3)), 2) + Math.Pow(((cTest3 - cSample3) / (1 * SCSample3)), 2)
                                       + Math.Pow((deltaEH3 / SHSample3), 2);
                    deltaECMC11Result3 = Math.Sqrt(deltaECMC11Result3);
                    deltaECMC21Result3 = Math.Pow((lDifference / (2 * SLSample3)), 2) + Math.Pow(((cTest3 - cSample3) / (1 * SCSample3)), 2)
                                       + Math.Pow((deltaEH3 / SHSample3), 2);
                    deltaECMC21Result3 = Math.Sqrt(deltaECMC21Result3);

                    SetTextBox(textBox21, Math.Round(lTestSample, 5).ToString());
                    SetTextBox(textBox22, Math.Round(aTestSample, 5).ToString());
                    SetTextBox(textBox23, Math.Round(bTestSample, 5).ToString());

                    lastTime = (DateTime.Now.Hour * 60 * 60) + (DateTime.Now.Minute * 60) + (DateTime.Now.Second);
                    lastTime = lastTime - firstTime;

                    if (Parameters.cmcType == "Normal")
                    {
                        deltaEList3.Add(deltaEResult3);
                        SetTextBox(textBox24, Math.Round(deltaEResult3, 5).ToString());

                        SetLabel(label36, Math.Round(deltaEResult3, 5).ToString());
                        SetChart(chart3, lastTime, Math.Round(deltaEResult3, 5));
                    }
                    else if (Parameters.cmcType == "CMC1:1")
                    {
                        deltaEList3.Add(deltaECMC11Result3);
                        SetTextBox(textBox24, Math.Round(deltaECMC11Result3, 5).ToString());

                        SetLabel(label36, Math.Round(deltaECMC11Result3, 5).ToString());
                        SetChart(chart3, lastTime, Math.Round(deltaECMC11Result3, 5));
                    }
                    else if (Parameters.cmcType == "CMC2:1")
                    {
                        deltaEList3.Add(deltaECMC21Result3);
                        SetTextBox(textBox24, Math.Round(deltaECMC21Result3, 5).ToString());

                        SetLabel(label36, Math.Round(deltaECMC21Result3, 5).ToString());
                        SetChart(chart3, lastTime, Math.Round(deltaECMC21Result3, 5));
                    }

                    for (int j = 1; j < deltaEList3.Count; j++)
                        deltaEAverage = deltaEAverage + deltaEList3[j];
                    deltaEAverage = deltaEAverage / (deltaEList3.Count - 1);
                    SetLabel(label38, Math.Round(deltaEAverage, 5).ToString());
                }
                else
                {
                    break;
                }
            }
        }

        private void Process7()
        {
            while (true)
            {
                if (threadState4 == true)
                {
                    int lastTime = 0;
                    double deltaE12AverageSum = 0;
                    double deltaE13AverageSum = 0;
                    double deltaE23AverageSum = 0;

                    if (deltaEList1.Count == deltaEValue && deltaEList2.Count == deltaEValue && deltaEList3.Count == deltaEValue)
                    {
                        deltaE12 = deltaEList1[deltaEValue - 1] - deltaEList2[deltaEValue - 1];
                        deltaEList12.Add(deltaE12);
                        deltaE13 = deltaEList1[deltaEValue - 1] - deltaEList3[deltaEValue - 1];
                        deltaEList13.Add(deltaE13);
                        deltaE23 = deltaEList2[deltaEValue - 1] - deltaEList3[deltaEValue - 1];
                        deltaEList23.Add(deltaE23);

                        lastTime = (DateTime.Now.Hour * 60 * 60) + (DateTime.Now.Minute * 60) + (DateTime.Now.Second);
                        lastTime = lastTime - firstTime;

                        SetTextBox(textBox1, deltaE12.ToString());
                        SetTextBox(textBox2, deltaE13.ToString());
                        SetTextBox(textBox3, deltaE23.ToString());

                        SetLabel(label40, deltaE12.ToString());
                        SetLabel(label44, deltaE13.ToString());
                        SetLabel(label48, deltaE23.ToString());

                        SetChart(chart4, lastTime, deltaE12);
                        SetChart(chart5, lastTime, deltaE13);
                        SetChart(chart6, lastTime, deltaE23);

                        for (int i = 1; i < deltaEList12.Count; i++)
                        {
                            deltaE12AverageSum = deltaE12AverageSum + deltaEList12[i];
                            deltaE13AverageSum = deltaE13AverageSum + deltaEList13[i];
                            deltaE23AverageSum = deltaE23AverageSum + deltaEList23[i];
                        }
                        deltaE12AverageSum = deltaE12AverageSum / (deltaEList12.Count - 1);
                        deltaE13AverageSum = deltaE13AverageSum / (deltaEList13.Count - 1);
                        deltaE23AverageSum = deltaE23AverageSum / (deltaEList23.Count - 1);

                        SetLabel(label42, Math.Round(deltaE12AverageSum, 5).ToString());
                        SetLabel(label46, Math.Round(deltaE13AverageSum, 5).ToString());
                        SetLabel(label50, Math.Round(deltaE23AverageSum, 5).ToString());

                        deltaEValue++;
                    }
                    else
                    {
                        SetTextBox(textBox1, "-");
                        SetTextBox(textBox2, "-");
                        SetTextBox(textBox3, "-");

                        SetLabel(label40, "-");
                        SetLabel(label44, "-");
                        SetLabel(label48, "-");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void Process8()
        {
            while (true)
            {
                if (threadState5 == true)
                {
                    Thread.Sleep(Parameters.saveTime);

                    DateTime time = DateTime.Now;
                    string timeString = time.ToString();

                    string connectionString = "DataSource=" + Parameters.nameDatabase + "; Version = 3;";
                    string requestCode = "insert into " + Parameters.nameTable + "(id, date, deltaE1, deltaE2, deltaE3, kf12, kf13, kf23) values (" + Parameters.id + ",'" +
                        timeString + "','" + Math.Round(deltaEResult1, 5).ToString() + "','" + Math.Round(deltaEResult2, 5).ToString() + "','" +
                        Math.Round(deltaEResult3, 5).ToString() + "','" + Math.Round(deltaE12, 5).ToString() + "','"
                        + Math.Round(deltaE13, 5).ToString() + "','" + Math.Round(deltaE23, 5).ToString() + "')";

                    Parameters.command = new SQLiteCommand();
                    Parameters.connection = new SQLiteConnection(connectionString);
                    Parameters.connection.Open();
                    if (Parameters.connection.State == ConnectionState.Open)
                    {
                        Parameters.command.Connection = Parameters.connection;
                        Parameters.command.CommandText = requestCode;
                        Parameters.command.ExecuteNonQuery();
                        Parameters.connection.Close();
                        Parameters.id++;
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data...", "Information");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void CreateStorage()
        {
            string connectionString = "DataSource=" + Parameters.nameDatabase + "; Version = 3;";
            string createDateTable = @"create table if not exists " + Parameters.nameTable + "(id INTEGER PRIMARY KEY, " +
                "date varchar(50) NOT NULL, deltaE1 varchar(50) NOT NULL, deltaE2 varchar(50) NOT NULL, deltaE3 varchar(50) NOT NULL, " +
                "kf12 varchar(50) NOT NULL, kf13 varchar(50) NOT NULL, kf23 varchar(50) NOT NULL);";

            if (File.Exists(Parameters.nameDatabase + ".sqlite"))
            {
                SQLiteConnection.CreateFile(Parameters.nameDatabase);
            }
            else
            {
                Parameters.connection = new SQLiteConnection(connectionString);
                Parameters.connection.Open();
                if (Parameters.connection.State == ConnectionState.Open)
                {
                    Parameters.command = new SQLiteCommand(createDateTable, Parameters.connection);
                    Parameters.command.ExecuteNonQuery();
                    Parameters.connection.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create table...", "Information");
                }
            }
        }

        private void ReadSettings()
        {
            string fileName = "Settings.txt";
            int textCount = 11;
            string[] readData = new string[textCount];
            Functions.ReadFile(fileName, textCount, readData, 0, 10);

            Parameters.selectDevicePort1 = readData[0];
            Parameters.selectDevicePort2 = readData[1];
            Parameters.selectDevicePort3 = readData[2];
            Parameters.integrationTime = label23.Text = readData[3];
            Parameters.averageScan = label26.Text = readData[4];
            Parameters.digitalGain = readData[5];
            Parameters.analogGain = readData[6];
            Parameters.testLoopCount = readData[7];
            Parameters.otherLoopCount = readData[8];
            Parameters.filter = readData[9];
            Parameters.cmcType = readData[10];
        }

        private void ReadStandartPlate1()
        {
            string fileName = "PlateValues1.txt";
            int textCount = 61;
            string[] readData = new string[textCount];
            Functions.ReadFile(fileName, textCount, readData, 0, 60);
            for (int i = 0; i < textCount; i++)
                standartPlate1[i] = Convert.ToDouble(readData[i]);
        }

        private void ReadStandartPlate2()
        {
            string fileName = "PlateValues2.txt";
            int textCount = 61;
            string[] readData = new string[textCount];
            Functions.ReadFile(fileName, textCount, readData, 0, 60);
            for (int i = 0; i < textCount; i++)
                standartPlate2[i] = Convert.ToDouble(readData[i]);
        }

        private void ReadParameterId()
        {
            string connectionString = "DataSource=" + Parameters.nameDatabase + "; Version = 3;";
            string requestCode = "select * from " + Parameters.nameTable;

            Parameters.command = new SQLiteCommand();
            Parameters.connection = new SQLiteConnection(connectionString);
            Parameters.connection.Open();
            if (Parameters.connection.State == ConnectionState.Open)
            {
                Parameters.command.Connection = Parameters.connection;
                Parameters.command.CommandText = requestCode;
                Parameters.command.ExecuteNonQuery();
                SQLiteDataReader reader = Parameters.command.ExecuteReader();
                while (reader.Read())
                    Parameters.id = Convert.ToInt32(reader["id"].ToString());
                Parameters.connection.Close();
                Parameters.id++;
            }
            else
            {
                MessageBox.Show("Failed to list data...", "Information");
            }
        }

        private void ReadNmIndisRelatedPixel()
        {
            if (Parameters.selectDevicePort1 != "A")
                nmCount1 = Functions.NmRelatedPixelCalculation(serialPort1, Parameters.dataLenght - 1, Parameters.dataCount, Parameters.firstNm);
            if (Parameters.selectDevicePort2 != "B")
                nmCount2 = Functions.NmRelatedPixelCalculation(serialPort2, Parameters.dataLenght - 1, Parameters.dataCount, Parameters.firstNm);
            if (Parameters.selectDevicePort3 != "C")
                nmCount3 = Functions.NmRelatedPixelCalculation(serialPort3, Parameters.dataLenght - 1, Parameters.dataCount, Parameters.firstNm);
        }

        private void DisconnectionFunction()
        {
            threadState1 = false;
            threadState2 = false;
            threadState3 = false;
            threadState4 = false;
            threadState5 = false;

            Functions.CloseSerialPort(serialPort1);
            Functions.CloseSerialPort(serialPort2);
            Functions.CloseSerialPort(serialPort3);

            GroupBoxEnableFalse();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox15.Text = "";
            textBox14.Text = "";
            textBox13.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox16.Text = "";
            textBox20.Text = "";
            textBox19.Text = "";
            textBox18.Text = "";
            textBox17.Text = "";
            textBox24.Text = "";
            textBox23.Text = "";
            textBox22.Text = "";
            textBox21.Text = "";

            label1.Text = "Default";
            label1.ForeColor = Color.Brown;
            label4.Text = "Default";
            label4.ForeColor = Color.Brown;
            label3.Text = "Default";
            label3.ForeColor = Color.Brown;
            label11.Text = "Default";
            label11.ForeColor = Color.Brown;
            label23.Text = "X";
            label26.Text = "X";
            label48.Text = "";
            label44.Text = "";
            label40.Text = "";
            label50.Text = "";
            label28.Text = "X";
            label32.Text = "X";
            label46.Text = "";
            label42.Text = "";
            label30.Text = "X";
            label34.Text = "X";
            label38.Text = "X";
            label36.Text = "X";

            chart3.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].Points.Clear();
            chart2.Series["Series1"].Points.Clear();
            chart6.Series["Series1"].Points.Clear();
            chart5.Series["Series1"].Points.Clear();
            chart4.Series["Series1"].Points.Clear();

            comboBox1.SelectedIndex = 0;
        }

        private void GroupBoxEnableFalse()
        {
            groupBox1.Enabled = false;
            groupBox3.Enabled = false;
            groupBox10.Enabled = false;
            groupBox11.Enabled = false;
            groupBox7.Enabled = false;
            groupBox5.Enabled = false;
            groupBox2.Enabled = false;
            groupBox9.Enabled = false;
            groupBox8.Enabled = false;
            groupBox4.Enabled = false;
            groupBox6.Enabled = false;
        }

        delegate void AddListBoxCallback(ListBox listBox, List<string> value);
        private void AddListBox(ListBox listBox, List<string> value)
        {
            if (listBox.InvokeRequired)
            {
                AddListBoxCallback d = new AddListBoxCallback(_AddListBox);
                listBox.Invoke(d, new object[] { listBox, value });
            }
            else
            {
                _AddListBox(listBox, value);
            }
        }

        private void _AddListBox(ListBox listBox, List<string> value)
        {
            if (value.Count > 0)
            {
                for (int i = 0; i < value.Count; i++)
                    listBox.Items.Insert(i, value[i]);
            }
            listBox.Items.Add("--------------");
        }

        delegate void SetChartCallback(Chart chart, int time, double value);
        private void SetChart(Chart chart, int time, double value)
        {
            if (chart.InvokeRequired)
            {
                SetChartCallback d = new SetChartCallback(_SetChart);
                chart.Invoke(d, new object[] { chart, time, value });
            }
            else
            {
                _SetChart(chart, time, value);
            }
        }

        private void _SetChart(Chart chart, int time, double value)
        {
            chart.Series[0].Points.AddXY(time, value);
            chart.ChartAreas["ChartArea1"].AxisY.Maximum = (int)value + 1;
            chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            graphicsMaximumDelta = (int)value + 1;
        }

        delegate void SetTextBoxCallback(TextBox textBox, string text);
        private void SetTextBox(TextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                SetTextBoxCallback d = new SetTextBoxCallback(_SetTextBox);
                textBox.Invoke(d, new object[] { textBox, text });
            }
            else
            {
                _SetTextBox(textBox, text);
            }
        }

        private void _SetTextBox(TextBox textBox, string text)
        {
            textBox.Text = text;
        }

        delegate void SetLabelCallback(Label label, string text);
        private void SetLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(_SetLabel);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                _SetLabel(label, text);
            }
        }

        private void _SetLabel(Label label, string text)
        {
            label.Text = text;
        }
    }
}

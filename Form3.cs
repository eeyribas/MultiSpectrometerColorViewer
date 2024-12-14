using MultiSpectrometerView.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiSpectrometerView
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ReadSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] newData = new string[11];

            if (comboBox1.Text != "" && comboBox1.Text != " ")
                Parameters.selectDevicePort1 = comboBox1.Text;
            else
                Parameters.selectDevicePort1 = "A";

            if (comboBox2.Text != "" && comboBox2.Text != " ")
                Parameters.selectDevicePort2 = comboBox2.Text;
            else
                Parameters.selectDevicePort2 = "B";

            if (comboBox3.Text != "" && comboBox3.Text != " ")
                Parameters.selectDevicePort3 = comboBox3.Text;
            else
                Parameters.selectDevicePort3 = "C";

            try
            {
                newData[0] = label11.Text = Parameters.selectDevicePort1;
                newData[1] = label12.Text = Parameters.selectDevicePort2;
                newData[2] = label13.Text = Parameters.selectDevicePort3;
                newData[3] = Parameters.integrationTime;
                newData[4] = Parameters.averageScan;
                newData[5] = Parameters.digitalGain;
                newData[6] = Parameters.analogGain;
                newData[7] = Parameters.testLoopCount;
                newData[8] = Parameters.otherLoopCount;
                newData[9] = Parameters.filter;
                newData[10] = Parameters.cmcType;
                Functions.WriteFile("Setting.txt", newData);

                comboBox1.ResetText();
                comboBox2.ResetText();
                comboBox3.ResetText();

                label4.Text = "Default";
                label4.ForeColor = Color.Brown;
                this.Refresh();
                Thread.Sleep(200);
                label4.Text = "Save";
                label4.ForeColor = Color.Green;
            }
            catch (Exception)
            {
                label4.Text = "Default";
                label4.ForeColor = Color.Brown;
                this.Refresh();
                Thread.Sleep(200);
                label4.Text = "Error";
                label4.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "Default";
            label4.ForeColor = Color.Brown;

            comboBox1.ResetText();
            comboBox2.ResetText();
            comboBox3.ResetText();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string serialPort in serialPorts)
            {
                comboBox1.Items.Add(serialPort);
                comboBox2.Items.Add(serialPort);
                comboBox3.Items.Add(serialPort);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReadSettings()
        {
            string filenameRead = "Setting.txt";
            int textCount = 11;
            string[] readData = new string[textCount];
            Functions.ReadFile(filenameRead, textCount, readData, 0, 10);

            Parameters.selectDevicePort1 = label11.Text = readData[0];
            Parameters.selectDevicePort2 = label12.Text = readData[1];
            Parameters.selectDevicePort3 = label13.Text = readData[2];
            Parameters.integrationTime = readData[3];
            Parameters.averageScan = readData[4];
            Parameters.digitalGain = readData[5];
            Parameters.analogGain = readData[6];
            Parameters.testLoopCount = readData[7];
            Parameters.otherLoopCount = readData[8];
            Parameters.filter = readData[9];
            Parameters.cmcType = readData[10];
        }
    }
}

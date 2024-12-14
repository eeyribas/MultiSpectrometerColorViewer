using MultiSpectrometerView.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiSpectrometerView
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            ReadSettings();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] newData = new string[11];

            if (textBox1.Text != "" && textBox1.Text != " ")
                Parameters.integrationTime = textBox1.Text;
            if (textBox2.Text != "" && textBox2.Text != " ")
                Parameters.averageScan = textBox2.Text;

            if (comboBox1.SelectedIndex == 0)
                Parameters.digitalGain = "Low";
            else
                Parameters.digitalGain = "High";
            Parameters.analogGain = label15.Text;

            if (textBox4.Text != "" && textBox4.Text != " ")
                Parameters.testLoopCount = textBox4.Text;
            if (textBox3.Text != "" && textBox3.Text != " ")
                Parameters.otherLoopCount = textBox3.Text;

            if (comboBox2.SelectedIndex == 0)
                Parameters.filter = "Normal";
            else if (comboBox2.SelectedIndex == 1)
                Parameters.filter = "MOA";

            if (comboBox3.SelectedIndex == 0)
                Parameters.cmcType = "Normal";
            else if (comboBox3.SelectedIndex == 1)
                Parameters.cmcType = "CMC1:1";
            else if (comboBox3.SelectedIndex == 2)
                Parameters.cmcType = "CMC2:1";

            try
            {
                newData[0] = Parameters.selectDevicePort1;
                newData[1] = Parameters.selectDevicePort2;
                newData[2] = Parameters.selectDevicePort3;
                newData[3] = label5.Text = Parameters.integrationTime;
                newData[4] = label7.Text = Parameters.averageScan;
                newData[5] = label8.Text = Parameters.digitalGain;
                newData[6] = label9.Text = Parameters.analogGain;
                newData[7] = label21.Text = Parameters.testLoopCount;
                newData[8] = label20.Text = Parameters.otherLoopCount;
                newData[9] = label26.Text = Parameters.filter;
                newData[10] = label30.Text = Parameters.cmcType;
                Functions.WriteFile("Setting.txt", newData);

                trackBar1.Value = 11;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 1;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                label16.Text = "Default";
                label16.ForeColor = Color.Brown;
                label17.Text = "Default";
                label17.ForeColor = Color.Brown;
                label24.Text = "Default";
                label24.ForeColor = Color.Brown;
                label28.Text = "Default";
                label28.ForeColor = Color.Brown;
                this.Refresh();
                Thread.Sleep(250);
                label16.Text = "Save";
                label16.ForeColor = Color.Green;
                label17.Text = "Save";
                label17.ForeColor = Color.Green;
                label24.Text = "Save";
                label24.ForeColor = Color.Green;
                label28.Text = "Save";
                label28.ForeColor = Color.Green;
            }
            catch (Exception)
            {
                label16.Text = "Error";
                label16.ForeColor = Color.Red;
                label17.Text = "Error";
                label17.ForeColor = Color.Red;
                label24.Text = "Error";
                label24.ForeColor = Color.Red;
                label28.Text = "Error";
                label28.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int trackbarValue = trackBar1.Value;
            trackbarValue = trackbarValue / 10;
            label15.Text = trackbarValue.ToString();
        }

        private void ReadSettings()
        {
            string filenameRead = "Setting.txt";
            int textCount = 11;
            string[] readData = new string[textCount];
            Functions.ReadFile(filenameRead, textCount, readData, 0, 10);

            Parameters.selectDevicePort1 = readData[0];
            Parameters.selectDevicePort2 = readData[1];
            Parameters.selectDevicePort3 = readData[2];
            Parameters.integrationTime = label5.Text = readData[3];
            Parameters.averageScan = label7.Text = readData[4];
            Parameters.digitalGain = label8.Text = readData[5];
            Parameters.analogGain = label9.Text = readData[6];
            Parameters.testLoopCount = label21.Text = readData[7];
            Parameters.otherLoopCount = label20.Text = readData[8];
            Parameters.filter = label26.Text = readData[9];
            Parameters.cmcType = label30.Text = readData[10];
        }
    }
}

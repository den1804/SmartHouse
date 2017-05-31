using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace arduino
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            notifyIcon1.Visible = false;
            this.notifyIcon1.MouseClick += new MouseEventHandler(notifyIcon1_MouseClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);

            // Открываем порт, и задаем скорость в 9600 бод
            
        }
        //****** поток ком порта
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string vlag = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), vlag);
        }
        private void serialPort1_DataReceived1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string temp = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent1(LineReceived1), temp);
        }
        private void serialPort1_DataReceived2(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string svet = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent2(LineReceived2), svet);
        }
        private void serialPort1_DataReceived3(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string mowe = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent3(LineReceived3), mowe);
        }
        private void serialPort1_DataReceived4(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string relestats = serialPort1.ReadLine();
            this.BeginInvoke(new LineReceivedEvent4(LineReceived4), relestats);
        }
        //запись влажности
        private delegate void LineReceivedEvent(string vlag);
        private void LineReceived(string vlag)
        {
            textBox1.Text = vlag;
            string path = "График_влажности.txt";
            string date = DateTime.Now.ToString();
            // Создание файла и запись в него
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(vlag);
                sw.WriteLine(date);
            }
        }

        //запись температуры
        private delegate void LineReceivedEvent1(string temp);
        private void LineReceived1(string temp)
        {
            textBox2.Text = temp.ToString();
            string path = "График_температуры.txt";
            string date = DateTime.Now.ToString();
            // Создание файла и запись в него
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(temp);
                sw.WriteLine(date);
            }
        }
        // запись света
        private delegate void LineReceivedEvent2(string svet);
        private void LineReceived2(string svet)
        {
            if (Convert.ToInt32(svet) > 100 && Convert.ToInt32(svet) < 200)
            {
                textBox3.Text = "Хорошое освещение";
            }
            else if (Convert.ToInt32(svet) < 100)
            {
                textBox3.Text = "Мало освещения";
            }
            else
            {
                textBox3.Text = "Достаточно освещения";
            }
            string path = "График_sveta.txt";
            string date = DateTime.Now.ToString();
            // Создание файла и запись в него
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(svet);
                sw.WriteLine(date);
            }
        }
        // move
        private delegate void LineReceivedEvent3(string move);
        private void LineReceived3(string move)
        {
            if (Convert.ToInt32(move) == 1)
                textBox4.Text = "Движение";
            else
                textBox4.Text = "Нет_Движения";  
            string path = "График_move.txt";
            string date = DateTime.Now.ToString();
            // Создание файла и запись в него
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(textBox4.Text + " "+ date);
            }
            
        }
        // rele
        private delegate void LineReceivedEvent4(string rele);
        private void LineReceived4(string rele)
        {
            if (Convert.ToInt32(rele) == 1)
                textBox5.Text = "Включено";
            else
                textBox5.Text = "Выключено";  
            
            string path = "График_rele.txt";
            string date = DateTime.Now.ToString();
            // Создание файла и запись в него
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(textBox5.Text+" "+date);
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void демоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            tabControl1.Visible = true;
            panel2.Visible = false;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, 30);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            StreamReader streamReader = new StreamReader("График_температуры.txt");
            chart1.Series[0].Points.Clear();
            while (!streamReader.EndOfStream)
            {
                string Y = streamReader.ReadLine();
                string X = streamReader.ReadLine();

                chart1.Series[0].Color = Color.Red;
                chart1.Series[0].BorderWidth = 1;
                chart1.Series[0].Points.AddXY(X, Y);
            }
            streamReader.Close();

            //График_влажности
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(0, 30);
            chart2.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            streamReader = new StreamReader("График_влажности.txt");
            chart2.Series[0].Points.Clear();
            while (!streamReader.EndOfStream)
            {
                string Y = streamReader.ReadLine();
                string X = streamReader.ReadLine();

                chart2.Series[0].Color = Color.Red;
                chart2.Series[0].BorderWidth = 1;
                chart2.Series[0].Points.AddXY(X, Y);
            }
            streamReader.Close();

            //График_sveta
            chart3.ChartAreas[0].AxisX.ScaleView.Zoom(0, 30);
            chart3.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart3.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart3.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart3.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            streamReader = new StreamReader("График_sveta.txt");
            chart3.Series[0].Points.Clear();
            while (!streamReader.EndOfStream)
            {
                string Y = streamReader.ReadLine();
                string X = streamReader.ReadLine();

                chart3.Series[0].Color = Color.Red;
                chart3.Series[0].BorderWidth = 1;
                chart3.Series[0].Points.AddXY(X, Y);
            }
            streamReader.Close();
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void поточнийСтанToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            tabControl1.Visible = false;
            panel2.Visible = false;
           
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            tabControl1.Visible = false;
            panel2.Visible = false;
        }
        private void состояниеРелеИДатчикаДвиженияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            tabControl1.Visible = false;
            panel2.Visible = true;

            StreamReader streamReader = new StreamReader("График_move.txt");
            DataSet ds = new DataSet();
            ds.Tables.Add("Score");
            string header = streamReader.ReadLine();
            string[] col = System.Text.RegularExpressions.Regex.Split(header, " ");
            for (int c = 0; c < col.Length; c++)
            {
                ds.Tables[0].Columns.Add(col[c]);
            }
            string row = streamReader.ReadLine();
            while (row != null)
            {
                string[] value = System.Text.RegularExpressions.Regex.Split(row, " ");
                ds.Tables[0].Rows.Add(value);
                row = streamReader.ReadLine();
            }
            dataGridView2.DataSource = ds.Tables[0];
            streamReader.Close();



            streamReader = new StreamReader("График_rele.txt");
            ds = new DataSet();
            ds.Tables.Add("Score1");
            header = streamReader.ReadLine();
            col = System.Text.RegularExpressions.Regex.Split(header, " ");
            for (int c = 0; c < col.Length; c++)
            {
                ds.Tables["Score1"].Columns.Add(col[c]);
            }
            row = streamReader.ReadLine();
            while (row != null)
            {
                string[] value = System.Text.RegularExpressions.Regex.Split(row, " ");
                ds.Tables["Score1"].Rows.Add(value);
                row = streamReader.ReadLine();
            }
            dataGridView1.DataSource = ds.Tables["Score1"];
            streamReader.Close();
            
        }

        private void проПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа разработана для работы с контроллером Arduino UNO, подключение при помощи порта COM3 и позволяет отображать и" +
 
               "сохранять на компьютер данные про температуру, освещение, влажность и состояние реле и движение ","Про программу");
        }

        private void проАвтораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 pro_avtora = new Form2();
            pro_avtora.Show();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.DtrEnable = true;
                serialPort1.Open();
                serialPort1.DataReceived += serialPort1_DataReceived;
                serialPort1.DataReceived += serialPort1_DataReceived1;
                serialPort1.DataReceived += serialPort1_DataReceived2;
                serialPort1.DataReceived += serialPort1_DataReceived3;
                serialPort1.DataReceived += serialPort1_DataReceived4;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

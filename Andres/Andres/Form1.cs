using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Andres
{
    public partial class Form1 : Form
    {

        /* ATRIBUTOS */
        bool encendido;
        string cadenatmp2;
        string Recibidos;
        // Este es una clase para comunicación con Arduino
        // mediante el puerto serial, esto es un objeto que
        // representa a dicho puerto
        System.IO.Ports.SerialPort ArduinoPort;

        public Form1()
        {
            InitializeComponent();

            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;

            try
            {
                serialPort1.Open();
                label1.Text = "Ami++";
            }
            catch (Exception error)
            {
                MessageBox.Show("Error mortal " + error.ToString(), "Oh no, oh no, oh no no no no no", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "Ami--";
            }

            /*
            // Crear el objeto para el serial port
            // Se hace la llamada al constructor...
            // Al declaralo, le indicamos al compilador que se
            // generará un objeto de dicho tipo
            // (System.IO.Ports.SerialPort), pero hasta aquí
            // ya se reserva la memoria
            ArduinoPort = new System.IO.Ports.SerialPort();
            // Ahora si, tenemos acceso a los stributos del objeto
            // que sirven para la configuración
            ArduinoPort.PortName = "COM3";
            ArduinoPort.BaudRate = 9600;
            try
            {
                // Y se abre la comunicación con el método correspondiente
                ArduinoPort.Open();
                if (ArduinoPort.IsOpen)
                {
                    ArduinoPort.Write("a");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error mortal " + error.ToString(), "Título", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            button1.BackColor = Color.Black;
            this.encendido = false;

            timer1.Enabled = false;

            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recepcion);
            //this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
        }

        private void Recepcion(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.cadenatmp2 = serialPort1.ReadExisting();

            // Acumulasización
            this.Recibidos += this.cadenatmp2;

            // Invocas un proceso por tramas
            this.Invoke(new EventHandler(Actualizar));
        }

        private void Actualizar(object s, EventArgs e)
        {
            textBox1.Text = Recibidos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.encendido)
            {
                // Apaga el botón
                button1.BackColor = Color.Black;
                this.encendido = false;
                //timer1.Enabled = false;
            }
            else
            {
                // prende el botón
                button1.BackColor = Color.Red;
                this.encendido = true;
                //timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("Hola Andrés", "El Andrés", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //textBox1.Text += "Hola Andrés";
            string a = ArduinoPort.ReadLine();
            textBox1.Text = a;

            string prueba = "21.98,34.67,666.0";

            string[] andres = prueba.Split(',');
            textBox1.Text += andres[0]; 

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            byte[] mBuffer = new byte[1];

            label2.Text = trackBar1.Value.ToString();
            mBuffer[0] = Convert.ToByte(trackBar1.Value);

            serialPort1.Write(mBuffer, 0, mBuffer.Length);
        }
    }
}

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
using OpenHardwareMonitor.Hardware;
using NvAPIWrapper.GPU;
using System.Timers;
using System.Management;

namespace CPU_GPU_TEMP_MINI_DISPLAY
{
    public partial class Form1 : Form
    {
        //public static Form1 form_access;
        public static SerialPort _SP = new SerialPort();
        public static System.Timers.Timer refresh_temps_timer;
        public static object my_form;

        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        public Form1()
        {
            //form_access = this;
            InitializeComponent();

            _SP.BaudRate = 115200;
            _SP.DataBits = 8;
            _SP.Parity = System.IO.Ports.Parity.None;
            _SP.Encoding = System.Text.Encoding.Default;
            _SP.StopBits = StopBits.One;

            // Populate a list of active serial ports:
            string[] COMports = SerialPort.GetPortNames();
            Array.Sort(COMports);

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                foreach (string s in portList)
                {
                    //Console.WriteLine(s);
                    cbSERIALPORTS.Items.Add(s);
                }

                foreach (string s in cbSERIALPORTS.Items)
                {
                    if(s.Contains("CP210x")) // find CP2102 interfacing microcontroller
                    {
                        cbSERIALPORTS.SelectedItem = s;
                        if (_SP.IsOpen) _SP.Close();
                        _SP.PortName = s.Substring(0, 4);
                        _SP.Open();
                        break;
                    }
                }
            }

            refresh_temps_timer = new System.Timers.Timer(2000);
            refresh_temps_timer.Elapsed += OnTimedEvent;
            refresh_temps_timer.AutoReset = true;
            refresh_temps_timer.Enabled = true;

            // auto connect to COM port named Silicon Labs CP210x
            
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if(_SP.IsOpen) pushFullInfoToSerial();
        }

        public static void pushFullInfoToSerial()
        {
            //Form1 form_access = new Form1();
            try
            {
                UpdateVisitor updateVisitor = new UpdateVisitor();
                Computer computer = new Computer();
                computer.Open();
                computer.CPUEnabled = true;
                computer.GPUEnabled = true;
                computer.Accept(updateVisitor);
                string full_serial_packet = "";
                string cpu_temp = "NA";
                string gpu_temp = "NA";
                int? num_of_cpus = 0;
                float? highest_cpu_temp = 0;
                float? current_cpu_temp = 0;

                int? num_of_gpus = 0;
                float? highest_gpu_temp = 0;
                float? current_gpu_temp = 0;

                //form_access.txtOUTPUT.AppendText("\r\n*** CPU: ***\r\n");

                for (int i = 0; i < computer.Hardware.Length; i++)
                {
                    if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {

                            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                num_of_cpus++;
                                //form_access.txtOUTPUT.AppendText($"{computer.Hardware[i].Sensors[j].Name} : {computer.Hardware[i].Sensors[j].Value.ToString()}\r\n");
                                current_cpu_temp = computer.Hardware[i].Sensors[j].Value;
                                if (current_cpu_temp > highest_cpu_temp) highest_cpu_temp = current_cpu_temp;
                            }
                        }
                    }
                }

                //form_access.txtOUTPUT.AppendText("\r\n*** GPU: ***\r\n");
                
                PhysicalGPU[] gpus = PhysicalGPU.GetPhysicalGPUs();
                foreach (PhysicalGPU gpu in gpus)
                {
                    num_of_gpus++;
                    //form_access.txtOUTPUT.AppendText($"gpu.FullName = {gpu.FullName}");
                    foreach (GPUThermalSensor sensor in gpu.ThermalInformation.ThermalSensors)
                    {
                        //form_access.txtOUTPUT.AppendText($" - Sensor ID#{sensor.SensorId.ToString()} Temp: {sensor.CurrentTemperature}\r\n");
                        current_gpu_temp = sensor.CurrentTemperature;
                        if(current_gpu_temp > highest_gpu_temp) highest_gpu_temp = current_gpu_temp;
                    }
                }

                //form_access.txtOUTPUT.AppendText($"\r\nHighest CPU Temp Overall: {highest_cpu_temp}");
                //form_access.txtOUTPUT.AppendText($"\r\nHighest GPU Temp Overall: {highest_gpu_temp}");

                //form_access.txtOUTPUT.AppendText($"\r\nNum CPUs: {num_of_cpus}");
                //form_access.txtOUTPUT.AppendText($"\r\nNum GPUs: {num_of_gpus}");

                cpu_temp = highest_cpu_temp.ToString();
                gpu_temp = highest_gpu_temp.ToString();

                if (cpu_temp.Length > 1 && highest_cpu_temp <= 99)
                {
                    cpu_temp = cpu_temp.Substring(0, 2);
                }
                else if (highest_cpu_temp > 99)
                {
                    cpu_temp = "!!";
                }
                else
                {
                    cpu_temp = "??";
                }

                if (gpu_temp.Length > 1 && highest_gpu_temp <= 99)
                {
                    gpu_temp = gpu_temp.Substring(0, 2);
                }
                else if (highest_gpu_temp > 99)
                {
                    gpu_temp = "!!";
                }
                else
                {
                    gpu_temp = "??";
                }

                //form_access.txtOUTPUT.AppendText($"\r\n2 digits of cpu_temp: {cpu_temp}");
                //form_access.txtOUTPUT.AppendText($"\r\n2 digits of cpu_temp: {gpu_temp}");

                full_serial_packet = $"<C{cpu_temp}G{gpu_temp}>";

                //form_access.txtOUTPUT.AppendText($"\r\nFull Serial Packet to Send: {full_serial_packet} \r\n");

                _SP.Write(full_serial_packet);

                computer.Close();
            }
            catch (Exception ex)
            {
                //form_access.txtOUTPUT.AppendText($"CANNOT READ CPU TEMP:\r\n{ex.Message}\r\n");
            }
        }

        private void cbSERIALPORTS_SelectedIndexChanged(object sender, EventArgs e)
        {
            string port = cbSERIALPORTS.SelectedItem.ToString();
            string short_port = port.Substring(0, 4);
            
            //Form1 form_access = new Form1();
            if (_SP.IsOpen) _SP.Close();

            _SP.PortName = short_port;
            try
            {
                _SP.Open();
            }
            catch
            {
                //form_access.txtOUTPUT.AppendText($"ERROR OPENING {_SP.PortName} Serial Communication Port.\r\n");
                return;
            }
            //form_access.txtOUTPUT.AppendText($"Connected to {_SP.PortName} Serial Communication Port.\r\n");
        }

        private void btnTOGGLESERIAL_Click(object sender, EventArgs e)
        {
            //Form1 form_access = new Form1();
            if (_SP.IsOpen)
            {
                _SP.Close();
                //form_access.txtOUTPUT.AppendText($"CLOSED {_SP.PortName} Serial Comm Port.\r\n");
            }
            else if (!_SP.IsOpen)
            {
                try
                {
                    _SP.Open();
                    //form_access.txtOUTPUT.AppendText($"OPENED {_SP.PortName} Serial Comm Port.\r\n");
                }
                catch (Exception ex)
                {
                    //form_access.txtOUTPUT.AppendText($"ERROR TOGGLING COM PORT:\r\n{ex.Message}\r\n");
                    return;
                }
            }
        }

        private void btnTESTDISPLAY_Click(object sender, EventArgs e)
        {
            //Form1 form_access = new Form1();
            string serial_data = txtTESTINPUT.Text.Trim();
            try
            {
                _SP.Write(serial_data);
            }
            catch (Exception ex)
            {
                //form_access.txtOUTPUT.AppendText($"COULD NOT SEND SERIAL DATA:\r\n{ex.Message}\r\n");
            }
        }

        private void btnFULLINFO_Click(object sender, EventArgs e)
        {
            pushFullInfoToSerial();
        }

        private void btnEXIT_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // hide the window to taskbar when minimized
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // restore the app from taskbar to normal window
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // start the app minimized
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }
    }
}

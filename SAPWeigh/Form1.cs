using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Utils;
using System.Threading;

namespace SAPWeigh
{
    public partial class Form1 : Form
    {
        //static string inbuff;
        static DbCon conn;
        static SerialPort serialPort1 = new SerialPort();

        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte ACK = 0x06;
        const byte NAK = 0x15;
        static ManualResetEvent terminateService = new ManualResetEvent(false);
        static readonly object eventLock = new object();
        static List<byte> unprocessedBuffer = null;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnToggle.Text = "Start";
            txtOutPut.Text = string.Empty;
            
            start();
            if (!conn.DebugFlg)
            {
                btnReload.Visible = false;
                btnToggle.Visible = false;
            }
            else
            {
                btnReload.Visible = true;
                btnToggle.Visible = true;
            }
            //inbuff = string.Empty;
        }

        
        /****
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            if(sp.BytesToRead > conn.InputBufferLen)
            {
                inbuff = sp.ReadLine();
            }
            else
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(conn.OutputPath))
            {
                string filepath = System.IO.Path.Combine(conn.OutputPath, "debug.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.Write(inbuff);
                }
            }
            //(?<=\[STX\])(?: (? !\[STX\]).)*? (?=\[ETX\])

            if (conn.STXETX)
            {
                var sepValues = inbuff.Split(new[] { '\u0002', '\u0003' }, StringSplitOptions.RemoveEmptyEntries);
                  
                foreach(string s in sepValues)
                {
                    if (conn.StartWT > 0 && conn.EndWT > 0 && conn.EndWT > conn.StartWT && inbuff.Length >= conn.StartWT + conn.EndWT)
                    {
                        try
                        {
                            string t = Regex.Replace(s.Substring(conn.StartWT, conn.EndWT), @"[^\u0000-\u007F]+", string.Empty);
                            if (conn.ReverseFlg)
                            {
                                
                                SetText(Utils.Helper.ReverseXor(t));
                            }
                            else
                            {
                                SetText(t);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor(s));
                            }
                            else
                            {
                                SetText(s);
                            }
                        }
                        
                    }
                    else
                    {
                        if (conn.ReverseFlg)
                        {
                            SetText(Utils.Helper.ReverseXor(s));
                        }
                        else
                        {
                            SetText(s);
                        }
                    }
                }//foreach
            }
            else
            {
                //consider inputbuffer len
                //split string by fixlength
                IEnumerable<string> sepValues = inbuff.Split(conn.InputBufferLen);
                if (sepValues.Count() > 0)
                {
                    foreach (string s1 in sepValues)
                    {
                        if (conn.StartWT > 0 && conn.EndWT > 0 && conn.EndWT > conn.StartWT && inbuff.Length >= conn.StartWT + conn.EndWT)
                        {
                            try
                            {
                                if (conn.ReverseFlg)
                                {
                                    SetText(Utils.Helper.ReverseXor(s1.Substring(conn.StartWT, conn.EndWT)));
                                }
                                else
                                {
                                    SetText(s1.Substring(conn.StartWT, conn.EndWT));
                                }
                                    
                            }
                            catch (Exception ex)
                            {
                                if (conn.ReverseFlg)
                                {
                                    SetText(Utils.Helper.ReverseXor(s1));
                                }
                                else
                                {
                                    SetText(s1);
                                }
                            }

                        }
                        else
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor(s1));
                            }
                            else
                            {
                                SetText(s1);
                            }
                                
                        }
                    }//for loop
                }
                else
                {
                    if (conn.StartWT > 0 && conn.EndWT > 0 && conn.EndWT > conn.StartWT && inbuff.Length >= conn.StartWT + conn.EndWT)
                    {
                        try
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor( inbuff.Substring(conn.StartWT, conn.EndWT)));
                            }
                            else
                            {
                                SetText(inbuff.Substring(conn.StartWT, conn.EndWT));
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor(inbuff));
                            }
                            else
                            {
                                SetText(inbuff);
                            }
                            
                        }
                    }
                    else
                    {
                        if (conn.ReverseFlg)
                        {
                            SetText(Utils.Helper.ReverseXor(inbuff));
                        }
                        else
                        {
                            SetText(inbuff);
                        }
                    }
                }
               
            }
            
            //sp.Dispose();
        }
        ***/

        private void start()
        {
            if (config())
            {
                unprocessedBuffer = null;
                //char a = '\u0003';
                serialPort1.PortName = conn.ComName;
                serialPort1.BaudRate = Convert.ToInt32(conn.Baudrate);
                serialPort1.Parity = (System.IO.Ports.Parity)System.Enum.Parse(typeof(System.IO.Ports.Parity), conn.Parity);

                serialPort1.DataBits = Convert.ToInt32(conn.Databit); ;
                serialPort1.DiscardNull = true;
                serialPort1.Handshake = (System.IO.Ports.Handshake)System.Enum.Parse(typeof(System.IO.Ports.Handshake), conn.HandShake);
                serialPort1.StopBits = (System.IO.Ports.StopBits)System.Enum.Parse(typeof(System.IO.Ports.StopBits), conn.Stopbit);
                serialPort1.Encoding = Encoding.UTF8;
                //serialPort1.NewLine = a.ToString();
                //serialPort1.ReadBufferSize = conn.InputBufferLen + 1;

                if (string.IsNullOrEmpty(conn.OutputPath))
                {
                    MessageBox.Show("Output Path is not defined", "Warninig", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (serialPort1.IsOpen)
                    serialPort1.Close();
                try
                {
                    serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    string t = serialPort1.ReadExisting();
                    
                    serialPort1.DataReceived += DataReceivedHandler;
                    serialPort1.ErrorReceived += ErrorReceivedHandler;

                    btnToggle.Text = "Stop";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Access to the port '" + conn.ComName + "' is denied or already in use..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WriteErr(ex,"ToggleSwitch");
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.DataReceived -= DataReceivedHandler;
                        serialPort1.ErrorReceived -= ErrorReceivedHandler;
                        serialPort1.Close();
                    }
                    btnToggle.Text = "Start";
                }
            }
        }

        public void SetText(string text)
        {
            
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.txtOutPut.InvokeRequired)
                {
                    //SetTextCallback d = new SetTextCallback(SetText);
                    //this.Invoke(d, new object[] {  text });
                    this.txtOutPut.Invoke((Action)(() => SetText(text)));
                }
                else
                {
                    txtOutPut.ResetText();
                    txtOutPut.Text = text;                    
                    txtOutPut.SelectAll();
                    txtOutPut.SelectionAlignment = HorizontalAlignment.Center;
                    txtOutPut.DeselectAll();
                    //txtOutPut.Refresh();
                    
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex,"DisplayOutput");
            }

        }

        private bool config()
        {

            conn = Helper.ReadConDb("DBCON");
            if(string.IsNullOrEmpty(conn.ComName))
            {
                frmConSetup b = new frmConSetup();
                b.typeofcon = "DBCON";
                b.ShowDialog();
                return false;
            }
            else
            {

                return true;
                
            }
                                  
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            btn_Close_Click(sender,e);
        }

        private void btnComConfig_Click(object sender, EventArgs e)
        {

            Form t = Application.OpenForms["frmConSetup"];

            if (t == null)
            {
                frmConSetup m = new frmConSetup();
                m.typeofcon = "DBCON";
                m.ShowDialog();
            }           
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            bool x = config();
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardInBuffer();
                //serialPort1.DataReceived -= serialPort1_DataReceived;
                serialPort1.DataReceived -= DataReceivedHandler;
                serialPort1.ErrorReceived -= ErrorReceivedHandler;

                serialPort1.Close();
                serialPort1.Dispose();
                btnToggle.Text = "Start";
            }
            else
            {
                btnToggle.Text = "Stop";
                start();
                
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            lock (eventLock)
            {
                byte[] buffer = new byte[4096];
                switch (e.EventType)
                {
                    case SerialData.Chars:
                        var port = (SerialPort)sender;
                        int bytesToRead = port.BytesToRead;
                        if (bytesToRead > buffer.Length)
                            Array.Resize(ref buffer, bytesToRead);
                        int bytesRead = port.Read(buffer, 0, bytesToRead);
                        ProcessBuffer(buffer, bytesRead);
                        break;
                    case SerialData.Eof:
                        terminateService.Set();
                        break;
                }
            }
        }
        private void ErrorReceivedHandler(object sender, SerialErrorReceivedEventArgs e)
        {
            lock (eventLock)
                if (e.EventType == SerialError.TXFull)
                {
                    Exception ex = new Exception("Error: TXFull. Can't handle this!");
                    WriteErr(ex, "Terminating Program");
                    terminateService.Set();
                }
                else
                {
                    Exception ex = new Exception(e.EventType.ToString());
                    WriteErr(ex,"Resetting everything");
                    var port = (SerialPort)sender;
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                    string x = port.ReadExisting();
                    unprocessedBuffer = null;
                    //port.Write(new byte[] { NAK }, 0, 1);
                }
        }

        private void ProcessBuffer(byte[] buffer, int length)
        {
            List<byte> message = unprocessedBuffer;
            if (conn.STXETX)
            {
                for (int i = 0; i < length; i++)
                {

                    if (buffer[i] == ETX)
                    {
                        if (message != null)
                        {
                            string buff = Encoding.UTF8.GetString(message.ToArray());

                            if (conn.DebugFlg)
                            {
                                if (!string.IsNullOrEmpty(conn.OutputPath))
                                {
                                    string filenm = "debug_STXETX_" + DateTime.Now.ToString("yyMMdd_HH") + ".txt";
                                    string filepath = System.IO.Path.Combine(conn.OutputPath, filenm);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true, Encoding.UTF8))
                                    {
                                        file.Write(buff);
                                    }
                                }
                            }


                            if (conn.StartWT > 0 && conn.EndWT > 0 && buff.Length > conn.StartWT && buff.Length >= conn.StartWT + conn.EndWT)
                            {
                               
                                if (conn.ReverseFlg)
                                {
                                   
                                    SetText(Utils.Helper.ReverseXor(buff.Substring(conn.StartWT, conn.EndWT)));
                                }
                                else
                                {
                                    SetText(buff.Substring(conn.StartWT, conn.EndWT));
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (conn.ReverseFlg)
                                    {
                                        SetText(Utils.Helper.ReverseXor(buff.Substring(conn.StartWT, conn.EndWT)));
                                    }
                                    else
                                    {
                                        SetText(buff.Substring(conn.StartWT, conn.EndWT));
                                    }
                                }
                                catch(Exception ex)
                                {
                                    WriteErr(ex,"STXETX-Invalid-StartWT-EndWt");
                                }
                                
                            }
                            message = null;
                        }
                    }
                    else if (buffer[i] == STX)
                    {
                        //message = null;
                        message = new List<byte>();
                    }
                    else if (message != null)
                        message.Add(buffer[i]);
                }
            }
            else
            {
                message = new List<byte>();
                //non stx-etx method
                for (int i = 0; i < length; i++)
                {
                    message.Add(buffer[i]);
                }

                string buff = Encoding.UTF8.GetString(message.ToArray());
                if (conn.DebugFlg)
                {
                    if (!string.IsNullOrEmpty(conn.OutputPath))
                    {
                        string filenm = "debug_Non_Pattern_" + DateTime.Now.ToString("yyMMdd_HH") + ".txt";
                        string filepath = System.IO.Path.Combine(conn.OutputPath, filenm);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                        {
                            file.Write(buff);
                        }
                    }
                }


               
                IEnumerable<string> sepValues = buff.Split(conn.InputBufferLen);
                if (sepValues.Count() > 0)
                {
                    foreach (string s1 in sepValues)
                    {
                        if (conn.StartWT > 0 && conn.EndWT > 0 && conn.EndWT > conn.StartWT && s1.Length >= conn.StartWT + conn.EndWT)
                        {
                            try
                            {
                               
                                if (conn.ReverseFlg)
                                {
                                    SetText(Utils.Helper.ReverseXor(s1.Substring(conn.StartWT, conn.EndWT)));
                                }
                                else
                                {                                    
                                    SetText(s1.Substring(conn.StartWT, conn.EndWT));
                                }

                            }
                            catch (Exception ex)
                            {

                                WriteErr(ex,"NONSTXETX->FixSplit1");
                            }
                        }
                        
                    }//for loop
                }
                else
                {
                    if (conn.StartWT > 0 && conn.EndWT > 0 && conn.EndWT > conn.StartWT && buff.Length >= conn.StartWT + conn.EndWT)
                    {

                        
                        try
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor(buff.Substring(conn.StartWT, conn.EndWT)));
                            }
                            else
                            {
                                SetText(buff.Substring(conn.StartWT, conn.EndWT));
                            }

                        }
                        catch (Exception ex)
                        {
                            WriteErr(ex, "NONSTXETX->FixSplit2->Else");

                        }
                    }
                    else
                    {
                        try
                        {
                            if (conn.ReverseFlg)
                            {
                                SetText(Utils.Helper.ReverseXor(buff));
                            }
                            else
                            {
                                SetText(buff);
                            }
                        }
                        catch(Exception ex)
                        {
                            WriteErr(ex, "NONSTXETX->FixSplit3->Else");
                        }
                        
                    }
                }
            }

            unprocessedBuffer = message;
        }

        private void WriteErr(Exception ex, string fn)
        {
            if (!string.IsNullOrEmpty(conn.OutputPath))
            {
                string filenm = "ErrorLog_" + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt";
                string filepath = System.IO.Path.Combine(conn.OutputPath, filenm);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-->" + fn  + "-->" + ex.Message);
                }
            }
        }

        private void close()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    //serialPort1.DataReceived -= DataReceivedHandler;
                    //serialPort1.ErrorReceived -= ErrorReceivedHandler;
                    serialPort1.Close();
                    //serialPort1.Dispose();
                }
                
            }
            catch (Exception ex)
            {
                WriteErr(ex, "FileClosing_Event");
            }
            finally{
                
                if (!string.IsNullOrEmpty(conn.OutputPath))
                {
                    string filepath = System.IO.Path.Combine(conn.OutputPath, "WB.txt");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, false))
                    {
                        //try to convert double
                        double wt = 0;

                        bool result = double.TryParse(txtOutPut.Text.ToString(), out wt);
                        if (result)
                        {
                            file.WriteLine(wt.ToString());
                        }
                        else
                        {
                            file.WriteLine(txtOutPut.Text.ToString());
                        }

                    }
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            close();
            this.close();
            System.Windows.Forms.Application.Exit();
        }
    }


    
}
public static class Extensions
{
    public static IEnumerable<string> Split(this string str, int n)
    {
        if (String.IsNullOrEmpty(str) || n < 1)
        {
            throw new ArgumentException();
        }

        for (int i = 0; i < str.Length; i += n)
        {
            yield return str.Substring(i, Math.Min(n, str.Length - i));
        }
    }
}
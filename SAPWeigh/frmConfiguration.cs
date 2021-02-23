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


namespace SAPWeigh
{
    public partial class frmConSetup : Form
    {
        private Utils.DbCon dbcon = new Utils.DbCon();
        public string typeofcon;
        //static SerialPort _serialPort;
        public frmConSetup()
        {
            InitializeComponent();
        }

        private void frmConSetup_Load(object sender, EventArgs e)
        {
            // _serialPort = new SerialPort();
            this.cmbComport.Items.Clear();
            this.cmbHandShake.Items.Clear();
            this.cmbStopbit.Items.Clear();
            this.cmbParity.Items.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {
                this.cmbComport.Items.Add(s);
            }

            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                this.cmbHandShake.Items.Add(s);
            }


            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                this.cmbStopbit.Items.Add(s);
            }

            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                this.cmbParity.Items.Add(s);
            }


            dbcon = Utils.Helper.ReadConDb(typeofcon);
            
            cmbComport.Text = dbcon.ComName;
            cmbBuadrate.Text = dbcon.Baudrate;
            cmbDatabits.Text = dbcon.Databit;
            cmbHandShake.Text = dbcon.HandShake;
            cmbParity.Text = dbcon.Parity;
            cmbStopbit.Text = dbcon.Stopbit;
            chkSTXETX.Checked = dbcon.STXETX;
            txtStartWT.Text = dbcon.StartWT.ToString();
            txtEndWT.Text = dbcon.EndWT.ToString();
            txtInputBufferLen.Text = dbcon.InputBufferLen.ToString();
            txtOutputPath.Text = dbcon.OutputPath.ToString();
            chkReverseFlg.Checked = dbcon.ReverseFlg;
            chkDebug.Checked = dbcon.DebugFlg;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dbcon.ComName = cmbComport.SelectedItem.ToString();
            dbcon.Baudrate = cmbBuadrate.SelectedItem.ToString();
            dbcon.Databit = cmbDatabits.SelectedItem.ToString();
            dbcon.Parity = cmbParity.SelectedItem.ToString();
            dbcon.Stopbit = cmbStopbit.SelectedItem.ToString();
            dbcon.HandShake = cmbHandShake.SelectedItem.ToString();
            dbcon.InputBufferLen = (string.IsNullOrEmpty(txtInputBufferLen.Text) ? 0 : Convert.ToInt32(txtInputBufferLen.Text.ToString()));
            dbcon.STXETX = chkSTXETX.Checked;
            dbcon.StartWT = (string.IsNullOrEmpty(txtStartWT.Text)?0:Convert.ToInt32(txtStartWT.Text.ToString()));
            dbcon.EndWT = (string.IsNullOrEmpty(txtEndWT.Text) ? 0 : Convert.ToInt32(txtEndWT.Text.ToString()));
            dbcon.OutputPath = txtOutputPath.Text.ToString();
            dbcon.ReverseFlg = chkReverseFlg.Checked;
            dbcon.DebugFlg = chkDebug.Checked;

            Utils.Helper.WriteConDb(dbcon, typeofcon);
            this.Close();
        }
        private void BrowseFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputPath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
        private string DataValidate()
        {
            string err = string.Empty;

            if (cmbComport.SelectedItem == null)
                err += "Comport Name is Required.." + Environment.NewLine;

            if (cmbBuadrate.SelectedItem == null)
                err += "Baudrate is Required.." + Environment.NewLine;

            if (cmbDatabits.SelectedItem == null)
                err += "Databit is Required.." + Environment.NewLine;

            if (cmbParity.SelectedItem == null)
                err += "Parity is Required.." + Environment.NewLine;

            if (cmbStopbit.SelectedItem == null)
                err += "Stopbit is Required.." + Environment.NewLine;

            if (cmbHandShake.SelectedItem == null)
                err += "Handshake is Required.." + Environment.NewLine;

            if(string.IsNullOrEmpty(txtOutputPath.Text.ToString()))
                err += "Output folder path is Required.." + Environment.NewLine;
            return err;
        }
    }
}

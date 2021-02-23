
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Utils
{


    class Helper
    {
        //public static string DbConstr = Properties.Settings.Default.dbConn.ToString();
        public static string confile = "connection.xml";
        
      
        static public string GetUserDataPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "SAPWeight");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="imageToConvert"></param>
        /// <param name="formatOfImage"></param>
        /// <returns></returns>
        public byte[] ConvertImageToBytes(System.Drawing.Image imageToConvert,
                                         System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }
        /// <summary>
        /// Convert Byte[] to image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public Image ConvertBytesToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }


        public static bool WriteConDb(DbCon cnstr, string typeofcon)
        {
            string filepath = string.Empty;
            switch (typeofcon.ToUpper())
            {
                case "DBCON":
                    filepath = System.IO.Path.Combine(GetUserDataPath(), confile);
                    break;
                
            }


            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(DbCon));

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(filepath);
                writer.Serialize(file, cnstr);
                file.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        public static DbCon ReadConDb(string typeofcon)
        {


            string filepath = string.Empty;
            switch (typeofcon.ToUpper())
            {
                case "DBCON":
                    filepath = System.IO.Path.Combine(GetUserDataPath(), confile);
                    break;
               
                default:
                    filepath = System.IO.Path.Combine(GetUserDataPath(), confile);
                    break;
            }


            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(DbCon));

            try
            {
                // Now we can read the serialized book ...
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(DbCon));
                System.IO.StreamReader file = new System.IO.StreamReader(
                    filepath);
                DbCon overview = (DbCon)reader.Deserialize(file);
                file.Close();

                return overview;
            }
            catch (Exception ex)
            {
                
                DbCon blank = new DbCon();
                return blank;
            }

        }

        public static string ReverseXor(string s)
        {
            char[] charArray = s.ToCharArray();
            int len = s.Length - 1;

            for (int i = 0; i < len; i++, len--)
            {
                charArray[i] ^= charArray[len];
                charArray[len] ^= charArray[i];
                charArray[i] ^= charArray[len];
            }

            return new string(charArray);
        }

    }

    public class DbCon
    {
        private string _ComName;
        private string _Databit;
        private string _Parity;
        private string _StopBit;
        private string _Baudrate;
        private string _HandShake;
        private int _inputBufferLen;
        private bool _STXETX;
        private bool _ReverseFlg;
        private int _StartWT;
        private int _EndWT;
        private string _OutputPath;
        private bool _DebugFlg;

        //Data Source=172.16.12.14;Initial Catalog=Dia;User ID=ipu_dia

        public bool STXETX
        {
            get { return _STXETX; }
            set { _STXETX = value; }
        }

        public bool DebugFlg
        {
            get { return _DebugFlg; }
            set { _DebugFlg = value; }
        }

        public bool ReverseFlg
        {
            get { return _ReverseFlg; }
            set { _ReverseFlg = value; }
        }

        public int StartWT
        {
            get { return _StartWT; }
            set { _StartWT = value; }
        }

        public int EndWT
        {
            get { return _EndWT; }
            set { _EndWT = value; }
        }

        public string ComName
        {
            get { return _ComName; }
            set { _ComName = value; }
        }

        public string OutputPath
        {
            get { return _OutputPath; }
            set { _OutputPath = value; }
        }

        public string Databit
        {
            get { return _Databit; }
            set { _Databit = value; }
        }

        public int InputBufferLen
        {
            get { return _inputBufferLen; }
            set { _inputBufferLen = value; }
        }

        public string Parity
        {
            get { return _Parity; }
            set { _Parity = value;}
        }

        public string Stopbit
        {
            get { return _StopBit; }
            set { _StopBit = value; }
        }

        public string Baudrate
        {
            get { return _Baudrate; }
            set { _Baudrate = value; }
        }

        public string HandShake
        {
            get { return _HandShake; }
            set { _HandShake = value; }
        }



        public DbCon()
        {
            _ComName = "";
            _Databit = "";
            _Parity = "";
            _StopBit = "";
            _Baudrate = "";
            _HandShake = "";
            _STXETX = false;
            _OutputPath = "";
            _StartWT = 0;
            _EndWT = 0;
            _inputBufferLen = 0;
            _ReverseFlg = false;

            this.ComName = _ComName;
            this.Databit = _Databit;
            this.Parity= _Parity;
            this.Stopbit = _StopBit;
            this.Baudrate = _Baudrate;
            this.HandShake = _HandShake;
            this.STXETX = _STXETX;
            this.OutputPath = _OutputPath;
            this.StartWT = _StartWT;
            this.EndWT = _EndWT;
            this.InputBufferLen = _inputBufferLen;
            this.ReverseFlg = _ReverseFlg;

        }

       
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Linq;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label7.Text = "100%";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar1.TickStyle = TickStyle.BottomRight;
            trackBar1.TickFrequency = 10;
            trackBar1.Value = 100;
           


        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label7.Text = trackBar1.Value.ToString() + "%";
            try
            {
                bitm(trackBar1.Value);
            }catch(Exception r)
            {

            }

            


        }


        private Bitmap bitm(int quality)
        {
            string[] files;

            if (checkBox1.Checked)
            {

                files = Directory.GetFiles(srcbox.Text);
                files[0] = prev.Text;
            }
            else
            {
                files = Directory.GetFiles(srcbox.Text);
            }

            Bitmap b = null;
           


            string ext = Path.GetExtension(files[0]).ToUpper();
            if (ext == ".PNG" || ext == ".JPG")
            {

                var FileName = Path.GetFileName(files[0]);



                using (Bitmap bmp1 = new Bitmap(files[0]))
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);

                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                       
                        bmp1.Save(ms, jpgEncoder, myEncoderParameters);
                        Bitmap resized = new Bitmap(bmp1, new Size(pictureBox1.Width,pictureBox1.Height));
                        pictureBox1.Image = resized;

                    }
                }
            }
            return b;

        }



        private void OpenFolderDialog(TextBox Filepath)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;

            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                Filepath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFolderDialog(srcbox);

                string[] files = Directory.GetFiles(srcbox.Text);
                ArrayList myArrayList = new ArrayList();

                foreach (var item in files)
                {
                    myArrayList.Add(Path.GetFileName(item));
                }
                files1.DataSource = myArrayList;
            }catch(Exception r)
            {

            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFolderDialog(dextexbox);
        }


        public static void CompressImage(string SoucePath, string DestPath, int quality, String prefix)
        {
            var FileName = Path.GetFileName(SoucePath);
            

            DestPath = DestPath + "\\" +prefix+FileName;

          

            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);

                myEncoderParameters.Param[0] = myEncoderParameter;

                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);

            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void Button4_Click(object sender, EventArgs e)
        {

            string[] files = Directory.GetFiles(srcbox.Text);
            int amount = files.Length+1;
            int count = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = amount;
            string[] excfiles = new string[files1.CheckedItems.Count + 1];


            // Determine if there are any items checked.  
            if (files1.CheckedItems.Count != 0)
            {
                // If so, loop through all checked items and print results.  
                string s = "";
                for (int x = 0; x < files1.CheckedItems.Count; x++)
                {
                    excfiles[x] = srcbox.Text+@"\"+files1.CheckedItems[x].ToString();
                }

            }


            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToUpper();
                if (ext == ".PNG" || ext == ".JPG"&& !excfiles.Contains(file))
                    CompressImage(file, dextexbox.Text,trackBar1.Value, pretext.Text.ToString());
                count++;

                progressBar1.Value = count+1;
            }



            MessageBox.Show("Compressed Images has been stored to\n" + dextexbox.Text);
            dextexbox.Text = "";
            srcbox.Text = "";

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                lab.Visible = true;
                but.Visible = true;
                prev.Visible = true;
            }
            else
            {
                lab.Visible = false;
                but.Visible = false;
                prev.Visible = false;
            }
        }

        private void But_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "C# Corner Open File Dialog";
            fdlg.InitialDirectory = srcbox.Text;
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                prev.Text = fdlg.FileName;
            }
        }

        private void Prev_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

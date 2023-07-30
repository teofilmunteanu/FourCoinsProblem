using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace ComplexExplorer
{
    public partial class ComplexForm : Form
    {
        private static AboutForm aboutForm = new AboutForm();
        private Graphics myDC;  // my device context
        private Bitmap myBmp;   // my bitmap
        private SolidBrush myBrush;

        public const int latBmp = 600;      // latura bitmapului    
        public readonly List<Pixel> bmpPixels = new List<Pixel>() { };
        public const int dimPal = 1024;     // numarul de culori in paleta
        private Color[] paleta = new Color[dimPal];

        public Color PenColor { get; set; }     // culoarea scrisului
        public Color ScreenColor { get; set; }  // culoarea fondului

        private bool isWorking = false;  // pentru oprirea fortata  a desenarii

        public double input1, input2, input3, input4, input5, input6;
        public double Raza;

        public ComplexForm()
        {
            InitializeComponent();
            this.Height = latBmp + 71;
            this.Width = latBmp + 31;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(latBmp + 25, latBmp + 39);
            this.picBox.Location = new System.Drawing.Point(12, 26);
            this.picBox.Size = new System.Drawing.Size(latBmp + 1, latBmp + 1);
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Size = new System.Drawing.Size(latBmp + 25, 24);
            this.KeyPreview = true;
            this.startToolStripMenuItem.Enabled = true;
            this.stopToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = false;

            //in vectorul paleta stocam dimPal culori de pe o curba inchisa din spatiul RGB, 
            //fixata in mod convenabil
            for (int k = 0; k < dimPal; k++)
            {
                double tcol = 56.123 + 2.0 * Math.PI * k / (double)dimPal;
                int rcol = (int)(128 + 128 * Math.Sin(tcol));
                int gcol = (int)(128 + 128 * Math.Sin(2 * tcol));
                int bcol = (int)(128 + 128 * Math.Cos(3 * tcol));
                paleta[k] = Color.FromArgb(rcol % 256, gcol % 256, bcol % 256);
            }

            //in lista bmpPixels stocam toti Pixel-ii corespunzatori PictureBox-ului picBox

            for (int ii = 0; ii < latBmp; ii++)
            {
                for (int jj = 0; jj < latBmp; jj++)
                {
                    bmpPixels.Add(new Pixel(ii, jj));
                }
            }

            ScreenColor = Color.MidnightBlue;
            PenColor = Color.White;

            writeTitleBar("");
        }

        private void ComplexForm_Load(object sender, EventArgs e)
        {
            myBmp = new Bitmap(latBmp, latBmp, PixelFormat.Format24bppRgb);
            myBmp.SetResolution(300F, 300F);
            picBox.Image = myBmp;
            myDC = Graphics.FromImage(myBmp);
            myBrush = new SolidBrush(ScreenColor);
            myDC.FillRectangle(myBrush, 0, 0, latBmp, latBmp);
        }

        private void ComplexForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isWorking = false;   //esential: ca sa stopam desenarea 
        }

        public virtual void makeImage()     //trebuie suprascrisa      
        {
            setText("Metoda makeImage()", "trebuie  suprascris\u0103!");
        }

        public void setText(params string[] mesaj)
        {
            myBrush.Color = PenColor;
            using (Font font = new Font("Arial", 3.2f))
            {
                for (int k = 0; k < mesaj.Length; k++)
                    myDC.DrawString(mesaj[k], font, myBrush, 0.0F, k * 15.0F);
            }
        }
        public void setTextAt(Complex z, string msg)
        {
            myBrush.Color = PenColor;
            using (Font font = new Font("Arial", 3.2f))
            {
                myDC.DrawString(msg, font, myBrush, getI(z.Re), jmax - getJ(z.Im));
            }
        }
        public void initScreen()
        {
            myBrush.Color = ScreenColor;
            myDC.FillRectangle(myBrush, 0, 0, latBmp, latBmp);
            picBox.Invalidate();
        }

        public bool resetScreen()
        {
            picBox.Refresh();           //pentru re-pictarea ferestrei
            Application.DoEvents();     //pentru procesarea mesajelor
            return isWorking;           //pentru semnalizarea opririi desenarii
        }


        private void writeTitleBar(string msg)
        {
            this.Text = this.GetType().ToString();
            if (msg != "") this.Text += " (" + msg + ")";
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                input1 = Convert.ToDouble(textBox1.Text);
                input2 = Convert.ToDouble(textBox2.Text);
                input3 = Convert.ToDouble(textBox3.Text);
                input4 = Convert.ToDouble(textBox4.Text);
                input5 = Convert.ToDouble(textBox5.Text);
                input6 = Convert.ToDouble(textBox6.Text);
                Raza = Convert.ToDouble(textBox7.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            this.startToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Enabled = true;
            this.saveToolStripMenuItem.Enabled = false;
            initScreen();
            isWorking = true;
            writeTitleBar("in lucru");
            makeImage();
            if (isWorking) writeTitleBar("complet");
            else writeTitleBar("oprit");
            isWorking = false;
            this.startToolStripMenuItem.Enabled = true;
            this.stopToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isWorking = false;
            this.startToolStripMenuItem.Enabled = true;
            this.stopToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isWorking = false;
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myBmp == null) return;

            mySaveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png";


            if (mySaveFileDialog.ShowDialog() != DialogResult.OK) return;
            string numeFisier = mySaveFileDialog.FileName;
            if (numeFisier.Length > 0)
            {
                switch (mySaveFileDialog.FilterIndex)
                {
                    case 1:
                        numeFisier = System.IO.Path.ChangeExtension(numeFisier, ".bmp");
                        myBmp.Save(numeFisier, ImageFormat.Bmp);
                        break;
                    case 2:
                        numeFisier = System.IO.Path.ChangeExtension(numeFisier, ".jpg");
                        myBmp.Save(numeFisier, ImageFormat.Jpeg);
                        break;
                    case 3:
                        numeFisier = System.IO.Path.ChangeExtension(numeFisier, ".png");
                        myBmp.Save(numeFisier, ImageFormat.Png);
                        break;
                }
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutForm.ShowDialog();
        }

        private void ComplexForm_KeyPress(object sender, KeyPressEventArgs e)
        {                // tasta <space> lanseaza/opreste desenarea
            if (e.KeyChar != ' ') return;
            e.Handled = true;
            if (!isWorking) startToolStripMenuItem_Click(null, null);
            else stopToolStripMenuItem_Click(null, null);

        }
        public void delaySec(double s)
        {
            if (s <= 0) return;
            if (s > 10) s = 10;
            Thread.Sleep((int)(1000 * s));
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

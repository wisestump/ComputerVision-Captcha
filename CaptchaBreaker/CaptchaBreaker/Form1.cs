using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptchaBreaker
{
    public partial class Form1 : Form
    {
        Image image;
        string openedImage;
        InfoForm infoForm;
        public Form1()
        {
            InitializeComponent();
            infoForm = new InfoForm();
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
            {
                OpenImage(files[0]);
            }
        }

        void OpenImage(string file)
        {
            try
            {
                Image im = Bitmap.FromFile(file);
                if (image != null)
                    image.Dispose();
                image = im;
                pictureBox1.Image = image;
                openedImage = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error opening file: {0}", ex.Message));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenImage(openFileDialog1.FileName);
                var classifier = new Classifier("../../../Classifier/network", "../../../Classifier");
                var classifierOut = classifier.Classify(openFileDialog1.FileName);
                var outLines = classifierOut.Split(new [] {"\r", "\r\n", "\n"},
                    StringSplitOptions.RemoveEmptyEntries);
                label1.Text = outLines[0];
                infoForm.textBox1.Text = string.Concat(outLines.Skip(1));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!infoForm.Visible)
                infoForm.Show();
        }

        private void Classify()
        {
            // Do classification and show results
        }
    }
}

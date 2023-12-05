using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFARoboticDrawing
{
    public partial class Form1 : Form
    {

        private PictureBox originalPictureBox;
        private PictureBox convertedPictureBox;
        private Button loadButton;
        private Button convertButton;
        public Form1()
        {
            originalPictureBox=new PictureBox();
            convertedPictureBox=new PictureBox();
            loadButton=new Button();
            convertButton=new Button();

            loadButton.Click += new EventHandler(loadButton_Click);
            convertButton.Click += new EventHandler(convertButton_Click);

            Controls.Add(originalPictureBox);
            Controls.Add(convertedPictureBox);
            Controls.Add(loadButton);
            Controls.Add(convertButton);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog=new OpenFileDialog();
            openFileDialog.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
             Image image=LoadImage(openFileDialog.FileName);
             originalPictureBox.Image= image;
             originalPictureBox.SizeMode=PictureBoxSizeMode.StretchImage;
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            if (originalPictureBox.Image!=null)
            {
                Image convertedImage=Convert(originalPictureBox.Image);
                Image drawingImage=ConvertToDrawing(convertedImage);
                convertedPictureBox.Image= drawingImage;
                convertedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Please load an image first.");
            }
        }

        private Image LoadImage(string path)
        {
            return Image.FromFile(path);
        }

        private Image Convert(Image originalImage)
        {
            Bitmap convertedImage = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g=Graphics.FromImage(convertedImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[]{.3f,.3f,.3f,0,0},
                    new float[]{.59f,.59f,.59f,0,0},
                    new float[]{.11f,.11f,.11f,0,0},
                    new float[]{0,0,0,1,0},
                    new float[]{0,0,0,0,1}
                });

                using (ImageAttributes attributes=new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(originalImage, new Rectangle(0,0,originalImage.Width,originalImage.Height),0,0,originalImage.Width,originalImage.Height,GraphicsUnit.Pixel,attributes);
                }
            }
            return convertedImage;
        }

        private Image ConvertToDrawing(Image convertedImage)
        {
            Bitmap drawingImage = new Bitmap(convertedImage.Width, convertedImage.Height);

            using (Graphics g=Graphics.FromImage(drawingImage))
            {
                g.DrawImage(convertedImage,0,0);
            }

            return drawingImage;
        }
    }
}

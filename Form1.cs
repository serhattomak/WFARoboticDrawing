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
            // Bitmap nesnesine dönüştürmek için önce Image tipini Bitmap tipine çeviriyoruz.
            Bitmap originalBitmap = new Bitmap(originalImage);
            Bitmap convertedImage = new Bitmap(originalImage.Width, originalImage.Height);

            int threshold = 128; // Eşik değeri, bu değer ayarlanabilir.

            for (int i = 0; i < originalBitmap.Width; i++)
            {
                for (int j = 0; j < originalBitmap.Height; j++)
                {
                    Color originalColor = originalBitmap.GetPixel(i, j);
                    // Gri tonlama hesaplaması yapıyoruz.
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                    // Eşik değerine göre siyah veya beyaz renk belirliyoruz.
                    Color bwColor = grayScale < threshold ? Color.Black : Color.White;
                    convertedImage.SetPixel(i, j, bwColor);
                }
            }

            return convertedImage;
        }

        private Image ConvertToDrawing(Image convertedImage)
        {
            // Dönüştürülen görüntüyü Bitmap'e çeviriyoruz.
            Bitmap sourceBitmap = new Bitmap(convertedImage);
            // Yeni bir çizim için boş bir Bitmap oluşturuyoruz. 
            // Boyutları kaynak görüntü ile aynı olacak.
            Bitmap drawingImage = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            using (Graphics g = Graphics.FromImage(drawingImage))
            {
                // Arka planı beyaz yapalım.
                g.Clear(Color.White);

                // Siyah pikselleri çizmek için bir kalem oluşturuyoruz.
                Pen blackPen = new Pen(Color.Black);

                for (int i = 0; i < sourceBitmap.Width; i++)
                {
                    for (int j = 0; j < sourceBitmap.Height; j++)
                    {
                        // Eğer piksel siyahsa, o noktada bir çizim yap.
                        if (sourceBitmap.GetPixel(i, j).R == 0) // Siyah pikseller için Kırmızı kanalı 0'dır.
                        {
                            g.DrawRectangle(blackPen, i, j, 1, 1); // Pikseli çiz
                        }
                    }
                }
            }

            return drawingImage;
        }
    }
}

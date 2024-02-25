using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
using System.IO;

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
            InitializeComponent(); // Kontrollerin ayarlanması InitializeComponent içinde yapılmalı.

            // Kontrollerin ayarlanması ve olay işleyicilerinin atanması.
            InitializeControls();
        }
        private void InitializeControls()
        {

            loadButton.Click += new EventHandler(loadButton_Click);
            processButton.Click += new EventHandler(processButton_Click);

            this.Controls.Add(originalPictureBox);
            this.Controls.Add(convertedPictureBox);
            this.Controls.Add(loadButton);
            this.Controls.Add(processButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files | *.jpg;*.jpeg;*.png;*.bmp;*.gif" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalPictureBox.Image = Image.FromFile(openFileDialog.FileName);
                originalPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            if (originalPictureBox.Image != null)
            {
                Bitmap image = new Bitmap(originalPictureBox.Image);
                Image convertedImage = ConvertToBlackAndWhite(image);
                Image drawingImage = ConvertToDrawing(convertedImage);
                convertedPictureBox.Image = drawingImage;
                convertedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Please load an image first.");
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            if (originalPictureBox.Image == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            // Görüntüyü işleme ve çıktı dosyası adını belirleme kısmı
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                DefaultExt = "txt",
                AddExtension = true,
                Title = "Save Robot Commands As"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(originalPictureBox.Image);
                Bitmap convertedImage = ConvertToBlackAndWhite(image); // Dönüşüm fonksiyonunuzu çağırın
                List<Point> pathPoints = FindPathPoints(convertedImage); // Yolu bulun
                WriteRobotCommandsToFile(pathPoints, saveFileDialog.FileName); // Komutları dosyaya yazın
            }
        }

        private Image LoadImage(string path)
        {
            return Image.FromFile(path);
        }

        private Bitmap ConvertToBlackAndWhite(Bitmap originalBitmap)
        {
            // Bitmap nesnesine dönüştürmek için önce Image tipini Bitmap tipine çeviriyoruz.
            Bitmap convertedImage = new Bitmap(originalBitmap.Width, originalBitmap.Height);

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

        public List<Point> FindPathPoints(Bitmap image)
        {
            List<Point> pathPoints = new List<Point>();

            // Görüntüdeki her pikseli tara
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    // Pikselin rengini al
                    Color pixelColor = image.GetPixel(x, y);

                    // Eğer piksel siyah ise (veya belirli bir eşik değerine göre koyu ise)
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                    {
                        // Bu pikselin koordinatını listeye ekle
                        pathPoints.Add(new Point(x, y));
                    }
                }
            }

            return pathPoints;
        }

        public void GenerateRobotCommands(List<Point> pathPoints)
        {
            foreach (Point point in pathPoints)
            {
                // Burada, her bir nokta için robotun anlayabileceği komutlar oluşacak.
                // Örneğin: MoveL(x, y) şeklinde bir komut olabilir.
                // Bu, robotun x, y koordinatına lineer hareket etmesi gerektiğini belirtir.
                Console.WriteLine($"MoveL({point.X}, {point.Y});");
            }
        }

        public void WriteRobotCommandsToFile(List<Point> pathPoints, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Dosya başlığı veya giriş komutları
                writer.WriteLine("BEGIN");

                foreach (Point point in pathPoints)
                {
                    // Nokta koordinatlarını robotun anlayabileceği bir komuta dönüştür
                    // Örnek: Staubli TX90L için komut formatı. Bu format robotunuza göre değişiklik gösterebilir.
                    string command = $"MoveL {{X: {point.X}, Y: {point.Y}, Z: 0, Rx: 0, Ry: 0, Rz: 0}};";
                    writer.WriteLine(command);
                }

                // Dosya sonu veya çıkış komutları
                writer.WriteLine("END");
            }
        }

        //private void processButton_Click(object sender, EventArgs e)
        //{
        //    if (originalPictureBox.Image == null)
        //    {
        //        MessageBox.Show("Please load an image first.");
        //        return;
        //    }

        //    Bitmap image = new Bitmap(originalPictureBox.Image);
        //    Bitmap convertedImage = ConvertToBlackAndWhite(image);
        //    convertedPictureBox.Image = convertedImage;
        //    convertedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        //    List<Point> pathPoints = FindPathPoints(convertedImage);
        //    string filePath = @"C:\\Users\\serha\\Desktop\\IUC\\Bitirme Projesi\deneme.txt"; // Komut dosyasının kaydedileceği yol
        //    WriteRobotCommandsToFile(pathPoints, filePath);
        //}
        
    }
}

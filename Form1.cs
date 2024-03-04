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
using System.Numerics;

namespace WFARoboticDrawing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Butonlar
        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Image Files | *.jpg; *.jpeg; *.png; *.bmp; *.gif" };
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
                Bitmap convertedImage = ConvertToBlackAndWhite(image);
                convertedPictureBox.Image = convertedImage;
                convertedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                List<Point> pathPoints = FindPathPoints(convertedImage);

                //// Yol noktalarını basitleştir
                //double tolerance = 2.0; // Basitleştirme toleransı, ihtiyacınıza göre ayarlayabilirsiniz
                //List<Point> simplifiedPath = DouglasPeucker.Simplify(pathPoints, tolerance);

                //// Koordinatları dönüştür (sol alt köşeyi 0,0 olarak kabul et)
                //List<Point> transformedPath = TransformPoints(simplifiedPath, image.Height);

                //// Robot komut dosyasını yaz
                //WriteRobotCommandsToFile(transformedPath, saveFileDialog.FileName);

                //// Optimize the path for directional consistency
                //List<Point> optimizedPath = OptimizePathForDirectionConsistency(pathPoints);

                //// Generate commands based on the optimized path and angle threshold
                //double angleThreshold = 10.0; // Define your angle threshold here
                //List<string> commands = GenerateOptimizedPathCommands(optimizedPath, angleThreshold);

                //// Write the commands to the file
                //WriteRobotCommandsToFile(commands, saveFileDialog.FileName);

                // 2

                // Simplify the path points based on neighborhood relations
                List<Point> simplifiedPath = SimplifyPathBasedOnNeighborhood(pathPoints, 20.0);

                // Optimize the path for direction consistency
                List<Point> optimizedPath = OptimizePathForDirectionConsistency(simplifiedPath);

                //// Transform the coordinates
                //List<Point> transformedPath = TransformCoordinates(optimizedPath, convertedImage.Height);

                //// Generate commands based on the optimized path
                //List<string> commands = GenerateOptimizedPathCommands(transformedPath, 10.0); // Adjust the angle threshold as needed

                //// Write the commands to the file
                //WriteRobotCommandsToFile(commands, saveFileDialog.FileName);

                // Identify and simplify curves in the path
                double angleThreshold = 10.0; // Adjust based on your needs
                double curveSimplificationTolerance = 2.0; // Adjust based on your needs
                List<Point> curveOptimizedPath = IdentifyAndSimplifyCurves(optimizedPath, angleThreshold, curveSimplificationTolerance);

                // Transform the coordinates
                List<Point> transformedPath = TransformCoordinates(curveOptimizedPath, convertedImage.Height);

                // Generate commands based on the optimized path
                List<string> commands = GenerateOptimizedPathCommands(transformedPath, 10.0); // Adjust the angle threshold as needed

                // Write the commands to the file
                WriteRobotCommandsToFile(commands, saveFileDialog.FileName);
            }
        }
        #endregion

        #region Siyah - Beyaz Dönüşümü
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
        #endregion

        #region Dönüştürme Metotları
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

        private List<Point> TransformCoordinates(List<Point> pathPoints, int imageHeight)
        {
            List<Point> transformedPoints = new List<Point>();
            foreach (var point in pathPoints)
            {
                // Subtract the y-coordinate from the image height to invert the y-axis
                var transformedPoint = new Point(point.X, imageHeight - point.Y);
                transformedPoints.Add(transformedPoint);
            }
            return transformedPoints;
        }

        #endregion

        #region Yol Metotları

        // Yol Noktalarını Bulma
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

        // Noktaları Dönüştürme
        public List<Point> TransformPoints(List<Point> pathPoints, int height)
        {
            List<Point> transformedPoints = new List<Point>();
            foreach (var point in pathPoints)
            {
                transformedPoints.Add(new Point(point.X, height - point.Y));
            }
            return transformedPoints;
        }
        #endregion

        #region Optimize Yol Komutları

        public List<string> GenerateOptimizedPathCommands(List<Point> pathPoints, double angleThreshold)
        {
            List<string> commands = new List<string>();
            if (pathPoints.Count == 0) return commands;

            // Simplify path
            List<Point> simplifiedPath = DouglasPeucker.Simplify(pathPoints, tolerance: 5.0);

            // Start with a MoveJ to the first point
            commands.Add($"MoveJ {{X: {simplifiedPath[0].X}, Y: {simplifiedPath[0].Y}, Z: 0, Rx: 0, Ry: 0, Rz: 0}};");

            for (int i = 1; i < simplifiedPath.Count - 1; i++)
            {
                Point prev = simplifiedPath[i - 1];
                Point current = simplifiedPath[i];
                Point next = simplifiedPath[i + 1];

                // Calculate angle between points
                double angle = CalculateAngle(prev, current, next);

                // If the angle is below a threshold, it's a straight line
                if (angle < angleThreshold)
                {
                    commands.Add($"MoveL {{X: {current.X}, Y: {current.Y}, Z: 0, Rx: 0, Ry: 0, Rz: 0}};");
                }
                else
                {
                    commands.Add($"MoveJ {{X: {current.X}, Y: {current.Y}, Z: 0, Rx: 0, Ry: 0, Rz: 0}};");
                }
            }

            // End with a MoveL to the last point
            Point last = simplifiedPath[simplifiedPath.Count - 1];
            commands.Add($"MoveL {{X: {last.X}, Y: {last.Y}, Z: 0, Rx: 0, Ry: 0, Rz: 0}};");

            return commands;
        }

        // Açı Hesaplama

        private double CalculateAngle(Point p1, Point p2, Point p3)
        {
            // Create vectors from points
            Vector v1 = new Vector(p1.X - p2.X, p1.Y - p2.Y); // Vector from p2 to p1
            Vector v2 = new Vector(p3.X - p2.X, p3.Y - p2.Y); // Vector from p2 to p3

            // Calculate the dot product of v1 and v2
            double dotProduct = v1.X * v2.X + v1.Y * v2.Y;

            // Calculate the magnitudes of v1 and v2
            double magV1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
            double magV2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);

            // Calculate the cosine of the angle
            double cosAngle = dotProduct / (magV1 * magV2);

            // Ensure the cosine value is within -1 to 1 range to avoid NaN errors due to floating-point imprecision
            cosAngle = Math.Max(-1, Math.Min(1, cosAngle));

            // Calculate the angle in radians and then convert to degrees
            double angle = Math.Acos(cosAngle) * (180 / Math.PI);

            return angle;
        }

        public struct Vector
        {
            public double X;
            public double Y;

            public Vector(double x, double y)
            {
                X = x;
                Y = y;
            }

            // Method to calculate the dot product of two vectors
            public static double DotProduct(Vector v1, Vector v2)
            {
                return v1.X * v2.X + v1.Y * v2.Y;
            }

            // Method to calculate the magnitude of a vector
            public double Magnitude()
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        private List<Point> OptimizePathForDirectionConsistency(List<Point> pathPoints)
        {
            if (pathPoints == null || pathPoints.Count < 3)
                return pathPoints;

            var optimizedPath = new List<Point> { pathPoints[0] };

            for (int i = 1; i < pathPoints.Count - 1; i++)
            {
                var current = pathPoints[i];
                var prev = optimizedPath.Last();
                var next = pathPoints[i + 1];

                if (!IsCollinear(prev, current, next))
                {
                    optimizedPath.Add(current);
                }
            }

            optimizedPath.Add(pathPoints.Last());
            return optimizedPath;
        }

        private bool IsCollinear(Point a, Point b, Point c)
        {
            // Check if the area of the triangle formed by the points is close to zero
            // If the area is zero (or close enough), the points are collinear
            return Math.Abs((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) < 1e-5;
        }

        #endregion

        #region Çizgi Optimize

        private bool IsLine(List<Point> points, double tolerance)
        {
            if (points.Count < 3) return true;

            var startPoint = points[0];
            var endPoint = points[points.Count - 1];

            for (int i = 1; i < points.Count - 1; i++)
            {
                if (DistanceFromLine(points[i], startPoint, endPoint) > tolerance)
                {
                    return false;
                }
            }
            return true;
        }

        private double DistanceFromLine(Point point, Point lineStart, Point lineEnd)
        {
            double numerator = Math.Abs((lineEnd.Y - lineStart.Y) * point.X -
                                        (lineEnd.X - lineStart.X) * point.Y +
                                        lineEnd.X * lineStart.Y -
                                        lineEnd.Y * lineStart.X);
            double denominator = Math.Sqrt(Math.Pow(lineEnd.Y - lineStart.Y, 2) +
                                           Math.Pow(lineEnd.X - lineStart.X, 2));

            return numerator / denominator;
        }


        #endregion

        #region Eğri Optimize

        private List<Point> IdentifyAndSimplifyCurves(List<Point> points, double angleThreshold, double curveSimplificationTolerance)
        {
            List<Point> simplifiedPoints = new List<Point>();
            List<Point> currentCurve = new List<Point>();

            for (int i = 1; i < points.Count - 1; i++)
            {
                Point prev = points[i - 1];
                Point current = points[i];
                Point next = points[i + 1];

                double angle = CalculateAngle(prev, current, next);

                if (Math.Abs(angle) > angleThreshold)
                {
                    // If the angle is above the threshold, it's part of a curve
                    if (currentCurve.Count == 0)
                        currentCurve.Add(prev);  // Start a new curve segment

                    currentCurve.Add(current);
                }
                else
                {
                    // If we're exiting a curve, add the last point of the curve and simplify the segment
                    if (currentCurve.Count > 0)
                    {
                        currentCurve.Add(current);
                        simplifiedPoints.AddRange(SimplifyCurve(currentCurve, curveSimplificationTolerance));
                        currentCurve.Clear();
                    }
                    else
                    {
                        // If it's not part of a curve, just add the point
                        simplifiedPoints.Add(current);
                    }
                }
            }

            // Don't forget to add the last curve if the path ends with it
            if (currentCurve.Count > 0)
            {
                currentCurve.Add(points.Last());
                simplifiedPoints.AddRange(SimplifyCurve(currentCurve, curveSimplificationTolerance));
            }
            else
            {
                simplifiedPoints.Add(points.Last());
            }

            return simplifiedPoints;
        }

        private List<Point> SimplifyCurve(List<Point> curvePoints, double tolerance)
        {
            // Here you can implement a curve fitting algorithm or a simplification method
            // For demonstration, we'll just use the Douglas-Peucker algorithm
            // In a real-world scenario, you might want to fit a bezier curve or a spline
            return DouglasPeucker.Simplify(curvePoints, tolerance);
        }
        #endregion

        #region Komşuluk Bulma
        // Komşuluk Bulma
        private List<List<Point>> IdentifyAndSimplifyClusters(List<Point> pathPoints, double proximityThreshold)
        {
            List<List<Point>> clusters = new List<List<Point>>();
            bool[] visited = new bool[pathPoints.Count];

            for (int i = 0; i < pathPoints.Count; i++)
            {
                if (visited[i]) continue;

                List<Point> cluster = new List<Point>();
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(i);

                while (queue.Count > 0)
                {
                    int currentIndex = queue.Dequeue();
                    if (visited[currentIndex]) continue;

                    visited[currentIndex] = true;
                    Point currentPoint = pathPoints[currentIndex];
                    cluster.Add(currentPoint);

                    for (int j = 0; j < pathPoints.Count; j++)
                    {
                        if (!visited[j] && DistanceBetweenPoints(currentPoint, pathPoints[j]) <= proximityThreshold)
                        {
                            queue.Enqueue(j);
                        }
                    }
                }

                if (cluster.Count > 1)
                {
                    clusters.Add(cluster);
                }
            }

            // Simplify clusters based on their internal connectivity
            List<List<Point>> simplifiedClusters = new List<List<Point>>();
            foreach (var cluster in clusters)
            {

                // Inside IdentifyAndSimplifyClusters method
                List<Point> simplifiedCluster = SimplifyCluster(cluster, 10.0);

                simplifiedClusters.Add(simplifiedCluster);
            }

            return simplifiedClusters;
        }

        private List<Point> SimplifyCluster(List<Point> cluster, double lineTolerance)
        {
            if (cluster == null || !cluster.Any())
                throw new ArgumentException("Cluster is null or empty");

            //// Calculate the average X and Y coordinates
            //int avgX = (int)cluster.Average(p => p.X);
            //int avgY = (int)cluster.Average(p => p.Y);

            //// Find the point that is closest to the average coordinates
            //Point closestPoint = cluster.OrderBy(p => Math.Pow(p.X - avgX, 2) + Math.Pow(p.Y - avgY, 2)).First();

            if (IsLine(cluster, lineTolerance))
            {
                // Keep only the start and end points for a line
                return new List<Point> { cluster.First(), cluster.Last() };
            }

            // Return a list containing only the closest point
            return cluster;
        }

        // İki nokta arasındaki mesafe

        private double DistanceBetweenPoints(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private List<Point> SimplifyPathBasedOnNeighborhood(List<Point> pathPoints, double threshold)
        {
            // Identify and simplify clusters
            List<List<Point>> clusters = IdentifyAndSimplifyClusters(pathPoints, threshold);

            // Flatten the list of lists into a single list
            List<Point> simplifiedPath = clusters.SelectMany(cluster => cluster).ToList();


            return simplifiedPath; // Adjust the threshold as needed.
        }

        #endregion

        #region Komut Metotları
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

        private void WriteRobotCommandsToFile(List<string> commands, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("BEGIN");
                foreach (string command in commands)
                {
                    writer.WriteLine(command);
                }
                writer.WriteLine("END");
            }
        }
        #endregion
    }
}

using HexGame.GameServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace HexGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TopGameService topGameService;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void newGame_Clicked(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(400, 400);
            using (Graphics g = Graphics.FromImage(image))
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, 4);
                g.DrawEllipse(pen, new System.Drawing.Rectangle(40, 40, 200, 200));
            }

            mainGameView.Source = BitmapToImageSource(image);
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

    }
}

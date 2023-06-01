using HexGame.GameServices;
using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

using SD = System.Drawing;

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
            topGameService = new TopGameService((int)mainGameView.Width,  (int)mainGameView.Height);
        }


        private void newGame_Clicked(object sender, MouseButtonEventArgs e)
        {
            topGameService.NewGame();
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            int mainViewX = (int)mainGameView.Width;
            int mainViewY = (int)mainGameView.Height;
            System.Drawing.Bitmap image = new SD.Bitmap(mainViewX, mainViewY);

            using (Graphics g = Graphics.FromImage(image))
            {
                var ImageSize = new SD.Rectangle(0, 0, mainViewX, mainViewY);

                topGameService.DrawGameFields(g, ImageSize);
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

        private void mainGameView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            topGameService.Click((int)e.GetPosition(mainGameView).X, (int)e.GetPosition(mainGameView).Y);
            UpdateBoard();
        }
    }
}

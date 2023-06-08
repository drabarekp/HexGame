using HexGame.GameServices;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HexGame.Enums;
using System;

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
            topGameService = new TopGameService((int)mainGameView.Width, (int)mainGameView.Height);
        }


        private void newGame_Clicked(object sender, MouseButtonEventArgs e)
        {
            var popup = new NewGamePopup();
            popup.ShowDialog();

            var random = new Random();

            topGameService.NewGame(popup.Iterations, popup.IsPlayerStart, popup.AlgorithmType, random.Next());
            UpdateBoard();

            if(!popup.IsPlayerStart) 
            {
                topGameService.PerformAIMove();
                UpdateBoard();
            }
        }

        private void newTournament_Clicked(object sender, MouseButtonEventArgs e)
        {
            // TODO
        }

        private void UpdateBoard()
        {
            int mainViewX = (int)mainGameView.Width;
            int mainViewY = (int)mainGameView.Height;
            var image = new Bitmap(mainViewX, mainViewY);

            using (Graphics g = Graphics.FromImage(image))
            {
                var ImageSize = new Rectangle(0, 0, mainViewX, mainViewY);

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

            topGameService.PerformAIMove();
            UpdateBoard();
        }
    }
}

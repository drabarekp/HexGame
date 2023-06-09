using HexGame.Engine;
using HexGame.Enums;
using HexGame.GameServices;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
            HideAIProcessing();
        }


        private void newGame_Clicked(object sender, MouseButtonEventArgs e)
        {
            var popup = new NewGamePopup();
            popup.ShowDialog();

            var random = new Random();

            topGameService.NewGame(popup.Iterations, popup.IsPlayerStart, popup.AlgorithmType, random.Next());
            UpdateBoard();

            if (!popup.IsPlayerStart)
            {
                topGameService.PerformAIMove();
                UpdateBoard();
            }
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

            ShowAIProcessing();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (ThreadStart)(() =>
            {
                topGameService.PerformAIMove();
                UpdateBoard();
                HideAIProcessing();
            }));
        }

        private void ShowAIProcessing()
        {
            AIStatusBar.Text = "AI myśli, proszę czekać.";

        }

        private void HideAIProcessing()
        {
            AIStatusBar.Text = "Program gotowy";
        }

        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            int iterations = int.Parse(IterationsTextBox.Text);
            int repetitions = int.Parse(RepetitionsTextBox.Text);
            int seed = int.Parse(SeedTextBox.Text);

            IAlgorithm algorithm1 = SetAlgorithm(Algorithm1TypeComboBox.SelectedIndex, iterations, seed);
            IAlgorithm algorithm2 = SetAlgorithm(Algorithm2TypeComboBox.SelectedIndex, iterations, seed);

            ResetResults();

            ShowAIProcessing();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (ThreadStart)(() =>
            {
                var simulationRunner = new SimulationRunner(algorithm1, algorithm2, repetitions);
                int algorithm1Wins = simulationRunner.RunSimulations();

                UpdateResults(algorithm1.AlgorithmName, algorithm2.AlgorithmName, algorithm1Wins, repetitions);

                HideAIProcessing();
            }));
        }

        private void ResetResults()
        {
            Algorithm1Label.Content = "Symulacja w toku";
            Algorithm1WinsLabel.Content = "";
            Algorithm1DefeatsLabel.Content = "";

            Algorithm2Label.Content = "";
            Algorithm2WinsLabel.Content = "";
            Algorithm2DefeatsLabel.Content = "";
        }

        private void UpdateResults(string algorithm1Name, string algorithm2Name, int algorithm1Wins, int repetitions)
        {
            Algorithm1Label.Content = algorithm1Name;
            Algorithm1WinsLabel.Content = algorithm1Wins;
            Algorithm1DefeatsLabel.Content = repetitions - algorithm1Wins;

            Algorithm2Label.Content = algorithm2Name;
            Algorithm2WinsLabel.Content = repetitions - algorithm1Wins;
            Algorithm2DefeatsLabel.Content = algorithm1Wins;
        }

        private IAlgorithm SetAlgorithm(int index, int iterations, int seed)
        {
            switch (index)
            {
                case (int)AlgorithmTypeEnum.BasicMCTS:
                    return new BasicMCTSAlgorithm(seed, iterations);

                case (int)AlgorithmTypeEnum.RAVE:
                    return new BasicMCTSAlgorithm(seed, iterations);

                case (int)AlgorithmTypeEnum.MAST:
                    return new BasicMCTSAlgorithm(seed, iterations);

                case (int)AlgorithmTypeEnum.Heuristic:
                    return new BasicMCTSAlgorithm(seed, iterations);
            }

            throw new InvalidOperationException();
        }
    }
}

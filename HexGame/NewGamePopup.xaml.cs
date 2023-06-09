using HexGame.Enums;
using System.Windows;

namespace HexGame
{
    /// <summary>
    /// Interaction logic for NewGamePopup.xaml
    /// </summary>
    public partial class NewGamePopup : Window
    {
        public int Iterations;
        public bool IsPlayerStart;
        internal AlgorithmTypeEnum AlgorithmType;

        public NewGamePopup()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Iterations = int.Parse(IterationsTextBox.Text);
            IsPlayerStart = (bool)PlayerStartCheckBox.IsChecked!;

            switch (AlgorithmTypeComboBox.SelectedIndex)
            {
                case (int)AlgorithmTypeEnum.BasicMCTS:
                    AlgorithmType = AlgorithmTypeEnum.BasicMCTS;
                    break;

                case (int)AlgorithmTypeEnum.RAVE:
                    AlgorithmType = AlgorithmTypeEnum.RAVE;
                    break;

                case (int)AlgorithmTypeEnum.MAST:
                    AlgorithmType = AlgorithmTypeEnum.MAST;
                    break;

                case (int)AlgorithmTypeEnum.Heuristic:
                    AlgorithmType = AlgorithmTypeEnum.Heuristic;
                    break;
            }

            Close();
        }
    }
}

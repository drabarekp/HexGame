using HexGame.Enums;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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

            switch(AlgorithmTypeComboBox.SelectedIndex)
            {
                case (int)AlgorithmTypeEnum.BasicMCTS:
                    AlgorithmType = AlgorithmTypeEnum.BasicMCTS;
                    break;

                case (int)AlgorithmTypeEnum.RAVE:
                    AlgorithmType = AlgorithmTypeEnum.BasicMCTS;
                    break;

                case (int)AlgorithmTypeEnum.MAST:
                    AlgorithmType = AlgorithmTypeEnum.BasicMCTS;
                    break;

                case (int)AlgorithmTypeEnum.Heuristic:
                    AlgorithmType = AlgorithmTypeEnum.BasicMCTS;
                    break;
            }

            Close();
        }
    }
}

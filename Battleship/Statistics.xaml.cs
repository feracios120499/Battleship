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

namespace Battleship
{
    /// <summary>
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labelCountShots.Content = BattleShipDB.SelectCountShots();
            labelCountHits.Content = BattleShipDB.SelectCountHits();
            labelCountGame.Content = BattleShipDB.SelectCountGame();
            labelCountWins.Content = BattleShipDB.SelectCountWins();
            labelCountKill.Content = BattleShipDB.SelectCountKill();
        }
    }
}

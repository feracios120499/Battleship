using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using static Battleship.BattleField;
using static Battleship.EnemyAI;
using static Battleship.Ships;

namespace Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ship> beginShips = new List<Ship> {
                new Ship { Type = TypeShip.one_deck, CountShip = 6 },
                new Ship {Type=TypeShip.two_deck,CountShip=3 },
                new Ship {Type=TypeShip.three_deck,CountShip=2 },
                new Ship {Type=TypeShip.four_deck,CountShip=1 }
            };
        public Ships ships;
        public TypeShip TypeShip { get; set; } = TypeShip.one_deck;
        public BattleField.Orientation orientation = BattleField.Orientation.Gorizontal;
        private EnemyAI enemy;
        private Level lvlEnemy = Level.Ez;
        public MainWindow()
        {
            InitializeComponent();
            this.MyField.ClickOnBox += MyField_ClickOnBox;
            ships = new Ships(beginShips);
            EnemyField.boxes.Enemy = true;
        }
        private void MyField_ClickOnBox(object sender, ShipEventArgs e)
        {
            if (ships[TypeShip].CountShip != 0)
            {
                try
                {
                    MyField.SupplyShip(e.startBox, TypeShip, orientation);
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show("Выход за границы поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ships[TypeShip].CountShip--;
            }
            else
            {
                MessageBox.Show("Такие корабли закончились!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ships.GetCountShips() == 0)
            {
                buttonStartGame.IsEnabled = true;
            }

        }
        private void radioButton1_Click(object sender, RoutedEventArgs e)
        {
            TypeShip = (TypeShip)Convert.ToInt32((sender as RadioButton).Content);
        }

        private void radioButtonVer_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButtonGor_Click(object sender, RoutedEventArgs e)
        {
            orientation = BattleField.Orientation.Gorizontal;
        }

        private void radioButtonVer_Click(object sender, RoutedEventArgs e)
        {
            orientation = BattleField.Orientation.Vertical;
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            MyField.ClearField();
            ships = new Ships(beginShips);
            Group1.IsEnabled = true;
            groupBox.IsEnabled = true;
            buttonStartGame.IsEnabled = false;
            if (MyField.ClickOnBox == null)
                MyField.ClickOnBox += MyField_ClickOnBox;
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            Group1.IsEnabled = false;
            groupBox.IsEnabled = false;
            groupBox1.IsEnabled = false;
            buttonClear.IsEnabled = false;
            buttonAutoPlacement.IsEnabled = false;
            buttonNewGame.IsEnabled = true;
            EnemyField.ClickOnBox += EnemyField_ClickBox;
            buttonStartGame.IsEnabled = false;
            EnemyField.AutoPlacementShips(new Ships(beginShips));
            enemy = new EnemyAI(lvlEnemy, MyField);
            BattleShipDB.IncCountGame();
        }
        private void EnemyField_ClickBox(object sender,ShipEventArgs e)
        {
            if (e.startBox.IsShooting)
            {
                MessageBox.Show("Вы сюда уже стреляли!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BattleShipDB.IncCountShots();
            if (e.startBox.IsBusy)
            {
                e.startBox.Hitting();
                BattleShipDB.IncCountHits();
                if (EnemyField.boxes.GetCountLivingShipsByName(e.startBox.NameShip) == 0)
                {
                    MessageBox.Show("Убил","Красавчик",MessageBoxButton.OK,MessageBoxImage.Information);
                    BattleShipDB.IncCountKill();
                }
            }
            else
            {
                e.startBox.Miss();
            }
            if (EnemyField.boxes.GetCountLivingShips() == 0)
            {
                BattleShipDB.IncCountGame();
                if(MessageBox.Show("Вы победили!Хотите начать сначала?", "Поздравляю", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    buttonNewGame_Click(buttonNewGame, null);
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            enemy.Shoot();
            if (MyField.boxes.GetCountLivingShips() == 0)
            {
                if (MessageBox.Show("Вы проиграли!Хотите начать сначала?", "Соболезнования", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    buttonNewGame_Click(buttonNewGame, null);
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }

        }

        private void buttonAutoPlacement_Click(object sender, RoutedEventArgs e)
        {
            MyField.AutoPlacementShips(ships);
            buttonStartGame.IsEnabled = true;
            MyField.ClickOnBox -= MyField_ClickOnBox;
        }

        private void buttonNewGame_Click(object sender, RoutedEventArgs e)
        {
            groupBox.IsEnabled = true;
            groupBox1.IsEnabled = true;
            Group1.IsEnabled = true;
            buttonStartGame.IsEnabled = false;
            buttonNewGame.IsEnabled = false;
            buttonClear.IsEnabled = true;
            buttonAutoPlacement.IsEnabled = true;
            MyField.ClearField();
            EnemyField.ClearField();
            EnemyField.ClickOnBox -= EnemyField_ClickBox;
            MyField.ClickOnBox += MyField_ClickOnBox;
        }

        private void radioButtonEzLvl_Click(object sender, RoutedEventArgs e)
        {
            lvlEnemy = Level.Ez;
        }

        private void radioButtonHardLvl_Click(object sender, RoutedEventArgs e)
        {
            lvlEnemy = Level.Hard;
        }

        private void StatisticMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Statistics form = new Statistics();
            form.ShowDialog();
        }


        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (buttonNewGame.IsEnabled)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML (*.xml)|*.xml";
                saveFileDialog.InitialDirectory = $"{Directory.GetCurrentDirectory()}\\Save\\";
                if (saveFileDialog.ShowDialog() == true)
                {
                    SaveAndLoad.Save(MyField.boxes, EnemyField.boxes, lvlEnemy, saveFileDialog.FileName);

                }
            }
            else
            {
                MessageBox.Show("Начните игру что бы сохранить", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (buttonClear.IsEnabled)
            {
                buttonClear_Click(buttonClear, null);
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML (*.xml)|*.xml";
                openFileDialog.InitialDirectory = $"{Directory.GetCurrentDirectory()}\\Save\\";
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        SaveAndLoad.Load(MyField.boxes, EnemyField.boxes, ref lvlEnemy, openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    StartSavedGame();
                }
            }
            else
            {
                MessageBox.Show("Начните новую игру что бы загрузить", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void StartSavedGame()
        {
            if (lvlEnemy == Level.Ez)
            {
                radioButtonEzLvl.IsChecked = true;
            }
            else
            {
                radioButtonHardLvl.IsChecked = true;
            }
            groupBox.IsEnabled = false;
            Group1.IsEnabled = false;
            groupBox1.IsEnabled = false;
            buttonClear.IsEnabled = false;
            buttonStartGame.IsEnabled = false;
            buttonAutoPlacement.IsEnabled = false;
            buttonNewGame.IsEnabled = true;
            enemy = new EnemyAI(lvlEnemy, MyField);
            EnemyField.ClickOnBox += EnemyField_ClickBox;
           
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

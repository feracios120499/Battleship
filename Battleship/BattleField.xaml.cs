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
using static Battleship.Ship;
using static Battleship.Ships;

namespace Battleship
{
    /// <summary>
    /// Логика взаимодействия для BattleField.xaml
    /// </summary>

    [Serializable]
    public partial class BattleField : UserControl
    {
        
        private static Random random = new Random();
        public class ShipEventArgs : EventArgs
        {
            public Box startBox { get; set; }
        }
        [Serializable]
        public class Box
        {
            public string NameShip { get; set; }
            internal StackPanel Panel { get; set; }
            private Color color;

            internal Color Color
            {
                get { return color; }
                set
                {
                    color = value;
                    Panel.Background = new SolidColorBrush(color);
                }
            }
            internal Color LastColor { get; set; }
            public bool IsBusy { get; set; } = false;
            public bool IsShooting { get; set; } = false;
            public Box(StackPanel Panel, Color Color)
            {
                this.Panel = Panel;
                this.Color = Color;
                this.LastColor = Color;
            }
            public Box()
            {

            }
            public void Hitting()
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(5, 5, 5, 5);
                stackPanel.Width = 20;
                stackPanel.Height = 20;
                stackPanel.Background = new SolidColorBrush(Colors.Red);
                Panel.Children.Add(stackPanel);
                IsShooting = true;
            }
            public void Miss()
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(10, 10, 10, 10);
                stackPanel.Width = 10;
                stackPanel.Height = 10;
                stackPanel.Background = new SolidColorBrush(Colors.Black);
                Panel.Children.Add(stackPanel);
                IsShooting = true;
            }
        }
        [Serializable]
        public class Boxes
        {
            public bool Enemy { get; set; }
            public List<Box> panels = new List<Box>();
            public void Add(Box item)
            {
                panels.Add(item);
            }
            public StackPanel GetLastPanel()
            {
                return panels.Last().Panel;
            }
            public Box GetBoxByPanel(StackPanel panel)
            {
                return panels.Find(p => p.Panel == panel);
            }
            public int GetIndexBox(StackPanel panel)
            {
                return panels.IndexOf(panels.Find(p => p.Panel == panel));
            }
            public void SetBoxColor(StackPanel panel, Color color)
            {
                this.GetBoxByPanel(panel).Color = color;
            }
            public void SetBoxLastFColor(StackPanel panel, Color color)
            {
                this.GetBoxByPanel(panel).LastColor = color;
            }
            public Color GetBoxColor(StackPanel panel)
            {
                return this.GetBoxByPanel(panel).Color;
            }
            public Color GetBoxLastColor(StackPanel panel)
            {
                return this.GetBoxByPanel(panel).LastColor;
            }
            public int GetCountBox()
            {
                return panels.Count;
            }
            public Box this[int index]
            {
                get { return panels[index]; }
                set { panels[index] = value; }
            }
            public int GetCountLivingShips()
            {
                int count = 0;
                foreach(var item in panels)
                {
                    if (item.IsBusy && !item.IsShooting)
                        count++;
                }
                return count;
            }
            public int GetCountLivingShipsByName(string name)
            {
                int count = 0;
                foreach(var item in panels)
                {
                    if(item.NameShip==name&&!item.IsShooting)
                        count++;
                }
                return count;
            }
            public Boxes()
            {

            }
        }
        public EventHandler<ShipEventArgs> ClickOnBox;
        public Boxes boxes = new Boxes();
        public enum Orientation
        {
            Vertical=0,
            Gorizontal=1
        }
        public MainWindow main { get; set; }
        public BattleField()
        {
            InitializeComponent();
            bool flag = true;
            for (int i = 1; i <= 100; i++)
            {
                if (i % 10 == 1)
                    flag = !flag;
                Color color = flag ? Color.FromArgb(80, 211, 211, 211) : Color.FromArgb(150, 211, 211, 211);
                flag = !flag;
                boxes.Add(new Box((StackPanel)this.FindName($"Box{i}"), color));
                //boxes.GetLastPanel().MouseMove += BattleField_MouseMove;
                //boxes.GetLastPanel().MouseLeave += BattleField_MouseLeave;
            }

        }
        private void BattleField_MouseLeave(object sender, MouseEventArgs e)
        {
            boxes.SetBoxColor(sender as StackPanel, boxes.GetBoxLastColor(sender as StackPanel));
        }
        private void BattleField_MouseMove(object sender, MouseEventArgs e)
        {
            boxes.SetBoxColor(sender as StackPanel, Colors.LightBlue);
        }
        private void Box_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickOnBox?.Invoke(this, new ShipEventArgs { startBox = boxes.GetBoxByPanel(sender as StackPanel) });
           
        }
        public bool CheckAvailabilityVertical(int startPosition, TypeShip ship)
        {
            if ((startPosition - 10) >= 0)
            {
                if (startPosition != 0 && (startPosition - 1) % 10 != 9)
                {
                    if (boxes[startPosition - 1 - 10].IsBusy)
                        return false;
                }
                if (boxes[startPosition - 10].IsBusy)
                    return false;
                if ((startPosition + 1) % 10 != 0)
                {
                    if (boxes[startPosition - 10 + 1].IsBusy)
                        return false;
                }
            }//проверяем верх
            if ((startPosition + (int)ship * 10) <= 99)
            {
                if ((startPosition + (int)ship * 10 - 1) % 10 != 9)
                {
                    if (boxes[startPosition - 1 + (int)ship * 10].IsBusy)
                        return false;
                }
                if (boxes[startPosition + (int)ship * 10].IsBusy)
                    return false;
                if ((startPosition + 1 + (int)ship * 10) % 10 != 0)
                {
                    if (boxes[startPosition + (int)ship * 10 + 1].IsBusy)
                        return false;
                }
            }//проверяем низ
            for (int i = 0, j = 0; i < (int)ship; i++, j += 10)
            {
                if (startPosition != 0 && (startPosition - 1) % 10 != 9)
                    if (boxes[startPosition - 1 + j].IsBusy)
                        return false;
                if ((startPosition + 1) % 10 != 0)
                    if (boxes[startPosition + 1 + j].IsBusy)
                        return false;
                if (boxes[startPosition + j].IsBusy)
                    return false;
            }//проверяем боковые стороны
            return true;
        }
        private bool CheckAvailabilityGorizontal(int startPosition, TypeShip ship)
        {
            if (startPosition != 0 && (startPosition - 1) % 10 != 9)
            {
                if (startPosition - 10 > 0)
                {
                    if (boxes[startPosition - 10 - 1].IsBusy)
                        return false;
                }
                if (startPosition != 0 && boxes[startPosition - 1].IsBusy)
                    return false;
                if (startPosition + 10 < 99)
                {
                    if (boxes[startPosition + 10 - 1].IsBusy)
                        return false;
                }
            }//проверяем лево
            if ((startPosition + (int)ship) % 10 != 0)
            {
                if (startPosition - 10 > 0)
                {
                    if (boxes[startPosition - 10 + (int)ship].IsBusy)
                        return false;
                }
                if (boxes[startPosition + (int)ship].IsBusy)
                    return false;
                if (startPosition + 10 < 99)
                {
                    if (boxes[startPosition + 10 + (int)ship].IsBusy)
                        return false;
                }
            }//проверяем право
            for (int i = 0; i < (int)ship; i++)
            {
                if (startPosition - 10 >= 0)
                    if (boxes[startPosition + i - 10].IsBusy)
                        return false;
                if (startPosition + 10 <= 99)
                    if (boxes[startPosition + i + 10].IsBusy)
                        return false;
                if (boxes[startPosition + i].IsBusy)
                    return false;
            }//проверяем стороны

            return true;
        }
        public void SupplyShip(Box startPosition, TypeShip ship, Orientation orientation)
        {
            string name;
            do
            {
                name = Guid.NewGuid().ToString();
            } while (!CheckAvailabilityName(name));
            int position = boxes.GetIndexBox(startPosition.Panel);
            if (orientation == Orientation.Vertical)
            {
                for (int i = 0, j = 0; i < (int)ship; i++, j += 10)
                {
                    if ((position + j) > 99)
                        throw new IndexOutOfRangeException();
                }
                if (!CheckAvailabilityVertical(position, ship))
                    throw new Exception("Рядом корабль");
                
                
                for (int i = 0, j = 0; i < (int)ship; i++, j += 10)
                {
                    boxes[boxes.GetIndexBox(startPosition.Panel) + j].NameShip = name;
                    if (!boxes.Enemy)
                    {
                        boxes[boxes.GetIndexBox(startPosition.Panel) + j].Color = Colors.Blue;
                        boxes[boxes.GetIndexBox(startPosition.Panel) + j].LastColor = Colors.Blue;
                    }
                    boxes[boxes.GetIndexBox(startPosition.Panel) + j].IsBusy = true;
                }
            }
            else
            {
                for (int i = 1; i < (int)ship; i++)
                {
                    if ((position + i) % 10 == 0)
                        throw new IndexOutOfRangeException();
                }
                if (!CheckAvailabilityGorizontal(position, ship))
                    throw new Exception("Рядом корабль");
                for (int i = 0; i < (int)ship; i++)
                {
                    boxes[boxes.GetIndexBox(startPosition.Panel) + i].NameShip = name;
                    if (!boxes.Enemy)
                    {
                        boxes[boxes.GetIndexBox(startPosition.Panel) + i].Color = Colors.Blue;
                        boxes[boxes.GetIndexBox(startPosition.Panel) + i].LastColor = Colors.Blue;
                    }
                    boxes[boxes.GetIndexBox(startPosition.Panel) + i].IsBusy = true;
                }
            }
        }
        public void ClearField()
        {
            bool flag = true;
            for (int i = 0; i < boxes.GetCountBox(); i++)
            {
                if (i % 10 == 0)
                    flag = !flag;
                Color color = flag ? Color.FromArgb(80, 211, 211, 211) : Color.FromArgb(150, 211, 211, 211);
                boxes[i].Color = color;
                flag = !flag;
                boxes[i].LastColor = color;
                boxes[i].IsBusy = false;
                boxes[i].IsShooting = false;
                boxes[i].Panel.Children.Clear();
            }
        }
        public void AutoPlacementShips(Ships ships)
        {
            ClearField();
            Restart:
            int count = 0;
            foreach (var item in ships.GetShips())
            {
                for (int i = 0; i < item.CountShip; i++)
                {
                    count = 0;
                    while (true)
                    {
                        var box = boxes[random.Next(0, 100)];
                        if (box.IsBusy)
                            continue;
                        var orientation = (Orientation)random.Next(0, 2);
                        try
                        {
                            SupplyShip(box, item.Type, orientation);
                        }
                        catch
                        { 
                            count++;
                            if (count >= 100)
                            {
                                ClearField();
                                goto Restart;
                            }
                            continue;
                        }
                        break;
                    }
                }
            }
        }
        public bool CheckAvailabilityName(string name)
        {
            for(int i = 0; i < boxes.GetCountBox(); i++)
            {
                if (boxes[i].NameShip == name)
                    return false;
            }
            return true;

        }
    }
}

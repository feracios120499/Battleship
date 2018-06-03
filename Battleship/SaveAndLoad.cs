using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using static Battleship.BattleField;
using static Battleship.EnemyAI;

namespace Battleship
{
    [Serializable]
    public class Saver
    {
        public Boxes myField { get; set; }
        public Boxes enemyField { get; set; }
        public Level lvl { get; set; }
        public Saver()
        {

        }
    }
    class SaveAndLoad
    {
        public static void Save(BattleField.Boxes myField, BattleField.Boxes enemyField,Level lvl, string path)
        {
            Saver saver = new Saver { myField = myField, enemyField = enemyField,lvl=lvl };
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Saver));
                using (FileStream fs = new FileStream(path, FileMode.Create,FileAccess.Write))
                {
                    formatter.Serialize(fs, saver);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static void Load(BattleField.Boxes myField, BattleField.Boxes enemyField,ref Level lvl, string path)
        {
            try
            {
                Saver saver;
                XmlSerializer formatter = new XmlSerializer(typeof(Saver));
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    saver = (Saver)formatter.Deserialize(fs);
                }
                lvl = saver.lvl;
                for (int i = 0; i < enemyField.GetCountBox(); i++)
                {
                    enemyField[i].IsBusy = saver.enemyField[i].IsBusy;
                    enemyField[i].IsShooting = saver.enemyField[i].IsShooting;
                    enemyField[i].NameShip = saver.enemyField[i].NameShip;
                    if (enemyField[i].IsBusy)
                    {
                        if (enemyField[i].IsShooting)
                        {
                            enemyField[i].Hitting();
                        }
                    }
                    else
                    {
                        if (enemyField[i].IsShooting)
                        {
                            enemyField[i].Miss();
                        }
                    }
                }
                for (int i = 0; i < myField.GetCountBox(); i++)
                {
                    myField[i].IsBusy = saver.myField[i].IsBusy;
                    myField[i].IsShooting = saver.myField[i].IsShooting;
                    myField[i].NameShip = saver.myField[i].NameShip;
                    if (myField[i].IsBusy)
                    {
                        myField[i].Color = Colors.Blue;
                        myField[i].LastColor = Colors.Blue;
                        if (myField[i].IsShooting)
                        {
                            myField[i].Hitting();
                        }
                    }
                    else
                    {
                        if (myField[i].IsShooting)
                        {
                            myField[i].Miss();
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Неверный файл или файл поврежден!");
            }
        }
    }
}

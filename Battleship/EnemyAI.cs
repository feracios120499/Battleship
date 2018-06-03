using System;

namespace Battleship
{
    public class EnemyAI
    {
        class LastShot
        {
            public bool IsHit { get; set; } = false;
            public int StartPositionShip { get; set; }
            public bool LEFT { get; set; } = true;
            public bool RIGHT { get; set; } = true;
            public bool DOWN { get; set; } = true;
            public bool TOP { get; set; } = true;
            public int CountShot { get; set; }
            public void Clear()
            {
                IsHit = false;
                LEFT = true;
                DOWN = true;
                RIGHT = true;
                TOP = true;
                CountShot = 0;
            }

        }
        public enum Side
        {
            LEFT,
            RIGHT,
            TOP,
            DOWN,
            NONE
        }
        public enum Level
        {
            Ez = 0,
            Hard = 1
        }
        public Level Lvl { get; set; }
        private BattleField battleField;
        private LastShot lastShot = new LastShot();
        private static Random random = new Random();
        public EnemyAI(Level lvl, BattleField battleField)
        {
            this.Lvl = lvl;
            this.battleField = battleField;
        }
        public bool CheckAround(int index, Side side)
        {
            if (index < 0)
                return false;
            if (battleField.boxes[index].IsShooting)
                return false;
            if (index - 10 >= 0)
            {
                if (index % 10 != 0 && index != 0)//верхний левый угол
                {
                    if (battleField.boxes[index - 10 - 1].IsBusy && battleField.boxes[index - 10 - 1].IsShooting)
                        return false;
                }
                if (battleField.boxes[index - 10].IsBusy && battleField.boxes[index - 10].IsShooting && side != Side.DOWN)//верхний
                    return false;
                if (index % 10 != 9 && index != 99)//верхний правый
                {
                    if (battleField.boxes[index - 10 + 1].IsBusy && battleField.boxes[index - 10 + 1].IsShooting)
                        return false;
                }
            }//проверяем верх
            if (index + 10 <= 99)
            {
                if (index % 10 != 0 && index != 0)
                {
                    if (battleField.boxes[index + 10 - 1].IsBusy && battleField.boxes[index + 10 - 1].IsShooting)
                        return false;
                }
                if (battleField.boxes[index + 10].IsBusy && battleField.boxes[index + 10].IsShooting && side != Side.TOP)//нижний
                    return false;
                if (index % 10 != 9 && index != 99)
                {
                    if (battleField.boxes[index + 10 + 1].IsBusy && battleField.boxes[index + 10 + 1].IsShooting)
                        return false;
                }
            }//проверяем низ
            if (index % 10 != 0 && side != Side.RIGHT)
            {
                if (battleField.boxes[index - 1].IsBusy && battleField.boxes[index - 1].IsShooting)
                    return false;
            }
            if (index % 10 != 9 && side != Side.LEFT)
            {
                if (battleField.boxes[index + 1].IsBusy && battleField.boxes[index + 1].IsShooting)
                    return false;
            }
            return true;
        }
        public void Shoot()
        {
            if (Lvl == Level.Ez)
            {
                while (true)
                {
                    int index = random.Next(0, 100);
                    if (battleField.boxes[index].IsShooting)
                    {
                        continue;
                    }
                    else
                    {
                        if (battleField.boxes[index].IsBusy)
                        {
                            battleField.boxes[index].Hitting();
                        }
                        else
                        {
                            battleField.boxes[index].Miss();
                        }
                        break;
                    }
                }
            }
            else
            {
                Restart:
                if (!lastShot.IsHit)
                {
                    while (true)
                    {
                        int index = random.Next(0, 100);
                        if (battleField.boxes[index].IsShooting)
                        {
                            continue;
                        }
                        else
                        {
                            if (battleField.boxes[index].IsBusy)
                            {
                                battleField.boxes[index].Hitting();
                                if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[index].NameShip) == 0)
                                {
                                    lastShot.Clear();
                                }
                                else
                                {
                                    lastShot.IsHit = true;
                                    lastShot.CountShot++;
                                    lastShot.StartPositionShip = index;
                                }
                            }
                            else
                            {
                                battleField.boxes[index].Miss();
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (lastShot.LEFT)
                    {
                        if (CheckAround(lastShot.StartPositionShip - lastShot.CountShot, Side.LEFT))
                        {
                            if ((lastShot.StartPositionShip - lastShot.CountShot) % 10 == 9 || lastShot.StartPositionShip - lastShot.CountShot < 0)
                            {
                                lastShot.LEFT = false;
                            }
                            else
                            {
                                if (Hit(lastShot.StartPositionShip - lastShot.CountShot))
                                {

                                    if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[lastShot.StartPositionShip].NameShip) != 0)
                                    {
                                        lastShot.TOP = false;
                                        lastShot.DOWN = false;
                                    }
                                    return;
                                }
                                else
                                {
                                    lastShot.LEFT = false;
                                    return;
                                }
                            }
                        }
                        lastShot.LEFT = false;
                    }
                    if (lastShot.TOP)
                    {
                        if (CheckAround(lastShot.StartPositionShip - lastShot.CountShot * 10, Side.TOP))
                        {
                            if (lastShot.StartPositionShip - lastShot.CountShot * 10 % 10 == 0 || lastShot.StartPositionShip - lastShot.CountShot * 10 < 0)
                            {
                                lastShot.TOP = false;
                            }
                            else
                            {

                                if (Hit(lastShot.StartPositionShip - lastShot.CountShot * 10))
                                {

                                    if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[lastShot.StartPositionShip].NameShip) != 0)
                                    {
                                        lastShot.LEFT = false;
                                        lastShot.RIGHT = false;
                                    }
                                    return;
                                }
                                else
                                {
                                    lastShot.TOP = false;
                                    return;
                                }
                            }
                        }
                        lastShot.TOP = false;
                    }
                    if (lastShot.RIGHT)
                    {
                        if (CheckAround(lastShot.StartPositionShip + lastShot.CountShot, Side.RIGHT))
                        {
                            if ((lastShot.StartPositionShip + lastShot.CountShot) % 10 == 9 || lastShot.StartPositionShip + lastShot.CountShot > 99)
                            {
                                lastShot.RIGHT = false;
                            }
                            else
                            {
                                if (Hit(lastShot.StartPositionShip + lastShot.CountShot))
                                {
                                    if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[lastShot.StartPositionShip].NameShip) != 0)
                                    {
                                        lastShot.TOP = false;
                                        lastShot.DOWN = false;
                                    }
                                    return;
                                }
                                else
                                {
                                    lastShot.RIGHT = false;
                                    return;
                                }
                            }
                        }
                        lastShot.RIGHT = false;
                    }
                    if (lastShot.DOWN)
                    {
                        if (CheckAround(lastShot.StartPositionShip + lastShot.CountShot * 10, Side.DOWN))
                        {
                            if (lastShot.StartPositionShip + lastShot.CountShot * 10 % 10 == 9 || lastShot.StartPositionShip + lastShot.CountShot * 10 > 99)
                            {
                                lastShot.DOWN = false;
                            }
                            else
                            {

                                if (Hit(lastShot.StartPositionShip + lastShot.CountShot * 10))
                                {

                                    if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[lastShot.StartPositionShip].NameShip) != 0)
                                    {
                                        lastShot.LEFT = false;
                                        lastShot.RIGHT = false;
                                    }
                                    return;
                                }
                                else
                                {
                                    lastShot.DOWN = false;
                                    return;
                                }
                            }
                        }
                        lastShot.DOWN = false;
                    }
                    lastShot.Clear();
                    goto Restart;
                }
            }
        }
        private bool Hit(int index)
        {
            if (battleField.boxes[index].IsBusy)
            {
                battleField.boxes[index].Hitting();
                if (battleField.boxes.GetCountLivingShipsByName(battleField.boxes[index].NameShip) == 0)
                {
                    lastShot.Clear();
                }
                else
                {
                    lastShot.IsHit = true;
                    lastShot.CountShot++;
                }
                return true;
            }
            else
            {
                battleField.boxes[index].Miss();
                lastShot.CountShot = 1;
                return false;
            }
        }
    }
}

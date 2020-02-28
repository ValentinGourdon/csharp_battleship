using System;
using System.Collections.Generic;
using System.Windows;

public class BotPlayer : Player{
    private int level;
    private List<Point> successShoots;

    public BotPlayer(string name,  Grid attack, Grid defense, List<Boat> boatsPos, List<Boat> boatsDef, int level) {
        this.name = name;
        this.attack = attack;
        this.defense = defense;
        this.boatsPos = boatsPos;
        this.boatsDef = boatsDef;
        this.level = level;
        this.successShoots = new List<Point>();
    }

    public void autoPlaceAllBoats() {
        int count = 1;
        while(count <= 5) {
            Boat boat = this.boatsPos.Find(x=> x.getId().Equals(count));
            if(boat != null) {
                // Auto Vertical or Horizontal
                if(new Random().Next(0, 2) == 1)
                    boat.setPosition(false);
                // Auto boat placement
                if(this.defense.placeBoat(boat, new Random().Next(0, 9), new Random().Next(0, 9), false)) {
                    this.boatsPos.Remove(boat);
                    count ++;
                }
            }
        }
    }

    public void autoShoot(Player opp, bool displayMode, int level) {
        Console.Clear();
        if(displayMode)
            this.displayBothGrid();
        Point p = null;

        switch(level) {
            case 1 :
                p = chooseShootSimple(opp);
                break;
            case 2 :
                p = chooseShootMedium(opp);
                break;
        }
        int x = p.getX(), y = p.getY();

        Console.WriteLine("Player " + this.name + " shoot at " + opp.getName());
        Console.WriteLine("Targeting " + new Grid().XYToString(x, y) + "...");
        if(displayMode)
            System.Threading.Thread.Sleep(500);
        else
            System.Threading.Thread.Sleep(1000);
        if(opp.getDefense().getGrid()[x, y] == 0) {
            opp.getDefense().setGrid(x, y, 6);
            Console.WriteLine("Missed...");
            this.attack.setGrid(x, y, 6);
        } else {
            Boat b = null;
            switch(opp.getDefense().getGrid()[x, y]) {
                case 1 :
                    b = opp.getBoatsDef().Find(x => x.getName().Contains("Carrier"));
                    break;
                case 2 :
                    b = opp.getBoatsDef().Find(x => x.getName().Contains("Battleship"));
                    break;
                case 3 :
                    b = opp.getBoatsDef().Find(x => x.getName().Contains("Cruiser"));
                    break;
                case 4 :
                    b = opp.getBoatsDef().Find(x => x.getName().Contains("Submarine"));
                    break;
                case 5 :
                    b = opp.getBoatsDef().Find(x => x.getName().Contains("Destroyer"));
                    break;
            }
            this.successShoots.Add(new Point(x, y));    // For Medium level
            b.setTouched();
            opp.getDefense().setGrid(x, y, 7);
            Console.WriteLine("Hit !");
            this.attack.setGrid(x, y, 7);
            // Sunk ?
            if(b.getTouched() == b.getLenght()) {
                Console.WriteLine("Sunk !!! " + this.getName() 
                                    + " sunk the enemy's " + b.getName());
                opp.getBoatsDef().Remove(b);
                this.successShoots.RemoveAt(0);
            }
        }
    }

    private Point chooseShootSimple(Player opp) {
        bool shootOk = false;
        int x = 99, y = 99;
        while(!shootOk) {
            x = new Random().Next(0, 10);
            y = new Random().Next(0, 10);
            // To be sure to shoot a non shot case
            if(opp.getDefense().getGrid()[x, y] != 6 &&
                opp.getDefense().getGrid()[x, y] != 7)
                shootOk = true;
        }
        return new Point(x, y);
    }

    private Point chooseShootMedium(Player opp) {
        if(this.successShoots.Count == 0) {
            return chooseShootSimple(opp);
        } else {
            if(isCaseShotAllAround(opp, this.successShoots[0])) {
                this.successShoots.RemoveAt(0);
                return chooseShootSimple(opp);
            } else {
                return getNextShoot(opp);
            }
        }
    }

    private Point getNextShoot(Player opp) {
        Console.WriteLine("getNextShoot()");
        displaySuccess();
        Point p = this.successShoots[0];
        Console.WriteLine("p : " + p.ToString());
        int[,] g = opp.getDefense().getGrid();
        int x = p.getX(), y = p.getY();
        
        if(x == 0 && y == 0) {
            if(g[x + 1, y] != 6 || g[x + 1, y] != 7) {
                return new Point(x + 1, y);
            } else if(g[x, y + 1] != 6 || g[x, y + 1] != 7) {
                return new Point(x, y + 1);
            }
        }
        else if(x == 0) {
            if(g[x + 1, y] != 6 || g[x + 1, y] != 7) { 
                return new Point(x + 1, y);
            } else if(g[x, y + 1] != 6 || g[x, y + 1] != 7) {
                return new Point(x, y + 1);
            } else if(g[x, y - 1] != 6 || g[x, y - 1] != 7) {
                return new Point(x, y - 1);
            }
        }
        else if(y == 0) {
            if(g[x + 1, y] != 6 || g[x + 1, y] != 7) {
                return new Point(x + 1, y);
            } else if(g[x - 1, y] != 6 || g[x - 1, y] != 7) {
                return new Point(x - 1, y);
            } else if(g[x, y + 1] != 6 || g[x, y + 1] != 7) {
                return new Point(x, y + 1);
            }
        }

        if(x == 9 && y == 9) {
            if(g[x - 1, y] != 6 || g[x - 1, y] != 7) {
                return new Point(x - 1, y);
            } else if(g[x, y - 1] != 6 || g[x, y - 1] != 7) {
                return new Point(x, y - 1);
            }
        }
        else if(x == 9) {
            if(g[x - 1, y] != 6 || g[x - 1, y] != 7) {
                return new Point(x - 1, y);
            } else if(g[x, y + 1] != 6 || g[x, y + 1] != 7) {
                return new Point(x, y + 1);
            } else if(g[x, y - 1] != 6 || g[x, y - 1] != 7) {
                return new Point(x, y - 1);
            }
        }
        else if(y == 9) {
            if(g[x + 1, y] != 6 || g[x + 1, y] != 7) {
                return new Point(x + 1, y);
            } else if(g[x - 1, y] != 6 || g[x - 1, y] != 7) {
                return new Point(x - 1, y);
            } else if(g[x, y - 1] != 6 || g[x, y - 1] != 7) {
                return new Point(x, y - 1);
            }
        }

        if(g[x + 1, y] != 6 || g[x + 1, y] != 7) {
            return new Point(x + 1, y);
        } else if(g[x - 1, y] != 6 || g[x - 1, y] != 7) {
            return new Point(x - 1, y);
        } else if(g[x, y + 1] != 6 || g[x, y + 1] != 7) {
            return new Point(x, y + 1);
        }else if(g[x, y - 1] != 6 || g[x, y - 1] != 7)
            return new Point(x, y - 1);
        return p;
    }

    private bool isCaseShotAllAround(Player opp, Point p) {
        Grid g = opp.getDefense();
        int x = p.getX(), y = p.getY();

        if(x == 0 && y == 0) {
            if((g.getGrid()[x + 1, y] != 6 || g.getGrid()[x + 1, y] != 7) && 
                (g.getGrid()[x, y + 1] != 6 || g.getGrid()[x, y + 1] != 7))
                return false;
        }
        else if(x == 0) {
            if((g.getGrid()[x + 1, y] != 6 || g.getGrid()[x + 1, y] != 7) && 
                (g.getGrid()[x, y + 1] != 6 || g.getGrid()[x, y + 1] != 7) &&
                (g.getGrid()[x, y - 1] != 6 || g.getGrid()[x, y - 1] != 7))
                return false;
        }
        else if(y == 0) {
            if((g.getGrid()[x + 1, y] != 6 || g.getGrid()[x + 1, y] != 7) && 
                (g.getGrid()[x - 1, y] != 6 || g.getGrid()[x - 1, y] != 7) &&
                (g.getGrid()[x, y + 1] != 6 || g.getGrid()[x, y + 1] != 7))
                return false;
        }

        if(x == 9 && y == 9) {
            if((g.getGrid()[x - 1, y] != 6 || g.getGrid()[x - 1, y] != 7) &&
                (g.getGrid()[x, y - 1] != 6 || g.getGrid()[x, y - 1] != 7))
                return false;
        }
        else if(x == 9) {
            if((g.getGrid()[x - 1, y] != 6 || g.getGrid()[x - 1, y] != 7) &&
                (g.getGrid()[x, y + 1] != 6 || g.getGrid()[x, y + 1] != 7) &&
                (g.getGrid()[x, y - 1] != 6 || g.getGrid()[x, y - 1] != 7))
                return false;
        }
        else if(y == 9) {
            if((g.getGrid()[x + 1, y] != 6 || g.getGrid()[x + 1, y] != 7) && 
                (g.getGrid()[x - 1, y] != 6 || g.getGrid()[x - 1, y] != 7) &&
                (g.getGrid()[x, y - 1] != 6 || g.getGrid()[x, y - 1] != 7))
                return false;
        }

        if((g.getGrid()[x + 1, y] != 6 || g.getGrid()[x + 1, y] != 7) && 
            (g.getGrid()[x - 1, y] != 6 || g.getGrid()[x - 1, y] != 7) &&
            (g.getGrid()[x, y + 1] != 6 || g.getGrid()[x, y + 1] != 7) &&
            (g.getGrid()[x, y - 1] != 6 || g.getGrid()[x, y - 1] != 7))
            return false;

        return true;
    }

    public override string ToString() {
        string ret = "BotPlayer : " + this.name + " - Level " + this.level + "\n";
        ret += getAllBoatsPos();
        ret += getAllBoatsDef();
        ret += "Lifes left : " + this.boatsDef.Count;
        return ret;
    }

    public int getLevel() {
        return this.level;
    }

    private void displaySuccess() {
        foreach(Point p in this.successShoots)
            Console.WriteLine(p.ToString());
    }
}
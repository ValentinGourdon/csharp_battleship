using System;
using System.Collections.Generic;

public class BotPlayer : Player{
    private int level;

    public BotPlayer(string name,  Grid attack, Grid defense, List<Boat> boatsPos, List<Boat> boatsDef, int level) {
        this.name = name;
        this.attack = attack;
        this.defense = defense;
        this.boatsPos = boatsPos;
        this.boatsDef = boatsDef;
        this.level = level;
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

    public void autoShoot(Player opp) {
        Console.Clear();
        this.displayBothGrid();
        int x = 99, y = 99;
        bool shootOk = false;

        while(!shootOk) {
            x = new Random().Next(0, 10);
            y = new Random().Next(0, 10);
            // To be sure to shoot a non shot case
            if(opp.getDefense().getGrid()[x, y] != 6 &&
                opp.getDefense().getGrid()[x, y] != 7)
                shootOk = true;
        }

        Console.WriteLine("Player " + this.name + " shoot at " + opp.getName());
        Console.WriteLine("Targeting " + new Grid().XYToString(x, y) + "...");
        System.Threading.Thread.Sleep(500);
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
            b.setTouched();
            opp.getDefense().setGrid(x, y, 7);
            Console.WriteLine("Hit !");
            this.attack.setGrid(x, y, 7);
            // Sunk ?
            if(b.getTouched() == b.getLenght()) {
                Console.WriteLine("Sunk !!! " + this.getName() 
                                    + " sunk the enemy's " + b.getName());
                opp.getBoatsDef().Remove(b);
            }
        }
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
}
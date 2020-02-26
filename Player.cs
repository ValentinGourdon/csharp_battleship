using System;
using System.Collections.Generic;

public class Player {
    protected string name;
    protected Grid attack;
    protected Grid defense;
    protected List<Boat> boatsPos;
    protected List<Boat> boatsDef;

    public Player() {}
    
    public Player(string name, Grid attack, Grid defense, List<Boat> boatsPos, List<Boat> boatsDef) {
        this.name = name;
        this.attack = attack;
        this.defense = defense;
        this.boatsPos = boatsPos;
        this.boatsDef = boatsDef;
    }

    public void placeAllBoats() {
        Console.WriteLine(this.name + ", please place all your boats on the grid.");
        while(this.boatsPos.Count > 0) {
            this.defense.display();
            Boat boat = getBoatFromChoice();
            if(boat != null) {
                if(placeBoat(boat)) {   // Wrong input managed in placeBoat()
                    this.boatsPos.Remove(boat);
                    Console.Clear();
                }
            } else 
                Console.WriteLine("Wrong input.");
        }
        this.defense.display();
        Console.WriteLine("Thank you " + this.name + ". Press any touch to continue.");
        Console.ReadKey();
        Console.Clear();
    }

    public bool placeBoat(Boat b) {
        Console.WriteLine("Please enter the position of your " + b.getName()
                            + ". (between A1 and J10)");
        string input = Console.ReadLine();
        int[] pos = new Grid().translateCoordinates(input);
        if(pos[0] != 99 && pos[1] != 99) {
            Console.WriteLine("In which position do you want to place your boat : Horizontal (H) or Vertical (V)");
            string inputPos = Console.ReadLine();
            if(inputPos == "Horizontal" || inputPos == "H" || inputPos == "h")
                b.setPosition(true);
            else if(inputPos == "Vertical" || inputPos == "V" || inputPos == "v")
                b.setPosition(false);
            else {
                Console.WriteLine("Wrong position.");
                return false;
            }
            if(this.defense.placeBoat(b, pos[0], pos[1], true))
                return true;
            else {
                Console.WriteLine("Can't place the boat.");
                return false;
            }
        } else {
            Console.WriteLine("Position out of the grid bounds.");
            return false;
        }
    }

    private Boat getBoatFromChoice() {
        Boat b = null;
        Console.WriteLine(getAllBoatsPos() + "Enter your choice :");
        string input = Console.ReadLine();
        if(input != "1" && input != "2" && input != "3" && input != "4" && input != "5")
            return b;
        int choice = int.Parse(input);
        switch(choice) {
            case 1:
                b = this.boatsPos.Find(x => x.getId() == choice);
                if(b != null)
                    Console.WriteLine(b.ToString());
                break;
            case 2:
                b = this.boatsPos.Find(x => x.getId() == choice);
                if(b != null)
                    Console.WriteLine(b.ToString());
                break;
            case 3:
                b = this.boatsPos.Find(x => x.getId() == choice);
                if(b != null)
                    Console.WriteLine(b.ToString());
                break;
            case 4:
                b = this.boatsPos.Find(x => x.getId() == choice);
                if(b != null)
                    Console.WriteLine(b.ToString());
                break;
            case 5:
                b = this.boatsPos.Find(x => x.getId() == choice);
                if(b != null)
                    Console.WriteLine(b.ToString());
                break;
        }
        return b;
    }

    public bool shoot(Player opp, string shootInput) {
        int[] pos = new Grid().translateCoordinates(shootInput);
        int x = pos[0], y = pos[1];
        if(pos[0] == 99 || pos[1] == 99)
            return false;
        Console.WriteLine("Targeting " + shootInput +"...");
        System.Threading.Thread.Sleep(1000);
        if(opp.getDefense().getGrid()[x, y] == 0) {
            opp.getDefense().setGrid(x, y, 6);
            Console.WriteLine("Missed...");
            this.attack.setGrid(x, y, 6);
        } else if(opp.getDefense().getGrid()[x, y] >= 1 && opp.getDefense().getGrid()[x, y] <= 5){
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
                Console.WriteLine("Sunk !!! Congratulation you've sunk " 
                                    + opp.getName() + "'s "+ b.getName());
                opp.getBoatsDef().Remove(b);
            }
        } else {
            Console.WriteLine("Impossible to shoot here, already targeted.");
            return false;
        }
        return true;
    }

    public override string ToString() {
        string ret = "Player : " + this.name + "\n";
        ret += getAllBoatsPos();
        ret += getAllBoatsDef();
        ret += "Lifes left : " + this.boatsDef.Count;
        return ret;
    }

    public string getAllBoatsPos() {
        string ret = "List of boats available to place :\n";
        for(int i = 0; i < boatsPos.Count; i++) {
            ret += this.boatsPos[i].ToString() + "\n";
        }
        return ret;
    }

    public string getAllBoatsDef() {
        string ret = "List of boats alive :\n";
        for(int i = 0; i < boatsDef.Count; i++) {
            ret += this.boatsDef[i].ToString() + "\n";
        }
        return ret;
    }

    public Grid getAttack() {
        return this.attack;
    }

    public Grid getDefense() {
        return this.defense;
    }

    public List<Boat> getBoatsPos() {
        return this.boatsPos;
    }

    public List<Boat> getBoatsDef() {
        return this.boatsDef;
    }

    public string getName() {
        return this.name;
    }

    public bool isAlive() {
        if(this.boatsDef.Count > 0)
            return true;
        else
            return false;
    }
    public void displayBothGrid() {
        Console.WriteLine("Player " + this.name + "\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write(this.defense.getType() + " :");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("                       " + this.attack.getType() + " :\n");
        string space = "        ";
        
        // 0 => nothing ( ), 1 => boat vertical(|), 2 => boat horizontal (-)
        // 3 => missed (o), 4 => boat touched (*)
        string strD = "";
        string strA = "";
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("    A B C D E F G H I J");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(space + "     A B C D E F G H I J\n");
        for(int i = 0; i < this.defense.getWidth(); i++) {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("    - - - - - - - - - - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(space + "    - - - - - - - - - - \n");
            if(i != 9) {
                strD += (i+1) + "  |";
                strA += (i+1) + "  |";
            }
            else {
                strD += (i+1) + " |";
                strA += (i+1) + " |";
            }
            for(int j = 0; j < this.defense.getHeight(); j++) {
                switch(this.defense.getGrid()[i, j]) {
                    case 0 :
                        strD += " |";
                        break;
                    case 1:
                        strD += "C|";
                        break;
                    case 2:
                        strD += "B|";
                        break;
                    case 3:
                        strD += "c|";
                        break;
                    case 4:
                        strD += "S|";
                        break;
                    case 5:
                        strD += "D|";
                        break;
                    case 6:
                        strD += "o|";
                        break;
                    case 7:
                        strD += "*|";
                        break;
                }
                switch(this.attack.getGrid()[i, j]) {
                    case 0 :
                        strA += " |";
                        break;
                    case 6:
                        strA += "o|";
                        break;
                    case 7:
                        strA += "*|";
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(strD);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(space + strA + "\n");
            strD = "";
            strA = "";
        }
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write("    - - - - - - - - - - ");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write(space + "    - - - - - - - - - - \n");
        Console.ResetColor();
    }
}
using System;
using System.Threading;

public class Grid {
    private string type;
    private int width;
    private int height;
    // 0 => nothing, 1 => boat vertical, 2 => boat horizontal
    // 3 => missed, 4 => boat touched
    private int[,] grid;
    private int nbBoatsPlaced;
    private readonly int nbBoatsMax = 5;

    public Grid(string type) {
        this.type = type;
        this.width = 10;
        this.height = 10;
        this.grid = new int[width,height];
        nbBoatsPlaced = 0;
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++)
                grid[i, j] = 0;
        }
    }

    public Grid() {}

    public void displayRaw() {
        string raw = this.type + " :\n";
        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                raw += this.grid[i, j];
            }
            Console.WriteLine(raw + "\n");
            raw = "";
        }
    }

    public void displayEmpty() {
        Console.WriteLine(this.type + " :\n");
        Console.WriteLine("    A B C D E F G H I J");
        for(int i = 0; i < this.height; i++) {
            if (i != 9) {
                Console.WriteLine("    - - - - - - - - - - ");
                Console.WriteLine((i+1) + "  | | | | | | | | | | |");
            } else {
                Console.WriteLine("    - - - - - - - - - - ");
                Console.WriteLine((i+1) + " | | | | | | | | | | |");
            }
        }
        Console.WriteLine("    - - - - - - - - - - ");
    }

    public void display() {
        if(this.type == "Attack") {
            Console.ForegroundColor = ConsoleColor.Red;
        } else if(this.type == "Defense") {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        Console.WriteLine(this.type + " :\n");
        // 0 => nothing ( ), 1 => boat vertical(|), 2 => boat horizontal (-)
        // 3 => missed (o), 4 => boat touched (*)
        string str = "";
        Console.WriteLine("    A B C D E F G H I J");
        for(int i = 0; i < this.width; i++) {
            Console.WriteLine("    - - - - - - - - - - ");
            if(i != 9)
                str += (i+1) + "  |";
            else
                str += (i+1) + " |";
            for(int j = 0; j < this.height; j++) {
                switch(this.grid[i, j]) {
                    case 0 :
                        str += " |";
                        break;
                    case 1:
                        str += "C|";
                        break;
                    case 2:
                        str += "B|";
                        break;
                    case 3:
                        str += "c|";
                        break;
                    case 4:
                        str += "S|";
                        break;
                    case 5:
                        str += "D|";
                        break;
                    case 6:
                        str += "o|";
                        break;
                    case 7:
                        str += "*|";
                        break;
                }
            }
            Console.WriteLine(str);
            str = "";
        }
        Console.WriteLine("    - - - - - - - - - - ");
        Console.ResetColor();
    }

    public bool placeBoat(Boat boat, int x, int y, bool mode) {
        // 0 => nothing, 1 => Carrier, 2 => Battleship
        // 3 => Cruiser, 4 => Submarine, 5 => Destroyer
        if(this.nbBoatsPlaced >= this.nbBoatsMax)
            return false;
        if(x < 0 || y < 0 || x > this.width || y > this.height)
            return false;
        // position : true = horizontal, false = vertical
        if(boat.getPosition()) {
            if(isPlaceFree(boat, x, y, mode)) {
                for(int i = y; i < (y + boat.getLenght()); i++) {
                    switch(boat.getName()) {
                        case "Carrier":
                            this.grid[x, i] = 1;
                            break;
                        case "Battleship":
                            this.grid[x, i] = 2;
                            break;
                        case "Cruiser":
                            this.grid[x, i] = 3;
                            break;
                        case "Submarine":
                            this.grid[x, i] = 4;
                            break;
                        case "Destroyer":
                            this.grid[x, i] = 5;
                            break;
                    }
                }
            } else {
                if(mode) {
                    Console.WriteLine("Impossible to place the " 
                                    + boat.getName() +" at the position "
                                    + "[" + x + ";" + y +"]");
                }
                return false;
            }
        } else {
            if(isPlaceFree(boat, x, y, mode)) {
                for(int i = x; i < (x + boat.getLenght()); i++) {
                    switch(boat.getName()) {
                        case "Carrier":
                            this.grid[i, y] = 1;
                            break;
                        case "Battleship":
                            this.grid[i, y] = 2;
                            break;
                        case "Cruiser":
                            this.grid[i, y] = 3;
                            break;
                        case "Submarine":
                            this.grid[i, y] = 4;
                            break;
                        case "Destroyer":
                            this.grid[i, y] = 5;
                            break;
                    }
                }
            } else {
                if(mode) {
                    Console.WriteLine("Impossible to place the " 
                                    + boat.getName() +" at the position "
                                    + "[" + x + ";" + y +"]");
                }
                return false;
            }
        }
        this.nbBoatsPlaced++;
        return true;
    }

    private bool isPlaceFree(Boat boat, int x, int y, bool mode) {
        // position : true = horizontal, false = vertical
        if(boat.getPosition()) {
            // if the boat is out of bonds
            if((y + boat.getLenght()) > this.width) {
                if(mode)
                    Console.WriteLine("Boat placed out of bonds.");
                return false;
            }
            // if the boat steps on an occupied place
            for(int i = y; i < (y + boat.getLenght()); i++) {
                if(this.grid[x, i] != 0)
                    return false;
            }
        } else {
            // if the boat is out of bonds
            if((x + boat.getLenght()) > this.height){
                if(mode)
                    Console.WriteLine("Boat placed out of bonds.");
                return false;
            }
            // if the boat steps on an occupied place
            for(int i = x; i < (x + boat.getLenght()); i++) {
                if(this.grid[i, y] != 0)
                    return false;
            }
        }
        return true;
    }

    public int[] translateCoordinates(string s) {
        s = s.ToUpper();
        int[] ret = new int[2]{99,99};
        // if the coordinates is like "A1" or "1A"
        // and the letter is between A and J 
        // and the number is between 1 and 10 it's OK.
        if(s.Length == 2) {
            if(Char.IsLetter(s[0]) && Char.IsNumber(s[1])) {
                ret[1] = charToInt(s[0]);
                ret[0] = (int)Char.GetNumericValue(s[1]) - 1;
            } else if(Char.IsLetter(s[1]) && Char.IsNumber(s[0])){
                ret[1] = charToInt(s[1]);
                ret[0] = (int)Char.GetNumericValue(s[0]) - 1;
            } else {
                Console.WriteLine("Wrong coordinate for " + s);
                return ret;
            }
        } else if(s.Length == 3) {
            if(Char.IsLetter(s[0]) && Char.IsNumber(s[1]) && Char.IsNumber(s[2])) {
                if(Char.IsLetter(s[0]) && s[1] == '1' && s[2] == '0') {
                    ret[1] = charToInt(s[0]);
                    ret[0] = 9;
                }
            } else if (Char.IsNumber(s[0]) && Char.IsNumber(s[1]) && Char.IsLetter(s[2])) {
                if(s[0] == '1' && s[1] == '0' && Char.IsLetter(s[2])) {
                    ret[1] = charToInt(s[2]);
                    ret[0] = 9;
                }
            } else {
                Console.WriteLine("Wrong coordinate for " + s);
                return ret;
            }
        }  
        return ret;
    }

    private int charToInt(char c) {
        int i = 99;
            switch(c) {
                case 'A' :
                case 'a' :
                    i = 0;
                    break;
                case 'B' :
                case 'b' :
                    i = 1;
                    break;
                case 'C' :
                case 'c' :
                    i = 2;
                    break;
                case 'D' :
                case 'd' :
                    i = 3;
                    break;
                case 'E' :
                case 'e' :
                    i = 4;
                    break;
                case 'F' :
                case 'f' :
                    i = 5;
                    break;
                case 'G' :
                case 'g' :
                    i = 6;
                    break;
                case 'H' :
                case 'h' :
                    i = 7;
                    break;
                case 'I' :
                case 'i' :
                    i = 8;
                    break;
                case 'J' :
                case 'j' :
                    i = 9;
                    break;
            }
        return i;
    }

    public string XYToString(int x, int y) {
        string ret = "";
        switch(y) {
            case 0 :
                ret += "A";
                break;
            case 1 :
                ret += "B";
                break;
            case 2 :
                ret += "C";
                break;
            case 3 :
                ret += "D";
                break;
            case 4 :
                ret += "E";
                break;
            case 5 :
                ret += "F";
                break;
            case 6 :
                ret += "G";
                break;
            case 7 :
                ret += "H";
                break;
            case 8 :
                ret += "I";
                break;
            case 9 :
                ret += "J";
                break;
        }
        ret += x;
        return ret;
    }

    public int[,] getGrid() {
        return this.grid;
    }

    public void setGrid(int x, int y, int shoot) {
        this.grid[x, y] = shoot;
    }

    public int getWidth() {
        return this.width;
    }

    public int getHeight() {
        return this.height;
    }

    public string getType() {
        return this.type;
    }
}
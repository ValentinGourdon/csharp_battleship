using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueGame = true;
            while(continueGame) {
                Console.Clear();

                // Build PLAYER 1 fleet
                List<Boat> boatsPosP1 = new List<Boat>();
                List<Boat> boatsDefP1 = new List<Boat>();

                Carrier carrierPosP1 = new Carrier();
                boatsPosP1.Add(carrierPosP1);
                Battleship battleshipPosP1 = new Battleship();
                boatsPosP1.Add(battleshipPosP1);
                Cruiser cruiserPosP1 = new Cruiser();
                boatsPosP1.Add(cruiserPosP1);
                Submarine submarinePosP1 = new Submarine();
                boatsPosP1.Add(submarinePosP1);
                Destroyer destroyerPosP1 = new Destroyer();
                boatsPosP1.Add(destroyerPosP1);

                Carrier carrierDefP1 = new Carrier();
                boatsDefP1.Add(carrierDefP1);
                Battleship battleshipDefP1 = new Battleship();
                boatsDefP1.Add(battleshipDefP1);
                Cruiser cruiserDefP1 = new Cruiser();
                boatsDefP1.Add(cruiserDefP1);
                Submarine submarineDefP1 = new Submarine();
                boatsDefP1.Add(submarineDefP1);
                Destroyer destroyerDefP1 = new Destroyer();
                boatsDefP1.Add(destroyerDefP1);

                Grid gridA_P1 = new Grid("Attack");
                Grid gridD_P1 = new Grid("Defense");


                // Build PLAYER 2 fleet
                List<Boat> boatsPosP2 = new List<Boat>();
                List<Boat> boatsDefP2 = new List<Boat>();

                Carrier carrierPosP2 = new Carrier();
                boatsPosP2.Add(carrierPosP2);
                Battleship battleshipPosP2 = new Battleship();
                boatsPosP2.Add(battleshipPosP2);
                Cruiser cruiserPosP2 = new Cruiser();
                boatsPosP2.Add(cruiserPosP2);
                Submarine submarinePosP2 = new Submarine();
                boatsPosP2.Add(submarinePosP2);
                Destroyer destroyerPosP2 = new Destroyer();
                boatsPosP2.Add(destroyerPosP2);

                Carrier carrierDefP2 = new Carrier();
                boatsDefP2.Add(carrierDefP2);
                Battleship battleshipDefP2 = new Battleship();
                boatsDefP2.Add(battleshipDefP2);
                Cruiser cruiserDefP2 = new Cruiser();
                boatsDefP2.Add(cruiserDefP2);
                Submarine submarineDefP2 = new Submarine();
                boatsDefP2.Add(submarineDefP2);
                Destroyer destroyerDefP2 = new Destroyer();
                boatsDefP2.Add(destroyerDefP2);

                Grid gridA_P2 = new Grid("Attack");
                Grid gridD_P2 = new Grid("Defense");

                Player p1 = null;
                Player p2 = null;

                BotPlayer bp1 = null;
                BotPlayer bp2 = null;

                // Launch the Game
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("WELCOME TO BATTLESHIP 2020 !!!");
                Console.ResetColor();
                string name1 = "Boulet";
                string name2 = "Timmy";
                bool choiceMode = false;
                string gameMode = "";
                while(!choiceMode) {
                    Console.WriteLine("\n1 - Player 1 VS Player 2\n2 - Player 1 VS CPU\n3 - CPU 1 VS CPU 2\nChoose your combat mode :");
                    gameMode = Console.ReadLine();
                    int botLevel = 0;
                    switch(gameMode) {
                        case "1":
                            Console.WriteLine("Player 1, enter your name : ");
                            name1 = Console.ReadLine();
                            
                            Console.WriteLine("Player 2, enter your name : ");
                            name2 = Console.ReadLine();
                            p1 = new Player(name1, gridA_P1, gridD_P1, boatsPosP1, boatsDefP1);
                            p2 = new Player(name2, gridA_P2, gridD_P2, boatsPosP2, boatsDefP2);
                            PvsPMatch(p1, p2);
                            choiceMode = true;
                            break;
                        
                        case "2":
                            Console.WriteLine("Player 1, enter your name : ");
                            name1 = Console.ReadLine();

                            botLevel = chooseBotLevel();
                            p1 = new Player(name1, gridA_P1, gridD_P1, boatsPosP1, boatsDefP1);
                            name2 = getRandomName();
                            bp2 = new BotPlayer(name2, gridA_P2, gridD_P2, boatsPosP2, boatsDefP2, botLevel);
                            Console.Clear();
                            Console.WriteLine(name1 + ", you will play against " 
                                                + name2 + " (lvl " + botLevel + ")");
                            Console.WriteLine("Press any touch to start.");
                            Console.ReadKey();
                            Console.Clear();
                            PvsBMatch(p1, bp2);
                            choiceMode = true;
                            break;


                        case "3" :
                            botLevel = chooseBotLevel();
                            name1 = getRandomName();
                            name2 = getRandomName();
                            bp1 = new BotPlayer(name1, gridA_P1, gridD_P1, boatsPosP1, boatsDefP1, botLevel);
                            bp2 = new BotPlayer(name2, gridA_P2, gridD_P2, boatsPosP2, boatsDefP2, botLevel);
                            Console.WriteLine(name1 + " (lvl " + botLevel + ") will play against " 
                                                + name2 + " (lvl " + botLevel + ")");
                            Console.WriteLine("Press any touch to start.");
                            Console.ReadKey();
                            Console.Clear();
                            BvsBMatch(bp1, bp2);
                            choiceMode = true;
                            break;

                        default:
                            Console.WriteLine("Wrong input.");
                            break;
                    }
                }

                bool inputCorrect = true;
                do {
                    Console.WriteLine("Do you want to play again ? (y/n)");
                    string continueInput = Console.ReadLine();
                    if(continueInput == "y")
                        continueGame = true;
                    else if(continueInput == "n")
                        continueGame = false;
                    else {
                        Console.WriteLine("Wrong input.");
                        inputCorrect = false;
                    }                        
                } while(!inputCorrect);
            }
            Console.Clear();
            Console.WriteLine("Thank you for playing. Bye !");
        }

        public static void PvsPMatch(Player p1, Player p2) {
            Console.Clear();

            p1.placeAllBoats();
            p2.placeAllBoats();

            int count = 0;
            while(p1.isAlive() && p2.isAlive()) {
                bool shootCorrect = false;
                while(!shootCorrect) {
                    p1.displayBothGrid();
                    Console.WriteLine(p1.getName() + ", it's your turn to shoot. Please enter coordinates : (A1 to J10)");
                    string shootInput = Console.ReadLine();
                    if(p1.shoot(p2, shootInput)) {
                        Console.WriteLine("Press any touch to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        shootCorrect = true;
                    }
                }

                if(!p2.isAlive())
                    break;
                
                shootCorrect = false;
                while(!shootCorrect) {
                    p2.displayBothGrid();
                    Console.WriteLine(p2.getName() + ", it's your turn to shoot. Please enter coordinates : (A1 to J10)");
                    string shootInput = Console.ReadLine();
                    if(p2.shoot(p1, shootInput)) {
                        Console.WriteLine("Press any touch to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        shootCorrect = true;
                    }
                }
                count ++;
            }
            if(p1.isAlive()) {
                Console.WriteLine("Congratulations " + p1.getName()
                                    + " you've beaten " + p2.getName() 
                                    + " with " + count + " shoots !");
            } else {
                Console.WriteLine("Congratulations " + p2.getName()
                                    + " you've beaten " + p1.getName() 
                                    + " with " + count + " shoots !");
            }
                            
            Console.WriteLine("Final grids :");
            p1.displayBothGrid();
            Console.WriteLine("\n");
            p2.displayBothGrid();
        }
        
        public static void BvsBMatch(BotPlayer p1, BotPlayer p2) {
            Console.Clear();

            p1.autoPlaceAllBoats();
            p1.displayBothGrid();
            Console.WriteLine("Press any touch to continue.");
            Console.ReadKey();
            Console.Clear();

            p2.autoPlaceAllBoats();
            p2.displayBothGrid();
            Console.WriteLine("Press any touch to continue.");
            Console.ReadKey();
            Console.Clear();

            bool stepContinue = false;
            bool stopByStep = true;
            while(!stepContinue) {
                Console.WriteLine("Do you want to stop at each step ? (Yes /No)");
                string stepInput = Console.ReadLine();
                switch(stepInput) {
                    case "Yes":
                    case "yes":
                    case "Y":
                    case "y":
                        stopByStep = true;
                        stepContinue = true;
                        break;
                    case "No":
                    case "no":
                    case "N":
                    case "n":
                        stopByStep = false;
                        stepContinue = true;
                        break;
                    default:
                        Console.WriteLine("Wrong inut.");
                        break;
                }
            }


            int count = 0;
            while(p1.isAlive() && p2.isAlive()) {
                count++;
                p1.autoShoot(p2, true, p1.getLevel());
                if(stopByStep) {
                    Console.WriteLine("Press any touch to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
                if(!p2.isAlive())
                        break;
                p2.autoShoot(p1, true, p2.getLevel());
                if(stopByStep) {
                    Console.WriteLine("Press any touch to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.Clear();

            if(p1.isAlive()) {
                Console.WriteLine("It's over ! " + p1.getName()
                                    + " has beaten " + p2.getName() 
                                    + " with " + count + " shoots !");
                
            } else {
                Console.WriteLine("It's over ! " + p2.getName()
                                    + " has beaten " + p1.getName() 
                                    + " with " + count + " shoots !");
                
            }
            Console.WriteLine("Press any touch to continue.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Final grids :");
            p1.displayBothGrid();
            Console.WriteLine("\n");
            p2.displayBothGrid();
        }

        public static void PvsBMatch(Player p1, BotPlayer p2) {
            Console.Clear();

            p1.placeAllBoats();
            p2.autoPlaceAllBoats();

            int count = 0;
            while(p1.isAlive() && p2.isAlive()) {
                bool shootCorrect = false;
                while(!shootCorrect) {
                    p1.displayBothGrid();
                    Console.WriteLine(p1.getName() + ", it's your turn to shoot. Please enter coordinates : (A1 to J10)");
                    string shootInput = Console.ReadLine();
                    if(p1.shoot(p2, shootInput)) {
                        Console.WriteLine("Press any touch to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        shootCorrect = true;
                    }
                }

                if(!p2.isAlive())
                    break;
                
                shootCorrect = false;
                while(!shootCorrect) {
                    p2.autoShoot(p1, false, p2.getLevel());
                    Console.WriteLine("Press any touch to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    shootCorrect = true;
                }
                count ++;
            }
            if(p1.isAlive()) {
                Console.WriteLine("Congratulations " + p1.getName()
                                    + " you've beaten " + p2.getName() 
                                    + " with " + count + " shoots !");
            } else {
                Console.WriteLine("Oh no " + p2.getName()
                                    + " ! You've lost against " + p1.getName() 
                                    + " in " + count + " shoots !");
            }

            Console.WriteLine("Press any touch to continue.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Final grids :");
            p1.displayBothGrid();
            Console.WriteLine("\n");
            p2.displayBothGrid();
        }

        private static int chooseBotLevel() {
            int level = 0;
            bool choice = false;
            while(!choice) {
                Console.WriteLine("Please choose the CPU level :\n"
                                + "\t1 - Easy (E)\n\t2 - Medium (M)");
                string input = Console.ReadLine();
                switch(input) {
                    case "1":
                    case "Easy":
                    case "easy":
                    case "E":
                    case "e":
                        level = 1;
                        choice = true;
                        break;
                    case "2":
                    case "Medium":
                    case "medium":
                    case "M":
                    case "m":
                        level = 2;
                        choice = true;
                        break;
                    default:
                        Console.WriteLine("Wrong input.");
                        break;
                }
            }
            Console.Clear();
            return level;
        }

        public static string getRandomName() {
            List<string> names = new List<string>() {
            "Michel",
            "Johnny",
            "Paul",
            "Jacqueline",
            "Odette",
            "Giselle",
            "Timmy",
            "Kimberly",
            "John",
            "Stacy",
            "Dylan",
            "Arnold",
            "Jennifer" };
            
            return names[new Random().Next(names.Count)];
        }
    }

    
}

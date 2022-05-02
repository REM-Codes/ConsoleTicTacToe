using System.Collections.Generic;

namespace TicTacToe
{
    class Program
    {
        private static Game Instance;

        public static void Main()
        {
            CreateInstance();
            while(!Instance.IsGameEnded) 
            {
                Instance.Update();
            }
            Instance.DisplayGameEnd(Instance.Winner);
        }

        private static void CreateInstance()
        {
            Instance = new Game();
        }
    }

    class Game
    {
        private static Graphics Graphics;
        private string CurrentPlayer = "O";
        public bool IsGameEnded { get
            {
                return _IsGameEnded();
            }
            private set { }
        }
        private static string _Winner = "-";
        public string Winner { get { return _Winner; } }

        public Game()
        {
            Initialize();
        }

        private static bool _IsGameEnded()
        {
            for(int i = 0; i < 3; i++)
            {
                if(Graphics.GetBoard()[i, 0] == Graphics.GetBoard()[i, 1] && Graphics.GetBoard()[i, 1] == Graphics.GetBoard()[i, 2] && Graphics.GetBoard()[i, 2] != "-")
                {
                    _Winner = Graphics.GetBoard()[i, 0] + " wins!";
                    return true;
                }
            }
            for(int i = 0; i < 3; i++)
            {
                if (Graphics.GetBoard()[0, i] == Graphics.GetBoard()[1, i] && Graphics.GetBoard()[1, i] == Graphics.GetBoard()[2, i] && Graphics.GetBoard()[2, i] != "-")
                {
                    _Winner = Graphics.GetBoard()[0, i] + " wins!";
                    return true;
                }
            }
            if(Graphics.GetBoard()[0,0] == Graphics.GetBoard()[1,1] && Graphics.GetBoard()[1, 1] == Graphics.GetBoard()[2, 2] && Graphics.GetBoard()[2, 2] != "-")
            {
                _Winner = Graphics.GetBoard()[0, 0] + " wins!";
                return true;
            }
            if (Graphics.GetBoard()[0, 2] == Graphics.GetBoard()[1, 1] && Graphics.GetBoard()[1, 1] == Graphics.GetBoard()[2, 0] && Graphics.GetBoard()[2, 0] != "-")
            {
                _Winner = Graphics.GetBoard()[1, 1] + " wins!";
                return true;
            }
            int number = 0;
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(Graphics.GetBoard()[i, j] != "-")
                    {
                        number++;
                    }
                }
            }
            if(number == 9)
            {
                _Winner = "it's a tie!";
                return true;
            }
            return false;
        }

        public void Initialize()
        {
            Graphics = new Graphics(this);
            Graphics.Initialize();
        }

        public void Update()
        {
            Graphics.Update();
            SwitchCurrentPlayer();
            GetInput();
        }

        public void DisplayGameEnd(string result)
        {
            Graphics.DisplayGameEnd(result);
        }

        private void GetInput()
        {
            Console.WriteLine();
            Console.Write(CurrentPlayer + "'s move: ");
            string input = Console.ReadLine();
            while (InputInvalid(input))
            {
                Graphics.Update();
                Console.WriteLine();
                Console.WriteLine("Invalid Input");
                Console.Write(CurrentPlayer + "'s move: ");
                input = Console.ReadLine()!;
            }
            Graphics.UpdateBoard(Validate(DecompileString(input)), CurrentPlayer);
        }

        private bool InputInvalid(string input)
        {
            if (input == null)
            {
                return true;
            }
            if (input.Length != 2)
            {
                return true;
            }
            if (DecompileString(input)[0] == 0)
            {
                return true;
            }
            if (DecompileString(input)[1] == 0)
            {
                return true;
            }
            if (Graphics.GetBoard()[Validate(DecompileString(input))[0], Validate(DecompileString(input))[1]] != "-")
            {
                return true;
            }
            return false;
        }

        private void SwitchCurrentPlayer()
        {
            switch (CurrentPlayer)
            {
                case "X":
                    CurrentPlayer = "O";
                    break;
                case "O":
                    CurrentPlayer = "X";
                    break;
            }
        }

        private int[] DecompileString(string target)
        {
            int[] result = new int[2];
            switch (target[0])
            {
                case 'a':
                    result[1] = 1;
                    break;
                case 'b':
                    result[1] = 2;
                    break;
                case 'c':
                    result[1] = 3;
                    break;
                default:
                    result[1] = 0;
                    break;
            }
            switch (target[1])
            {
                case '1':
                    result[0] = 1;
                    break;
                case '2':
                    result[0] = 2;
                    break;
                case '3':
                    result[0] = 3;
                    break;
                default:
                    result[0] = 0;
                    break;
            }
            return result;
        }

        private int[] Validate(int[] coordinates)
        {
            return new int[2] { coordinates[0] - 1, coordinates[1] - 1 };
        }
    }

    class Graphics {
        Game Instance;
        private string[,] Board = new string[,] { { "-", "-", "-" }, { "-", "-", "-" }, { "-", "-", "-" } };

        public Graphics(Game instance)
        {
            Instance = instance;
        }

        public void Initialize() 
        {
            Console.Clear();
            Draw(BuildBoard(Board));
        }

        public void Update()
        {
            Draw(BuildBoard(Board));
        }

        public void UpdateBoard(int[] move, string currPlayer)
        {
            AddMove(move, currPlayer);
        }

        public string[,] GetBoard()
        {
            return Board;
        }

        public void DisplayGameEnd(string result)
        {
            Update();
            Console.WriteLine();
            Console.WriteLine("Game result: " + result);
        }

        private void AddMove(int[] move, string currPlayer)
        {
            Board[move[0], move[1]] = currPlayer;
        }

        private string BuildBoard(string[,] values)
        {
            string Board = "";
            Board = AppendLine("   a     b     c", Board);
            Board = LinePadder(Board);
            Board = DrawLine(0, values, Board);
            Board = LineSpacer(Board);
            Board = LinePadder(Board);
            Board = DrawLine(1, values, Board);
            Board = LineSpacer(Board);
            Board = LinePadder(Board);
            Board = DrawLine(2, values, Board);
            Board = LinePadder(Board);
            return Board;
        }
        
        private void Draw(string Board)
        {
            Console.Clear();
            Console.Write(Board);
        }

        private string DrawLine(int lineIndex, string[,] values, string previous)
        {
            string lineNumber = "" + (lineIndex+1);
            return AppendLine(lineNumber + "  " + values[lineIndex, 0] + "  |  " + values[lineIndex, 1] + "  |  " + values[lineIndex, 2], previous);
        }
        
        private string LineSpacer(string previous)
        {
            return AppendLine(" _____|_____|_____", previous);
        }

        private string LinePadder(string previous)
        {
            return AppendLine("      |     |", previous);
        }
        
        private string AppendLine(string line, string previous)
        {
            return previous += line + "\n";
        }
    }
}
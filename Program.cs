using System;
using System.Threading;

namespace SnakeGame
{
    internal class Program
    {
        #region parameter

        public Random rand = new Random();
        public ConsoleKeyInfo keyPess = new ConsoleKeyInfo();

        private int score, headX, headY, fruitX, fruitY, nTail;
        private int[] tailX = new int[100];
        private int[] tailY = new int[100];

        private const int height = 20;
        private const int width = 60;
        private const int panel = 10;
        private int timeEvolution = 0;
        private string foodType;

        private bool gameOver, reset, isPrinted;

        private string dir, pre_dir;

        #endregion parameter

        //hien thi man hinh khi bat dau game
        private void ShowBanner()
        {
            Console.SetWindowSize(width, height + panel);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.CursorVisible = false;
            Console.WriteLine("GAME");
            Console.WriteLine("Press key to play");
            Console.WriteLine("press Q to Quit");
            Console.WriteLine("press R to Reset");

            keyPess = Console.ReadKey(true);
            if (keyPess.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
        }

        //nhung cai dat ban dau
        private void SetUp()
        {
            dir = "RIGHT"; pre_dir = ""; // buoc di dau tien qua phai
            score = 0; nTail = 3;
            gameOver = reset = isPrinted = false;

            headX = 20; //khong vuot qua width
            headY = 10; //khong duoc vuot qua height
            timeEvolution = rand.Next(2, 4);

            RandomMoi();
        }

        //random diem moi xuat hien
        private void RandomMoi()
        {
            if (timeEvolution != 0) foodType = "foodBase";
            else foodType = "deluxeFood";
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);
        }

        //cap nhat man hinh
        private void Update()
        {
            while (!gameOver)
            {
                CheckInput();
                Logic();
                Render();
                Console.WriteLine("Press 'P' if u wanna pause game");
                if (reset) break;

                //dung man hinh sau
                Thread.Sleep(60);
            }
            if (gameOver) Lose();
        }

        //dieu khien phim
        private void CheckInput()
        {
            while (Console.KeyAvailable)
            {
                keyPess = Console.ReadKey(true);
                // UP, pre_dir = RIGHT
                pre_dir = dir; // luu lai huong di truoc do
                switch (keyPess.Key)
                {
                    case ConsoleKey.Q: Environment.Exit(0); break;
                    case ConsoleKey.P:
                        dir = "PAUSE";
                        Console.WriteLine("Paused");
                        break;

                    case ConsoleKey.LeftArrow:
                        if (pre_dir == "RIGHT")
                        {
                            headY--;
                            headX--;
                            dir = "LEFT";
                        }
                        else
                            dir = "LEFT";
                        break;

                    case ConsoleKey.RightArrow:
                        if (pre_dir == "LEFT")
                        {
                            headY--;
                            headX++;
                            dir = "RIGHT";
                        }
                        else dir = "RIGHT";
                        break;

                    case ConsoleKey.UpArrow:
                        if (dir == "DOWN") continue;
                        dir = "UP";
                        break;

                    case ConsoleKey.DownArrow:
                        if (dir == "UP") continue;
                        dir = "DOWN";
                        break;
                }
            }
        }

        //kiem tra logic
        private void Logic()
        {
            //1 2 3 4 5
            //x 1 2 3 4 5
            int preX = tailX[0], preY = tailY[0]; //(x,y)
            int tempX, tempY;

            if (dir != "PAUSE")
            {
                tailX[0] = headX; tailY[0] = headY;

                for (int i = 1; i < nTail; i++)
                {
                    tempX = tailX[i]; tempY = tailY[i];
                    tailX[i] = preX; tailY[i] = preY;
                    preX = tempX; preY = tempY;
                }
            }
            switch (dir)
            {
                case "RIGHT": headX++; break;
                case "LEFT": headX--; break;
                case "UP": headY--; break;
                case "DOWN": headY++; break;
                case "PAUSE":
                    {
                        while (true)
                        {
                            keyPess = Console.ReadKey(true);
                            if (keyPess.Key == ConsoleKey.P)
                            {
                                break;
                            }
                            return;
                        }
                        dir = pre_dir;
                        break;
                    }
            }
            //kiem tra cham tuong
            if (headX <= 0 || headX >= width - 1 ||
               headY <= 0 || headY >= height - 1)
            {
                gameOver = true;
            }
            else gameOver = false;
            if (headX == fruitX && headY == fruitY)
            {
                if (foodType == "foodBase")
                {
                    score += 10;
                    timeEvolution--;
                }
                else if (foodType == "deluxeFood")
                {
                    score += 30;
                    timeEvolution = rand.Next(2, 4);
                }
                nTail += 1;
            }
            //kiem tra cham duoi
            for (int i = 0; i < nTail; i++)
            {
                if (headX == tailX[i] && headY == tailY[i])
                {
                    gameOver = true;
                }
                if (fruitX == tailX[i] && fruitY == tailY[i])
                {
                    RandomMoi();
                }
            }
        }

        //hien thi doi tuong ra man hinh
        private void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1) // vien tren va duoi
                    {
                        Console.Write("*");
                    }
                    else if (j == 0 || j == width - 1)
                    {
                        Console.Write("*");
                    }
                    else if (j == fruitX && i == fruitY)
                    {
                        if (foodType == "foodBase")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("$");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else if (foodType == "deluxeFood")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("@");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                    }
                    else if (j == headX && i == headY)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        isPrinted = false;
                        for (int k = 0; k < nTail; k++)
                        {
                            if (j == tailX[k] && i == tailY[k])
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("o");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                isPrinted = true;
                            }
                        }
                        if (!isPrinted) Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Diem so: " + score);
        }

        //xu ly khi thua
        private void Lose()
        {
            Console.WriteLine("LOSE");
            Console.WriteLine("press Q to Quit");
            Console.WriteLine("press P to Pause");
            Console.WriteLine("press R to Reset");
            while (true)
            {
                keyPess = Console.ReadKey(true);
                if (keyPess.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
                if (keyPess.Key == ConsoleKey.R)
                {
                    reset = true;
                    break;
                }
            }
        }

        private static void Main(string[] args)
        {
            Program program = new Program();
            program.ShowBanner();
            while (true)
            {
                program.SetUp();
                program.Update();
                Console.Clear();// xoa man hinh hien thi
            }

            //Console.ReadKey();
        }
    }
}
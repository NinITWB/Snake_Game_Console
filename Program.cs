using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Program
    {
        
        #region parameter
        public Random rand = new Random();
        public ConsoleKeyInfo keyPess = new ConsoleKeyInfo();

        int score, headX, headY, fruitX, fruitY, nTail;
        int[] tailX = new int[100];
        int[] tailY = new int[100];

        const int height = 20;
        const int width = 60;
        const int panel = 10;
        int timeEvolution = 0;
        string foodType;
        int mark = 0;

        bool gameOver, reset, isPrinted;
        bool isPaused = false;

        string dir, pre_dir;

        #endregion
        //hien thi man hinh khi bat dau game
        void ShowBanner()
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
        void SetUp()
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
        void RandomMoi()
        {
            if (timeEvolution != 0) foodType = "foodBase";
            else foodType = "deluxeFood";
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);
        }
        //cap nhat man hinh
        void Update()
        {
            while (!gameOver)
            {
                CheckInput();
                Logic();
                Render();
                if (reset) break;
                Console.WriteLine("mark: " + mark + "Direction: " + dir);
               

                //dung man hinh sau 
                Thread.Sleep(60);
              
            }
            if (gameOver) Lose();
        }
        //dieu khien phim
        void CheckInput()
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
                        if (mark == 0) mark = 1;
                        if (mark == 1)
                        {
                            mark = 0;
                            continue;
                        }
                        dir = "PAUSE";
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
        void Logic()
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
                        /*while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("PAUSE");
                            Console.WriteLine("press E to Quit");
                            Console.WriteLine("press P to Pause");
                            Console.WriteLine("press R to Reset");

                            
                            if (keyPess.Key == ConsoleKey.Q)
                            {
                                Environment.Exit(0);
                            }
                            if (keyPess.Key == ConsoleKey.R)
                            {
                                reset = true;
                                break;
                            }
                            
                        }*/
                        if (keyPess.Key == ConsoleKey.P && mark == 1)
                        {
                            return;
                        }
                        if (mark == 0)
                        {
                            dir = pre_dir; 
                        }
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
        void Render()
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
        void Lose()
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

        static void Main(string[] args)
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

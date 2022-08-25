using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;
using System.Threading;
using System.IO;

namespace CourseProject
{
    class Field
    {
        private static Random random = new Random();
        private const string FIELD_PATH = @"C:\field\field.txt";

        private int n;
        private int m;
        private Vector2 snakePosition;
        private int snakeLength;

        private Cell[][] field;

        private bool gameEnded = false;
        private Vector2 foodPosition;

        public Snake snake;
        public Vector2 direction;
        public Vector2? bufferDirection = null;


        public Field()
        {

            if (!this.LoadField())
            {
                this.direction = new Vector2(1, 0);
                this.n = 20;
                this.m = 60;
                this.snakePosition = new Vector2(10, 10);
                this.snakeLength = 3;
                for (int i = 0; i < n; ++i)
                {
                    field[i] = new Cell[m];
                    for (int j = 0; j < m; ++j)
                    {
                        if (i == 0 || i == n - 1 || j == 0 || j == m - 1)
                        {
                            field[i][j] = new WallCell();
                        }
                        else
                        {
                            field[i][j] = new EmptyCell();
                        }
                    }
                }
            }
            this.snake = new Snake(this, this.snakePosition, this.direction, this.snakeLength);
        }

        public void GenerateFood()
        {
            if (this.foodPosition != null)
            {
                this[foodPosition] = new EmptyCell();
            }
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < this.field.Length; ++i)
            {
                for (int j = 0; j < this.field[i].Length; ++j)
                {
                    if (typeof(EmptyCell).IsInstanceOfType(this.field[i][j]) &&
                        !this.snake.snake.Contains(new Vector2(j, i))
                    )
                    {
                        list.Add(new Vector2(j, i));
                    }
                }
            }

            var result = list.ElementAt(random.Next(0, list.Count));
            this.foodPosition = result;

            this.field[(int)Math.Round(result.Y)][(int)Math.Round(result.X)] = new FoodCell();
            Console.SetCursorPosition((int)Math.Round(result.X), (int)Math.Round(result.Y));
            this.field[(int)Math.Round(result.Y)][(int)Math.Round(result.X)].Draw();
        }

        public void Start()
        {
            this.GenerateFood();
            this.Draw();

            new Thread(() =>
            {
                while (true)
                {
                    this.HandleKeyDown(Console.ReadKey(true).Key);
                }
            }).Start();
            Thread.Sleep(2000);
            while (!this.gameEnded)
            {
                Thread.Sleep(400);
                this.Tick();
            }
        }

        private bool LoadField()
        {
            if (!File.Exists(FIELD_PATH)) return false;

            string[] lines = File.ReadAllLines(FIELD_PATH);

            int[] firstLine = lines[0].Split(' ').Select(number => Convert.ToInt32(number)).ToArray();

            this.n = firstLine[0];
            this.m = firstLine[1];

            int[] secondLine = lines[1].Split(' ').Select(number => Convert.ToInt32(number)).ToArray();

            this.snakePosition = new Vector2(secondLine[0], secondLine[1]);
            this.snakeLength = secondLine[2];

            int[] thirdLine = lines[2].Split(' ').Select(number => Convert.ToInt32(number)).ToArray();

            this.direction = new Vector2(thirdLine[0], thirdLine[1]);

            this.field = new Cell[n][];
            for (int i = 0; i < this.n; ++i)
            {
                this.field[i] = new Cell[this.m];
                for (int j = 0; j < this.m; ++j)
                {
                    if (lines[i + 3][j] == '#')
                    {
                        this.field[i][j] = new WallCell();
                    }
                    else
                    {
                        this.field[i][j] = new EmptyCell();
                    }
                }
            }

            return true;
        }

        async public void HandleKeyDown(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                this.bufferDirection = new Vector2(-1, 0);
            }
            else if (key == ConsoleKey.RightArrow)
            {
                this.bufferDirection = new Vector2(1, 0);
            }
            else if (key == ConsoleKey.UpArrow)
            {
                this.bufferDirection = new Vector2(0, -1);
            }
            else if (key == ConsoleKey.DownArrow)
            {
                this.bufferDirection = new Vector2(0, 1);
            }

            if (this.bufferDirection.HasValue && Vector2.Dot(this.bufferDirection.Value, this.direction) != 0)
            {
                this.bufferDirection = null;
            }
        }

        public void EndGame()
        {
            this.gameEnded = true;
            Console.SetCursorPosition(3, Console.WindowHeight - 1);
            Console.Beep();
            Console.WriteLine($"Game ended, your score = {this.snake.snake.Count}");
            Environment.Exit(0);
        }

        public void Tick()
        {
            if (this.bufferDirection != null)
            {
                this.direction = (Vector2)this.bufferDirection;
                this.bufferDirection = null;
            }

            if (this.snake.snake.Contains(this.snake.Head + this.direction))
            {
                this.EndGame();
                return;
            }

            var prevTail = this.snake.Tail;

            this[this.snake.Head + this.direction].Interact(this);

            Console.SetCursorPosition((int)Math.Round(this.snake.Head.X), (int)Math.Round(this.snake.Head.Y));
            this.snake.Draw();

            Console.SetCursorPosition((int)Math.Round(prevTail.X), (int)Math.Round(prevTail.Y));
            this[prevTail].Draw();
        }

        public void Draw()
        {
            Console.SetWindowSize(m + 1, n + 1);
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    this.field[i][j].Draw();
                }
                Console.WriteLine();
            }

            foreach (var snakePostiion in this.snake.snake)
            {
                Console.SetCursorPosition((int)Math.Round(snakePostiion.X), (int)Math.Round(snakePostiion.Y));
                this.snake.Draw();
            }
        }

        private Cell this[Vector2 pos]
        {
            get => this.field[(int)Math.Round(pos.Y)][(int)Math.Round(pos.X)];
            set { this.field[(int)Math.Round(pos.Y)][(int)Math.Round(pos.X)] = value; }
        }
    }
}
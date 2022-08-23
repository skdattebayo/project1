using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;
using System.Threading;

namespace CourseProject
{
    class Field
    {
        private static Random random = new Random();

        const int n = 20;
        const int m = 60;

        private Cell[][] field = new Cell[n][];

        private bool gameEnded = false;
        private Vector2 foodPosition;

        public Snake snake;
        public Vector2 direction = new Vector2(1, 0);
        public Vector2? bufferDirection = null;


        public Field()
        {
            this.snake = new Snake(this, new Vector2(10, 10), this.direction, 3);
            for (int i = 0; i < n; ++i)
            {
                field[i] = new Cell[m];
                for (int j = 0; j < m; ++j)
                {
                    if (i == 0 || i == n - 1 || j == 0 || j == m - 1)
                    {
                        field[i][j] = new WallCell();
                    } else
                    {
                        field[i][j] = new EmptyCell();
                    }
                }
            }
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

            while (!this.gameEnded) {
                Thread.Sleep(100);
                this.Tick();
            }
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
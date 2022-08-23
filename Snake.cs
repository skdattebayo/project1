using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CourseProject
{
    class Snake
    {
        public LinkedList<Vector2> snake = new LinkedList<Vector2>();
        private Field field;

        public Snake(Field field, Vector2 initialPosition, Vector2 snakeDirection, int snakeLength)
        {
            this.field = field;
            for (int i = 0; i < snakeLength; i++)
            {
                snake.AddLast(initialPosition + snakeDirection * i);
            }
        }

        public void Move(Vector2 direction)
        {
            this.snake.RemoveFirst();
            this.snake.AddLast(this.snake.Last.Value + direction);
        }

        public void Eat(Vector2 direction)
        {
            this.snake.AddLast(this.snake.Last.Value + direction);
        }

        public void Draw()
        {
            Console.Write("@");
        }

        public Vector2 Head  {
            get => this.snake.Last.Value;
        }

        public Vector2 Tail
        {
            get => this.snake.First.Value;
        }

        public IEnumerable<Vector2> snakePositions => this.snake;
    }
}

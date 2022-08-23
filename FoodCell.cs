using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject
{
    class FoodCell : Cell
    {
        public override void Draw()
        {
            Console.Write("&");
        }

        public override void Interact(Field field)
        {
            field.snake.Eat(field.direction);
            field.GenerateFood();
        }
    }
}

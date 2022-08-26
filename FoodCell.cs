using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CourseProject
{
    class FoodCell : Cell
    {
        public override void Draw(Graphics graphics, int xCellStart, int yCellStart, int squareSide)
        {
            graphics.FillEllipse(Brushes.Orange, xCellStart, yCellStart,squareSide, squareSide);
        }

        public override void Interact(Field field)
        {
            field.snake.Eat(field.direction);
            field.GenerateFood();
        }
    }
}

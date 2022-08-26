using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CourseProject
{
    class EmptyCell : Cell
    {
        public override void Draw(Graphics graphics, int xCellStart, int yCellStart, int squareSide)
        {
            graphics.FillRectangle(Brushes.White, xCellStart, yCellStart, squareSide, squareSide);
        }

        public override void Interact(Field field)
        {
            field.snake.Move(field.direction);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CourseProject
{
    class WallCell : Cell
    {
        public override void Draw(Graphics graphics, int xCellStart, int yCellStart, int squareSide)
        {
            graphics.FillRectangle(Brushes.Black, xCellStart, yCellStart, squareSide, squareSide);
        }

        public override void Interact(Field field)
        {
            field.EndGame();
        }
    }
}

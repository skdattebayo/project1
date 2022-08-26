using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CourseProject
{
    abstract class Cell
    {
        public abstract void Draw(Graphics graphics, int xCellStart, int yCellStart, int squareSide);

        public abstract void Interact(Field field);
    }
}

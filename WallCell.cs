using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject
{
    class WallCell : Cell
    {
        public override void Interact(Field field)
        {
            field.EndGame();
        }

        public override void Draw()
        {
            Console.Write("#");
        }
    }
}

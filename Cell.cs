using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject
{
    abstract class Cell
    {
        public abstract void Draw();

        public abstract void Interact(Field field);
    }
}

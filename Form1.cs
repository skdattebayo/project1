using CourseProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProject1
{
    public partial class Form1 : Form
    {
        public readonly Field field;
        public Graphics graphics;

        public Form1(Field field)
        {
            InitializeComponent();
            this.field = field;

            field.SetFormSize(this);
            field.Start(this);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.field.Draw(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.field.HandleKeyDown(e.KeyCode);
        }
    }
}

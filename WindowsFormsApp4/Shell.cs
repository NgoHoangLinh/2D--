using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Shell
    {
        public int x { get { return _rectangle.X; } set { _rectangle.X = value; } }
        public int y { get { return _rectangle.Y; } set { _rectangle.Y = value; } }
        Rectangle _rectangle;
        public Rectangle rectangle { get { return _rectangle; } set { _rectangle = value; } }
        public int shellID { get; set; }
        public int whoShoot { get; set; }
        public Shell(int x, int y, int ID, int whoShoot)
        {
            this.whoShoot = whoShoot;
            shellID = ID;
            this.x = x;
            this.y = y;
            rectangle = new Rectangle(x, y, 20, 5);

        }
        public void move()
        {
            _rectangle.X += 15;
        }
        
    }
}

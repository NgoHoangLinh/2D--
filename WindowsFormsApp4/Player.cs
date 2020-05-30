using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Player
    {
        public Bitmap texture { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int score { get; set; }
        public int ID { get; set; }
        public Player(int x, int y, Bitmap texture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
            score = 0;
        }
    }
}

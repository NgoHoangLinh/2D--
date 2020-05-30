using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Enemy
    {
        Rectangle _rect;
        public Rectangle rect { get { return _rect; } set { _rect = value; } }
        public int x { get { return _rect.X; } set { _rect.X = value; } }
        public int y { get { return _rect.Y; } set { _rect.Y = value; } }
        public string xDirection { get; set; }
        public string yDirection { get; set; }
        public int killBonus { get; set; }
        public int enemyID { get; set; }
        public int killedBy { get; set; }

        public Enemy(int ID)
        {
            _rect = new Rectangle(600, 50, 20, 20); //Wigth and Height from left-top point of rectangle
            enemyID = ID;
            xDirection = "left";
            yDirection = "down";
            killBonus = 10; //TODO need to depend on type of enemy
        }
        public void move()
        {
            if(xDirection.ToLower() == "left" && yDirection.ToLower() == "down")
            {
                _rect.X -= 2;
                _rect.Y += 2;
            }
            if (xDirection.ToLower() == "left" && yDirection.ToLower() == "up")
            {
                _rect.X -= 2;
                _rect.Y -= 2;
            }
            if (xDirection.ToLower() == "right" && yDirection.ToLower() == "down")
            {
                _rect.X += 2;
                _rect.Y += 2;
            }
            if (xDirection.ToLower() == "right" && yDirection.ToLower() == "up")
            {
                _rect.X += 2;
                _rect.Y -= 2;
            }
        }
    }
}

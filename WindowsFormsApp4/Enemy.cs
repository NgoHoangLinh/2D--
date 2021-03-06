﻿using System;
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
        public int speed { get; set; }

        public Enemy(int ID)
        {
            _rect = new Rectangle(600, 50, 20, 20); //Wigth and Height from left-top point of rectangle
            enemyID = ID;
            xDirection = "left";
            yDirection = "down";
            killBonus = 10;
        }
        public Enemy(int ID, int x, int y, int killBonus, int xDirection, int yDirection)
        {
            _rect = new Rectangle(x, y, 20, 20); //Wigth and Height from left-top point of rectangle
            enemyID = ID;
            switch (xDirection)
            {
                case 1:
                    this.xDirection = "left";
                    break;
                case 2:
                    this.xDirection = "right";
                    break;
            }
            switch (yDirection)
            {
                case 1:
                    this.yDirection = "up";
                    break;
                case 2:
                    this.yDirection = "down";
                    break;
            }
            this.killBonus = killBonus;
            speed = killBonus / 10;
        }
        public void move()
        {
            if(xDirection.ToLower() == "left" && yDirection.ToLower() == "down")
            {
                _rect.X -= speed;
                _rect.Y += speed;
            }
            if (xDirection.ToLower() == "left" && yDirection.ToLower() == "up")
            {
                _rect.X -= speed;
                _rect.Y -= speed;
            }
            if (xDirection.ToLower() == "right" && yDirection.ToLower() == "down")
            {
                _rect.X += speed;
                _rect.Y += speed;
            }
            if (xDirection.ToLower() == "right" && yDirection.ToLower() == "up")
            {
                _rect.X += speed;
                _rect.Y -= speed;
            }
        }
    }
}

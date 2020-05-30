using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        ////////////////////////////////////////Server/////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////Client////////////////////////////////////
        
        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////

        public static Form1 instance = null;
        public Bitmap handlerTexture = Resource1.Player1,
            targetTexture = Resource1.Player2;
        bool WPressed = false, APressed = false, SPressed = false,
            DPressed = false, isReloading = false;
        int reloadProgress = 0;
        Graphics g = null;
        public static Player player, anotherPlayer, playerHost, playerClient;
        public List<Shell> shells = new List<Shell>();
        public List<Enemy> enemies = new List<Enemy>();
        Fringle fringle = new Fringle(500, 0, 500, 500);
        delegate void MoveDetegate();
        event MoveDetegate MoveEvent;
        ClientUnit clientUnit = new ClientUnit();
        public static bool isServer = false;
        Thread thread = null;
        public static bool timerBlock = true;

        public Form1()
        {
            instance = this;
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            UpdateStyles();
            label1.BackColor = Color.Transparent; // secretly calling Refresh()
        }

        public Enemy createNewEnemy(int ID)
        {
            Enemy en = new Enemy(ID);
            MoveEvent += en.move;
            return en;
        }
        public Shell createNewShell(int x, int y, int ID, int whoShoot)
        {
            Shell sh = new Shell(x, y, ID, whoShoot);
            MoveEvent += sh.move;
            return sh;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Disconnect);
                thread.Abort();
                e.Cancel = true;
                Hide();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                WPressed = true;
            }
            /*
            if (e.KeyCode == Keys.A)
            {
                APressed = true;
            }
            */
            if (e.KeyCode == Keys.S)
            {
                SPressed = true;
            }
            /*
            if (e.KeyCode == Keys.D)
            {
                DPressed = true;
            }
            */
            if (e.KeyCode == Keys.Space && !isReloading)
            {
                ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Shoot);
                isReloading = true;
                reloadProgress = 0;
            }
        }

        private void serverButton_Click(object sender, EventArgs e)
        {
            serverButton.Enabled = false;
            isServer = true;
            Server s = new Server();
            thread = new Thread(s.Main);
            thread.Start();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            connectButton.Enabled = false;
            ClientUnit cu = new ClientUnit();
            Thread thread = new Thread(cu.Main);
            thread.Start();
            while (!ClientUnit.connected)
            {

            }
            if (!serverButton.Enabled)
            {
                //player = new Player(0, 0, Resource1.Player1);
                //anotherPlayer = new Player(0, 50, Resource1.Player2);
            }
            else
            {
                //player = new Player(0, 50, Resource1.Player2);
                //anotherPlayer = new Player(0, 0, Resource1.Player1);
                serverButton.Enabled = false;
            }
            
            //ClientUnitMain();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isServer)
            {
                //enemies.Add(createNewEnemy()); // TODO нужно создавать сервером
                ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.EnemyCreate);
            }
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                WPressed = false;
            }
            /*
            if (e.KeyCode == Keys.A)
            {
                APressed = false;
            }
            */
            if (e.KeyCode == Keys.S)
            {
                SPressed = false;
            }
            /*
            if (e.KeyCode == Keys.D)
            {
                DPressed = false;
            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (!timerBlock)
            {
                timer2.Start();
                timerBlock = true;
            }
            List<Enemy> en = new List<Enemy>();
            List<Shell> sh = new List<Shell>();
            bool moveDone = false;
            Refresh();
            //Произошла отрисовка столкновения. После чего удаляю пулю и врага
            
            for (int i = 0; i < shells.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if ((shells[i].x + 20 >= enemies[j].x) && (shells[i].x <= enemies[j].x + 20) &&
                        (shells[i].y + 5 >= enemies[j].y) && (shells[i].y <= enemies[j].y + 20))
                    {
                        
                        enemies[j].killedBy = shells[i].whoShoot;

                        if(enemies[j].killedBy == 0)
                        {
                            player.score += enemies[j].killBonus;
                        }

                        en.Add(enemies[j]);
                        sh.Add(shells[i]);
                    }
                }
            }
            if (en.Count > 0)
            {
                List<int> killedEnemiesIDs = new List<int>();
                List<int> killedShellsIDs = new List<int>();

                foreach (Enemy enemy in en)
                {
                    enemies.Remove(enemy);
                    killedEnemiesIDs.Add(enemy.enemyID);
                }
                foreach (Shell shell in sh)
                {
                    shells.Remove(shell);
                    killedShellsIDs.Add(shell.shellID);
                }
                ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.EnemyKilled, killedEnemiesIDs.ToArray(), killedShellsIDs.ToArray());
            }
            /////
            MoveEvent?.Invoke();

            if (player != null)
            {
                label1.Text = player.score.ToString();

                if (WPressed)
                {
                    player.y -= 10;
                    moveDone = true;
                }
                if (APressed)
                {
                    player.x -= 10;
                    moveDone = true;
                }
                if (SPressed)
                {
                    player.y += 10;
                    moveDone = true;
                }
                if (DPressed)
                {
                    player.x += 10;
                    moveDone = true;
                }
                if (moveDone)
                {
                    ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Position);
                }
            }
            if (isReloading)
            {
                reloadProgress += 2;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            if (anotherPlayer != null)
            {
                g.DrawImage(anotherPlayer.texture, new Rectangle(anotherPlayer.x, anotherPlayer.y, 50, 50));
            }
            if (player != null)
            {
                g.DrawImage(player.texture, new Rectangle(player.x, player.y, 50, 50));
            }

            g.DrawLine(new Pen(Color.Red), fringle.x1, fringle.y1, fringle.x2, fringle.y2);
            for(int i = 0; i < shells.Count; i++)
            {
                g.FillRectangle(new SolidBrush(Color.Black), shells[i].rectangle);
                g.DrawString(shells[i].whoShoot.ToString(), new Font("Arial", 13), new SolidBrush(Color.Black), shells[i].x + 10, shells[i].y - 20);
                if (shells[i].x >= ClientSize.Width)
                {
                    shells.Remove(shells[i]);
                    Console.WriteLine(shells.Count);
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].x + 20 >= ClientSize.Width)
                {
                    enemies[i].xDirection = "left";
                }
                if(enemies[i].x <= fringle.x1)
                {
                    enemies[i].xDirection = "right";
                }
                if (enemies[i].y + 20 >= ClientSize.Height)
                {
                    enemies[i].yDirection = "up";
                }
                if (enemies[i].y <= 0)
                {
                    enemies[i].yDirection = "down";
                }
                g.FillRectangle(new SolidBrush(Color.Black), enemies[i].rect);
                g.DrawString(enemies[i].enemyID.ToString(), new Font("Arial", 13), new SolidBrush(Color.Black), enemies[i].x + 10, enemies[i].y - 20);
            }


            if (isReloading)
            {
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, 80, 10));
                g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, reloadProgress, 10));
                
                if(reloadProgress >= 80)
                {
                    isReloading = false;
                }
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, reloadProgress, 10));
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, 80, 10));
            }
        }
        public void shoot(int ID, int whoShoot)
        {
            if(player != null)
            {
                shells.Add(createNewShell(player.x + 25, player.y + 25, ID, whoShoot));
            }
        }
        public void shootAnotherPlayer(int ID, int whoShoot)
        {
            if (anotherPlayer != null)
            {
                shells.Add(createNewShell(anotherPlayer.x + 25, anotherPlayer.y + 25, ID, whoShoot));
            }
        }

    }
}

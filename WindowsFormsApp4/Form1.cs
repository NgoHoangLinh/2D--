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
        static int shellID_Solo = 0;
        static int enemyID_Solo = 0;
        Graphics g = null;
        public static Player player, anotherPlayer, playerHost, playerClient;
        public List<Shell> shells = new List<Shell>();
        public List<Enemy> enemies = new List<Enemy>();
        Fringle fringle = new Fringle(500, 0, 500, 500);
        delegate void MoveDetegate();
        event MoveDetegate MoveEvent;
        ClientUnit clientUnit = new ClientUnit();
        public static bool isServer = false;
        Thread threadServer = null;
        Thread threadClient = null;
        public static bool timerBlock = true;
        public static bool firstConntect = false;
        public static bool playSolo = false;
        DateTime time = new DateTime(2020, 7, 13, 0, 1, 0);
        DateTime time1 = new DateTime(2020, 7, 13, 0, 0, 0);
        public static ManualResetEvent disconEvt = new ManualResetEvent(false);

        public Form1()
        {
            instance = this;
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            UpdateStyles();
            scoreLabel.BackColor = Color.Transparent; // secretly calling Refresh()
            anotherPlayerScoreLabel.BackColor = Color.Transparent; // secretly calling Refresh()
        }

        public Enemy createNewEnemy(int ID, int randX, int randY, int randBonus, int randXDir, int randYDir)
        {
            Enemy en = new Enemy(ID, randX, randY, randBonus, randXDir, randYDir);
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
                if (firstConntect)
                {
                    try
                    {
                        disconEvt.Reset();
                        ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Disconnect);
                        disconEvt.WaitOne();
                        if (isServer)
                        {
                            threadServer.Abort();
                        }
                    }
                    catch
                    {

                    }


                    try
                    {
                        ClientUnit.thread.Abort();
                        ClientUnit.socket.Shutdown(SocketShutdown.Both);
                        ClientUnit.socket.Close();
                    }
                    catch
                    {

                    }

                    try
                    {
                        threadClient.Abort();

                        if (isServer)
                        {
                            for (int i = 0; i < Server.clients.Count; i++)
                            {
                                Server.clients[i].thread.Abort();
                            }
                        }
                    }
                    catch
                    {

                    }
                    e.Cancel = true;
                }
                Application.Exit();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                WPressed = true;
            }
            if (e.KeyCode == Keys.S)
            {
                SPressed = true;
            }
            if (e.KeyCode == Keys.Space && !isReloading)
            {
                if (!playSolo)
                {
                    ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Shoot);
                    isReloading = true;
                    reloadProgress = 0;
                }
                else
                {
                    shoot(shellID_Solo++, player.ID);
                    isReloading = true;
                    reloadProgress = 0;
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e) // играть против игрока
        {
            startConnectPanel.Enabled = true;
            startConnectPanel.Visible = true;

            mainPanel.Enabled = false;
            mainPanel.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e) //start
        {
            startConnectPanel.Enabled = false;
            startConnectPanel.Visible = false;

            isServer = true;
            Server s = new Server();
            threadServer = new Thread(s.Main);
            threadServer.Start(portTextBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            startConnectPanel.Enabled = false;
            startConnectPanel.Visible = false;

            firstConntect = true;

            ClientUnit cu = new ClientUnit();
            threadClient = new Thread(cu.Main);

            threadClient.Start(textBox2.Text + " " + portTextBox1.Text);
        }

        private void playAIButton_Click(object sender, EventArgs e)
        {
            mainPanel.Enabled = false;
            mainPanel.Visible = false;

            playSolo = true;
            timerBlock = false;

            player = new Player(0, 50, Resource1.Player1);
        }

        private void toRulesPanelButton_Click(object sender, EventArgs e)
        {
            mainPanel.Enabled = false;
            mainPanel.Visible = false;

            rulesPanel.Enabled = true;
            rulesPanel.Visible = true;
        }

        private void backToMainButton_Click(object sender, EventArgs e)
        {
            rulesPanel.Enabled = false;
            rulesPanel.Visible = false;

            mainPanel.Enabled = true;
            mainPanel.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (firstConntect)
            {
                try
                {
                    disconEvt.Reset();
                    ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Disconnect);
                    disconEvt.WaitOne();
                    if (isServer)
                    {
                        threadServer.Abort();
                    }
                }
                catch
                {

                }


                try
                {
                    ClientUnit.thread.Abort();
                    ClientUnit.socket.Shutdown(SocketShutdown.Both);
                    ClientUnit.socket.Close();
                }
                catch
                {

                }

                try
                {
                    threadClient.Abort();

                    if (isServer)
                    {
                        for (int i = 0; i < Server.clients.Count; i++)
                        {
                            Server.clients[i].thread.Abort();
                        }
                    }
                }
                catch
                {

                }
            }
            Application.Exit();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            if (isServer)
            {
                ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.EnemyCreate, " " + random.Next(550, 700) + " " + random.Next(50, 350) + " " + (random.Next(1, 5) * 10) + " " + random.Next(1, 3) + " " + random.Next(1, 3));
            }
            if (playSolo)
            {
                enemies.Add(createNewEnemy(enemyID_Solo++, random.Next(550, 700), random.Next(50, 350), random.Next(1, 5) * 10, random.Next(1, 3), random.Next(1, 3)));
            }
            
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                WPressed = false;
            }
            if (e.KeyCode == Keys.S)
            {
                SPressed = false;
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playSolo)
            {
                time = time.AddMilliseconds(-25);
                if (time.Subtract(time1).TotalMilliseconds >= 0)
                {
                    timerLlabel.Text = time.ToString("mm:ss.fffK");
                }
                else
                {
                    timerLlabel.Text = time1.ToString("mm:ss.fffK");
                    timer1.Stop();
                }
            }
            
            if (playSolo && enemies.Count <= 0)
            {
                Random random = new Random();
                enemies.Add(createNewEnemy(enemyID_Solo++, random.Next(550, 700), random.Next(50, 350), random.Next(1, 5) * 10, random.Next(1, 3), random.Next(1, 3)));
                timer2.Stop();
                timer2.Start();
            }
            if (!playSolo && !firstConntect && Server.serverStarted)
            {
                firstConntect = true;
                ClientUnit cu = new ClientUnit();
                threadClient = new Thread(cu.Main);
                //thread.Start(textBox1.Text);
                threadClient.Start(textBox2.Text + " " + portTextBox1.Text);
            }
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
                    try
                    {
                        if ((shells[i].x + 20 >= enemies[j].x) && (shells[i].x <= enemies[j].x + 20) &&
                        (shells[i].y + 5 >= enemies[j].y) && (shells[i].y <= enemies[j].y + 20))
                        {

                            enemies[j].killedBy = shells[i].whoShoot;

                            Console.WriteLine("killed by " + enemies[j].killedBy + "                                          player ID " + player.ID);

                            if (enemies[j].killedBy == player.ID)
                            {
                                player.score += enemies[j].killBonus;
                            }
                            else
                            {
                                anotherPlayer.score += enemies[j].killBonus;
                            }

                            en.Add(enemies[j]);
                            sh.Add(shells[i]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("           !!!!!!!!!! Exception in enemies/shells cycle");
                    }
                    
                }
            }
            if (en.Count > 0)
            {
                List<int> killedEnemiesIDs = new List<int>();
                List<int> killedShellsIDs = new List<int>();

                string package = "";
                

                foreach (Enemy enemy in en)
                {
                    enemies.Remove(enemy);
                    killedEnemiesIDs.Add(enemy.enemyID);
                    package += " " + enemy.enemyID + " " + enemy.killedBy;

                }
                foreach (Shell shell in sh)
                {
                    shells.Remove(shell);
                    killedShellsIDs.Add(shell.shellID);
                    package += " " + shell.shellID;
                }
                if (!playSolo)
                {
                    ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.EnemyKilled, package);
                }
            }
            /////
            MoveEvent?.Invoke();

            if (player != null)
            {
                scoreLabel.Text = player.score.ToString();
                if(anotherPlayer != null)
                {
                    anotherPlayerScoreLabel.Text = anotherPlayer.score.ToString();
                }

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
                if (!playSolo && moveDone)
                {
                    ClientUnit.Send(ClientUnit.socket, ClientUnit.PacketInfo.Position);
                }
            }
            if (player != null && anotherPlayer != null)
            {
                if (player.score >= 200 && anotherPlayer.score < 200)
                {
                    timer1.Stop();
                    MessageBox.Show("You win! Congratulations!");
                }
                if (anotherPlayer.score >= 200 && player.score < 200)
                {
                    timer1.Stop();
                    MessageBox.Show("You lose =(");
                }
                if (player.score >= 200 && anotherPlayer.score >= 200)
                {
                    timer1.Stop();
                    MessageBox.Show("Draw");
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
            try
            {
                for (int i = 0; i < shells.Count; i++)
                {
                    g.FillRectangle(new SolidBrush(Color.Black), shells[i].rectangle);

                    if (shells[i].x >= ClientSize.Width)
                    {
                        shells.Remove(shells[i]);
                        Console.WriteLine(shells.Count);
                    }
                }
            }
            catch
            {

            }
            try { 
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].x + 20 >= ClientSize.Width)
                    {
                        enemies[i].xDirection = "left";
                    }
                    if (enemies[i].x <= fringle.x1)
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
                    Brush brush;
                    Brush pen;
                    switch (enemies[i].speed)
                    {
                        case 1:
                            brush = new SolidBrush(Color.Black);
                            pen = new SolidBrush(Color.White);
                            break;
                        case 2:
                            brush = new SolidBrush(Color.Red);
                            pen = new SolidBrush(Color.Blue);
                            break;
                        case 3:
                            brush = new SolidBrush(Color.Yellow);
                            pen = new SolidBrush(Color.Green);
                            break;
                        case 4:
                            brush = new SolidBrush(Color.Green);
                            pen = new SolidBrush(Color.Yellow);
                            break;
                        default:
                            brush = new SolidBrush(Color.Purple);
                            pen = new SolidBrush(Color.White);
                            break;
                    }
                    g.FillRectangle(brush, enemies[i].rect);
                    g.DrawString(enemies[i].enemyID.ToString(), new Font("Arial", 13), pen, enemies[i].x, enemies[i].y);
                }
            }
            catch
            {

            }

            g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, 80, 10));
            if (reloadProgress < 80)
            {
                isReloading = true;
                g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, reloadProgress, 10));
            }
            else
            {
                isReloading = false;
                g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, reloadProgress, 10));
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

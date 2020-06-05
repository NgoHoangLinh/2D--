namespace WindowsFormsApp4
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.scoreLabel = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.playSoloButton = new System.Windows.Forms.Button();
            this.playVsPlayerButton = new System.Windows.Forms.Button();
            this.startConnectPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.portTextBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timerLlabel = new System.Windows.Forms.Label();
            this.anotherPlayerScoreLabel = new System.Windows.Forms.Label();
            this.rulesPanel = new System.Windows.Forms.Panel();
            this.rulesTextBox = new System.Windows.Forms.TextBox();
            this.rulesLabel = new System.Windows.Forms.Label();
            this.backToMainButton = new System.Windows.Forms.Button();
            this.toRulesPanelButton = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.startConnectPanel.SuspendLayout();
            this.rulesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 25;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 5000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // scoreLabel
            // 
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scoreLabel.Location = new System.Drawing.Point(92, 10);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(62, 23);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "000";
            this.scoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.toRulesPanelButton);
            this.mainPanel.Controls.Add(this.button3);
            this.mainPanel.Controls.Add(this.playSoloButton);
            this.mainPanel.Controls.Add(this.playVsPlayerButton);
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(800, 409);
            this.mainPanel.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(360, 216);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Выход";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // playSoloButton
            // 
            this.playSoloButton.Location = new System.Drawing.Point(329, 120);
            this.playSoloButton.Name = "playSoloButton";
            this.playSoloButton.Size = new System.Drawing.Size(131, 23);
            this.playSoloButton.TabIndex = 1;
            this.playSoloButton.Text = "Одиночный режим";
            this.playSoloButton.UseVisualStyleBackColor = true;
            this.playSoloButton.Click += new System.EventHandler(this.playAIButton_Click);
            // 
            // playVsPlayerButton
            // 
            this.playVsPlayerButton.Location = new System.Drawing.Point(329, 70);
            this.playVsPlayerButton.Name = "playVsPlayerButton";
            this.playVsPlayerButton.Size = new System.Drawing.Size(132, 23);
            this.playVsPlayerButton.TabIndex = 0;
            this.playVsPlayerButton.Text = "Игра против игрока";
            this.playVsPlayerButton.UseVisualStyleBackColor = true;
            this.playVsPlayerButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // startConnectPanel
            // 
            this.startConnectPanel.Controls.Add(this.label2);
            this.startConnectPanel.Controls.Add(this.label1);
            this.startConnectPanel.Controls.Add(this.portTextBox1);
            this.startConnectPanel.Controls.Add(this.textBox2);
            this.startConnectPanel.Controls.Add(this.button5);
            this.startConnectPanel.Controls.Add(this.button4);
            this.startConnectPanel.Enabled = false;
            this.startConnectPanel.Location = new System.Drawing.Point(0, 0);
            this.startConnectPanel.Name = "startConnectPanel";
            this.startConnectPanel.Size = new System.Drawing.Size(800, 409);
            this.startConnectPanel.TabIndex = 3;
            this.startConnectPanel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(588, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "port";
            // 
            // portTextBox1
            // 
            this.portTextBox1.Location = new System.Drawing.Point(481, 121);
            this.portTextBox1.Name = "portTextBox1";
            this.portTextBox1.Size = new System.Drawing.Size(87, 20);
            this.portTextBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(209, 121);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(139, 20);
            this.textBox2.TabIndex = 2;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(89, 170);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 1;
            this.button5.TabStop = false;
            this.button5.Text = "Connect";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(89, 70);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 0;
            this.button4.TabStop = false;
            this.button4.Text = "Start";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(391, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 20);
            this.button2.TabIndex = 5;
            this.button2.TabStop = false;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timerLlabel
            // 
            this.timerLlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timerLlabel.Location = new System.Drawing.Point(186, 9);
            this.timerLlabel.Name = "timerLlabel";
            this.timerLlabel.Size = new System.Drawing.Size(91, 23);
            this.timerLlabel.TabIndex = 6;
            this.timerLlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // anotherPlayerScoreLabel
            // 
            this.anotherPlayerScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.anotherPlayerScoreLabel.Location = new System.Drawing.Point(92, 380);
            this.anotherPlayerScoreLabel.Name = "anotherPlayerScoreLabel";
            this.anotherPlayerScoreLabel.Size = new System.Drawing.Size(62, 23);
            this.anotherPlayerScoreLabel.TabIndex = 7;
            this.anotherPlayerScoreLabel.Text = "000";
            this.anotherPlayerScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rulesPanel
            // 
            this.rulesPanel.Controls.Add(this.backToMainButton);
            this.rulesPanel.Controls.Add(this.rulesTextBox);
            this.rulesPanel.Controls.Add(this.rulesLabel);
            this.rulesPanel.Location = new System.Drawing.Point(0, 0);
            this.rulesPanel.Name = "rulesPanel";
            this.rulesPanel.Size = new System.Drawing.Size(800, 409);
            this.rulesPanel.TabIndex = 6;
            this.rulesPanel.Visible = false;
            // 
            // rulesTextBox
            // 
            this.rulesTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.rulesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rulesTextBox.Location = new System.Drawing.Point(12, 54);
            this.rulesTextBox.Multiline = true;
            this.rulesTextBox.Name = "rulesTextBox";
            this.rulesTextBox.Size = new System.Drawing.Size(601, 240);
            this.rulesTextBox.TabIndex = 1;
            this.rulesTextBox.Text = resources.GetString("rulesTextBox.Text");
            // 
            // rulesLabel
            // 
            this.rulesLabel.AutoSize = true;
            this.rulesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rulesLabel.Location = new System.Drawing.Point(368, 14);
            this.rulesLabel.Name = "rulesLabel";
            this.rulesLabel.Size = new System.Drawing.Size(66, 16);
            this.rulesLabel.TabIndex = 0;
            this.rulesLabel.Text = "Правила";
            // 
            // backToMainButton
            // 
            this.backToMainButton.Location = new System.Drawing.Point(713, 9);
            this.backToMainButton.Name = "backToMainButton";
            this.backToMainButton.Size = new System.Drawing.Size(75, 23);
            this.backToMainButton.TabIndex = 2;
            this.backToMainButton.Text = "Назад";
            this.backToMainButton.UseVisualStyleBackColor = true;
            this.backToMainButton.Click += new System.EventHandler(this.backToMainButton_Click);
            // 
            // toRulesPanelButton
            // 
            this.toRulesPanelButton.Location = new System.Drawing.Point(359, 170);
            this.toRulesPanelButton.Name = "toRulesPanelButton";
            this.toRulesPanelButton.Size = new System.Drawing.Size(75, 23);
            this.toRulesPanelButton.TabIndex = 3;
            this.toRulesPanelButton.Text = "Правила";
            this.toRulesPanelButton.UseVisualStyleBackColor = true;
            this.toRulesPanelButton.Click += new System.EventHandler(this.toRulesPanelButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 407);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.rulesPanel);
            this.Controls.Add(this.startConnectPanel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.anotherPlayerScoreLabel);
            this.Controls.Add(this.timerLlabel);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.mainPanel.ResumeLayout(false);
            this.startConnectPanel.ResumeLayout(false);
            this.startConnectPanel.PerformLayout();
            this.rulesPanel.ResumeLayout(false);
            this.rulesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button playVsPlayerButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button playSoloButton;
        private System.Windows.Forms.Panel startConnectPanel;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label timerLlabel;
        public System.Windows.Forms.Label anotherPlayerScoreLabel;
        private System.Windows.Forms.TextBox portTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel rulesPanel;
        private System.Windows.Forms.TextBox rulesTextBox;
        private System.Windows.Forms.Label rulesLabel;
        private System.Windows.Forms.Button backToMainButton;
        private System.Windows.Forms.Button toRulesPanelButton;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinoGame
{
    public partial class Form1 : Form
    {
        bool jumping = false; 
        int jumpSpeed = 20;
        int force = 5;
        int obstacleSpeed = 3;
        int score = 0;
        Random random = new Random();


        public Form1()
        {
            InitializeComponent();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                resetGame();
            }
            if (jumping)
            {
                jumping = false;
            }
        }

        private void resetGame()
        {
            jumping = false;
            jumpSpeed = 15;
            force = 3;
            obstacleSpeed = 1;
            score = 0;
            trex.Top = floor.Top - trex.Height;
            trex.Image = Properties.Resources.running;

            foreach (Control x in this.Controls)
            {
                int position = random.Next(600, 1000);
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left = 600 + (x.Left + position + x.Width * 3);        
                }
            }
            gameTimer.Start();
        }

        private void gameEvent()
        {
            trex.Top += jumpSpeed;

            scoreText.Text = "Score: " + score;


            if(jumping && force < 0)
            {
                jumping = false;
            }
            if(jumping)
            {
                jumpSpeed = -15;
                force -= 1;
            }
            else {
                jumpSpeed = 15;
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left + x.Width < -120)
                    {
                        x.Left = this.ClientSize.Width + random.Next(200, 800);
                        score++;
                    }
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        scoreText.Text += " Press R to restart the game!";
                    }
                }
            }

            if((trex.Top + trex.Height > floor.Top ) && (!jumping))
            {
                force = 5;
                trex.Top = floor.Top - trex.Height;
                jumpSpeed = 0;
            }

            if(score%20 == 0)
            {
                obstacleSpeed += 3;
            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            gameEvent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

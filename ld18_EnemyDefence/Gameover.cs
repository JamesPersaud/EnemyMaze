using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ld18_EnemyDefence
{
    public partial class Gameover : Form
    {
        public Gameover()
        {
            InitializeComponent();
        }

        private void Gameover_Load(object sender, EventArgs e)
        {
            lblScore.Text = "You scored: " + GameEngine.Instance.PlayerScore.ToString();
            if (GameEngine.Instance.PlayerScore > GameEngine.Instance.HiScore)
            {
                GameEngine.Instance.HiScore = GameEngine.Instance.PlayerScore;
                GameEngine.Instance.SaveHiscore();
                lblHiscore1.Visible = true;
                lblHiscore2.Visible = true;
                GameEngine.Instance.LoadHiscore();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.KEYS_DOWN = 0;
            this.Close();
        }
    }
}

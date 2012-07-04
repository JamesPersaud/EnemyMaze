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
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            string play = string.Empty;
            play += "Move the player using the keys : W,S,A,D" + Environment.NewLine + Environment.NewLine;
            play += "By clicking on the screen with either mouse button you can add or remove walls in the maze. You may not completely enclose enemies." + Environment.NewLine + Environment.NewLine;
            play += "Collect shiny things to increase your score." + Environment.NewLine + Environment.NewLine;
            play += "Avoid enemies, which can lower your life force." + Environment.NewLine + Environment.NewLine;
            play += "You can use enemies as weapons against other enemies by causing them to collide with eachother. This will also increase your score." + Environment.NewLine + Environment.NewLine;
            play += "The game is over when you lose all of your life force." + Environment.NewLine + Environment.NewLine;

            textBox1.Text = play;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.KEYS_DOWN = 0;
            this.Close();
        }
    }
}

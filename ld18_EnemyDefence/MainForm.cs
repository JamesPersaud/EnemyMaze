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
    public partial class MainForm : Form
    {
        About aboutBox = new About();
        Help helpBox = new Help();

        public MainForm()
        {
            InitializeComponent();
        }

        public void ShowLife()
        {
            int life = GameEngine.Instance.PlayerLife;
            Bitmap bit = new Bitmap(200, 16);
            using (Graphics g = Graphics.FromImage(bit))
            {
                g.Clear(Color.White);                              
                Brush br;
                if (life > 80)
                    br = Brushes.Green;
                else if (life > 50)
                    br = Brushes.Yellow;
                else if (life > 20)
                    br = Brushes.Orange;
                else
                    br = Brushes.Red;

                g.FillRectangle(br,new Rectangle(0,0,life*2,16));
                g.DrawRectangle(Pens.Black, new Rectangle(0, 0, 199, 15));
            }

            pictureBox1.Image = bit;

        }

        public void showelapsed(float f,float e,float t)
        {
            this.textBox1.Text = f.ToString();
            this.textBox2.Text = e.ToString();
            this.textBox3.Text = t.ToString();
            this.txtMouse.Text = WindowManager.Instance.mouseGridLocation.X.ToString() + " " + WindowManager.Instance.mouseGridLocation.Y.ToString();
        }

        public void showkeys()
        {
            string s = "";
            if ((GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_W) != 0)
                s += "W";
            if ((GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_A) != 0)
                s += "A";
            if ((GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_S) != 0)
                s += "S";
            if ((GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_D) != 0)
                s += "D";

            txtkeys.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.SpawnEnemy(4);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.PauseRendering = true;
            helpBox.ShowDialog();
            GameEngine.Instance.PauseRendering = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.PauseRendering = true;
            aboutBox.ShowDialog();
            GameEngine.Instance.PauseRendering = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ld18_EnemyDefence
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.ludumdare.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameEngine.Instance.KEYS_DOWN = 0;
            this.Close();
        }
    }
}

namespace ld18_EnemyDefence
{
    partial class Gameover
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblHiscore1 = new System.Windows.Forms.Label();
            this.lblHiscore2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game Over :(";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(14, 133);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(33, 13);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "score";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Play Again";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblHiscore1
            // 
            this.lblHiscore1.AutoSize = true;
            this.lblHiscore1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHiscore1.Location = new System.Drawing.Point(12, 73);
            this.lblHiscore1.Name = "lblHiscore1";
            this.lblHiscore1.Size = new System.Drawing.Size(170, 26);
            this.lblHiscore1.TabIndex = 3;
            this.lblHiscore1.Text = "Congratulations!";
            this.lblHiscore1.Visible = false;
            // 
            // lblHiscore2
            // 
            this.lblHiscore2.AutoSize = true;
            this.lblHiscore2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHiscore2.Location = new System.Drawing.Point(12, 176);
            this.lblHiscore2.Name = "lblHiscore2";
            this.lblHiscore2.Size = new System.Drawing.Size(185, 26);
            this.lblHiscore2.TabIndex = 4;
            this.lblHiscore2.Text = "A new high score!";
            this.lblHiscore2.Visible = false;
            // 
            // Gameover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblHiscore2);
            this.Controls.Add(this.lblHiscore1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Gameover";
            this.Text = "Gameover";
            this.Load += new System.EventHandler(this.Gameover_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblHiscore1;
        private System.Windows.Forms.Label lblHiscore2;
    }
}
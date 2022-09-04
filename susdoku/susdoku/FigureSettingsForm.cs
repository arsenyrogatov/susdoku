using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static susdoku.Form1;

namespace susdoku
{
    public partial class FigureSettingsForm : Form
    {
        public FigureSettingsForm()
        {
            InitializeComponent();
            mode = 1;
            isCorrect = true;
        }

        int mode;
        public bool isCorrect;
        public int[,] groups;

        private void group1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 1;
        }

        private void group2_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 2;
        }

        private void group3_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 3;
        }

        private void group4_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 4;
        }

        private void group5_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 5;
        }

        private void group6_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 6;
        }

        private void group7_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 7;
        }

        private void group8_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 8;
        }

        private void group9_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            mode = 9;
        }

        
        public List<Kvadrat> getPole()
        {
            isCorrect = true;
            List<Kvadrat> result = new List<Kvadrat>();
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            result.Add(new Kvadrat());
            groups = new int[9, 9];

            int x = 0, y = 0;

            for (int i = 8; i >= 0; i--)
            {
                for (int j = 8; j >= 0; j--)
                {
                    var curColor = ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).BackColor;

                    int mode = 0;
                    if (curColor == Color.PaleTurquoise)
                        mode = 1;
                    if (curColor == Color.DeepPink)
                        mode = 2;
                    if (curColor == Color.PaleGoldenrod)
                        mode = 3;
                    if (curColor == Color.Salmon)
                        mode = 4;
                    if (curColor == Color.SlateBlue)
                        mode = 5;
                    if (curColor == Color.SpringGreen)
                        mode = 6;
                    if (curColor == Color.Pink)
                        mode = 7;
                    if (curColor == Color.Gold)
                        mode = 8;

                    result[mode].coord.Add(new int[] { y, x });
                    groups[y, x] = mode;
                    x++;

                    if (x == 3 || x == 6 || x == 9)
                    {
                        if (y != 2 && y != 5 && y != 8)
                        {
                            x -= 3;
                            y++;
                        }
                        else
                        {
                            y -= 2;
                        }
                    }
                }
                if (x == 9)
                {
                    x = 0;
                    y += 3;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (result[i].coord.Count != 9)
                {
                    MessageBox.Show("Ошибка заполнения!");
                    isCorrect = false;
                    break;
                }
            }
            return result;
        }

        private void stextBox1_Click(object sender, EventArgs e)
        {
            Color curColor = Color.Black;
            switch (mode)
            {
                case 1:
                    curColor = Color.DodgerBlue;
                    break;
                case 2:
                    curColor = Color.PaleTurquoise;
                    break;
                case 3:
                    curColor = Color.DeepPink;
                    break;
                case 4:
                    curColor = Color.PaleGoldenrod;
                    break;
                case 5:
                    curColor = Color.Salmon;
                    break;
                case 6:
                    curColor = Color.SlateBlue;
                    break;
                case 7:
                    curColor = Color.SpringGreen;
                    break;
                case 8:
                    curColor = Color.Pink;
                    break;
                case 9:
                    curColor = Color.Gold;
                    break;
            }
            ((TextBox)sender).BackColor = curColor;
        }
    }
}

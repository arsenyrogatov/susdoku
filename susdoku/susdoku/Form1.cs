using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace susdoku
{
    public partial class Form1 : Form
    {
        int selectionMode;

        public static List<int[,]> ogr;
        public static bool diagonals;
        public static bool asterisk;
        public static List<int[]> ast_coords;
        public static bool figur;
        public static List<Kvadrat> kvadr;

        public class Kvadrat
        {
            public List<int[]> coord;
            public Kvadrat()
            {
                coord = new List<int[]>();
            }
        }

        public Form1()
        {
            InitializeComponent();
            selectionMode = 0;
            ogr = new List<int[,]>();
            diagonals = false;
            asterisk = false;
            ast_coords = new List<int[]>();
            ast_coords.Add(new int[] { 1, 4 });
            ast_coords.Add(new int[] { 2, 2 });
            ast_coords.Add(new int[] { 2, 6 });
            ast_coords.Add(new int[] { 4, 1 });
            ast_coords.Add(new int[] { 4, 4 });
            ast_coords.Add(new int[] { 4, 7 });
            ast_coords.Add(new int[] { 6, 2 });
            ast_coords.Add(new int[] { 6, 6 });
            ast_coords.Add(new int[] { 7, 4 });
            figur = false;
            kvadr = new List<Kvadrat>();
            defaultKvadr();
        }

        #region nvm

        private void defaultKvadr()
        {
            
        }

        private void diagonals_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            diagonals = diagonals_checkBox.Checked;
        }

        private void asterisk_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            asterisk = asterisk_checkBox.Checked;
        }

        private void clear_linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clear();
            ogr = new List<int[,]>();
            ogr_panel.Enabled = true;

            solve_button.Enabled = true;
        }

        static List<int[,]> toList(int[,] pole)
        {
            var result = new List<int[,]>();

            var subPole = new int[3, 3];
            var subOgr = new int[3, 3];
            int x = 0, y = 0;

            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine("--->" + y.ToString());
                subPole = new int[,]
                {
                        { pole[x,y], pole[x,y+1],pole[x,y+2]},
                        { pole[x+1,y], pole[x+1,y+1],pole[x+1,y+2]},
                        { pole[x+2,y], pole[x+2,y+1],pole[x+2,y+2]}
                };
                result.Add(Copy(subPole));
                y += 3;
                if (y == 9)
                {
                    y = 0;
                    x += 3;
                }
            }

            return result;
        }

        private void mode0_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            selectionMode = 0;
        }

        private void mode1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            selectionMode = 1;
        }

        private void mode2_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            selectionMode = 2;
        }

        private void stextBox1_Click(object sender, EventArgs e)
        {
            if (!figur)
            {
                if (selectionMode == 1)
                {
                    ((TextBox)sender).BackColor = Color.Gold;
                }
                if (selectionMode == 0)
                {
                    ((TextBox)sender).BackColor = Color.White;
                }
                if (selectionMode == 2)
                {
                    ((TextBox)sender).BackColor = Color.DeepSkyBlue;
                }
            }
        }

        void clear()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).Text = "";
                    ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).BackColor = Color.White;
                }
            }
        }

        static T[,] Copy<T>(T[,] array)
        {
            T[,] newArray = new T[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    newArray[i, j] = array[i, j];
            return newArray;
        }

        #endregion

        static void mixColors(TextBox tb, int o, bool v, int pTI, int tbTI)
        {
            int[] ld = new int[] { 0, 4, 8 };
            int[] rd = new int[] { 2, 4, 6 };
            bool d = (diagonals && ((pTI == 4 && (ld.Contains(tbTI) || rd.Contains(tbTI))) || ((pTI == 0 || pTI == 8) && ld.Contains(tbTI)) || ((pTI == 2 || pTI == 6) && rd.Contains(tbTI))));
            if (!d)
            {
                if (v)
                {
                    tb.BackColor = Color.FromArgb(237, 237, 237);
                }
            }
            else
            {
                if (!v)
                {
                    if (o == 0) //purple
                    {
                        tb.BackColor = Color.FromArgb(254, 168, 255);
                    }
                    if (o == 1) //gold + purple
                    {
                        tb.BackColor = Color.FromArgb(255, 108, 128);
                    }
                    if (o == 2) //deepskyblue + purple
                    {
                        tb.BackColor = Color.FromArgb(126, 96, 255);
                    }
                }
            }
            bool a = (asterisk && ((pTI == 0 && tbTI == 8) || (pTI == 1 && tbTI == 4) || (pTI == 2 && tbTI == 6) || (pTI == 3 && tbTI == 4) || (pTI == 4 && tbTI == 4) || (pTI == 5 && tbTI == 4) || (pTI == 6 && tbTI == 2) || (pTI == 7 && tbTI == 4) || (pTI == 8 && tbTI == 0)));
            if (a)
            {
                tb.BackColor = Color.FromArgb(0, 250, 154);
            }
        }

        List<int[,]> getPole()
        {
            ogr = new List<int[,]>();
            var pole = new List<int[,]>();
            var subPole = new int[3, 3];
            var subOgr = new int[3, 3];
            int x = 0, y = 0;

            for (int i = 8; i >= 0; i--)
            {
                for (int j = 8; j >= 0; j--)
                {
                    var text = ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).Text;
                    
                    subPole[y, x] = text == "" ? -1 : Convert.ToInt32(text);
                    int curOgr = 0;
                    if (((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).BackColor == Color.Gold)
                        curOgr = 1;
                    if (((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).BackColor == Color.DeepSkyBlue)
                        curOgr = 2;
                    if(!figur)
                    mixColors(((TextBox)((Panel)base_panel.Controls[i]).Controls[j]), curOgr, text != "", ((Panel)base_panel.Controls[i]).TabIndex, ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).TabIndex);
                    subOgr[y, x] = curOgr;
                    x++;
                    if (x == 3)
                    {
                        x = 0;
                        y++;
                    }
                }
                pole.Add(Copy(subPole));
                ogr.Add(Copy(subOgr));
                x = 0;
                y = 0;
            }
            return pole;
        }

        void colorGroups(List<int[,]> pole)
        {
            int poleIndex = 0;
            var subPole = pole[poleIndex];
            int x = 0, y = 0;
            for (int i = 8; i >= 0; i--)
            {
                for (int j = 8; j >= 0; j--)
                {
                    
                    Color curColor = Color.Black;
                    switch (subPole[y, x])
                    {
                        case 0:
                            curColor = Color.DodgerBlue;
                            break;
                        case 1:
                            curColor = Color.PaleTurquoise;
                            break;
                        case 2:
                            curColor = Color.DeepPink;
                            break;
                        case 3:
                            curColor = Color.PaleGoldenrod;
                            break;
                        case 4:
                            curColor = Color.Salmon;
                            break;
                        case 5:
                            curColor = Color.SlateBlue;
                            break;
                        case 6:
                            curColor = Color.SpringGreen;
                            break;
                        case 7:
                            curColor = Color.Pink;
                            break;
                        case 8:
                            curColor = Color.Gold;
                            break;
                    }
                    ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).BackColor = curColor;
                    x++;
                    if (x == 3)
                    {
                        x = 0;
                        y++;
                    }
                }
                poleIndex++;
                if (poleIndex < 9)
                    subPole = pole[poleIndex];
                x = 0;
                y = 0;
            }
        }

        void setPole(List<int[,]> pole)
        {
            int poleIndex = 0;
            var subPole = pole[poleIndex];
            int x = 0, y = 0;
            for (int i = 8; i >= 0; i--)
            {
                for (int j = 8; j >= 0; j--)
                {
                    ((TextBox)((Panel)base_panel.Controls[i]).Controls[j]).Text = subPole[y, x].ToString() == "-1" ? "" : subPole[y, x].ToString();

                    x++;
                    if (x == 3)
                    {
                        x = 0;
                        y++;
                    }
                }
                poleIndex++;
                if (poleIndex < 9)
                    subPole = pole[poleIndex];
                x = 0;
                y = 0;
            }
        }

        private void solve_button_Click(object sender, EventArgs e)
        {
            var pole = getPole();
            ogr_panel.Enabled = false;
            loading_panel.Visible = true;
            clear_linkLabel.Enabled = false;
            solve_button.Enabled = false;
            new Thread(() => {
                Sus sudoku = new Sus(pole);
                sudoku.Solve();

                setPole(sudoku.a);
                loading_panel.Visible = false;
                clear_linkLabel.Enabled = true;
            }).Start();
        }

        private class Sus
        {
            int[,] ogr_;
            public List<int[,]> a;
            public Sus(List<int[,]> oa_)
            {
                ogr_ = toMatrix(ogr);
                a = oa_;
            }

            public void Solve()
            {
                if (!Recurs(toMatrix(a)))
                {
                    MessageBox.Show("Нет решения!");
                }
            }

            bool Recurs(int[,] ma)
            {
                var coords = freeCoords(ma);
                if (coords.Length == 0)
                {
                    if (check(ma))
                    {
                        //MessageBox.Show("Решение найдено!");
                        a = toList(ma);
                        return true;
                    }
                    else
                        return false;
                }
                printMatr(ma);
                Console.WriteLine();
                int r = coords[0], c = coords[1];

                for (int i = 1; i < 10; i++)
                {
                    if (ogr_[r, c] == 0 || (i % 2 == 0 && ogr_[r, c] == 1) || (i % 2 != 0 && ogr_[r, c] == 2))
                    {
                        ma[r, c] = i;
                        if (!check(ma))
                        {
                            ma[r, c] = -1;
                        }
                        else
                        {
                            if (Recurs(ma))
                                return true;
                            else
                                ma[r, c] = -1;
                        }
                    }
                }
                return false;
            }

            bool check(int[,] ma)
            {
                for (int i = 0; i < 9; i++)
                {
                    List<int> ver = new List<int>();
                    List<int> hor = new List<int>();
                    for (int j = 0; j < 9; j++)
                    {
                        Console.Write(ma[i, j].ToString() + " ");
                        if (ma[i, j] != -1)
                        {
                            if (hor.Contains(ma[i, j]))
                                return false;
                            else
                                hor.Add(ma[i, j]);
                        }
                        if (ma[j, i] != -1)
                        {
                            if (ver.Contains(ma[j, i]))
                                return false;
                            else
                                ver.Add(ma[j, i]);
                        }
                    }
                    Console.WriteLine();
                }

                if (diagonals)
                {
                    List<int> up = new List<int>();
                    List<int> down = new List<int>();
                    for (int i = 0; i < 9; i++)
                    {
                            if (ma[i, i] != -1)
                            {
                                if (up.Contains(ma[i, i]))
                                    return false;
                                else
                                up.Add(ma[i, i]);
                            }
                            if (ma[8-i, i] != -1)
                            {
                                if (down.Contains(ma[8 - i, i]))
                                    return false;
                                else
                                down.Add(ma[8 - i, i]);
                            }
                        }                    
                }

                if (asterisk)
                {
                    List<int> astlst = new List<int>();
                    for (int i = 0; i < 9; i++)
                    {
                        if (ma[ast_coords[i][0], ast_coords[i][1]] != -1)
                        {
                            if (astlst.Contains(ma[ast_coords[i][0], ast_coords[i][1]]))
                                return false;
                            else
                                astlst.Add(ma[ast_coords[i][0], ast_coords[i][1]]);
                        }
                    }
                }

                if (!figur)
                    return checkSub(ma, 0, 0) && checkSub(ma, 0, 3) && checkSub(ma, 0, 6) && checkSub(ma, 3, 0) && checkSub(ma, 3, 3) && checkSub(ma, 3, 6) && checkSub(ma, 6, 0) && checkSub(ma, 6, 3) && checkSub(ma, 6, 6);
                else
                    return checkSubFigur(ma);
            }

            bool checkSub(int[,] ma, int r, int c)
            {
                List<int> p = new List<int>();
                for (int i = r; i < r + 3; i++)
                {
                    for (int j = c; j < c + 3; j++)
                    {
                        if (ma[i, j] != -1)
                        {
                            if (p.Contains(ma[i, j]))
                                return false;
                            else
                                p.Add(ma[i, j]);
                        }
                    }
                }
                return true;
            }

            bool checkSubFigur(int[,] ma)
            {
                for (int i = 0; i < 9; i++)
                {
                    List<int> p = new List<int>();
                    for (int j = 0; j < 9; j++)
                    {
                        int row = kvadr[i].coord[j][0];
                        int col = kvadr[i].coord[j][1];
                        if (ma[row,col] != -1)
                        {
                            if (p.Contains(ma[row, col]))
                                return false;
                            else
                                p.Add(ma[row, col]);
                        }
                    }
                }
                return true;
            }

            int[] freeCoords(int[,] ma)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (ma[i, j] == -1)
                        {
                            return new int[] { i, j };
                        }
                    }
                }
                return new int[] { };
            }

            int[,] toMatrix(List<int[,]> a_)
            {
                int[,] pole = new int[9, 9];
                int x = 0, y = 0;
                for (int q = 0; q < 9; q++)
                {
                    if (q < 3)
                        y = 0;
                    if (q > 2 && q < 6)
                        y = 3;
                    if (q > 5)
                        y = 6;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pole[y, x] = a_[q][i, j];
                            x++;
                            if (x == 3 || x == 6 || x == 9)
                            {
                                y++;
                                x -= 3;
                            }
                        }
                    }
                    x += 3;
                    if (x == 9)
                        x = 0;
                }
                return pole;
            }
            
            static void printMatr(int[,] m)
            {
                int rows = m.GetLength(0);
                int cols = m.GetLength(1);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write(m[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void figurnoe_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            figur = false;
            if (figurnoe_checkBox.Checked)
            {
                FigureSettingsForm fsf = new FigureSettingsForm();
                fsf.cancel_button.Click += (object se, EventArgs ea) =>
                {
                    figurnoe_checkBox.Checked = false;
                    fsf.Close();
                };
                fsf.OK_button.Click += (object se, EventArgs ea) =>
                {
                    kvadr = fsf.getPole();
                    if (fsf.isCorrect)
                    {
                        figur = true;
                        colorGroups(toList(fsf.groups));
                        fsf.Close();
                    }
                };
                fsf.ShowDialog();
            }
        }
    }
}

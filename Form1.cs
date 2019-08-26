using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bejeweled_multiplayer
{
    public partial class Form1 : Form
    {
        int size = 10;
        Button dragged;
        bool[,] revisado;
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel2.ColumnCount = size;
            tableLayoutPanel2.RowCount = size;
            tableLayoutPanel2.Padding = Padding.Empty;
            tableLayoutPanel2.Margin = Padding.Empty;
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            for (int i = 0; i < size*size; i++)
            {
                Button button = new Button();
                button.AllowDrop = true;
                button.Text = "";
                button.TabStop = false;
                button.Padding = Padding.Empty;
                button.Margin = Padding.Empty;
                button.Dock = DockStyle.Fill;
                button.MouseDown += this.button1_MouseDown;
                button.MouseUp += this.button1_MouseUp;
                tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100/size));
                tableLayoutPanel2.Controls.Add(button);
                button.DragDrop += this.button1_DragDrop;
                button.DragEnter += this.button1_DragEnter;
                button.BackColor = Color.White;
            }
            fill();
        }
        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            Button a = sender as Button;
            string temp = e.Data.GetData(DataFormats.Text).ToString();
            dragged.BackColor = a.BackColor;
            a.BackColor = Color.FromArgb(Int32.Parse(temp));
            dragged.Text = "";
        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Button a= sender as Button;
            dragged = a;
            a.Text = "S";
            a.DoDragDrop(a.BackColor.ToArgb().ToString(),DragDropEffects.Move);
        }
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            this.label2.Focus();
            dragged.Text = "";
        }
        private void score()
        {
            int temp1;
            int temp2;
            int h = 0;
            int v = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp1 = tableLayoutPanel2.GetControlFromPosition(i, j).BackColor.ToArgb();
                   
                    if (!temp1.Equals(Color.White))
                    {
                        //horizontal
                        h = 1;
                        if (i + h < size)
                        {//este if se puede ir con optimizacion
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i + h, j).BackColor.ToArgb();
                            h++;
                        }
                        else
                        {
                            temp2 = Color.White.ToArgb();
                        }
                        while (i + h < size && temp1.Equals(temp2))
                        {//este while se puede optimizar si i=size-2 pero tenia problemas, lo dejo asi por ahora
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i + h, j).BackColor.ToArgb();
                            h++;
                        }
                        if (i + h == size && temp1.Equals(temp2))
                        {//este if se puede ir con optimizacion
                            h++;
                        }
                        //vertical
                        v = 1;
                        if (j + v < size)
                        {//este if se puede ir con optimizacion
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i , j + v ).BackColor.ToArgb();
                            v++;
                        }
                        else
                        {
                            temp2 = Color.White.ToArgb();
                        }
                        while (j + v < size && temp1.Equals(temp2))
                        {//este while se puede optimizar si j=size-2 pero tenia problemas, lo dejo asi por ahora
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i, j + v).BackColor.ToArgb();
                            v++;
                        }
                        if (j + v == size && temp1.Equals(temp2))
                        {//este if se puede ir con optimizacion
                            v++;
                        }

                        //se dan puntos
                        if (v > 3)
                        {
                            label2.Text = (Int64.Parse(label2.Text) + v * 10).ToString();
                        }
                        if (h > 3)
                        {
                            label2.Text = (Int64.Parse(label2.Text) + h * 10).ToString();
                        }
                        //se borran los cuadros del match
                        if (v > 3)
                        {
                            for (int l = 0; l < v - 1; l++)
                            {
                                tableLayoutPanel2.GetControlFromPosition(i, j + l).BackColor = Color.White;
                            }
                        }
                        if (h > 3)
                        {
                            for (int l = 0; l < h - 1; l++)
                            {
                                tableLayoutPanel2.GetControlFromPosition(i + l, j).BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }
        private void score2()
        {
            revisado = new bool[size, size];
            int temp1;
            int temp2;
            int k;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp1 = tableLayoutPanel2.GetControlFromPosition(i, j).BackColor.ToArgb();
                    if (temp1!=Color.White.ToArgb() && !revisado[i,j])
                    {//si no es blanco y no ha sido revisado
                        if (i+2<size)
                        {//para revision vertical, se evitan las ultimas posiciones
                            k = 1;
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i+k, j).BackColor.ToArgb();
                            while (temp1==temp2)
                            {
                                k++;
                                if (i + k < size)
                                {
                                    temp2 = tableLayoutPanel2.GetControlFromPosition(i + k, j).BackColor.ToArgb();
                                }
                                else
                                {
                                    break;//tambien podria cambiar temp2=white
                                }
                            }
                            if (k>=3)
                            {//k=matches+1
                                revisado[i , j] = true;
                                for (int l = 1; l < k; l++)
                                {//si hay una ficha a la derecha y no hay a la izq, no lo marque como visto para poder ser revisado en el proximo ciclo
                                    if (j - 1 >= 0)
                                    {//se busca la ficha de la izquierda
                                        temp2 = tableLayoutPanel2.GetControlFromPosition(i + l, j - 1).BackColor.ToArgb();
                                    }
                                    else
                                    {//si no hay ficha a la izq, haga temp2!=temp1
                                        temp2 = Color.White.ToArgb();
                                    }
                                    if (temp1 != temp2)
                                    {//si a la izquierda no hay ficha, se debe revisar a la derecha para determinar si se marca como visto
                                        if (j+1<size)
                                        {//si es posible verificar a la derecha
                                            temp2 = tableLayoutPanel2.GetControlFromPosition(i+l, j + 1).BackColor.ToArgb();
                                        }
                                        else
                                        {//si no es posible, haga temp1!=temp2
                                            temp2 = Color.White.ToArgb();
                                        }
                                        if (temp1!=temp2)
                                        {//si a la derecha hay ficha, no se marca como visto, para que en el proximo ciclo haga la verificacion completa
                                            revisado[i + l, j] = true;
                                        }
                                    }
                                    else
                                    {//si a la izquierda hay una ficha, ya se verifico, por tanto se puede marcar como visto
                                        revisado[i + l, j] = true;
                                    }
                                }
                            }
                        }
                        if (j+2<size)
                        {//para revision hotizontal, se evitan las ultimas posiciones
                            k = 1;
                            temp2 = tableLayoutPanel2.GetControlFromPosition(i , j + k).BackColor.ToArgb();
                            while (temp1 == temp2)
                            {
                                //si arriba hay una ficha que concuerde, no haga nada, esa sera analizada en el proximo ciclo
                                //si arriba no hay ficha que concuerde y abajo si hay, haga revision
                                k++;
                                if (j + k < size)
                                {
                                    temp2 = tableLayoutPanel2.GetControlFromPosition(i , j + k).BackColor.ToArgb();
                                }
                                else
                                {
                                    break;//tambien podria cambiar temp2=white
                                }
                            }
                            if (k >= 3)
                            {//k=matches+1
                                revisado[i, j] = true;
                                for (int l = 1; l < k; l++)
                                {//si hay una ficha a la abajo y no hay arriba, no lo marque como visto para poder ser revisado en el proximo ciclo
                                    if (i - 1 >= 0)
                                    {//se busca la ficha de arriba
                                        temp2 = tableLayoutPanel2.GetControlFromPosition(i -1, j + l).BackColor.ToArgb();
                                    }
                                    else
                                    {//si no hay ficha arriba, haga temp2!=temp1
                                        temp2 = Color.White.ToArgb();
                                    }
                                    if (temp1 != temp2)
                                    {//si arriba no hay ficha, se debe revisar aabajo para determinar si se marca como visto
                                        if (i+1<size)
                                        {//si es posible verificar abajo
                                            temp2 = tableLayoutPanel2.GetControlFromPosition(i + 1, j + l).BackColor.ToArgb();
                                        }
                                        else
                                        {//si no hay ficha arriba, haga temp2!=temp1
                                            temp2 = Color.White.ToArgb();
                                        }
                                        if (temp1 != temp2)
                                        {//si a la derecha hay ficha, no se marca como visto, para que en el proximo ciclo haga la verificacion completa
                                            revisado[i , j + l] = true;
                                        }
                                    }
                                    else
                                    {//si arriba hay una ficha, ya se verifico, por tanto se puede marcar como visto
                                        revisado[i, j + l] = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            erase();
        }
        private void erase()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (revisado[i,j])
                    {
                        tableLayoutPanel2.GetControlFromPosition(i, j).BackColor= Color.White;
                        label2.Text = (Int64.Parse(label2.Text) + 10).ToString();
                    }
                }
            }
            fill();
        }
        private void fill()
        {
            Color temp;
            Color[] colores = { Color.Red, Color.Blue, Color.Yellow, Color.Green , Color.Purple, Color.Brown, Color.Aqua, Color.Orange};
            Random rand = new Random();
            int k = 0;
            int k2 = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = size-1; j >=0; j--)
                {
                    temp= tableLayoutPanel2.GetControlFromPosition(i, j).BackColor;
                    if (temp.Equals(Color.White))
                    {
                        k = k2 = 0;
                        while (j - k > 0)
                        {//se busca un NO blanco y se "baja" a la posicion del siguiente blanco
                            k++;
                            if(!tableLayoutPanel2.GetControlFromPosition(i, j - k).BackColor.Equals(Color.White))
                            {
                                tableLayoutPanel2.GetControlFromPosition(i, j - k2).BackColor = tableLayoutPanel2.GetControlFromPosition(i, j - k).BackColor;
                                tableLayoutPanel2.GetControlFromPosition(i, j - k).BackColor=Color.White;
                                tableLayoutPanel2.Refresh();
                                k2++;
                            }
                        }
                        //ahora j-k2 debe contener la posicion del blanco mas alto
                        while (j - k2 >= 0)
                        {//se llena de aleatorios
                            tableLayoutPanel2.GetControlFromPosition(i, j-k2).BackColor = colores[rand.Next(colores.Length)];
                            tableLayoutPanel2.Refresh();
                            k2++;
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            score2();
        }
    }
}

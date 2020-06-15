using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        Bitmap[] bitmapGroup;
        Group gg;
        Layer[] selectedGroup;
        //Bitmap tempGrab;
        LinkedList<Group> gr;
        LinkedList<Layer> l;
        //int move = 0;
        Bitmap bmp ;
        bool grab = false;
        int groupCounter = 0;
        int layerCounter = 0;
        Color c;
        int size = 25;
        Graphics g;
        Brush b;
        //Rectangle rect;
        //GraphicsUnit units;

        Pen p;

        float lastMouseX = 0 ;
        float lastMouseY = 0;


        float x = 0;
        float y = 0;

        float change = 3f;
        float changeGroup = 1f;

        Bitmap temp;

        Layer tLayer;

        float resulotion = 10f;

        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            l = new LinkedList<Layer>();
            gr = new LinkedList<Group>();

            c = Color.Black;

            //c = Color.FromArgb();

            g = pictureBox1.CreateGraphics();

            //g.SmoothingMode = SmoothingMode.HighSpeed;

            //g.InterpolationMode = InterpolationMode.Low;

            //g.CompositingQuality = CompositingQuality.HighSpeed;

            //g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            //g.SmoothingMode = SmoothingMode.HighSpeed;

            //b = Brushes.Black;
            trackBar1.Value = 8;
            size = trackBar1.Value * 3;

            listBox1.SelectionMode = SelectionMode.MultiExtended;


            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp.SetResolution(resulotion, resulotion);

            label1.Text = "Information";

            //units = GraphicsUnit.Pixel;

            makePen(c);

            //DoubleBuffered = true;
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint| ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserMouse, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            /*
            button1.Image = Image.FromFile("E:\\pics\\edit.png");
            button2.Image = Image.FromFile("E:\\pics\\color.png");
            button3.Image = Image.FromFile("E:\\pics\\eraser4.png");
            button4.Image = Image.FromFile("E:\\pics\\add.png");
            button5.Image = Image.FromFile("E:\\pics\\brush2.png");
            button6.Image = Image.FromFile("E:\\pics\\grab.png");
            */


            AddLayer();
        }

        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndices.Count == 1)
            {
                if (listBox1.Items[listBox1.SelectedIndex].ToString().Contains("Group"))
                {
                    //MessageBox.Show("Group Changed");
                }
                else
                {

                    int index = findIndex();

                    //tLayer = l.ElementAt<Layer>(listBox1.SelectedIndex);
                    try//because of that findIndex works with Strings of name of Layers
                    {
                        tLayer = l.ElementAt<Layer>(index - 1);
                    }
                    catch (Exception ee)
                    {
                        tLayer = l.ElementAt<Layer>(listBox1.SelectedIndex);

                        //MessageBox.Show(listBox1.SelectedIndex + "");
                    }
                    

                    tLayer.g.SmoothingMode = SmoothingMode.HighSpeed;
                    tLayer.g.InterpolationMode = InterpolationMode.Low;
                    tLayer.g.CompositingQuality = CompositingQuality.HighSpeed;
                    tLayer.g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                    temp = new Bitmap(tLayer.bitmap);
                    temp.SetResolution(resulotion, resulotion);

                    //saveBitmaps(listBox1.SelectedIndex);
                } 
            }  
        }

        public int findIndex()
        {
            String s = listBox1.Items[listBox1.SelectedIndex].ToString();
            int index;
            s = s.Remove(0, 5);
            //MessageBox.Show(s);
            Int32.TryParse(s, out index);
            //MessageBox.Show(gr.ElementAt<Group>(index - 1).group.Length + "");

            return index;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (l.Count == 0)
                {
                    MessageBox.Show("Add Layer to Sketch!");
                }
            }
                   
            if (grab == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndices.Count == 1)
                    {
                        

                        if (listBox1.Items[listBox1.SelectedIndex].ToString().Contains("Group"))
                        {

                            if (e.X > lastMouseX)
                            {
                                x += changeGroup;
                                lastMouseX = e.X;
                            }
                            else if (e.X < lastMouseX)
                            {
                                x -= changeGroup;
                                lastMouseX = e.X;
                            }


                            if (e.Y > lastMouseY)
                            {
                                y += changeGroup;
                                lastMouseY = e.Y;
                            }
                            else if (e.Y < lastMouseY)
                            {
                                y -= changeGroup;
                                lastMouseY = e.Y;
                            }

                            int index = findIndex();

                            gg = gr.ElementAt<Group>(index - 1);
                            selectedGroup = new Layer[gg.group.Length];

                            for (int i = 0; i < gg.group.Length ; i++)
                            {
                                selectedGroup[i] = l.ElementAt<Layer>(gg.group[i]);
                                selectedGroup[i].reset();
                            }
 
                            bitmapGroup = new Bitmap[gg.group.Length];
                            
                            for (int j = 0; j < selectedGroup.Length ; j++)
                            {
                                

                                bitmapGroup[j] = new Bitmap(selectedGroup[j].bitmap);
                                bitmapGroup[j].SetResolution(resulotion, resulotion);

                                selectedGroup[j].g.Clear(Color.Transparent);
                                selectedGroup[j].g.DrawImage(bitmapGroup[j], x, y);
                            }

                            pictureBox1.Image = RenderAll2();
                        }
                        else
                        {

                            if (e.X > lastMouseX)
                            {
                                x += change;
                                lastMouseX = e.X;
                            }
                            else if (e.X < lastMouseX)
                            {
                                x -= change;
                                lastMouseX = e.X;
                            }


                            if (e.Y > lastMouseY)
                            {
                                y += change;
                                lastMouseY = e.Y;
                            }
                            else if (e.Y < lastMouseY)
                            {
                                y -= change;
                                lastMouseY = e.Y;
                            }
                            

                            tLayer.g.Clear(Color.Transparent);

                            tLayer.g.DrawImage(temp, x, y);


                            label1.Text = "e.X = " + e.X + "   e.Y : " + e.Y + "   x : " + x + "   y : " + y +
                                "   lastMouseX : " + lastMouseX + "   lasMouseY : " + lastMouseY;

                            //restoreBitmaps(listBox1.SelectedIndex);
                            pictureBox1.Image = RenderAll2();
                            //pictureBox1.Image = RenderAll2();
                        }
                    }
                }
                else
                {
                    x = 0;
                    y = 0;
                    lastMouseY = 0;
                    lastMouseY = 0;
                }   
            }
            else if (grab == false)
            {

                if (listBox1.SelectedItems.Count == 1 && listBox1.SelectedIndex != -1)
                {
                    if (e.Button == MouseButtons.Left)
                    {

                        if (listBox1.Items[listBox1.SelectedIndex].ToString().Contains("Group"))
                        {

                        }
                        else
                        {
                            int index = findIndex();


                            //tLayer = l.ElementAt<Layer>(listBox1.SelectedIndex);
                            tLayer = l.ElementAt<Layer>(index-1);

                            //tLayer.g.DrawLine(p, e.X - size / 2, e.Y - size / 2, size, size);
                            if (lastMouseX == 0 && lastMouseY == 0)
                            {
                            }
                            else
                            {
                                tLayer.g.DrawLine(p, lastMouseX, lastMouseY, e.X, e.Y);

                            }

                            pictureBox1.Image = RenderAll2();

                            label2.Text = "Drawing";

                            //lastMouseX = e.X;
                            //lastMouseY = e.Y;
                        }               
                    }
                }
            }

            lastMouseX = e.X;
            lastMouseY = e.Y;
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            size = trackBar1.Value*3;

            makePen(c);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = findIndex();
            index--;

            if (listBox1.SelectedIndex != -1)
            {
                
                tLayer = l.ElementAt<Layer>(index);

                tLayer.g.Clear(Color.Transparent);
                //tLayer.reset();

                pictureBox1.Image = RenderAll2();
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog myColor = new ColorDialog();

            if (myColor.ShowDialog() == DialogResult.OK)
            {
                c = myColor.Color;

                makePen(c);

                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            grab = false;

            makePen(Color.White);

            pictureBox1.Cursor = Cursors.Cross;

        }

        private void button4_Click(object sender, EventArgs e)
        {

            AddLayer();

            refreshList();

            selectLastListItem();
        }

        public void AddLayer()
        {
            layerCounter++;
            String s = "" ;
            bool rotate = true;

            while (rotate)
            {
                s = "Layer" + layerCounter;
                rotate = false;
                for(int i=0;i<l.Count;i++)
                {
                    if (l.ElementAt<Layer>(i).name == s)
                    {
                        rotate = true;
                    }
                }

                if (rotate == true)
                {
                    layerCounter++;
                }
                else
                {
                    rotate = false;
                }
            }


            
            Layer layer = new Layer(pictureBox1, s, listBox1);

            l.AddLast(layer);

            listBox1.SelectedIndex = -1; // Deselect All
            listBox1.SelectedItem = l.Last<Layer>().name; // Select the one which Created NOW
        }

        public void selectLastListItem()
        {
            listBox1.SelectedItem = l.Last<Layer>().name;
        }

        public void AddLayer(Bitmap bmp)
        {
            layerCounter++;
            String s = "Layer" + layerCounter;
            Layer layer = new Layer(pictureBox1, s, listBox1);
            layer.bitmap = bmp;

            layer.reset();

            l.AddLast(layer);

            listBox1.SelectedIndex = -1; // Deselect All
            listBox1.SelectedItem = l.Last<Layer>().name; // Select the one which Created NOW
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //pictureBox1.Image =  RenderAll2();
        }

        public void delete()
        {
            int index = findIndex();

            if (listBox1.Items[listBox1.SelectedIndex].ToString().Contains("Group"))
            {

                gr.Remove(gr.ElementAt<Group>(index-1));

                groupCounter--;
            }
            else
            {
                bool rotate = true;

                while (rotate)
                {
                    try
                    {
                        l.Remove(l.ElementAt<Layer>(index - 1));
                        layerCounter--;

                        rotate = false;
                    }
                    catch (Exception ee)
                    {
                        if (listBox1.SelectedIndex < index-1)
                        {
                            index++;
                        }
                        else
                        {
                            index--;
                        }
                        
                    }
                }
                //l.Remove(l.ElementAt<Layer>(listBox1.SelectedIndex));
                
                
                //listBox1.Items.Remove(listBox1.SelectedItem.ToString());
            }

            refreshList();

            pictureBox1.Image = RenderAll2();


            //MessageBox.Show(index + "");
            if (index-1 > 0)
                listBox1.SelectedIndex = index - 1 -1;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Delete)
            {
                if (listBox1.SelectedIndex != -1 && listBox1.SelectedItems.Count == 1)
                {
                    delete();
                }
                
            }
            else if (e.KeyCode == Keys.J) // Join
            {
                JoinIt();
            }
            else if (e.KeyCode == Keys.D)
            {
                duplicate();
            }
            else if (e.KeyCode == Keys.H)
            {
                View();
            }
            else if (e.KeyCode == Keys.G)
            {
                Group();
            }
        }

        public void Group()
        {
            if (listBox1.SelectedIndices.Count > 1)
            {
                groupCounter++;
                //Layer group = new Layer(pictureBox1, "Group" + layerCounter);
                int[] layers = new int[listBox1.SelectedIndices.Count];

                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    layers[i] = listBox1.SelectedIndices[i];
                    //MessageBox.Show(layers[i] + "");
                }

                Group newGroup = new Group( layers , "Group" + groupCounter ) ;

                //MessageBox.Show();
                gr.AddLast(newGroup);

                refreshList();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            int tempIndex = listBox1.SelectedIndex;
            bool repeat = false;
            if (e.KeyCode == Keys.Return && tempIndex != -1)
            {
                foreach (string s in listBox1.Items)
                {
                    if (textBox1.Text.Trim() == s)
                    {
                        repeat = true;
                        
                    }
                }

                if (listBox1.SelectedItem.ToString() == textBox1.Text.Trim())
                {

                }
                else if (repeat == true )
                {
                    MessageBox.Show("Duplicate name !!!");
                }

                if (repeat == false)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);
                    //--------------------------
                    listBox1.Items.Insert(tempIndex, textBox1.Text.Trim());

                    listBox1.SelectedIndex = tempIndex;
                    //listBox1.ClearSelected();
                }
                
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            grab = true;

            pictureBox1.Cursor = Cursors.Hand;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            grab = false;

            makePen(c);
            pictureBox1.Cursor = Cursors.Default;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Images | *.png";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               //int width = Convert.ToInt32(pictureBox1.Width); 
               //int height = Convert.ToInt32(pictureBox1.Height); 
               //Bitmap bmp2 = new Bitmap(width,height);        
               //pictureBox1.DrawToBitmap(bmp2 , new Rectangle(0, 0, width, height));
               pictureBox1.Image.Save(dialog.FileName);
            }
        }


        public Bitmap RenderAll2()
        {
            Bitmap b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            b.SetResolution(resulotion, resulotion);
            
            //b.SetResolution(1.1f, 1.1f);
            //MessageBox.Show(b.RawFormat.ToString());

            g = Graphics.FromImage(b);
            //MessageBox.Show("g.dpiX : " + g.DpiX + " " + "g.dpiY : " + g.DpiY);


            //g.Dispose();
            //g.SmoothingMode = SmoothingMode.HighSpeed;

            //g.InterpolationMode = InterpolationMode.Low;

            //g.CompositingQuality = CompositingQuality.HighSpeed;

            //g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            //g.SmoothingMode = SmoothingMode.HighSpeed;

            
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;


            String s = "";

            for (int k = 0; k < l.Count; k++)
            {
                if (l.ElementAt<Layer>(k).visible == true)
                {
                    g.DrawImage(l.ElementAt<Layer>(k).bitmap, 0, 0);

                    s += "Drawing Element " + k;

                    if (k < l.Count - 1)
                    {
                        s += " | ";
                    }
                }
            }

            label3.Text = s;

            //pictureBox1.Invalidate();
            

            return b;
        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            label2.Text = "Waiting...";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            l = new LinkedList<Layer>();
            gr = new LinkedList<Group>();

            listBox1.Items.Clear();

            layerCounter = 0;
            groupCounter = 0;

            pictureBox1.Image = RenderAll2();
        }

        public void makePen(Color c)
        {
            b = new SolidBrush(c);
            p = new Pen(b, size);
            p.SetLineCap(LineCap.Round, LineCap.Round,DashCap.Round);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedItems.Count == 1)
            {
                delete();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            JoinIt();
        }


        public void JoinIt()
        {
            label3.Text = "";
            if (listBox1.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                {
                    //label3.Text += listBox1.SelectedItems[i];
                    label3.Text += listBox1.SelectedIndices[i] + " ";
                }

                Join(listBox1.SelectedIndices);

            }
        }

        public void Join(ListBox.SelectedIndexCollection list)
        {

            int[] layers = new int[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                layers[i] = list[i];
            }

            Layer temp = new Layer(pictureBox1, "tempName");

            //MessageBox.Show(list.Count.ToString());

            for (int i = 0; i < list.Count; i++)
            {
                temp.g.DrawImage(l.ElementAt<Layer>(list[i]).bitmap, 0, 0);
                //MessageBox.Show(l.ElementAt<Layer>(list[i]).name);
                //l.Remove(l.ElementAt<Layer>(i));

                //listBox1.Items.RemoveAt(i);
            }

            for (int i = layers.Length - 1; i >= 0; i--)
            {
                //temp.g.DrawImage(l.ElementAt<Layer>(list[i]).bitmap, 0, 0);
                //MessageBox.Show(l.ElementAt<Layer>(list[i]).name);
                //MessageBox.Show(layers[i].ToString());
                l.Remove(l.ElementAt<Layer>(layers[i]));
                layerCounter--;
                listBox1.Items.RemoveAt(layers[i]);
            }

            layerCounter++;
            String s = "Layer" + layerCounter;
            temp.name = s;
            l.AddLast(temp);
            //listBox1.Items.Add(temp.name);
            refreshList();


            listBox1.SelectedIndex = listBox1.Items.Count - 1;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            duplicate();
        }

        public void duplicate()
        {
            if (listBox1.Items[listBox1.SelectedIndex].ToString().Contains("Group"))
            {
                //Do Nothing
            }
            else
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    //MessageBox.Show(listBox1.SelectedIndex.ToString());
                    Duplicate(listBox1.SelectedIndex);
                }

                refreshList();

                pictureBox1.Image = RenderAll2();
            } 
        }

        public void Duplicate(int x)
        {
            layerCounter++;
            String s = "Layer" + layerCounter;
            Layer temp = new Layer(pictureBox1, s);

            temp.g.DrawImage(l.ElementAt<Layer>(x).bitmap, 0, 0);

            l.AddLast(temp);
            listBox1.Items.Add(temp.name);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            View();
        }

        public void View()
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                //MessageBox.Show(listBox1.SelectedIndex.ToString());
                Hide(listBox1.SelectedIndex);
            }
        }

        public void Hide(int x)
        {
            Layer t = l.ElementAt<Layer>(x);
            if (t.visible == true)
            {
                t.visible = false;
            }
            else if (t.visible == false)
            {
                t.visible = true;
            }

            listBox1.Items.Clear();

            for (int i = 0; i < l.Count; i++)
            {
                if (l.ElementAt<Layer>(i).visible == false)
                {
                    listBox1.Items.Add(l.ElementAt<Layer>(i).name + "(hidden)");
                }
                else
                {
                    listBox1.Items.Add(l.ElementAt<Layer>(i).name);
                }
            }

            for (int i = 0; i < gr.Count; i++)
            {
                listBox1.Items.Add(gr.ElementAt<Group>(i).name);
            }

            listBox1.SelectedIndex = x;

            pictureBox1.Image = RenderAll2();
        }

        private void addLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLayer();
        }

        private void deleteLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedItems.Count == 1)
            {
                delete();
            }
        }

        private void duplicateLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            duplicate();
        }

        private void joinLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JoinIt();
        }

        private void hideViewLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            c = Color.Black;
            makePen(Color.Black);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            c = Color.Red;
            makePen(Color.Red);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            c = Color.Gray;
            makePen(Color.Gray);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            c = Color.Brown;
            makePen(Color.Brown);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            c = Color.Yellow;
            makePen(Color.Yellow);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            c = Color.Lime;
            makePen(Color.Lime);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            c = Color.Magenta;
            makePen(Color.Magenta);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            c = Color.Crimson;
            makePen(Color.Crimson);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            c = Color.Indigo;
            makePen(Color.Indigo);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            c = Color.Blue;
            makePen(Color.Blue);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            c = Color.SkyBlue;
            makePen(Color.SkyBlue);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            c = Color.Cyan;
            makePen(Color.Cyan);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            c = Color.MediumSpringGreen;
            makePen(Color.MediumSpringGreen);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            c = Color.Green;
            makePen(Color.Green);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            c = Color.DarkOrange;
            makePen(Color.DarkOrange);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            c = Color.Chocolate;
            makePen(Color.Chocolate);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            c = Color.IndianRed;
            makePen(Color.IndianRed);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap op;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All files (*.*)|*.*";


            if (DialogResult.OK == open.ShowDialog())
            {
                op = new Bitmap(open.FileName);
                op.SetResolution(resulotion, resulotion);

                AddLayer(op);
            }

            pictureBox1.Image = RenderAll2();
        }

        public void refreshList()
        {
            listBox1.Items.Clear();

            for (int i = 0; i < l.Count; i++)
            {
                listBox1.Items.Add(l.ElementAt<Layer>(i).name);
            }

            for (int i = 0; i < gr.Count; i++)
            {
                listBox1.Items.Add(gr.ElementAt<Group>(i).name);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int selected = listBox1.SelectedIndex;
            //int selected = findIndex();
            //selected -= 1;

            //MessageBox.Show(selected + "");

            bool rotate = true;

            bool exceptionOccur = false;

            int before = listBox1.SelectedIndex - 1;

            //MessageBox.Show(before + "");

            Layer temp = new Layer() ;
            Layer temp2 = new Layer() ;

            if (selected > 0)
            {

                LinkedList<Layer> ltemp = new LinkedList<Layer>();


                try
                {
                    temp = l.ElementAt<Layer>(selected);
                }
                catch (Exception ee)
                {
                    exceptionOccur = true;

                    selected = findIndex();
                    selected--;

                    temp = l.ElementAt<Layer>(selected);
                }
                

                while (rotate)
                {
                    try
                    {
                        temp2 = l.ElementAt<Layer>(before);
                        rotate = false;
                    }
                    catch (Exception ee)
                    {
                        before--;
                    }
                }
                
                
                for (int i = 0; i < l.Count; i++)
                {
                    if (i == before)
                    {
                        ltemp.AddLast(temp);
                    }
                    else if (i == selected)
                    {
                        ltemp.AddLast(temp2);
                    }
                    else
                    {
                        ltemp.AddLast(l.ElementAt<Layer>(i));
                    }
                    
                }
               
                //l.Remove(l.ElementAt<Layer>(selected));

                //l.AddAfter(temp , l.ElementAt<Layer>(selected-1));

                l = ltemp;

                refreshList();

                if (exceptionOccur == true)
                {
                    listBox1.SelectedIndex = selected;
                }
                else
                {
                    listBox1.SelectedIndex = selected - 1;
                }

                pictureBox1.Image = RenderAll2();
            }
       
        }

        private void button29_Click(object sender, EventArgs e)
        {
            int selected = listBox1.SelectedIndex;
            //int selected = findIndex();
            //selected -= 1;

            int after = listBox1.SelectedIndex + 1;
            bool rotate = true;

            bool exceptionOccur = false;

            Layer temp = new Layer();
            Layer temp2 = new Layer();

            if (selected < l.Count-1 ) // l.Count-1 is the size and we want to select at least 1 before that
            {

                LinkedList<Layer> ltemp = new LinkedList<Layer>();

                try
                {
                    temp = l.ElementAt<Layer>(selected);
                }
                catch (Exception ee)
                {
                    exceptionOccur = true;

                    selected = findIndex();
                    selected++;

                    temp = l.ElementAt<Layer>(selected);
                }

                

                while (rotate)
                {
                    try
                    {
                        temp2 = l.ElementAt<Layer>(after);
                        rotate = false;
                    }
                    catch (Exception ee)
                    {
                        after++;
                    }
                }
                

                for (int i = 0; i < l.Count; i++)
                {
                    if (i == after)
                    {
                        ltemp.AddLast(temp);
                    }
                    else if (i == selected)
                    {
                        ltemp.AddLast(temp2);
                    }
                    else
                    {
                        ltemp.AddLast(l.ElementAt<Layer>(i));
                    }

                }

                //l.Remove(l.ElementAt<Layer>(selected));

                //l.AddAfter(temp , l.ElementAt<Layer>(selected-1));

                l = ltemp;

                refreshList();


                if (exceptionOccur == true)
                {
                    listBox1.SelectedIndex = selected;
                }
                else
                {
                    listBox1.SelectedIndex = selected + 1;
                }

                pictureBox1.Image = RenderAll2();
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            c = Color.White;
            makePen(c);
        }

        private void groupLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Group();
        }

    }


    public class Layer
    {
        public Bitmap bitmap;
        public Graphics g;
        public String name;
        public bool visible;
        float resulotion = 10f;

        public Layer()
        {
        }

        public Layer(PictureBox pic, String name)
        {
            this.bitmap = new Bitmap(pic.Width, pic.Height);
            this.bitmap.SetResolution(resulotion, resulotion);

            this.name = name;
            this.visible = true;
            //l.Items.Add(name);

            this.g = Graphics.FromImage(bitmap);

            //this.g.SmoothingMode = SmoothingMode.HighSpeed;

            //this.g.InterpolationMode = InterpolationMode.Low;

            //this.g.CompositingQuality = CompositingQuality.HighSpeed;

            //this.g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            //g.SmoothingMode = SmoothingMode.HighSpeed;

            

        }

        public Layer(PictureBox pic, String name, ListBox l)
        {
            this.bitmap = new Bitmap(pic.Width, pic.Height);
            this.bitmap.SetResolution(resulotion, resulotion);

            this.name = name;
            this.visible = true;
            l.Items.Add(name);

            this.g = Graphics.FromImage(bitmap);

            //this.g.SmoothingMode = SmoothingMode.HighSpeed;

            //this.g.InterpolationMode = InterpolationMode.Low;

            //this.g.CompositingQuality = CompositingQuality.HighSpeed;

            //this.g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            //g.SmoothingMode = SmoothingMode.HighSpeed;

            

        }

        public void reset()
        {
            this.g = Graphics.FromImage(this.bitmap);
        }

    }


    public class Group
    {
        public String name;
        public bool visible;
        //public LinkedList<Layer> group;
        public int[] group;


        public Group()
        {

        }

        public Group(int[] layers, String name)
        {
            this.name = name;

            group = new int[layers.Length];

            for (int i = 0; i < layers.Length; i++)
            {
                this.group[i] = layers[i];
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace grafikaLab6V2
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        Image guy;
        Bitmap bitmapguy;
        int guyWidth, guyHeight;
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            guy = pictureBox1.Image;
            guy = resizeImage(guy, new Size(pictureBox1.Size.Width, pictureBox1.Size.Height));
            bitmapguy = new Bitmap(guy);
            guyWidth = bitmapguy.Width;
            guyHeight = bitmapguy.Height;

        }
        public int checkIfInRGB(int value)
        {
            if (value >= 255) return 254;
            if (value <= 0) return 1;
            else return value;
        }
        private int zmniejszKontrast(int color, int delta)
        {
            if (color < 127 + delta) return (127 / checkIfInRGB(127 + delta)) * color;
            else if (color > 127 - delta) return ((127 * color) + (255 * delta)) / checkIfInRGB(127 + delta);
            else return 127;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(guy);
            for (int y = 0; y < guyHeight; y++)
            {
                for (int x = 0; x < guyWidth; x++)
                {
                    Color guyPixel = bitmapguy.GetPixel(x, y);

                    int a = guyPixel.A;
                    int r = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (guyPixel.R - trackBar1.Value));
                    int g = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (guyPixel.G - trackBar1.Value));
                    int b = checkIfInRGB((127 / checkIfInRGB(127 - trackBar1.Value)) * (guyPixel.B - trackBar1.Value));
                    temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            wypelnijHistrogram(temp);
            pictureBox2.Image = temp;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(guy);
            for (int y = 0; y < guyHeight; y++)
            {
                for (int x = 0; x < guyWidth; x++)
                {
                    Color guyPixel = bitmapguy.GetPixel(x, y);

                    int a = guyPixel.A;
                    int r = zmniejszKontrast(guyPixel.R, trackBar2.Value);
                    int g = zmniejszKontrast(guyPixel.G, trackBar2.Value);
                    int b = zmniejszKontrast(guyPixel.B, trackBar2.Value);
                    temp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            wypelnijHistrogram(temp);
            pictureBox2.Image = temp;
        }

        private void wypelnijHistrogram(Bitmap temp)
        {
            int[] czerw = new int[256];
            int[] ziel = new int[256];
            int[] nieb = new int[256];
            for (int x = 0; x < guyWidth; x++)
            {
                for (int y = 0; y < guyHeight; y++)
                {
                    Color pixel = temp.GetPixel(x, y);
                    czerw[pixel.R]++;
                    ziel[pixel.G]++;
                    nieb[pixel.B]++;
                }
            }

            //Wyswietl histogram na wykresie
            chart1.Series["Red"].Points.Clear();
            chart1.Series["Green"].Points.Clear();
            chart1.Series["Blue"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart1.Series["Red"].Points.AddXY(i, czerw[i]);
                chart1.Series["Green"].Points.AddXY(i, ziel[i]);
                chart1.Series["Blue"].Points.AddXY(i, nieb[i]);
            }
            chart1.Invalidate();
        }
    }

}

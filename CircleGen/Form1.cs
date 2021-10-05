using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CircleGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public Random random = new Random();
        int new_number = 0;
        public List<int> randomList = new List<int>();
        private void Form1_Load(object sender, EventArgs e)
        {
            //размер полотна
            int width = 800, height = width;
            //количество кругов
            int circles_amount = 100;
            //радиус кругов
            int radius = random.Next(10, 30);
            
            Bitmap bitmap = new Bitmap(width, height);
            //генерация кругов
            for (int ii = 0; ii < circles_amount; ii++)
            {
                using (Graphics gr = Graphics.FromImage(bitmap))
                    {
                        int randX = NewRandomNumber(width, radius) == 0 ? NewRandomNumber(width, radius) : new_number; 
                        int randY = NewRandomNumber(width, radius) == 0 ? NewRandomNumber(width, radius) : new_number;
                        gr.FillEllipse(Brushes.Black,  randX, randY, radius, radius);
                    }
            }

            SaveExt.Image(bitmap);
            Close();
        }

        int NewRandomNumber(int bound, int radius)
        {
            new_number = random.Next(0, bound - radius);
            if (!randomList.Contains(new_number))
            {
                randomList.Add(new_number);
                return new_number;
            }
            return 0;
        }
    }
    
    public static class SaveExt
    {
        public static void Image(Bitmap bitmap)
        {
            string desktop = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
            int i = 0, filenum = 0;
            do
            {
                filenum = i + 1;
                i++;
            } while (File.Exists(@$"{desktop}\{i}.png"));

            bitmap.Save(@$"{desktop}\{filenum}.png", ImageFormat.Png);
        }
    }
}

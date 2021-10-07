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
        public List<int> listY = new List<int>();
        public List<int> listX = new List<int>();
        private void Form1_Load(object sender, EventArgs e)
        {
            //размер полотна
            int width = 800, height = width;
            //количество кругов
            int circles_amount = 200;
            //радиус кругов
            int diameter = random.Next(10, 40);
            //количество картинок для генерации
            int picNum = 20;
            
            //генерация картинок
            for (int i = 0; i < picNum; i++)
            {
                Bitmap bitmap = new Bitmap(width, height);
                //генерация кругов
                for (int ii = 0; ii < circles_amount; ii++)
                {
                    using (Graphics gr = Graphics.FromImage(bitmap))
                    {
                        int randX = NewRandomNumber(width, diameter) == 0 ? NewRandomNumber(width, diameter) : new_number;
                        int randY = NewRandomNumber(width, diameter) == 0 ? NewRandomNumber(width, diameter) : new_number;
                        if (CheckOverlap(diameter, randX, randY))
                        {
                            gr.FillEllipse(Brushes.Black, randX, randY, diameter, diameter);
                        }
                    }
                }
                SaveExt.Image(bitmap);
                listX.Clear();
                listY.Clear();
                randomList.Clear();
            }
            Close();
        }
        
        bool CheckOverlap(int diameter, int randX, int randY)
        {
            for (int i = 0; i < listX.Count; i++)
            {
                int otherX = listX[i];
                int otherY = listY[i];
                Rectangle rect1 = new Rectangle(randX, randY, diameter, diameter);
                Rectangle rect2 = new Rectangle(otherX, otherY, diameter, diameter);

                if (rect1.IntersectsWith(rect2))
                {
                    return false;
                }
            }
            listX.Add(randX);
            listY.Add(randY);
            return true;
        }

        int NewRandomNumber(int bound, int diameter)
        {
            new_number = random.Next(0, bound - diameter);

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
            string desktop = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Выборка";
            int i = 0, filenum = 0;
            do
            {
                filenum += 1;
                i++;
            } while (File.Exists(@$"{desktop}\{i}.png"));

            if (Directory.Exists(desktop) == false)
            {
                Directory.CreateDirectory(desktop);
                bitmap.Save(@$"{desktop}\{filenum}.png", ImageFormat.Png);
            }
            else
            {
                bitmap.Save(@$"{desktop}\{filenum}.png", ImageFormat.Png);
            }
        }
    }
}

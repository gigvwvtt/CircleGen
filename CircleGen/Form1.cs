using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CircleGen
{
	public partial class Form1 : Form
	{
		private readonly List<int> listX = new();
		private readonly List<int> listY = new();
		private int newNumber;

		private readonly Random random = new();
		private readonly List<int> randomList = new();

		private readonly List<int> diameters = new();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//размер полотна
			const int width = 512;
			const int height = width;
			//количество кругов
			const int circlesAmount = 30;
			
			//количество картинок для генерации
			const int picNum = 10;

			//проверка на наличие директории
			string Directory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
			if (!System.IO.Directory.Exists(@$"{Directory}\Круги"))
            {
				System.IO.Directory.CreateDirectory(@$"{Directory}\Круги");
			}
			Directory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Круги";
			//очистка директории
			DirectoryInfo di = new DirectoryInfo(Directory);
            foreach (var file in di?.GetFiles())
            {
                file?.Delete();
			}

            var drawFont = new Font("Arial", 14);
			var drawBrush = new SolidBrush(Color.Red);

			//генерация картинок
			for (var i = 0; i < picNum; i++)
			{
				var bitmap = new Bitmap(width, height);
				//генерация кругов
				for (var ii = 0; ii < circlesAmount; ii++)
				{
                    //радиус кругов
					var diameter = random.Next(10, 80);
					using var gr = Graphics.FromImage(bitmap);
					var randX = NewRandomNumber(width, diameter);
					var randY = NewRandomNumber(width, diameter);
					if (CheckOverlap(diameter, randX, randY)) continue;
					gr.FillEllipse(Brushes.Black, randX, randY, diameter, diameter);
					gr.DrawString(diameter.ToString(), drawFont, drawBrush, randX, randY);
					diameters.Add(diameter);
				}

				SaveExt.Image(bitmap);
				SaveExt.Csv(diameters, isColumn: true);
				listX.Clear();
				listY.Clear();
				randomList.Clear();
			}

			Close();
		}

		private bool CheckOverlap(int diameter, int randX, int randY)
		{
			for (var i = 0; i < listX.Count; i++)
			{
				var otherX = listX[i];
				var otherY = listY[i];
				var rect1 = new Rectangle(randX, randY, diameter, diameter);
				var rect2 = new Rectangle(otherX, otherY, diameter, diameter);
				if (rect1.IntersectsWith(rect2)) return true;
			}

			listX.Add(randX);
			listY.Add(randY);
			return false;
		}

		private int NewRandomNumber(int bound, int diameter)
		{
			if (randomList.Count == bound - diameter)
				throw new ArgumentOutOfRangeException("Использованный костыль не позволяет генерировать более чем " +
				                                      ((bound - diameter) / 2 - 1) + " кругов");
			do
			{
				newNumber = random.Next(0, bound - diameter);
			} while (randomList.Contains(newNumber));

			randomList.Add(newNumber);
			return newNumber;
		}
	}

	public static class SaveExt
	{
		// каталог для сохранения
		private static readonly string Directory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Круги";
		// очищать предыдущие сгенерированные круги
		private static bool clearDirectory = true;

		public static void Image(Bitmap bitmap)
		{
			var filenum = GetFileNumber("png");
			bitmap.Save(@$"{Directory}\{filenum}.png", ImageFormat.Png);
		}

		public static void Csv<T>(List<T> values, bool isColumn)
		{
			var fileNamePrefix = "diam";
			var filenum = GetFileNumber("csv", fileNamePrefix);
			var separator = isColumn ? "\n" : ";";
			var csvData = string.Join(separator, values);
			File.WriteAllText(@$"{Directory}\{fileNamePrefix}{filenum}.csv", csvData);
			values.Clear();
		}

		private static int GetFileNumber(string extension, string prefix = "")
		{
			var filenum = 1;
			while (File.Exists(@$"{Directory}\{prefix}{filenum}.{extension}"))
			{
				filenum++;
			}

			if (clearDirectory) filenum = 1;
			clearDirectory = false;
			return filenum;
		}
	}
}
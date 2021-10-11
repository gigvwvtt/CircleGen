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
		private readonly List<int> listX = new();
		private readonly List<int> listY = new();
		private int newNumber;

		private readonly Random random = new();
		private readonly List<int> randomList = new();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//размер полотна
			const int width = 800;
			const int height = width;
			//количество кругов
			const int circlesAmount = 300;
			//радиус кругов
			var diameter = random.Next(10, 40);
			//количество картинок для генерации
			const int picNum = 10;

			//генерация картинок
			for (var i = 0; i < picNum; i++)
			{
				var bitmap = new Bitmap(width, height);
				//генерация кругов
				for (var ii = 0; ii < circlesAmount; ii++)
				{
					using var gr = Graphics.FromImage(bitmap);
					var randX = NewRandomNumber(width, diameter);
					var randY = NewRandomNumber(width, diameter);
					if (!CheckOverlap(diameter, randX, randY))
						gr.FillEllipse(Brushes.Black, randX, randY, diameter, diameter);
				}

				SaveExt.Image(bitmap);
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
		// очищать предыдущие сгенерированные круги
		private static bool clearDirectory = true;

		public static void Image(Bitmap bitmap)
		{
			var desktop = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Круги";
			var filenum = 1;
			while (File.Exists(@$"{desktop}\{filenum}.png"))
			{
				if (clearDirectory) File.Delete(@$"{desktop}\{filenum}.png");
				filenum++;
			}

			if (clearDirectory) filenum = 1;
			clearDirectory = false;
			if (!Directory.Exists(desktop)) Directory.CreateDirectory(desktop);
			bitmap.Save(@$"{desktop}\{filenum}.png", ImageFormat.Png);
		}
	}
}
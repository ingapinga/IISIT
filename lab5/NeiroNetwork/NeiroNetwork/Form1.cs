using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace NeiroNetwork
{
	public partial class Form1 : Form
	{
		const int pixel_number_h = 20, pixel_number_w = 20;
		const int pixel_h = 20, pixel_w = 20;
		bool statemousedown = false;
		Bitmap img = new Bitmap(pixel_number_h * pixel_h, pixel_number_w * pixel_w);
		Graphics canvas;
		byte[,] pixels_values = new byte[pixel_number_h, pixel_number_w];
		NeuralNet neuralNet = new NeuralNet(new List<int> { 10 }, pixel_number_h * pixel_number_w);

		public Form1()
		{
			InitializeComponent();
			canvas = Graphics.FromImage(img);
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			statemousedown = true;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if(statemousedown && (e.X >= 0) && (e.X < pixel_number_h * pixel_number_w) && (e.Y >= 0) && (e.Y < pixel_number_h * pixel_number_w))
			{
				int x = Convert.ToInt32(Math.Floor((double)e.X / pixel_number_w));
				int y = Convert.ToInt32(Math.Floor((double)e.Y / pixel_number_h));
				pixels_values[x, y] = 1;
				canvas.FillRectangle(Brushes.White, x * pixel_w, y * pixel_h, pixel_w, pixel_h);
				pictureBox1.Image = img;
			}
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			statemousedown = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = null;
			img = new Bitmap(pixel_number_h * pixel_h, pixel_number_w * pixel_w);
			canvas = Graphics.FromImage(img);
			pixels_values = new byte[pixel_number_h, pixel_number_w];
		}

		private void button2_Click(object sender, EventArgs e)
		{
			List<double> values = new List<double>();
			for (int i = 0; i < pixels_values.GetLength(1); i++)
				for (int j = 0; j < pixels_values.GetLength(0); j++)
					values.Add(pixels_values[i, j]);
			textBox1.Text = neuralNet.GuessNumber(values).ToString();
		}
	}
}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
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
        string filepath = @"H:\ИИСИТ\5 лаб\NeiroNetwork\NeiroNetwork\bin\Debug\Выборка\";
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
            if (statemousedown && (e.X >= 0) && (e.X < pixel_number_h * pixel_number_w) && (e.Y >= 0) && (e.Y < pixel_number_h * pixel_number_w))
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

        private void button_teach_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните текстовое поле значением на рисунке");
                return;
            }
            List<byte> pixels = new List<byte>();
            for (int i = 0; i < pixels_values.GetLength(1); i++)
                for (int j = 0; j < pixels_values.GetLength(0); j++)
                    pixels.Add(pixels_values[i, j]);
            neuralNet.BackPropagation(Int32.Parse(textBox1.Text), pixels);
            textBox1.Text = "";
        }

        private void button_LearnFromExcel_Click(object sender, EventArgs e)
        {
            Excel.Application ex = new Excel.Application();
            for (int k = 1; k < 10; k++)
            {
                if (!File.Exists(filepath + k.ToString() + ".xlsx"))
                    break;
                Excel.Workbook workBook = ex.Workbooks.Open(filepath + k.ToString() + ".xlsx");
                List<double> cells_values = new List<double>();
                foreach (Excel.Worksheet sheet in workBook.Worksheets)
                {
                    //Excel.Range range = (Excel.Range)sheet.Cells[]
                    for (int i = 1; i <= pixel_number_w; i++)
                        for (int j = 1; j <= pixel_number_h; j++)
                            cells_values.Add(sheet.Cells[j, i].Value);
                    if (k != neuralNet.GuessNumber(cells_values))
                        neuralNet.BackPropagation(k, ListDoubleToByte(cells_values));
                }
                workBook.Close(false);
                progressBar1.Value = 100 * k / 9;
            }
            ex.Quit();
            progressBar1.Value = 0;
        }
        private List<byte> ListDoubleToByte(List<double> list1)
        {
            List<byte> list2 = new List<byte>();
            foreach (double d in list1)
                list2.Add(Convert.ToByte(d));
            return list2;
        }
        private List<double> ArrayByteToListDouble(byte[,] array)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < array.GetLength(1); i++)
                for (int j = 0; j < array.GetLength(0); j++)
                    list.Add(array[i, j]);
            return list;
        }

        private void button_saveToExcel_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните текстовое поле значением на рисунке");
                return;
            }
            Excel.Application ex = new Excel.Application();
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            for (int i = 1; i <= pixel_number_w; i++)
                for (int j = 1; j <= pixel_number_h; j++)
                    sheet.Cells[j, i] = pixels_values[i - 1, j - 1].ToString();
            //Excel.Range r = sheet.get_Range(sheet.Cells[1, 1], sheet.get_Range(sheet.Cells[20, 20]));
            if (File.Exists(filepath + textBox1.Text + ".xlsx"))
            {
                Excel.Workbook workBook1 = ex.Workbooks.Open(filepath + textBox1.Text + ".xlsx");
                //var sheets = workBook1.Sheets as Excel.Sheets; //var NewSheet = (Excel.Worksheet)sheets.Add(sheets[1], Type.Missing, Type.Missing, Type.Missing);
                sheet.Copy(workBook1.Worksheets[ex.Sheets.Count]);
                workBook1.Close(true);
                workBook.Close(false);
            }
            else
            {
                workBook.SaveAs(filepath + textBox1.Text + ".xlsx");
                workBook.Close(false);
            }
            ex.Quit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            img = new Bitmap(pixel_number_h * pixel_h, pixel_number_w * pixel_w);
            canvas = Graphics.FromImage(img);
            pixels_values = new byte[pixel_number_h, pixel_number_w];
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<double> pixels = ArrayByteToListDouble(pixels_values);
            int answer = neuralNet.GuessNumber(pixels);
            if (answer == 0)
                textBox1.Text = "не знаю";
            else
                textBox1.Text = neuralNet.GuessNumber(pixels).ToString();
        }
    }
}

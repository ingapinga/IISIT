using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollDec
{
	public partial class Form1 : Form
	{
		Random r = new Random();
		public Form1()
		{
			InitializeComponent();
			dataGridView1.Rows.Add();
			dataGridView1.Rows.Add();
			dataGridView1.Rows.Add();
			dataGridView1.Rows.Add();
			dataGridView1.Rows.Add();
			GenerateRandomData();
		}

		private void GenerateRandomData()	// случайно заполняем таблицу
		{
			for (int j = 0; j < 5; j++)
			{
				int[] alt = new int[] { 1, 2, 3, 4, 5 };
				for (int i = 0; i < alt.Length; i++)
				{
					int new_pos = r.Next(0, 5);
					int buffer = alt[i];
					alt[i] = alt[new_pos];
					alt[new_pos] = buffer;
				}
				for (int i = 0; i < alt.Length; i++)
					dataGridView1[j, i].Value = alt[i];
			}
			FillLabels();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			GenerateRandomData();
		}

		void FillLabels()	// находим лидеров по каждому методу
		{
			int[,] array_data = new int[5, 5];
			for (int i = 0; i < dataGridView1.RowCount; i++)
				for (int j = 0; j < dataGridView1.ColumnCount; j++)
					array_data[i, j] = (int)dataGridView1[j, i].Value;
			
			int[] results = RelativeMajority(array_data);
			label11.Text = "Альтернатива №1: " + results[0];
			label12.Text = "Альтернатива №2: " + results[1];
			label13.Text = "Альтернатива №3: " + results[2];
			label14.Text = "Альтернатива №4: " + results[3];
			label15.Text = "Альтернатива №5: " + results[4];
			label16.Text = GetAnswer(results);

			// Кондорсе

			results = MethodBorda(array_data);
			label51.Text = "Альтернатива №1: " + results[0];
			label52.Text = "Альтернатива №2: " + results[1];
			label53.Text = "Альтернатива №3: " + results[2];
			label54.Text = "Альтернатива №4: " + results[3];
			label55.Text = "Альтернатива №5: " + results[4];
			label56.Text = GetAnswer(results);
		}

		string GetAnswer(int[] results)	// формируем ответ на основании результатов методов
		{
			int max_value = results[0];	// максимальные баллы
			int max_alt = 1;			// альтернатива с максимальными баллами
			int count_max_alt = 1;      // количество альтернатив с максимальными баллами
			for (int i = 1; i < results.Length; i++)
				if (max_value < results[i])
				{
					max_value = results[i];
					max_alt = i + 1;
					count_max_alt = 1;
				}
				else if (max_value == results[i])
					count_max_alt++;
			if (count_max_alt > 1)
				return "Нет решения!";
			else
				return "Коллективное решение: альтернатива №" + max_alt;
		}

		int[] RelativeMajority(int[,] opinions)	// метод относительного большинства
		{
			int[] alternatives_res = { 0, 0, 0, 0, 0 };		// результаты по каждой альтернативе
			for(int i = 0; i < opinions.GetLength(1); i++)
				switch(opinions[0, i])
				{
					case 1:
						alternatives_res[0]++;
						break;
					case 2:
						alternatives_res[1]++;
						break;
					case 3:
						alternatives_res[2]++;
						break;
					case 4:
						alternatives_res[3]++;
						break;
					case 5:
						alternatives_res[4]++;
						break;
				}
			return alternatives_res;
		}

		int[] MethodBorda(int[,] opinions) // метод Борда
		{
			int[] alternatives_res = { 0, 0, 0, 0, 0 };
			for(int i = 0; i < opinions.GetLength(1); i++)	// пробегаемся по строкам
				for(int j = 0; j < opinions.GetLength(0); j++)  // пробегаемся по столбцам (z-образно)
					switch (opinions[i, j])
					{
						case 1:
							alternatives_res[0] += opinions.GetLength(0) - i - 1;
							break;
						case 2:
							alternatives_res[1] += opinions.GetLength(0) - i - 1;
							break;
						case 3:
							alternatives_res[2] += opinions.GetLength(0) - i - 1;
							break;
						case 4:
							alternatives_res[3] += opinions.GetLength(0) - i - 1;
							break;
						case 5:
							alternatives_res[4] += opinions.GetLength(0) - i - 1;
							break;
					}
			return alternatives_res;
		}

	}
}

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

        private void GenerateRandomData()   // случайно заполняем таблицу
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

        void FillLabels()   // находим лидеров по каждому методу
        {
            int[,] array_data = new int[5, 5];
            for (int i = 0; i < dataGridView1.RowCount; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    array_data[i, j] = Int32.Parse(dataGridView1[j, i].Value.ToString());

            int[] results = RelativeMajority(array_data);
            label11.Text = "Альтернатива №1: " + results[0];
            label12.Text = "Альтернатива №2: " + results[1];
            label13.Text = "Альтернатива №3: " + results[2];
            label14.Text = "Альтернатива №4: " + results[3];
            label15.Text = "Альтернатива №5: " + results[4];
            label16.Text = GetAnswer(results);

            // Кондорсе
            results = Condorcet(array_data);
            label21.Text = "Альтернатива №1: " + results[0];
            label22.Text = "Альтернатива №2: " + results[1];
            label23.Text = "Альтернатива №3: " + results[2];
            label24.Text = "Альтернатива №4: " + results[3];
            label25.Text = "Альтернатива №5: " + results[4];
            label26.Text = GetAnswerCondorcet(results);

            results = CondorcetCopeland(array_data);
            label31.Text = "Альтернатива №1: " + results[0];
            label32.Text = "Альтернатива №2: " + results[1];
            label33.Text = "Альтернатива №3: " + results[2];
            label34.Text = "Альтернатива №4: " + results[3];
            label35.Text = "Альтернатива №5: " + results[4];
            label36.Text = GetAnswer(results);

            results = CondorcetSimp(array_data);
            label41.Text = "Альтернатива №1: " + results[0];
            label42.Text = "Альтернатива №2: " + results[1];
            label43.Text = "Альтернатива №3: " + results[2];
            label44.Text = "Альтернатива №4: " + results[3];
            label45.Text = "Альтернатива №5: " + results[4];
            label46.Text = GetAnswer(results);

            results = MethodBorda(array_data);
            label51.Text = "Альтернатива №1: " + results[0];
            label52.Text = "Альтернатива №2: " + results[1];
            label53.Text = "Альтернатива №3: " + results[2];
            label54.Text = "Альтернатива №4: " + results[3];
            label55.Text = "Альтернатива №5: " + results[4];
            label56.Text = GetAnswer(results);
        }

        string GetAnswer(int[] results) // формируем ответ на основании результатов методов
        {
            int max_value = results[0]; // максимальные баллы
            int max_alt = 1;            // альтернатива с максимальными баллами
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

        string GetAnswerCondorcet(int[] results)
        {
            int count_total_wins = 0;
            int win = 0;
            for (int i = 0; i < results.Length && count_total_wins < 2; i++)
            {
                if (results[i] == alternatives.Length - 1)
                {
                    count_total_wins++;
                    win = i + 1;
                }
            }
            if (count_total_wins == 1)
                return "Коллективное решение: альтернатива №" + win;
            else
                return "Нет решения!";
        }

        int[] RelativeMajority(int[,] opinions) // метод относительного большинства
        {
            int[] alternatives_res = { 0, 0, 0, 0, 0 };     // результаты по каждой альтернативе
            for (int i = 0; i < opinions.GetLength(1); i++)
                switch (opinions[0, i])
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
            for (int i = 0; i < opinions.GetLength(1); i++) // пробегаемся по строкам
                for (int j = 0; j < opinions.GetLength(0); j++)  // пробегаемся по столбцам (z-образно)
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
        int[] alternatives = new int[] { 1, 2, 3, 4, 5 };

        public int[] Condorcet(int[,] opinions)
        {
            int[] alternatives_res = new int[alternatives.Length];
            for (int i = 0; i < alternatives.Length - 1; i++)
            {
                int a = alternatives[i];            // первая альтернатива в паре
                for (int j = i + 1; j < alternatives.Length; j++)
                {
                    int b = alternatives[j];   // вторая альтернатива в паре
                    int la = 0;     // l(x1,x2)
                    for (int k = 0; k < opinions.GetLength(1); k++)     // идем по экспертам
                    {
                        bool pos_a = false;         // индикатор, нашли ли мы в мнении одного эксперта "a" для пары l(a,b), раньше чем "b"
                        for (int m = 0; m < opinions.GetLength(0); m++) // идем по мнениям экспертов
                        {
                            if (opinions[m, k] == a)
                            {
                                pos_a = true;           // мы нашли "а" раньше "b" 
                                break;
                            }
                            if (opinions[m, k] == b)     // мы нашли "b" раньше "a" 
                                break;
                        }
                        if (pos_a) la++;             // если встретили "а" раньше, то увеличиваем очки l(a,b); очки для l(b,a) можно посчитать вычитанием
                    }
                    if (la > opinions.GetLength(1) - la)
                        alternatives_res[i]++;     // если очки l(a,b) больше, то добавляем балл альтернативе "a"
                    else if (la < opinions.GetLength(1) - la)
                        alternatives_res[j]++;     // если очки l(b,a) больше, то добавляем балл альтернативе "b"
                }
            }
            return alternatives_res;
        }

        public int[] CondorcetCopeland(int[,] opinions)
        {
            int[] alternatives_res = new int[alternatives.Length];
            for (int i = 0; i < alternatives.Length; i++)
            {
                int a = alternatives[i];            // первая альтернатива в паре
                for (int j = 0; j < alternatives.Length; j++)
                {
                    if (i == j)
                        continue;
                    int b = alternatives[j];   // вторая альтернатива в паре
                    int kk = 0;                 // количество экспертов, которые предпочтут a b
                    for (int k = 0; k < opinions.GetLength(1); k++)     // идем по экспертам
                    {
                        bool pos_a = false;         // индикатор, нашли ли мы в мнении одного эксперта "a" для пары l(a,b), раньше чем "b"
                        for (int m = 0; m < opinions.GetLength(0); m++) // идем по мнениям экспертов
                        {
                            if (opinions[m, k] == a)
                            {
                                pos_a = true;           // мы нашли "а" раньше "b" 
                                break;
                            }
                            if (opinions[m, k] == b)     // мы нашли "b" раньше "a" 
                                break;
                        }
                        if (pos_a) kk++;             // если встретили "а" раньше, то увеличиваем количество экспертов, которые предпочли a а не b; 
                    }
                    if (kk > opinions.GetLength(1) - kk)
                        alternatives_res[i]++;     // если большинство предпочитает первую альтернативу, то добавляем к ее результату 1
                    else if (kk < opinions.GetLength(1) - kk)
                        alternatives_res[i]--;     // если большинство предпочитает вторую альтернативу, товычитаем 1 из результата первой
                }
            }
            return alternatives_res;
        }
        public int[] CondorcetSimp(int[,] opinions)
        {
            int[] alternatives_res = new int[alternatives.Length];
            for (int i = 0; i < alternatives.Length; i++)
                alternatives_res[i] = -1;
            for (int i = 0; i < alternatives.Length; i++)
            {
                int a = alternatives[i];            // первая альтернатива в паре
                for (int j = 0; j < alternatives.Length; j++)
                {
                    if (i == j)
                        continue;
                    int b = alternatives[j];   // вторая альтернатива в паре
                    int n = 0;                 // количество экспертов, которые предпочтут a b
                    for (int k = 0; k < opinions.GetLength(1); k++)     // идем по экспертам
                    {
                        bool pos_a = false;         // индикатор, нашли ли мы в мнении одного эксперта "a" для пары l(a,b), раньше чем "b"
                        for (int m = 0; m < opinions.GetLength(0); m++) // идем по мнениям экспертов
                        {
                            if (opinions[m, k] == a)
                            {
                                pos_a = true;           // мы нашли "а" раньше "b" 
                                break;
                            }
                            if (opinions[m, k] == b)     // мы нашли "b" раньше "a" 
                                break;
                        }
                        if (pos_a) n++;             // если встретили "а" раньше, то увеличиваем количество экспертов, которые предпочли a а не b; 
                    }
                    if (n < alternatives_res[i] || alternatives_res[i] < 0)
                        alternatives_res[i] = n;     // берем минимальную оценку 
                }
            }
            return alternatives_res;
        }
        private void button_Count_Click(object sender, EventArgs e)
        {
            FillLabels();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int n;
            if (!Int32.TryParse(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(), out n))
                MessageBox.Show("число введите");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Expert_system
{
    public partial class Form1 : Form
    {
        MLV mlv = new MLV();
        public Form1()
        {
            InitializeComponent();
            Fill_QuestionInfo(true);
        }
        private void Fill_QuestionInfo(bool start)
        {
            string question;
            List<string> answers;
            mlv.Get_QuestionInfo(out question, out answers, start);
            if (question == null || answers == null)
            {
                MessageBox.Show("Ошибка загрузки данных. Загрузите БЗ при запуске формы.");
            }
            else
            {
                if (answers[0] == "####")
                {
                    label_result.Visible = true;
                    label_result.Text +=  question;
                    button_ok.Enabled = false;
                }
                else
                {
                    label_question.Text = question;
                    comboBox_answer.DataSource = answers;
                    button_ok.Enabled = true;
                }
            }
        }
        private void создатьБЗToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mlv.Fill_Test_Base();
            Fill_QuestionInfo(true);
            button_ok.Enabled = true;
            label_result.Visible = false;
            label_result.Text = "Result: ";
        }
        private void button_restart_Click(object sender, EventArgs e)       // нажатие на кнопку рестарта
        {
            mlv.Fill_Fact_List();
            Fill_QuestionInfo(false);
            label_result.Visible = false;
            label_result.Text = "Result: ";
            button_ok.Enabled = true;
        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            mlv.Set_Answer(comboBox_answer.Items[comboBox_answer.SelectedIndex].ToString());
            Fill_QuestionInfo(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemSet
{
	public partial class Form1 : Form
	{
		LogicalMachine lm = new LogicalMachine();

		public Form1()
		{
			InitializeComponent();
			foreach (Entity entity in lm.kb.Entities)
				comboBox1.Items.Add(entity.NameImP);
			foreach (TypeOfConnection type in lm.kb.Types_of_connection)
				comboBox2.Items.Add(type.Name);
			UpdateCombo3();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckFullCombo();
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateCombo3();
			textBox1.Text = "";
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckFullCombo();
		}

		void UpdateCombo3()
		{
			comboBox3.Items.Clear();
			if (comboBox2.SelectedIndex != -1)
				switch (lm.kb.Types_of_connection[comboBox2.SelectedIndex].Padezh)
				{
					case 'v':
						foreach (Entity entity in lm.kb.Entities)
							comboBox3.Items.Add(entity.NameViP);
						break;
					case 't':
						foreach (Entity entity in lm.kb.Entities)
							comboBox3.Items.Add(entity.NameTvP);
						break;
					default:
						foreach (Entity entity in lm.kb.Entities)
							comboBox3.Items.Add(entity.NameImP);
						break;
				}
			else
				foreach (Entity entity in lm.kb.Entities)
					comboBox3.Items.Add(entity.NameImP);
		}

		void CheckFullCombo()
		{
			if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "")
			{
				textBox1.Text = lm.GetAnswer(lm.kb.Entities[comboBox1.SelectedIndex], lm.kb.Types_of_connection[comboBox2.SelectedIndex], lm.kb.Entities[comboBox3.SelectedIndex]);
			}
			else
				textBox1.Text = "";
		}
	}
}

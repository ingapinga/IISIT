using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expert_system
{
	public partial class Form2 : Form
	{
		public Form2(List<string> fact_names, List<string> fact_values)
		{
			InitializeComponent(); 
			dataGridView1.Columns.Add("Name", "Name");
			dataGridView1.Columns.Add("Value", "Value");
			for (int i = 0; i < fact_names.Count; i++)
				dataGridView1.Rows.Add();
			for (int i = 0; i < fact_names.Count; i++)
			{
				dataGridView1.Rows[i].Cells[0].Value = fact_names[i];
				if (fact_values[i] == "")
				{
					dataGridView1.Rows[i].Cells[1].Value = "(none)";
					dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.DimGray;
				}
				else
				{
					dataGridView1.Rows[i].Cells[1].Value = fact_values[i];
					dataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
				}
			}
		}
	}
}

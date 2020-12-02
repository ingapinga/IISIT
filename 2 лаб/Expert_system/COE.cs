using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expert_system
{
	class COE       // component of the explanation (компонента объяснения)
	{
		public void PrintOutCurrentsFacts(List<CurrentFact> cf)
		{
			List<string> Fact_names = new List<string>();
			List<string> Fact_values = new List<string>();
			for(int i = 0; i < cf.Count; i++)
			{
				Fact_names.Add(cf[i].Name);
				Fact_values.Add(cf[i].Value);
			}
			Form2 f = new Form2(Fact_names, Fact_values);

			f.ShowDialog();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemSet
{
	class LogicalMachine
	{
		public readonly KnowledgeBase kb = new KnowledgeBase();
		public LogicalMachine()
		{
			kb.GenerateKnowledgeBase();
		}
		public string GetAnswer(Entity parent, TypeOfConnection type_of_c, Entity child)
		{
			foreach (Connection connection in parent.Connections)
				if (connection.Type == type_of_c)
				{
					if (connection.Child == child)
						return "да";
					else
						if (GetAnswer(connection.Child, type_of_c, child) == "да")
						return "да";
				}
			return "нет";
		}
	}
}

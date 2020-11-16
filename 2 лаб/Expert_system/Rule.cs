using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Expert_system
{
	[DataContract]
	class Rule		// класс правил
	{
		[DataMember]
		public List<CurrentFact> Condition { get; set; }		// условия, которые должны выполняться для срабатывания правила
		[DataMember]
		public string Question { get; set; }					// вопрос, который должен задаваться при соблюдении условий
		[DataMember]
		public string Variable { get; set; }					// переменная, в которую заносится результирующее значение
		[DataMember]
		public string Value { get; set; }						// значение, которое присваивается переменной
		[DataMember]
		public int Priority { get; set; }						// приоритет

		public Rule(List<CurrentFact> condition, string question, string variable, string value, int priority)
		{
			Condition = condition;
			Question = question;
			Variable = variable;
			Value = value;
			Priority = priority;
		}
	}

	[DataContract]
	class Fact		// класс фактов с возможными значениями
	{
		[DataMember]
		public string Name { get; set; }			// название факта
		[DataMember]
		public List<string> Values { get; set; }	// список возможных значений факта

		public Fact(string name, List<string> values)
		{
			Name = name;
			Values = values;
		}
	}

	[DataContract]
	class CurrentFact		// класс фактов в рабочей памяти
	{
		[DataMember]
		public string Name { get; set; }		// название факта
		[DataMember]
		public string Value { get; set; }		// действующее значение в системе

		public CurrentFact(string name)
		{
			Name = name;
			Value = "";
		}
		public CurrentFact(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}

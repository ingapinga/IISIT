using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Expert_system
{
	public partial class Form1 : Form
	{
		List<Rule> rules;					// список всех правил
		List<Fact> facts;					// список всех названий фактов и их возможные значения
		List<CurrentFact> current_facts;	// список всех существующих фактов с текущим значением

		public Form1()
		{
			InitializeComponent();
			UploadKB();
		}

		private void создатьБЗToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region Заполнение списков facts и rules
			facts = new List<Fact> { 
				new Fact("pet", new List<string>{"You should get a dog.","You should get a cat.","You should get a fishes.","You should get a lizard.","You should get a parrot.","You shouldn't get a pet."}),
				new Fact("free_time", new List<string>{"yes", "no"}),
				new Fact("allergies", new List<string>{"yes", "no"}),
				new Fact("noise_tolerance", new List<string>{"yes", "no"}),
				new Fact("spending", new List<string>{"yes", "no"}),
				new Fact("for_whom", new List<string>{"me","child","office"}),
				new Fact("fur", new List<string>{"yes", "no"}),
				new Fact("purpose", new List<string>{ "decorative", "interactive"}),
				new Fact("exotic", new List<string>{"yes", "no"})
			};
			
			rules = new List<Rule> {
				new Rule(new List<CurrentFact>{
					new CurrentFact("free_time", ""),
					new CurrentFact("pet", "")},
					"Do you have a lot of free time?", "free_time", "", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("allergies", ""),
					new CurrentFact("pet", "")},
					"Do you have any pet allergies?", "allergies", "", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("noise_tolerance", ""),
					new CurrentFact("pet", "")},
					"Do you tolerate noise?", "noise_tolerance", "", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("spending", ""),
					new CurrentFact("pet", "")},
					"Are you ready to spend a lot of money?", "spending", "", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("for_whom", ""),
					new CurrentFact("pet", "")},
					"For whom is the pet for?", "for_whom", "", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("free_time", "yes"),
					new CurrentFact("allergies", "no"),
					new CurrentFact("pet", "")},
					"", "fur", "yes", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("free_time", "no"),
					new CurrentFact("allergies", "no"),
					new CurrentFact("pet", "")},
					"", "fur", "no", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("allergies", "yes"),
					new CurrentFact("pet", "")},
					"", "fur", "no", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("spending", "yes"),
					new CurrentFact("for_whom", "me"),
					new CurrentFact("pet", "")},
					"", "purpose", "decorative", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("for_whom", "office"),
					new CurrentFact("pet", "")},
					"", "purpose", "decorative", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("spending", "no"),
					new CurrentFact("for_whom", "me"),
					new CurrentFact("pet", "")},
					"", "purpose", "interactive", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("for_whom", "child"),
					new CurrentFact("pet", "")},
					"", "purpose", "interactive", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("spending", "no"),
					new CurrentFact("purpose", "decorative"),
					new CurrentFact("pet", "")},
					"", "exotic", "no", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("purpose", "interactive"),
					new CurrentFact("pet", "")},
					"", "exotic", "no", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("purpose", "decorative"),
					new CurrentFact("spending", "yes"),
					new CurrentFact("pet", "")},
					"", "exotic", "yes", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("fur", "yes"),
					new CurrentFact("noise_tolerance", "yes"),
					new CurrentFact("exotic", "no"),
					new CurrentFact("pet", "")},
					"", "pet", "You should get a dog.", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("fur", "yes"),
					new CurrentFact("noise_tolerance", "no"),
					new CurrentFact("exotic", "no"),
					new CurrentFact("pet", "")},
					"", "pet", "You should get a cat.", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("fur", "no"),
					new CurrentFact("noise_tolerance", "no"),
					new CurrentFact("exotic", "no"),
					new CurrentFact("pet", "")},
					"", "pet", "You should get a fishes.", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("fur", "no"),
					new CurrentFact("noise_tolerance", "no"),
					new CurrentFact("exotic", "yes"),
					new CurrentFact("pet", "")},
					"", "pet", "You should get a lizard.", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("fur", "no"),
					new CurrentFact("noise_tolerance", "yes"),
					new CurrentFact("exotic", "yes"),
					new CurrentFact("pet", "")},
					"", "pet", "You should get a parrot.", 0),
				new Rule(new List<CurrentFact>{
					new CurrentFact("pet", "")},
					"", "pet", "You shouldn't get a pet.", -10),
			};
			#endregion

			DataContractJsonSerializer jsonSerializerR = new DataContractJsonSerializer(typeof(List<Rule>));    // создается объект для работы с правилами в JSON
			using (FileStream file = new FileStream("rules.json", FileMode.Create))
			{
				jsonSerializerR.WriteObject(file, rules);	// заносятся данные о списке правил в JSON
			}
			DataContractJsonSerializer jsonSerializerF = new DataContractJsonSerializer(typeof(List<Fact>));    // создается объект для работы с фактами в JSON
			using (FileStream file = new FileStream("facts.json", FileMode.Create))
			{
				jsonSerializerF.WriteObject(file, facts);   // заносятся данные о списке фактов в JSON
			}
			UploadKB();
		}

		void UploadKB()
		{
			DataContractJsonSerializer jsonSerializerR = new DataContractJsonSerializer(typeof(List<Rule>));	// создается объект для работы с правилами в JSON
			if (File.Exists("rules.json"))
				using (FileStream file = new FileStream("rules.json", FileMode.Open))
				{
					rules = jsonSerializerR.ReadObject(file) as List<Rule>;		// считываются правила из JSON
				}
			else
				MessageBox.Show("Нет файла rules.json. Создайте БЗ");
			DataContractJsonSerializer jsonSerializerF = new DataContractJsonSerializer(typeof(List<Fact>));    // создается объект для работы с фактами в JSON
			if (File.Exists("facts.json"))
				using (FileStream file = new FileStream("facts.json", FileMode.Open))
				{
					facts = jsonSerializerF.ReadObject(file) as List<Fact>;	// считываются факты из JSON
				}
			else
				MessageBox.Show("Нет файла facts.json. Создайте БЗ");
		}
	}
}

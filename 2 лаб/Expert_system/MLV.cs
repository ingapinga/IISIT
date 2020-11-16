using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Expert_system
{
    class MLV
    {
        List<Rule> rules;                   // список всех правил
        List<Fact> facts;                   // список всех названий фактов и их возможные значения
        List<CurrentFact> current_facts;    // список всех существующих фактов с текущим значением
        string c_fact;                      // показывает по какому факту сейчас работаем
        public bool UploadKB()
        {
            bool no_errors = true;
            DataContractJsonSerializer jsonSerializerR = new DataContractJsonSerializer(typeof(List<Rule>));    // создается объект для работы с правилами в JSON
            if (File.Exists("rules.json"))
                using (FileStream file = new FileStream("rules.json", FileMode.Open))
                {
                    rules = jsonSerializerR.ReadObject(file) as List<Rule>;     // считываются правила из JSON
                }
            else
                no_errors = false;
            DataContractJsonSerializer jsonSerializerF = new DataContractJsonSerializer(typeof(List<Fact>));    // создается объект для работы с фактами в JSON
            if (File.Exists("facts.json"))
                using (FileStream file = new FileStream("facts.json", FileMode.Open))
                {
                    facts = jsonSerializerF.ReadObject(file) as List<Fact>; // считываются факты из JSON
                }
            else
                no_errors = false;
            return no_errors;
        }
        public void Get_QuestionInfo(out string question, out List<string> answers, bool start)
        {
            question = null;
            answers = null;
            if (start)
            {
                if (UploadKB())
                {
                    Fill_Fact_List();
                    Ask_Question(ref question, ref answers);
                }
            }
            else
            {
                Ask_Question(ref question, ref answers);
                while (question == "repeat")
                    Ask_Question(ref question, ref answers);
            }
        }
        public void Fill_Fact_List()           // метод, заполняющий список существующих фактов фактами  
        {
            current_facts = new List<CurrentFact>();        // выделяем память под факты
            for (int i = 0; i < facts.Count; i++)           // заносим факты в список (без значений)
                current_facts.Add(new CurrentFact(facts[i].Name));
        }
        private void Ask_Question(ref string question, ref List<string> answers)
        {
            List<Rule> new_rules = new List<Rule>();
            for (int i = 0; i < rules.Count; i++)
                if (Check_Rule(rules[i]))
                    new_rules.Add(rules[i]);
            if (new_rules.Count == 0)
            {
                for (int i = 0; i < facts.Count && (question == null || question=="repeat"); i++)
                    if (current_facts[i].Name == c_fact)
                        question = current_facts[i].Value;
                answers = new List<string> { "####" };
                return;
            }
            new_rules.Sort((x, y) => x.Priority.CompareTo(y.Priority));     // разрешение конфликта по приоритету
            new_rules.Reverse();
            if (new_rules.Count > 1 && new_rules[0].Priority == new_rules[1].Priority)
            {
                for (int i = 1; i < new_rules.Count; i++)
                    if (new_rules[0].Priority != new_rules[i].Priority)
                    {
                        new_rules.RemoveAt(i);
                        i--;
                    }
                new_rules.Sort((x, y) => x.Condition.Count.CompareTo(y.Condition.Count));       // разрешение конфликта по количеству условий (среди правил с одинаковым приоритетом)
                new_rules.Reverse(); //
            }
            c_fact = new_rules[0].Variable.Name;
            if (new_rules[0].Question != "")
            {
                question = new_rules[0].Question;
                answers = new_rules[0].Variable.Values;
            }
            else
            {
                for (int i = 0; i < current_facts.Count; i++)                           // если правило не имеет вопроса, то значение факта определяется самостоятельно (заносится)
                    if (new_rules[0].Variable.Name == current_facts[i].Name)
                    {
                        current_facts[i].Value = new_rules[0].Value;
                        break;
                    }
                question = "repeat";
            }

        }
        private bool Check_Rule(Rule rule)         // проверка на то, можно ли использовать правило
        {
            for (int i = 0; i < rule.Condition.Count; i++)
            {
                string val = null;
                for (int j = 0; j < current_facts.Count && val == null; j++)
                {
                    if (rule.Condition[i].Name == current_facts[j].Name)
                        val = current_facts[j].Value;
                }
                if (rule.Condition[i].Value != val)
                    return false;
            }
            return true;
        }
        public void Set_Answer(string answer)
        {
            for (int i = 0; i < current_facts.Count; i++)
            {
                if (c_fact == current_facts[i].Name)
                {
                    current_facts[i].Value = answer;
                    break;
                }
            }
        }
        public void Fill_Test_Base()
        {
            #region Заполнение списков facts и rules
            facts = new List<Fact> {
                new Fact("pet", new List<string>{"You should get a dog.","You should get a cat.","You should get a fishes.","You should get a lizard.","You should get a parrot.","You shouldn't get a pet."}, true),
                new Fact("free_time", new List<string>{"yes", "no"},false),
                new Fact("allergies", new List<string>{"yes", "no"},false),
                new Fact("noise_tolerance", new List<string>{"yes", "no"},false),
                new Fact("spending", new List<string>{"yes", "no"},false),
                new Fact("for_whom", new List<string>{"me","child","office"},false),
                new Fact("fur", new List<string>{"yes", "no"},false),
                new Fact("purpose", new List<string>{ "decorative", "interactive"},false),
                new Fact("exotic", new List<string>{"yes", "no"},false)
            };

            rules = new List<Rule> {
                new Rule(new List<CurrentFact>{
                    new CurrentFact("free_time", ""),
                    new CurrentFact("pet", "")},
                    "Do you have a lot of free time?", facts[1], "", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("allergies", ""),
                    new CurrentFact("pet", "")},
                    "Do you have any pet allergies?", facts[2], "", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("noise_tolerance", ""),
                    new CurrentFact("pet", "")},
                    "Do you tolerate noise?", facts[3], "", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("spending", ""),
                    new CurrentFact("pet", "")},
                    "Are you ready to spend a lot of money?", facts[4], "", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("for_whom", ""),
                    new CurrentFact("pet", "")},
                    "For whom is the pet for?", facts[5], "", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", ""),
                    new CurrentFact("free_time", "yes"),
                    new CurrentFact("allergies", "no"),
                    new CurrentFact("pet", "")},
                    "", facts[6], "yes", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", ""),
                    new CurrentFact("free_time", "no"),
                    new CurrentFact("allergies", "no"),
                    new CurrentFact("pet", "")},
                    "", facts[6], "no", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", ""),
                    new CurrentFact("allergies", "yes"),
                    new CurrentFact("pet", "")},
                    "", facts[6], "no", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("purpose", ""),
                    new CurrentFact("spending", "yes"),
                    new CurrentFact("for_whom", "me"),
                    new CurrentFact("pet", "")},
                    "", facts[7], "decorative", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("purpose", ""),
                    new CurrentFact("for_whom", "office"),
                    new CurrentFact("pet", "")},
                    "", facts[7], "decorative", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("purpose", ""),
                    new CurrentFact("spending", "no"),
                    new CurrentFact("for_whom", "me"),
                    new CurrentFact("pet", "")},
                    "", facts[7], "interactive", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("purpose", ""),
                    new CurrentFact("for_whom", "child"),
                    new CurrentFact("pet", "")},
                    "", facts[7], "interactive", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("exotic", ""),
                    new CurrentFact("spending", "no"),
                    new CurrentFact("purpose", "decorative"),
                    new CurrentFact("pet", "")},
                    "", facts[8], "no", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("exotic", ""),
                    new CurrentFact("purpose", "interactive"),
                    new CurrentFact("pet", "")},
                    "", facts[8], "no", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("exotic", ""),
                    new CurrentFact("purpose", "decorative"),
                    new CurrentFact("spending", "yes"),
                    new CurrentFact("pet", "")},
                    "", facts[8], "yes", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", "yes"),
                    new CurrentFact("noise_tolerance", "yes"),
                    new CurrentFact("exotic", "no"),
                    new CurrentFact("pet", "")},
                    "", facts[0], "You should get a dog.", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", "yes"),
                    new CurrentFact("noise_tolerance", "no"),
                    new CurrentFact("exotic", "no"),
                    new CurrentFact("pet", "")},
                    "", facts[0], "You should get a cat.", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", "no"),
                    new CurrentFact("noise_tolerance", "no"),
                    new CurrentFact("exotic", "no"),
                    new CurrentFact("pet", "")},
                    "", facts[0], "You should get a fishes.", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", "no"),
                    new CurrentFact("noise_tolerance", "no"),
                    new CurrentFact("exotic", "yes"),
                    new CurrentFact("pet", "")},
                    "", facts[0], "You should get a lizard.", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("fur", "no"),
                    new CurrentFact("noise_tolerance", "yes"),
                    new CurrentFact("exotic", "yes"),
                    new CurrentFact("pet", "")},
                    "", facts[0], "You should get a parrot.", 0),
                new Rule(new List<CurrentFact>{
                    new CurrentFact("pet", "")},
                    "", facts[0], "You shouldn't get a pet.", -10)
            };
            #endregion
            DataContractJsonSerializer jsonSerializerR = new DataContractJsonSerializer(typeof(List<Rule>));    // создается объект для работы с правилами в JSON
            using (FileStream file = new FileStream("rules.json", FileMode.Create))
            {
                jsonSerializerR.WriteObject(file, rules);   // заносятся данные о списке правил в JSON
            }
            DataContractJsonSerializer jsonSerializerF = new DataContractJsonSerializer(typeof(List<Fact>));    // создается объект для работы с фактами в JSON
            using (FileStream file = new FileStream("facts.json", FileMode.Create))
            {
                jsonSerializerF.WriteObject(file, facts);   // заносятся данные о списке фактов в JSON
            }
            UploadKB();
        }
    }
}

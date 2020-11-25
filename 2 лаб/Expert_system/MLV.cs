using System.Collections.Generic;

namespace Expert_system
{
    class MLV
    {
        List<Rule> rules;                   // список всех правил
        List<Fact> facts;                   // список всех названий фактов и их возможные значения
        List<CurrentFact> current_facts;    // список всех существующих фактов с текущим значением
        string c_fact;                      // показывает по какому факту сейчас работаем
        public void Get_QuestionInfo(out string question, out List<string> answers, bool start)
        {
            question = null;
            answers = null;
            if (start)
            {
                if (Rule.UploadKB(ref rules, ref facts))
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
                    if (facts[i].Printout)
                        for(int j=0;j<current_facts.Count && (question == null || question == "repeat"); j++)
                            if (facts[i].Name==current_facts[j].Name)
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
        private bool Check_Rule(Rule rule)         // проверка на то, можно ли использовать правило (совпадают ли условия правила)
        {
            for (int i = 0; i < rule.Condition.Count; i++)
            {
                for (int j = 0; j < current_facts.Count; j++)
                {
                    if (rule.Condition[i].Name == current_facts[j].Name && rule.Condition[i].Value != current_facts[j].Value)
                        return false;
                    else 
                    if (rule.Condition[i].Name == current_facts[j].Name)
                        break;
                }
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
            Rule.FillTestBase(ref rules, ref facts);
		}
    }
}

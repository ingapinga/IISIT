using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemSet
{
	class KnowledgeBase
	{
		public List<Entity> Entities = new List<Entity>();
		public List<TypeOfConnection> Types_of_connection = new List<TypeOfConnection>();

		public void GenerateKnowledgeBase()
		{
			Entities.Add(new Entity("ящерица", "ящерицу", "ящерицей")); // 0
			Entities.Add(new Entity("попугай", "попугая", "попугаем")); // 1
			Entities.Add(new Entity("собака", "собаку", "собакой")); // 2
			Entities.Add(new Entity("кошка", "кошку", "кошкой")); // 3
			Entities.Add(new Entity("рыбки", "рыбок", "рыбками")); // 4
			Entities.Add(new Entity("свободное время", "свободное время", "свободным временем")); // 5
			Entities.Add(new Entity("аллерген", "аллерген", "аллергеном")); // 6
			Entities.Add(new Entity("шум", "шум", "шумом")); // 7
			Entities.Add(new Entity("шерсть", "шерсть", "шерстью")); // 8
			Entities.Add(new Entity("высокая стоимость", "высокую стоимость", "высокой стоимостью")); // 9
			Entities.Add(new Entity("низкая стоимость", "низкую стоимость", "низкой стоимостью")); // 10
			Entities.Add(new Entity("взрослый", "взрослого", "взрослым")); // 11
			Entities.Add(new Entity("ребенок", "ребенка", "ребенком")); // 12
			Entities.Add(new Entity("офис", "офис", "офисом")); // 13
			Entities.Add(new Entity("декоративное назначение", "декоративное назначение", "декоративным назначением")); // 14
			Entities.Add(new Entity("интерактивное назначение", "интерактивное назначение", "интерактивным назначением")); // 15
			Entities.Add(new Entity("экзотический вид", "экзотический вид", "экзотическим видом")); // 16
			Entities.Add(new Entity("обычный вид", "обычный вид", "обычным видом")); //17

			Types_of_connection.Add(new TypeOfConnection("является", 't'));
			Types_of_connection.Add(new TypeOfConnection("имеет", 'v'));
			Types_of_connection.Add(new TypeOfConnection("требует", 'v'));

			new Connection(Entities[0], Types_of_connection[0], Entities[16]);
			new Connection(Entities[1], Types_of_connection[0], Entities[16]);
			new Connection(Entities[1], Types_of_connection[1], Entities[7]);
			new Connection(Entities[2], Types_of_connection[1], Entities[7]);
			new Connection(Entities[2], Types_of_connection[0], Entities[17]);
			new Connection(Entities[2], Types_of_connection[1], Entities[8]);
			new Connection(Entities[3], Types_of_connection[1], Entities[8]);
			new Connection(Entities[3], Types_of_connection[0], Entities[17]);
			new Connection(Entities[4], Types_of_connection[0], Entities[17]);
			new Connection(Entities[8], Types_of_connection[2], Entities[5]);
			new Connection(Entities[8], Types_of_connection[0], Entities[6]);
			new Connection(Entities[13], Types_of_connection[2], Entities[14]);
			new Connection(Entities[11], Types_of_connection[2], Entities[14]);
			new Connection(Entities[11], Types_of_connection[2], Entities[15]);
			new Connection(Entities[12], Types_of_connection[2], Entities[15]);
			new Connection(Entities[14], Types_of_connection[1], Entities[9]);
			new Connection(Entities[14], Types_of_connection[1], Entities[10]);
			new Connection(Entities[15], Types_of_connection[1], Entities[10]);
			new Connection(Entities[16], Types_of_connection[1], Entities[14]);
			new Connection(Entities[17], Types_of_connection[1], Entities[14]);
			new Connection(Entities[17], Types_of_connection[1], Entities[15]);
		}
	}

	class Entity
	{
		public string NameImP { get; }
		public string NameViP { get; }
		public string NameTvP { get; }
		public List<Connection> Connections { get; set; }
		public Entity(string nameImP, string nameViP, string nameTvP)
		{
			NameImP = nameImP;
			NameViP = nameViP;
			NameTvP = nameTvP;
			Connections = new List<Connection>();
		}
	}

	class Connection
	{ 
		public Entity Parent { get; }
		public Entity Child { get; }
		public TypeOfConnection Type { get; }
		public Connection(Entity parent, TypeOfConnection type, Entity child)
		{
			Parent = parent;
			Child = child;
			Type = type;
			parent.Connections.Add(this);
		}
	}

	class TypeOfConnection
	{
		public string Name { get; }
		public char Padezh { get; }
		public TypeOfConnection(string name, char padezh)
		{
			Name = name;
			Padezh = padezh;
		}
	}
}

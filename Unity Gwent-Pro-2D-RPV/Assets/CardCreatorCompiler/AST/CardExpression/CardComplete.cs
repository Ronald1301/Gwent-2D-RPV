using System.Collections.Generic;
using System.Linq;

namespace Gwent
{
    public class DataCardComplete
    {
        public DataCardComplete(string name, string type, string power, Queue<object> ability, string faction, bool[] range)
        {
            Name = name;
            Type = type;
            Power = power;
            while (ability.Count > 0)  Ability.Enqueue(ability.Dequeue());
            Faction = faction;
            foreach (var item in range)
            {
                Range.Append(item);
            }
        }
        public DataCardComplete()
        {
            Name = null!;
            Type = null!;
            Power = null!;
            Ability = null!;
            Faction = null!;
            Range = null!;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Power { get; set; }
        public string Faction { get; set; }
        public bool[] Range { get; set; } = new bool[2];
        public Queue<object>? Ability { get; set; } = new();
    }
}
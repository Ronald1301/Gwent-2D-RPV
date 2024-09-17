using System.Collections.Generic;
using System.Linq;

namespace Gwent
{
    public  class DataCardComplete
    {
        public DataCardComplete(string name, string type, string power, Queue<List<string>> ability, string faction, bool[] range)
        {
            Name = name;
            Type = type;
            Power = power;
            while (ability.Count > 0) 
            {
                foreach (var item in ability.Dequeue())
                {
                    NamesAbility.Enqueue(item);
                }
            }
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
            Faction = null!;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Power { get; set; }
        public string Faction { get; set; }
        public bool[] Range { get; set; } = new bool[3];
        public Queue<string>? NamesAbility { get; set; } = new();
    }
}
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Gwent
{
    public class DataCardComplete
    {
        public DataCardComplete(Token name, Token type, Token power, List<Token> ability, Token faction, bool[] range, Token image)
        {
            Name = name;
            Type = type;
            Power = power;
            foreach (var item in ability)
            {
                Ability.Enqueue(item);   
            }
            Faction = faction;
            foreach (var item in range)
            {
                Range.Append(item);
            }
            Image = image;
        }
        public DataCardComplete()
        {
            Name = null!;
            Type = null!;
            Power = null!;
            Ability = null!;
            Faction = null!;
            Range = null!;
            Image = null!;
        }
        public object Name { get; set; }
        public object Type { get; set; }
        public object Power { get; set; }
        public Queue<object> Ability { get; set; }= new();
        public object Faction { get; set; }
        public bool[] Range { get; set; }= new bool[2];
        public object Image { get; set; }

    }
}
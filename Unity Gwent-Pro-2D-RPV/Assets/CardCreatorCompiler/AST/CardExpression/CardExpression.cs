
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class CardExpression : Expression
    {
        public Expression Name;
        public Expression Type;
        public Expression Faction;
        public Expression Power;
        public List<Expression> Range = new();
        public OnActivationExpression OnActivation;

        protected override Scope? Context { get; set; }

        public CardExpression(Expression name, Expression type, Expression faction, Expression power, List<Expression> range, OnActivationExpression onActivation)
        {
            this.Name = name;
            this.Type = type;
            this.Faction = faction;
            this.Power = power;
            foreach (var item in range)
            {
                this.Range.Add(item);
            }
            this.OnActivation = onActivation;
        }
        public CardExpression()
        {
            this.Name = null!;
            this.Type = null!;
            this.Faction = null!;
            this.Power = null!;
            this.OnActivation = null!;
        }

        public override object Evaluate()
        {
            DataCardComplete card = new DataCardComplete
            {
                Name = this.Name.Evaluate(),
                Type = Type.Evaluate(),
                Faction = Faction.Evaluate(),
                Power = Power.Evaluate()
            };
            foreach (var item in Range)
            {
                if (!card.Range[0] && item.Evaluate().ToString() == "Melee")
                {
                    card.Range[0] = true;
                }
                else if (!card.Range[1] && item.Evaluate().ToString() == "Ranged")
                {
                    card.Range[1] = true;
                }
                else if (!card.Range[2] && item.Evaluate().ToString() == "Siege")
                {
                    card.Range[2] = true;
                }
                else Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "Range is not valid"));
            }
            card.Ability = (Queue<object>)OnActivation.Evaluate();
            Additional.cardCompletes.Add(card);
            return card!;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope Son = new Scope(current,new(),new());
            Name.SetScope(Son);
            Type.SetScope(Son);
            Faction.SetScope(Son);
            Power.SetScope(Son);
            foreach (var item in Range)
            {
                item.SetScope(Son);
            }
            OnActivation.SetScope(Son);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Name is null)
            {
                throw new Exception("Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                throw new Exception("Name is not IDExpression");
            }
            if (Type is null)
            {
                throw new Exception("Type is null");
            }
            if (Type.CheckSemantic() != Scope.DataType.String)
            {
                throw new Exception("Type is not IDExpression");
            }
            if (Faction is null)
            {
                throw new Exception("Faction is null");
            }
            if (Faction.CheckSemantic() != Scope.DataType.String)
            {
                throw new Exception("Faction is not IDExpression");
            }
            if (Power is null)
            {
                throw new Exception("Power is null");
            }
            if (Power.CheckSemantic() != Scope.DataType.Number)
            {
                throw new Exception("Power is not IntExpression");
            }
            foreach (var item in Range)
            {
                if (item.CheckSemantic() != Scope.DataType.String)
                {
                    throw new Exception("Range is not valid");
                }
            }
            OnActivation.CheckSemantic();
            return Scope.DataType.Void;

        }
    }
}

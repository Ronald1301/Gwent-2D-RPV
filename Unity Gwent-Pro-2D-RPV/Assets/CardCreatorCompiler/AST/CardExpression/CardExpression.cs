using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;

namespace Gwent
{
    public class CardExpression : Expression
    {
        public Expression Name;
        public Expression Type;
        public Expression Faction;
        public Expression Power;
        public List<Expression> Range;
        public OnActivationExpression OnActivation;
        private string[] Types = { "Oro", "Plata", "Lider", "Clima", "Aumento", "Señuelo", "Despeje" ,
        "Gold", "Silver", "Leader", "Weather", "Increase", "Lure", "Clearance" ,"Jefe","Boss",};

        protected override Scope? Context { get; set; }

        public CardExpression(Expression name, Expression type, Expression faction, Expression power, List<Expression> range, OnActivationExpression onActivation)
        {
            this.Name = name;
            this.Type = type;
            this.Faction = faction;
            this.Power = power;
            this.Range = new();
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
            this.Range = new();
            this.OnActivation = null!;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope Son = new Scope(current, new(), new());
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
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Name is not string");
            }
            if (Type is null)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Type is null");
            }
            if (Type.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Type is not string");
            }
            if (Faction is null)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Faction is null");
            }
            if (Faction.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Faction is not string");
            }
            if (Power is null)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Power is null");
            }
            if (Power.CheckSemantic() != Scope.DataType.Number)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Power is not number");
            }
            foreach (var item in Range)
            {
                if (item.CheckSemantic() != Scope.DataType.String)
                {
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "Range is not string");
                }
            }
            OnActivation.CheckSemantic();
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            DataCardComplete card = new()
            {
                /*
                Name = this.Name.Evaluate().ToString()!, 
                Type = this.Type.Evaluate().ToString()!,
                Faction = this.Faction.Evaluate().ToString()!,
                Power = this.Power.Evaluate().ToString()!,
                */
            };
            try
            {
                card.Name = this.Name.Evaluate().ToString();
                card.Type = Types.Contains(Type.Evaluate().ToString()) ? Type.Evaluate().ToString() : throw new Exception("Invalid Type for Card");
                card.Faction = Faction.Evaluate().ToString()!;
                card.Power = (Power.Evaluate() is double x) ? x.ToString() : throw new Exception("Power must be integer");

            }
            catch (System.Exception e)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, e.Message);
            }
            
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
                else
                {
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "Invalid Range");
                }
            }
            var names = OnActivation.Evaluate() as Queue<List<string>>;
            if (names!.Peek() is not null)
            {
                while (names!.Count > 0)
                {
                    foreach (var item in names.Dequeue())
                    {
                        if (EngineCompiler.effects.ContainsKey((item, "")))
                        {
                            var value = EngineCompiler.effects[(item, "")];
                            EngineCompiler.effects.Remove((item, ""));
                            EngineCompiler.effects.Add((item, card.Name), value);
                        }
                        card.NamesAbility!.Enqueue(item);
                    }
                }
            }
            EngineCompiler.cards.Add(card.Name!.ToString()!, card);
            return card!;
        }
    }
}

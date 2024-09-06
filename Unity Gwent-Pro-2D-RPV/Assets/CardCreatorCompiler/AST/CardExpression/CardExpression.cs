using System;
using System.Collections.Generic;

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
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is not IDExpression");
            }
            if (Type is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Type is null");
            }
            if (Type.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Type is not IDExpression");
            }
            if (Faction is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Faction is null");
            }
            if (Faction.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Faction is not IDExpression");
            }
            if (Power is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Power is null");
            }
            if (Power.CheckSemantic() != Scope.DataType.Number)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Power is not IntExpression");
            }
            foreach (var item in Range)
            {
                if (item.CheckSemantic() != Scope.DataType.String)
                {
                    EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                    throw new Exception("Range is not valid");
                }
            }
            OnActivation.CheckSemantic();
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            DataCardComplete card = new()
            {
                Name = this.Name.Evaluate().ToString(),
                Type = Type.Evaluate().ToString(),
                Faction = Faction.Evaluate().ToString(),
                Power = Power.Evaluate().ToString()
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
                else
                {
                    EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                    throw new("Range is not valid");
                }
            }
            card.Ability = OnActivation.Evaluate() as Queue<object>;
            EngineCompiler.cards.Add(card.Name.ToString()!, card);
            return card!;
        }
    }
}

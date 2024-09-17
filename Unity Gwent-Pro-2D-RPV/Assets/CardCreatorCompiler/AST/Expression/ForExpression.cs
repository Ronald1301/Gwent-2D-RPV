using System;
using System.Collections.Generic;
using System.Linq;

namespace Gwent
{
    public class ForExpression : Expression
    {
        protected override Scope? Context { get; set; }
        readonly InExpression Condition;
        readonly Statement Body;
        public ForExpression(InExpression condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }
        public override Scope.DataType CheckSemantic()
        {
            if (Condition.CheckSemantic() != Scope.DataType.Boolean)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Condition is not boolean");
            }
            Body.CheckSemantic();
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            if (!Context!.Items.ContainsKey(Condition.Collection.ID))
            {
                //Context.Items.Add(Condition.Collection.ID, Condition.Collection.Evaluate());
                Context.Items.Add(Condition.Collection.ID, Condition.Item);
            }
            while (Convert.ToBoolean(Condition.Evaluate()))
            {
                Body.Evaluate();
            }
            return 0;
        }

        public override void SetScope(Scope current)
        {
            Scope son = new(current, new(), new());
            Context = son;
            Condition.SetScope(son);
            Body.SetScope(son);

        }
    }
    public class InExpression : Expression
    {
        public readonly Assignment Item;
        public int Index = 0;
        public readonly Assignment Collection;
        protected override Scope? Context { get; set; }

        public InExpression(Assignment item, Assignment collection)
        {
            Item = item;
            Collection = collection;
        }

        public override object Evaluate()
        {
            if (Collection is IEnumerable<object> collection)
            {
                if (collection.Count() > Index)
                {
                    Context!.Items[Item.ID] = collection.ElementAt(Index);
                    Index++;
                    return true;
                }
                else
                    return false;
            }
            else
            {
               EngineCompiler.CreateError(ErrorCode.EvaluateError, "Collection is not a collection");
                return false;
            }
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Item.SetScope(current);
            Collection.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Context!.Items.ContainsKey(Collection.ID) || Context!.Items.ContainsKey(Item.ID))
            {
                while (Context.Father != null)
                {
                    if (Context.Items.ContainsKey(Collection.ID) || Context.Items.ContainsKey(Item.ID))
                    {
                        EngineCompiler.CreateError(ErrorCode.SemanticError, "Item or Collection already exists in the current scope");
                    }
                    Context = Context.Father;
                }
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Item or Collection already exists in the current scope");
            }
            if (Item.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Item is not IDExpression");
            }
            if (Collection.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Collection is not IDExpression");
            }
            return Scope.DataType.Boolean;
        }
    }
}
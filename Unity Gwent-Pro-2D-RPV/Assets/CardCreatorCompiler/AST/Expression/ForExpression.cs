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
                EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                throw new Exception("Condition is not a boolean");
            }
            Body.CheckSemantic();
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            if (!Context!.Items.ContainsKey(Condition.Collection.ID))
            {
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
            Context = current;
            Condition.SetScope(current);
            Body.SetScope(current);
        }
    }
    public class InExpression : Expression
    {
        public readonly Assignment Item ;
        public int Index = 0;
        public readonly Assignment Collection ;
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
                EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                throw new Exception("Collection is not a IEnumerable");
            }
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new(current, new(), new());
            Item.SetScope(son);
            Collection.SetScope(son);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Context!.Items.ContainsKey(Collection.ID) || Context!.Items.ContainsKey(Item.ID))
            {
                while (Context.Father != null)
                {
                    if (Context.Items.ContainsKey(Collection.ID) || Context.Items.ContainsKey(Item.ID))
                    {
                        EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                        throw new Exception("Item or Collection already exists in the current scope");
                    }
                    Context = Context.Father;
                }
                EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                throw new Exception("Item or Collection already exists in the current scope");
            }
            if (Item.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                throw new Exception("Item is not IDExpression");
            }
            if (Collection.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error=new TypeError(ErrorCode.SemanticError);
                throw new Exception("Collection is not IDExpression");
            }
            return Scope.DataType.Boolean;
        }
    }
}
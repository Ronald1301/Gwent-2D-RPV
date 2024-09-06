using System;

namespace Gwent
{
    public class Assignment : Expression
    {
        public enum Operators
        {
            Equal, TwoPoint, SumEqual, DifEqual, MulEqual, DivEqual, UnknownWrapper
        }

        public IDExpression ID;
        public Operators operators;
        public Expression Argument;
        //public Token.TokenType Type { get; set; }
        protected override Scope? Context { get; set; }

        public Assignment(IDExpression token, Operators opera, Expression argument = null!)// : base(token, argument)
        {
            ID = token;
            operators = opera;
            Argument = argument;
        }


        /*
                public override string ToString()
                {
                    return $"{iDExpression} {operators} {Argument
        */

        public override void SetScope(Scope current)
        {
            Context = current;
            current.datatype.Add(ID.token, Scope.DataType.Unknown);
            ID.SetScope(current);
            Argument.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Context!.datatype.Keys)
            {
                if (item.Value == ID.token.Value)
                {
                    Context.datatype[item] = ID.CheckSemantic();
                    return Context.datatype[item];
                }
            }
            EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
            throw new("The variable is not declared");
        }
        public override object Evaluate()
        {
            try
            {
                foreach (var item in Context!.datatype.Keys)
                {
                    if (item.Value == ID.token.Value)
                    {
                        var result = Argument.Evaluate();
                        if (Context.Items.ContainsKey(ID))
                        {
                            Context.Items[ID] = result;
                            if (operators == Operators.TwoPoint || operators == Operators.Equal)
                            {
                                return result;
                            }
                        }
                        else
                        {
                            Context.Items.Add(ID, result);
                            return result;
                        }
                    }
                }
                foreach (var item in Context!.Items.Keys)
                {
                    if (item.token.Value == ID.token.Value)
                    {
                        return this.operators switch
                        {
                            Operators.SumEqual =>
                                Context.Items[ID] = Convert.ToDouble(Context.Items[ID]) + Convert.ToDouble(Argument.Evaluate()),
                            Operators.DifEqual =>
                                Context.Items[ID] = Convert.ToDouble(Context.Items[ID]) - Convert.ToDouble(Argument.Evaluate()),
                            Operators.MulEqual =>
                                Context.Items[ID] = Convert.ToDouble(Context.Items[ID]) * Convert.ToDouble(Argument.Evaluate()),
                            Operators.DivEqual =>
                                Context.Items[ID] = Convert.ToDouble(Context.Items[ID]) / Convert.ToDouble(Argument.Evaluate()),
                            _ =>
                                throw new("Error en la asignación")
                        };
                    }

                }
                throw new System.Exception("Error en la asignación");
            }
            catch (System.Exception e)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new(e.Message);
            }
        }
        /*
   public override object Evaluate()
   {
       switch (operators)
       {
           case Operators.Equal:
           case Operators.TwoPoint:
               foreach (var item in Context!.datatype.Keys)
               {
                   if (item.Value == ID.token.Value)
                   {
                       var result = Argument.Evaluate();
                       Context.Items.Add(ID, result);
                       return result;
                   }
               }
               break;
           case Operators.SumEqual:
               foreach (var item in Context!.datatype.Keys)
               {
                   if (item.Value == ID.token.Value)
                   {
                       var result = Argument.Evaluate();
                       Context.Items[ID] = (double)Context.Items[ID] + (double)result;
                       return Context.Items[ID];
                   }
               }
               break;
           case Operators.DifEqual:
               foreach (var item in Context!.datatype.Keys)
               {
                   if (item.Value == ID.token.Value)
                   {
                       var result = Argument.Evaluate();
                       Context.Items[ID] = (double)Context.Items[ID] - (double)result;
                       return Context.Items[ID];
                   }
               }
               break;
           case Operators.MulEqual:
               foreach (var item in Context!.datatype.Keys)
               {
                   if (item.Value == ID.token.Value)
                   {
                       var result = Argument.Evaluate();
                       Context.Items[ID] = (double)Context.Items[ID] * (double)result;
                       return Context.Items[ID];
                   }
               }
               break;
          // case Operators.DivEqual:
           default:
               foreach (var item in Context!.datatype.Keys)
               {
                   if (item.Value == ID.token.Value)
                   {
                       var result = Argument.Evaluate();
                       Context.Items[ID] = (double)Context.Items[ID] / (double)result;
                       return Context.Items[ID];
                   }
               }
               break;
       }
       throw new();
   }
*/
    }
}
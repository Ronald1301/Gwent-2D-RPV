using System;

namespace Gwent
{
    public class Assignment : Expression
    {
        public enum Operators
        {
            Equal, TwoPoint, SumEqual, DifEqual, MulEqual, DivEqual, UnknownWrapper
        }

        public IDExpression? ID;
        public DotExpression? dotExpression;
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
        public Assignment(DotExpression dot, Operators opera, Expression argument = null!)// : base(token, argument)
        {
            dotExpression = dot;
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
            if (dotExpression is not null) dotExpression.SetScope(current);
            else
            {
                current.datatype.Add(ID!.token, ID.Type);
                ID.SetScope(current);
            }
            if (Argument is not null) Argument.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Context!.datatype.Keys)
            {
                if (item.Value == ID!.token.Value)
                {
                    Context.datatype[item] = ID.CheckSemantic();
                    return Context.datatype[item];
                }
            }
            foreach (var item in Context!.Items.Keys)
            {
                if (item.token.Value == ID!.token.Value)
                {
                    return ID.CheckSemantic();
                }
            }
            EngineCompiler.CreateError(ErrorCode.SemanticError, "Undeclared variable");
            return Scope.DataType.Unknown;
        }
        public override object Evaluate()
        {
            try
            {
                if (dotExpression is null)
                {
                    foreach (var item in Context!.datatype.Keys)
                    {
                        if (item.Value == ID!.token.Value)
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
                        if (item.token.Value == ID!.token.Value)
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
                else
                {
                    var valueDotExpression = dotExpression.Left.Evaluate();
                    var typeDot = dotExpression.Type;
                    return null!;
                    //return dotExpression.Right.Evaluate(valueDotExpression, typeDot, Argument.Evaluate());
                }
            }
            catch (System.Exception e)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, e.Message);
                return null!;
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
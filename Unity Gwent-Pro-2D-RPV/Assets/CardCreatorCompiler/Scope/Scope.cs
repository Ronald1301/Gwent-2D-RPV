using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwent
{
    public class Scope
    {
        public Scope(Scope father, Dictionary<IDExpression, object> items, Dictionary<Token, Scope.DataType> type)
        {
            Father = father;
            Items = items;
            datatype = type;
        }
        public Scope? Father;
        public Dictionary<IDExpression, object> Items;

        //public Dictionary<IDExpression, Stack<object>> Items2 { get; set; }  //para funciones recursivas
        public Dictionary<Token, DataType> datatype;

        public enum DataType
        {
            Number, String, Boolean, Card, IEnumerableCards,
            Context,
            Void,
            Unknown
        }

    }
}
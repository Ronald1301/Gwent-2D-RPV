using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwent
{
    public abstract class Error
    {
        public abstract string Text();
    }
    public class LocationError
    {
        int Line { get; set; }
        int Column { get; set; }

        public LocationError(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }

        public override string ToString()
        {
            return "Line: " + Line + " Column: " + Column;
        }


    }
}
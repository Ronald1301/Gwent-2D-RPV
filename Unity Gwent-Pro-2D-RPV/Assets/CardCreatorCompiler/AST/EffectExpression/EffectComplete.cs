using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Gwent
{
    public class EffectComplete
    {
        public object Name { get; set; }
        public List<object> Params { get; set; }
        public object Body { get; set; }

        public EffectComplete(object name, List<object> Params, object body)
        {
            Name = name;
            this.Params = Params;
            Body = body;
        }
        public EffectComplete()
        {
            Name = null!;
            Params = null!;
            Body = null!;
        }

    }
}

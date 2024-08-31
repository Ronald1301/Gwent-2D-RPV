using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public static class Additional
    {
        public static List<Error> errors = new();
        public static List<DataCardComplete> cardCompletes = new();
        public static List<EffectComplete> effectCompletes = new();
        public static List<EffectDeclarationExpression> effectDeclaration = new();
    }
}
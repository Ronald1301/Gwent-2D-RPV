
using System.Collections.Generic;
using System;


namespace Gwent
{
    public static class EngineCompiler
    {
        public static Error error;
        //public static List<DataCardComplete> cardCompletes = new();
        public static Dictionary<string, EffectComplete> effectsSemi;
        public static Dictionary<(string, string), (EffectComplete, SelectorExpression)> effects;
        public static Dictionary<string, DataCardComplete> cards;

        public static void Initialize()
        {
            error = new Error(ErrorCode.NoExist, "");
            effectsSemi = new Dictionary<string, EffectComplete>();
            effects = new Dictionary<(string, string), (EffectComplete, SelectorExpression)>();
            cards = new Dictionary<string, DataCardComplete>();
        }
        public static string PrintResult()
        {
            string resultString = "";
            foreach (var item in cards)
            {
                resultString += "Card :" + item.Key.ToString() + "\n";
            }
            foreach (var item in effects)
            {
                resultString += "Effect :" + item.Key.ToString() + "\n";
            }
            if (resultString == "")
            {
                resultString = "No cards or effects were created";
            }
            return resultString;
        }
        public static void CompileCode(string Code)
        {
            // Compare this snippet from Parser/Parser.cs:
            var lexer = new LexicalAnalyzer(Code);
            var tokensList = lexer.Analyze();
            var parser = new Parser(tokensList);
            var ast = parser.Parsing();
            ast.SetScope(new(null!, new(), new()));
            ast.CheckSemantic();
            ast.Evaluate();
        }

        public static void CreateError(ErrorCode type, string text)
        {
            error = new Error(type, text);
            throw new Exception("Error found");
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace Gwent
{
    public static class EngineCompiler
    {
        public static TypeError error;
        //public static List<DataCardComplete> cardCompletes = new();
        public static Dictionary<string, EffectComplete> effectsSemi = new();
        public static Dictionary<string, (EffectComplete,SelectorExpression)> effects = new();
        public static Dictionary<string, DataCardComplete> cards = new();

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
            if (ast == null)
            {
                UnityEngine.Debug.Log("Error in parsing");
                return;
            }
            ast.SetScope(new(null!, new(), new()));
            ast.CheckSemantic();
            System.Console.WriteLine(ast.Evaluate());
        }
    }
}